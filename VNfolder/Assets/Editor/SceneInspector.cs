using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Scene))]
public class SceneInspector : Editor
{

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        Scene scene = (Scene)target;
        
        if (GUILayout.Button("Write Dialogue from File"))
        {
            foreach (Conversation.ConversationData conversation in scene.conversationsMaster)
            {
                conversation.WriteDialogue();
            }
        }
    }
}
