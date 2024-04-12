using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Scene : MonoBehaviour
{

    public bool startFromBegining;
    [SerializeField]
    public List<Conversation.ConversationData> conversationsMaster;

    public Text nameText;
    public Text dialogueText;
    public PlayerData playerData;
    public string conversationTitle;
    public SpriteRenderer middleSpriteRender;
    public SpriteRenderer leftSpriteRender;
    public SpriteRenderer rightSpriteRender;
    public SpriteRenderer cgSpriteRender;
    public GameObject canvas;
    public GameObject button;
    public string nextSceneName;

    private Queue<Dialogue.DialogueData> dialogueDatum;
    private int conversationIndex;
    private Conversation.ConversationData currentConversation;
    private int dialogueIndex;
    private string overflowString;
    private string overflowName;
    private bool overflow;
    private bool clickingForward;
    private List<GameObject> buttonsToDelete;
    private bool onChoiceScreen;

    
    public Scene()
    {
        buttonsToDelete = new List<GameObject>();
        dialogueDatum = new Queue<Dialogue.DialogueData>();
        conversationIndex = 0;
        dialogueIndex = 0;
        overflow = false;
        clickingForward = true;
        startFromBegining = true;
        onChoiceScreen = false;
        onChoiceScreen = false;
    }
    
    void Start()
    {
        if (PersistantManager.Instance.dialogueIndex == 0)
        {
            StartDialogue(conversationTitle);
        }
        else if (PersistantManager.Instance.onChoiceScreen)
        {
            StartDialogue(PersistantManager.Instance.conversationIndex, PersistantManager.Instance.dialogueIndex);
            EndDialogue(PersistantManager.Instance.choiceList);
        }
        else
        {
            StartDialogue(PersistantManager.Instance.conversationIndex, PersistantManager.Instance.dialogueIndex);
        }

    }

    //---------------------------------------------------- Start Dialogue  ---------------------------------------------------//
    public void StartDialogue(int currentConversationIndex)
    {
        clearButtons();
        StartCoroutine(waitForChoice());
        onChoiceScreen = false;
        PersistantManager.Instance.setOnChoiceScreen(false);
        dialogueDatum.Clear();
        dialogueIndex = 0;
        conversationIndex = currentConversationIndex;
        currentConversation = conversationsMaster[conversationIndex];
        foreach (Dialogue.DialogueData dialogueData in conversationsMaster[conversationIndex].dialogueArray)
        {
            dialogueDatum.Enqueue(dialogueData);
        }

        DisplayNextSentence();
    }

    public void StartDialogue(int currentConversationIndex, int currentDialogueIndex)
    {
        clearButtons();
        startFromBegining = false;
        clickingForward = true;
        dialogueDatum.Clear();
        dialogueIndex = currentDialogueIndex;
        conversationIndex = currentConversationIndex;
        currentConversation = conversationsMaster[conversationIndex];
        foreach (Dialogue.DialogueData dialogueData in conversationsMaster[conversationIndex].dialogueArray)
        {
            dialogueDatum.Enqueue(dialogueData);
        }


        for(int i = 0; i < (currentDialogueIndex-1); i++)
        {
            dialogueDatum.Dequeue();
        }

        DisplayNextSentence();
    }
    public void StartDialogue(string conversationTitle)
    {
        clearButtons();
        dialogueDatum.Clear();
        int i = 0;
        foreach(Conversation.ConversationData conversation in conversationsMaster)
        {
            if (conversation.title.Equals(conversationTitle))
            {
                conversationIndex = i;
                break;
            }

            i++;
        }

        StartDialogue(conversationIndex);
    }

    //---------------------------------------------------- Display Next Sentence  ---------------------------------------------------//
    public void DisplayNextSentence()
    {
        Dialogue.DialogueData dialogueDataCurrent;
        string sentence = "";
        string name = "";
        if (overflow)
        {
            sentence = overflowString;
            name = overflowName;
            overflow = false;
            
        }
        else
        {

            if (dialogueDatum.Count == 0)
            {
                EndDialogue();
                return;
            }
            dialogueDataCurrent = dialogueDatum.Dequeue();
            sentence = dialogueDataCurrent.dialogue;
            name = dialogueDataCurrent.characterName;
            Sprite cgSprite = dialogueDataCurrent.CG;
            Sprite mainSprite = dialogueDataCurrent.sprite;
            Sprite additionalSprite = dialogueDataCurrent.additionalSprite;
            bool mainSpriteHighlighted = dialogueDataCurrent.mainSpriteHighlighted;
            bool additionalSpriteHighlighted = dialogueDataCurrent.additionalSpriteHighlighted;
            
            if (cgSprite != null)
            {
                cgSpriteRender.sprite = cgSprite;
            }
            else
            {
                cgSpriteRender.sprite = null;
            }

            if (additionalSprite == null)
            {
                leftSpriteRender.color = new Color(1f, 1f, 1f, 0f);
                rightSpriteRender.color = new Color(1f, 1f, 1f, 0f);
                middleSpriteRender.sprite = mainSprite;
                if (mainSpriteHighlighted)
                    middleSpriteRender.color = new Color(1f, 1f, 1f, 1f);
                else
                    middleSpriteRender.color = new Color(.5f, .5f, .5f, 1f);

            }
            else
            {
                middleSpriteRender.color = new Color(1f, 1f, 1f, 0f);
                leftSpriteRender.sprite = mainSprite;
                rightSpriteRender.sprite = additionalSprite;
                if (mainSpriteHighlighted)
                {
                    leftSpriteRender.color = new Color(1f, 1f, 1f, 1f);
                    rightSpriteRender.color = new Color(.5f, .5f, .5f, 1f);
                }
                else if (additionalSpriteHighlighted)
                {
                    leftSpriteRender.color = new Color(.5f, .5f, .5f, 1f);
                    rightSpriteRender.color = new Color(1f, 1f, 1f, 1f);
                }
                else
                {
                    leftSpriteRender.color = new Color(.5f, .5f, .5f, 1f);
                    rightSpriteRender.color = new Color(.5f, .5f, .5f, 1f);
                }
            }
            dialogueIndex++;
        }

        if (sentence.ToCharArray().Length > 180)
        {
            int index = sentence.IndexOf(' ', 180);
            if (index > 0)
            {
                overflowString = sentence.Substring(index + 1);
                overflowName = name;
                overflow = true;
                sentence = sentence.Substring(0, index);
            }

            
            
        }

        nameText.text = name;
        dialogueText.text = sentence;
        StopAllCoroutines();
        StartCoroutine(TypeSetence(sentence));

    }

    //---------------------------------------------------- End Dialogue  ---------------------------------------------------//
    public void EndDialogue()
    {
        clickingForward = false;

        if (conversationsMaster[conversationIndex].choicesMaster.choices.Length != 0)
        {
            int i = 0;
            onChoiceScreen = true;
            foreach (Choice.ChoiceData choice in conversationsMaster[conversationIndex].choicesMaster.choices)
            {
                GameObject newButton = Instantiate(button) as GameObject;
                Button buttonComponent = newButton.GetComponentInChildren<Button>();
                newButton.transform.SetParent(canvas.transform, false);
                newButton.GetComponentInChildren<Text>().text = choice.choice;
                Vector3 pos = newButton.transform.position;
                pos.y += (1.5f*i);
                newButton.transform.position = pos;
                buttonComponent.onClick.AddListener(() => StartDialogue(choice.startConversationTitle));
                buttonComponent.onClick.AddListener(() => playerData.addTags(choice.tagsToAdd));
                buttonComponent.onClick.AddListener(() => playerData.removeTags(choice.tagsToRemove));
                buttonsToDelete.Add(newButton);

                i++;
            }
            return;
        }
        else if (!(conversationsMaster[conversationIndex].nextConversationTitle.Equals("")))
        {
            int conversationDefault = 0;
            int i = 0;
           foreach(Conversation.ConversationData conversation in conversationsMaster)
            {
                if (conversation.title.Equals(conversationsMaster[conversationIndex].nextConversationTitle))
                {
                    if (playerData.compareTags(conversation))
                    {
                        StartDialogue(i);
                        return;
                    }
                    else if (conversation.tags.ToLower().Equals("default"))
                    {
                        conversationDefault = i;
                    }
                }
                i++;
            }
            StartDialogue(conversationDefault);
            return;
        }
        else
        {
            SceneManager.LoadScene(nextSceneName, LoadSceneMode.Single);
            PersistantManager.Instance.setConversationIndex(0);
            PersistantManager.Instance.setDialogueIndex(0);
        }
    }

    public void EndDialogue(List<Choice.ChoiceData> choiceArray)
    {
        clickingForward = false;
        int i = 0;
        onChoiceScreen = true;
        foreach (Choice.ChoiceData choice in choiceArray)
        {
            GameObject newButton = Instantiate(button) as GameObject;
            Button buttonComponent = newButton.GetComponentInChildren<Button>();
            newButton.transform.SetParent(canvas.transform, false);
            newButton.GetComponentInChildren<Text>().text = choice.choice;
            Vector3 pos = newButton.transform.position;
            pos.y += (1.5f * i);
            newButton.transform.position = pos;
            buttonComponent.onClick.AddListener(() => StartDialogue(choice.startConversationTitle));
            buttonComponent.onClick.AddListener(() => playerData.addTags(choice.tagsToAdd));
            buttonComponent.onClick.AddListener(() => playerData.removeTags(choice.tagsToRemove));
            buttonsToDelete.Add(newButton);

            i++;
        }
        
    }

        //---------------------------------------------------- Co-Routines  ---------------------------------------------------//
        IEnumerator TypeSetence (string sentence)
    {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return null;
        }
    }

    IEnumerator waitForChoice()
    {
        yield return new WaitForSeconds(5);
        clickingForward = true;
    }


    //---------------------------------------------------- Clear all Buttons  ---------------------------------------------------//
    public void clearButtons()
    {
        foreach(GameObject button in buttonsToDelete)
        {
            Destroy(button);
        }

        buttonsToDelete.Clear();
    }

    //---------------------------------------------------- Getters and Setters  ---------------------------------------------------//

    public int getConversationIndex()
    {
        return conversationIndex;
    }

    public int getDialogueIndex()
    {
        return dialogueIndex;
    }

    public bool getOnChoiceScreen()
    {
        return onChoiceScreen;
    }

    public void setClickingFoward(bool temp)
    {
        clickingForward = temp;
    }

    //---------------------------------------------------- Save and Load Scene Data  ---------------------------------------------------//

    public void SaveSceneData()
    {
        SaveSystem.Save(this);
    }

    public void LoadSceneData()
    {
        SaveData data = SaveSystem.LoadScene();
        PersistantManager.Instance.setDialogueIndex(data.dialogueIndex);
        PersistantManager.Instance.setConversationIndex(data.conversationIndex);

        if (data.onChoiceScreen)
        {
            onChoiceScreen = true;
            PersistantManager.Instance.setOnChoiceScreen(true);
            int i = 0;
            List<Choice.ChoiceData> choiceArray = new List<Choice.ChoiceData>();
            foreach(string choice in data.choicesArray)
            {
                Choice.ChoiceData choiceData = new Choice.ChoiceData(choice, data.startConversationTitlesArray[i], data.tagsToAddArray[i], data.tagsToRemoveArray[i]);
                choiceArray.Add(choiceData);
                i++;
            }
            PersistantManager.Instance.setChoiceList(choiceArray);
        }
    }

    //---------------------------------------------------- Update  ---------------------------------------------------//
    void Update()
    {
        if (Input.GetMouseButtonUp(0) && clickingForward)
        {
            DisplayNextSentence();
        }
    }

    
}
