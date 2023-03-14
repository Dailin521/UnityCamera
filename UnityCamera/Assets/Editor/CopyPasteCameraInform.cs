using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CopyPasteCameraInform : MonoBehaviour
{
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
