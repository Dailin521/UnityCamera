using UnityEditor;
using UnityEngine;

public class CopyPasteCameraInform : MonoBehaviour
{
    [MenuItem("GameObject/移动相机到该位置")]
    static void CopyPathObj2()
    {
        Transform trans = Selection.activeTransform;
        if (null == trans) return;
        Transform cam = FindObjectOfType<CameraMove>().transform;
        if (null == cam) return;
        cam.transform.position = trans.position;
    }
    [MenuItem("GameObject/复制并粘贴相机参数信息")]
    static void CopyPathObj()
    {
        Transform trans = Selection.activeTransform;
        if (null == trans) return;
        CameraInitInform inform = trans.GetComponent<CameraInitInform>();
        if (null == inform) return;
        CameraMove cameraMove = GameObject.FindObjectOfType<CameraMove>();
        inform.X_Rotate = cameraMove.transform.position.y;
        inform.Y_Rotate = cameraMove.followYAngle;
        inform.Scroll = cameraMove.CameraScrollSize;
    }
}
