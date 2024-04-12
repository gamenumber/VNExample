using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dialogue : MonoBehaviour
{
    [System.Serializable]
    public class DialogueData
    {
        
        public string characterName;
        [TextArea(2,8)]
        public string dialogue;

        [SerializeField]
        public Sprite sprite;
        public Sprite additionalSprite;
        public Sprite CG;
        public bool mainSpriteHighlighted;
        public bool additionalSpriteHighlighted;

        public void Start()
        {
            
        }

    }
    
}
