using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroy : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        GameObject player = GameObject.FindGameObjectWithTag("player");

        DontDestroyOnLoad(this.gameObject);
    }
}
