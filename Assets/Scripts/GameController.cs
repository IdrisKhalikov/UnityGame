using System;
using Cinemachine;
using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using MoreMountains.TopDownEngine;
using TMPro;
using UnityEditor;
using UnityEngine;

namespace GameController
{
    public class GameController : MonoBehaviour

    {
    // Start is called before the first frame update
    public GameObject generator;
    public GameObject pauseScreen;
    public GameObject loseScreen;
    public GameObject levelCompleteScreeen;
    public CinemachineVirtualCamera camera;
    public Texture2D cursorTexture;
    public int Score;
    public int Level;
    private int enemyCounter;
    public TMP_Text textMesh;
    public bool IsPlaying;
    [SerializeField] GameObject Player;
    public static GameObject PlayerCharacter;

    void Start()
    {
        SetCursor();
        PlayerCharacter = Player;
    }

    private void SetCursor()
    {
        var offset = new Vector2(cursorTexture.width / 2, cursorTexture.height / 2);
        Cursor.SetCursor(cursorTexture, offset, CursorMode.Auto);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && IsPlaying)
        {
            pauseScreen.SetActive(true);
            IsPlaying = false;
        }
    }

    public void StartNewGame()
    {
        Score = 0;
        Level = 1;
        textMesh.text = Score.ToString();
        CreateNewMap();
    }

    public void CreateNewLevel()
    {
        Level++;
        CreateNewMap();
    }

    private void CreateNewMap()
    {
        IsPlaying = true;
        enemyCounter = Level * 10;
        var startPos = generator.GetComponent<MapCreator>().CreateMap(Level);
        DestroyImmediate(GameObject.FindWithTag("Player"));
        var newPlayer = Instantiate(Player, startPos, Quaternion.Euler(0, 0, 0));
        newPlayer.GetComponent<Character>().Reset();
        PlayerCharacter = newPlayer;
        SetCameraFollow(newPlayer);
        newPlayer.GetComponent<Character>()._isStart = false;
    }

    private void SetCameraFollow(GameObject followObject)
    {
        camera.Follow = null;
        camera.LookAt = null;
        camera.Follow = followObject.transform;
        camera.LookAt = followObject.transform;
    }

    public void DestroyEvent(GameObject gameObject)
    {
        if (gameObject.tag == "Player")
        {
            loseScreen.SetActive(true);
            IsPlaying = false;
        }

        if (gameObject.tag == "Enemy")
        {
            Score += gameObject.GetComponent<Health>().PointsWhenDestroyed;
            textMesh.text = Score.ToString();
            enemyCounter--;
            if (enemyCounter == 0)
            {
                Level++;
                levelCompleteScreeen.SetActive(true);
                IsPlaying = false;
            }
        }
    }

    public void SetUnPause()
    {
        TopDownEngineEvent.Trigger(TopDownEngineEventTypes.UnPause, null);
    }
    }
}
