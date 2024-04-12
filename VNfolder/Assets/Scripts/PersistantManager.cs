using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistantManager : MonoBehaviour
{
    public static PersistantManager Instance { get; private set; }
    public List<string> tags;
    
    public int conversationIndex;
    public int dialogueIndex;


    public bool onChoiceScreen;
    public List<Choice.ChoiceData> choiceList;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void setConversationIndex(int temp)
    {
        conversationIndex = temp;
    }

    public void setDialogueIndex(int temp)
    {
        dialogueIndex = temp;
    }

    public void setTags(List<string> temp)
    {
        tags = temp;
    }

    public void setChoiceList(List<Choice.ChoiceData> temp)
    {
        choiceList = temp;
    }

    public void setOnChoiceScreen(bool temp)
    {
        onChoiceScreen = temp;
    }

}
