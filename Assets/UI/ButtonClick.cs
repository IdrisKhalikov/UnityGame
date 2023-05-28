using Cinemachine;
using MoreMountains.TopDownEngine;
using TMPro;
using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonClick : MonoBehaviour
{
    [SerializeField] private static Sprite defaultState;
    [SerializeField] private static  Sprite pressedState;
    [SerializeField] private static TMP_FontAsset white;
    [SerializeField] private static TMP_FontAsset black;
    [SerializeField] public GameController gameController;
    private static GameObject previous;
    private static GameObject next;

    public void Awake()
    {
        white = Resources.Load<TMP_FontAsset>("Fonts/WhiteFont");
        black = Resources.Load<TMP_FontAsset>("Fonts/BlackFont");
        pressedState = Resources.Load<Sprite>("UI/pressed");
        defaultState = Resources.Load<Sprite>("UI/defaultState");
    }

    public void SetButtonState(bool pressed, GameObject button)
    {
        button.GetComponentInChildren<TMP_Text>().font = pressed ? white : black;
        button.GetComponent<SVGImage>().sprite = pressed ? pressedState : defaultState;
    }

    public void OnPointerEnter(BaseEventData data)
    {
        var sender = (data as PointerEventData).pointerEnter;
        SetButtonState(true, sender);
    }

    public void OnPointerExit(BaseEventData data)
    {
        var sender = (data as PointerEventData).pointerEnter;
        SetButtonState(false, sender);
    }

    public void ReturnBack(BaseEventData data)
    {
        var sender = (data as PointerEventData).pointerEnter;
        var menu = sender.transform.root.gameObject;
        previous.SetActive(true);
        previous = menu;
        menu.SetActive(false);
    }

    public void Change(BaseEventData data)
    {
        var sender = (data as PointerEventData).pointerClick;
        var menu = sender.transform.root.gameObject;
        next.SetActive(true);
        previous = menu;
        SetButtonState(false, sender);
        menu.SetActive(false);
    }

    public void SetNext(GameObject nextMenu)
    {
        next = nextMenu;
    }

    public void StartGame(BaseEventData data)
    {
        var sender = (data as PointerEventData).pointerClick;
        var menu = sender.transform.root.gameObject;
        menu.SetActive(false);
        gameController.StartNewGame();
    }

    public void ContinueGame(BaseEventData data)
    {
        var sender = (data as PointerEventData).pointerClick;
        var menu = sender.transform.root.gameObject;
        menu.SetActive(false);
    }

    public void ExitApplication()
    {
        Application.Quit(0);
    }
}
