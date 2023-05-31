using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameController;

public class OnDestroyEvent : MonoBehaviour
{
    public void SendEvent()
    {
        GameObject.Find("GameController").GetComponentInChildren<GameController.GameController>().DestroyEvent(gameObject);
    }
}
