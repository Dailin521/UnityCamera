using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CameraMove), true)] //特性来描述要自定义的是哪个类, 第二个参数代表是否对其子类起效.
public class CameraMoveEditor : Editor
{
    private CameraMove m_target;
    private SerializedProperty m_isEdgeScrolling;
    private SerializedProperty m_edgeScroSize;
    private SerializedProperty m_edgeScroMove;
    private SerializedProperty m_isEditYAngle;

    private void OnEnable()
    {
        m_target = target as CameraMove;
        m_isEdgeScrolling = serializedObject.FindProperty("isEdgeScrolling");
        m_edgeScroSize = serializedObject.FindProperty("edgeScrollSize");
        m_edgeScroMove = serializedObject.FindProperty("edgeMoveSpeed");

    }
    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();  //默认绘制GUI

        serializedObject.Update();
        //......//

        var curValue = m_isEdgeScrolling.boolValue;
        m_isEdgeScrolling.boolValue = EditorGUILayout.Toggle("是否边缘滚动", m_isEdgeScrolling.boolValue);
        if (m_isEdgeScrolling.boolValue)
        {
            EditorGUILayout.PropertyField(m_edgeScroSize);
            EditorGUILayout.PropertyField(m_edgeScroMove);
        }
        GUI.enabled = false;
        EditorGUILayout.PropertyField(serializedObject.FindProperty("moveSpeed"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("rotateSpeed"));
        GUI.enabled = true;
        EditorGUILayout.PropertyField(serializedObject.FindProperty("followYAngle"));
        EditorGUILayout.Space(5);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("maxScrollSize"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("minScrollSize"));
        EditorGUILayout.Space(5);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("taregtScrollSize"));
        serializedObject.ApplyModifiedProperties();
    }
}
