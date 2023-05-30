using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnDestroyEvent : MonoBehaviour
{
    public void SendEvent()
    {
        GameObject.Find("GameController").GetComponentInChildren<GameController>().DestroyEvent(gameObject);
    }
}
