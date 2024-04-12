using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.IO;

public class Conversation : MonoBehaviour

{
    [System.Serializable]
    public class ConversationData
    {
        [SerializeField]
        public string title;
        public string tags;
        public TextAsset script;
        public List<Dialogue.DialogueData> dialogueArray;
        public ChoiceClusters.ChoiceClustersData choicesMaster;
        public string nextConversationTitle;

        public ConversationData()
        {
            tags = "Default";
        }
        
        public void WriteDialogue()
        {
            if (!script)
            {
                return;
            }

            if (dialogueArray.Count != 0)
                return;

            string text = script.text;
            string[] splitArray = text.Split(char.Parse("\n"));



            Dialogue.DialogueData temp = new Dialogue.DialogueData();
            int i = 0;
            foreach(string line in splitArray)
            {
                if (i == 0 || i % 2 == 0)
                    temp.characterName = line;
                else
                {
                    temp.dialogue = line;
                    dialogueArray.Add(temp);
                    temp = new Dialogue.DialogueData();
                }

                i++;
            }

        }


    }
    


}
