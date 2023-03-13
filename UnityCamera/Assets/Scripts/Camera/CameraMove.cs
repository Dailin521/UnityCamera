using Cinemachine;
using DG.Tweening;
using UnityEngine;

[ExecuteInEditMode]
public class CameraMove : MonoBehaviour
{
    private float xMove;
    private float yMove;
    Vector3 moveDir;
    Vector3 inputDir;
    [Header("左键移动速度")]
    [SerializeField] private float moveSpeed = 1;
    [Header("右键旋转速度")]
    [SerializeField] private float rotateSpeed = 1;
    [Header("是否启用边缘移动")]
    [SerializeField] private bool isEdgeScrolling = false;
    [Tooltip("边缘滚动的界限大小")]
    [SerializeField, Range(0, 50)] private int edgeScrollSize = 20;
    [Tooltip("边缘滚动的速度")]
    [SerializeField] float edgeMoveSpeed = 1;
    Vector2 currentMousePos;
    private float initialEdgeMoveSpeed = 0;
    public CinemachineVirtualCamera virtualCamera;
    void Init()
    {
        initialEdgeMoveSpeed = edgeMoveSpeed;
        virtualCamera = transform.Find("CMvcam").GetComponent<CinemachineVirtualCamera>();
        InitScrollSize();
    }
    private void Awake()
    {
        Init();
    }
    void Update()
    {
        HandleCameraMove();
        CameraZoom();
        CameraRotate();
        ClickMove();
    }
    void HandleCameraMove()
    {
        xMove = Input.GetAxis("Horizontal") * 0.1f;
        yMove = Input.GetAxis("Vertical") * 0.1f;
        inputDir.z = yMove;
        inputDir.x = xMove;
        if (Input.GetMouseButton(0))
        {
            inputDir.x = Input.GetAxis("Mouse X");
            inputDir.z = Input.GetAxis("Mouse Y");
            edgeMoveSpeed = 1;
        }
        else
        {
            edgeMoveSpeed = initialEdgeMoveSpeed;
        }
        if (isEdgeScrolling && Application.isPlaying)
        {
            currentMousePos = Input.mousePosition;
            if (currentMousePos.x < edgeScrollSize)
            {
                inputDir.x = -0.1f;
            }
            if (currentMousePos.y < edgeScrollSize)
            {
                inputDir.z = -0.1f;
            }
            if (currentMousePos.x > Screen.width - edgeScrollSize)
            {
                inputDir.x = 0.1f;
            }
            if (currentMousePos.y > Screen.height - edgeScrollSize)
            {
                inputDir.z = 0.1f;
            }
        }
        moveDir = transform.forward * inputDir.z + transform.right * inputDir.x;
        transform.position += moveDir * moveSpeed * edgeMoveSpeed * Time.deltaTime;

    }
    [SerializeField][Header("Y方向角度"), Range(15, 60)] float followYAngle = 45;
    void CameraRotate()
    {
        if (Input.GetMouseButton(1))
        {
            transform.localEulerAngles += new Vector3(0, Input.GetAxis("Mouse X") * rotateSpeed, 0);
            followYAngle += Input.GetAxis("Mouse Y");
            followYAngle = Mathf.Clamp(followYAngle, 15, 60);
            followOffest = virtualCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset;
            virtualCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset = new Vector3(followOffest.x, -followOffest.z * (Mathf.Tan(Mathf.Deg2Rad * followYAngle)), followOffest.z);
        }
    }
    private Vector3 followOffest;
    [Header("最大缩放")]
    [SerializeField] float maxScrollSize = 50;
    [Header("最小缩放")]
    [SerializeField] float minScrollSize = 5;
    float currentScrollSize = 10;
    [Header("缩放平滑")]
    [SerializeField] private float scrollDamping = 10;
    [Header("目标缩放值")]
    [SerializeField] float taregtScrollSize = 10;
    //用于设置相机 缩放大小
    public float CameraScrollSize
    {
        get { return taregtScrollSize; }
        set { taregtScrollSize = value; }
    }
    [SerializeField] float scrollScale = 3;
    void CameraZoom()
    {
        if (Input.GetMouseButton(1))
        {
            return;
        }
        if (Input.GetMouseButtonUp(1))
        {
            InitScrollSize();
        }
        if (Input.mouseScrollDelta.y != 0)
        {
            taregtScrollSize -= Input.mouseScrollDelta.y * scrollScale;
        }
        taregtScrollSize = Mathf.Clamp(taregtScrollSize, minScrollSize, maxScrollSize);
        currentScrollSize = Mathf.Lerp(currentScrollSize, taregtScrollSize, Time.deltaTime * scrollDamping);
        followOffest = virtualCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset;
        Vector3 vector3 = new Vector3(followOffest.x, -followOffest.z * (Mathf.Tan(Mathf.Deg2Rad * followYAngle)), followOffest.z);
        virtualCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset = vector3.normalized * currentScrollSize;
    }
    void InitScrollSize()
    {
        Vector3 temp = virtualCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset;
        taregtScrollSize = temp.y / temp.normalized.y;
        currentScrollSize = taregtScrollSize;
    }
    void ClickMove()
    {
        if (IsDoubleClick((int)SwitchMouse.左键))
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
            {
                transform.DOKill();

                if (hit.collider.GetComponent<CameraInitInform>() != null)
                {
                    Debug.Log("双击事件");
                    var a = hit.collider.GetComponent<CameraInitInform>();
                    transform.DORotate(new Vector3(0, a.X_Rotate, 0), 1);
                    DOTween.To(() => followYAngle, x => followYAngle = x, a.Y_Rotate, 1);
                    transform.DOMove(hit.transform.position, 1f).OnComplete(() =>
                    {
                        DOTween.To(() => taregtScrollSize, x => taregtScrollSize = x, a.Scroll, 1);
                    });
                }
                else
                {
                    transform.DOMove(hit.transform.position, 1f).OnComplete(() =>
                    {
                        DOTween.To(() => CameraScrollSize, x => CameraScrollSize = x, 10, 1);
                    });
                }
            }
        }
    }
    //鼠标双击等事件
    enum SwitchMouse { 左键, 右键, 中键 }//注意添加显示转换
    int clickCout = 1;
    float lastClickTime = 0;
    float clickTime = 0.3f;
    bool IsDoubleClick(int mouse)
    {
        if (Input.GetMouseButtonDown(mouse))
        {
            if ((Time.realtimeSinceStartup - lastClickTime) < clickTime)
            {
                Debug.Log("间隔内点击");
                clickCout += 1;
            }
            else
            {
                clickCout = 1;
                lastClickTime = Time.realtimeSinceStartup;
                Debug.Log("单击");
            }

            if (clickCout == 2)
            {
                Debug.Log("双击");
                return true;
            }
        }
        return false;
    }
}
