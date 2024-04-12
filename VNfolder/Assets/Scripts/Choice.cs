using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Choice : ScriptableObject
{
    [System.Serializable]
    public class ChoiceData
    {
        [SerializeField]
        public string choice;
        public string startConversationTitle;
        public string tagsToAdd;
        public string tagsToRemove;

        public ChoiceData(string currentChoice, string currentStartCoversationTitle, string currentTagsToAdd, string currentTagsToRemove)
        {
            choice = currentChoice;
            startConversationTitle = currentStartCoversationTitle;
            tagsToAdd = currentTagsToAdd;
            tagsToRemove = currentTagsToRemove;
        }

    }

}
