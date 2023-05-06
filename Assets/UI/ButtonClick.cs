using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class ButtonClick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{

    [SerializeField] private SVGImage background;
    [SerializeField] private Sprite defaultState;
    [SerializeField] private Sprite pressed;
    [SerializeField] private TMP_Text textField;
    [SerializeField] private TMP_FontAsset white;
    [SerializeField] private TMP_FontAsset black;
    
    public void OnPointerDown(PointerEventData eventData)
    {
        background.sprite = pressed;
        textField.font = white;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        background.sprite = defaultState;
        textField.font = black;
    }
}
