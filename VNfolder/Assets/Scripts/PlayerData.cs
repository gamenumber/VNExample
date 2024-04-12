using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using System.Linq;

public class PlayerData : MonoBehaviour
{
    public List<string> tags;

    void Start()
    {
        tags = PersistantManager.Instance.tags;
        
    }

    public void addTags(string tagsToAddString)
    {
        string[] tagsToAdd = tagsToAddString.Split(',');
        foreach (string tag in tagsToAdd)
            tags.Add(tag);
    }

    public void removeTags(string tagsToRemoveString)
    {
        string[] tagsToRemove = tagsToRemoveString.Split(',');
        foreach (string tag in tagsToRemove)
            tags.Remove(tag);
    }

    public bool compareTags(Conversation.ConversationData conversation)
    {
        string[] tagsToSearchFor = conversation.tags.Split(',');
        foreach(string tagToSearchFor in tagsToSearchFor)
        {
            if (!tags.Contains(tagToSearchFor))
                return false;
        }
        return true;
    }

    public void SavePlayerData()
    {
        SaveSystem.Save(this);
    }

    public void LoadPlayerData()
    {
        SaveData data = SaveSystem.LoadPlayer();

        SceneManager.LoadScene(data.unityScene);
        tags = data.playerTags.ToList();
        PersistantManager.Instance.setTags(data.playerTags.ToList());
    }


}

