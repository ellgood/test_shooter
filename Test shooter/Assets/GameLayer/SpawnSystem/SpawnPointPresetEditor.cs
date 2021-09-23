using UnityEditor;
using UnityEngine;

namespace GameLayer.SpawnSystem
{
#if UNITY_EDITOR
    [CustomEditor(typeof(SpawnPointsPreset))]
    public class SpawnPointPresetEditor: Editor
    {
        private SerializedProperty _spawnPoints;

        private void OnSceneGUI()
        {
            serializedObject.Update();
            
            EditorGUI.BeginChangeCheck();
            for (var i = 0; i < _spawnPoints.arraySize; i++)
            {
                SerializedProperty element = _spawnPoints.GetArrayElementAtIndex(i);
                SerializedProperty positionProp = element.FindPropertyRelative("position");
                SerializedProperty rotationProp = element.FindPropertyRelative("rotation");

                Handles.color = Color.yellow;
               
                Handles.DrawWireDisc(positionProp.vector3Value, Vector3.up, 0.5f);
                
                Handles.DrawLine(positionProp.vector3Value, positionProp.vector3Value + Quaternion.Euler(0f, rotationProp.floatValue, 0f) * Vector3.forward);
                Handles.Label(positionProp.vector3Value + Vector3.up*2.5f, $"{i}");
                Quaternion newRotation = Handles.RotationHandle(Quaternion.Euler(0, rotationProp.floatValue, 0), positionProp.vector3Value);

                rotationProp.floatValue = newRotation.eulerAngles.y;
                
                positionProp.vector3Value = Handles.PositionHandle(positionProp.vector3Value, Quaternion.identity);
            }

            if (EditorGUI.EndChangeCheck())
            {
                serializedObject.ApplyModifiedProperties();
            }
        }

        private void OnEnable()
        {
            _spawnPoints = serializedObject.FindProperty("spawnPoints");
        }
    }
#endif
}