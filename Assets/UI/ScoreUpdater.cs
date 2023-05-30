using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreUpdater : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<TMP_Text>().text 
            = GameObject.Find("GameController").GetComponent<GameController>().Score.ToString();
    }

    private void OnEnable()
    {
        GetComponent<TMP_Text>().text
            = GameObject.Find("GameController").GetComponent<GameController>().Score.ToString();
    }
}
