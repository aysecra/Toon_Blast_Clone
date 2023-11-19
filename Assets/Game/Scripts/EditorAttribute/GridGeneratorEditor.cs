using ToonBlastClone.Components;
using UnityEditor;
using UnityEngine;

namespace ToonBlastClone.Editor
{
#if UNITY_EDITOR
    // [CustomEditor(typeof(GridGenerator), true)]
    // public class GridGeneratorEditor : UnityEditor.Editor
    // {
    //     public override void OnInspectorGUI()
    //     {
    //         base.OnInspectorGUI();
    //         
    //         GridGenerator gridGenerator = (GridGenerator) target;
    //         GUIStyle guiStyle = EditorStyles.miniButtonLeft;
    //         guiStyle.margin = new RectOffset(0, 0, 10, 0);
    //
    //         EditorGUILayout.BeginHorizontal();
    //         {
    //             GUILayout.Space(EditorGUIUtility.labelWidth);
    //
    //             if (GUILayout.Button("Generate", EditorStyles.miniButtonLeft))
    //             {
    //                 gridGenerator.Generate();
    //             }
    //
    //             guiStyle = EditorStyles.miniButtonRight;
    //             guiStyle.margin = new RectOffset(0, 0, 10, 0);
    //
    //             if (GUILayout.Button("Clear", EditorStyles.miniButtonRight))
    //             {
    //                 gridGenerator.ClearCell();
    //             }
    //         }
    //         EditorGUILayout.EndHorizontal();
    //     }
    // }
#endif
}