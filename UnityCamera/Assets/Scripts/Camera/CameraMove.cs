using Cinemachine;
using UnityEngine;

public class CameraMove : MonoBehaviour
{

    private float xMove;
    private float yMove;
    Vector3 moveDir;
    [SerializeField] Vector3 inputDir;
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
    }
    void HandleCameraMove()
    {
        xMove = Input.GetAxis("Horizontal");
        yMove = Input.GetAxis("Vertical");
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
        if (isEdgeScrolling)
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
    [SerializeField] float taregtRotateY = 10;
    float followY = 45;
    void CameraRotate()
    {
        if (Input.GetMouseButton(1))
        {
            transform.localEulerAngles += new Vector3(0, Input.GetAxis("Mouse X") * rotateSpeed, 0);
            followOffest = virtualCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset;
            taregtRotateY = followOffest.y;
            followY += Input.GetAxis("Mouse Y");
            followY = Mathf.Clamp(followY, 15, 60);
            virtualCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset = new Vector3(followOffest.x, -followOffest.z * (Mathf.Tan(Mathf.Deg2Rad * followY)), followOffest.z);
        }
    }
    private Vector3 followOffest;
    [SerializeField] float maxScrollSize = 50;
    [SerializeField] float minScrollSize = 5;
    float currentScrollSize = 10;
    [Header("缩放平滑")]
    [SerializeField] private float scrollDamping = 10;
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
        virtualCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset = followOffest.normalized * currentScrollSize;
    }
    void InitScrollSize()
    {

        Vector3 temp = virtualCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset;
        taregtScrollSize = temp.y / temp.normalized.y;
        currentScrollSize = taregtScrollSize;
    }
}
