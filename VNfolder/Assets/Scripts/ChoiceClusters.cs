using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoiceClusters : ScriptableObject
{
    [System.Serializable]
    public class ChoiceClustersData
    {
        [SerializeField]
        public Choice.ChoiceData[] choices;
    }
}
