using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class SaveData
{
    public string[] playerTags;
    public string unityScene;
    public int conversationIndex;
    public int dialogueIndex;

    public bool onChoiceScreen;
    public string[] choicesArray;
    public string[] startConversationTitlesArray;
    public string[] tagsToAddArray;
    public string[] tagsToRemoveArray;

    private List<string> choicesList;
    private List<string> startConversationTitlesList;
    private List<string> tagsToAddList;
    private List<string> tagsToRemoveList;
    
    public SaveData (PlayerData playerData)
    {
        playerTags = playerData.tags.ToArray();
        unityScene = SceneManager.GetActiveScene().name;
    }

    public SaveData (Scene scene)
    {
        conversationIndex = scene.getConversationIndex();
        dialogueIndex = scene.getDialogueIndex();

        choicesList = new List<string>();
        startConversationTitlesList = new List<string>();
        tagsToAddList = new List<string>();
        tagsToRemoveList = new List<string>();

        if (scene.getOnChoiceScreen())
        {
            int i = 0;
            onChoiceScreen = true;
            foreach (Choice.ChoiceData choice in scene.conversationsMaster[conversationIndex].choicesMaster.choices)
            {
                choicesList.Add(choice.choice);
                startConversationTitlesList.Add(choice.startConversationTitle);
                tagsToAddList.Add(choice.tagsToAdd);
                tagsToRemoveList.Add(choice.tagsToRemove);

                i++;
            }

            choicesArray = choicesList.ToArray();
            startConversationTitlesArray = startConversationTitlesList.ToArray();
            tagsToRemoveArray = tagsToRemoveList.ToArray();
            tagsToAddArray = tagsToAddList.ToArray();
        }
    }

}
