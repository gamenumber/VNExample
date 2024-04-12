using UnityEngine;
using UnityEditor;

public class SceneEditorWindow : EditorWindow
{
    [MenuItem("Window/Scene Editor")]
    public static void ShowWindow()
    {
        GetWindow<SceneEditorWindow>("Scene Editor");
    }
    private void OnGUI()
    {
        
    }
}
