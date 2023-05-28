using Cinemachine;
using UnityEngine;

public class GameController : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject generator;
    public GameObject pauseScreen;
    public CinemachineVirtualCamera camera;
    public Texture2D cursorTexture;

    void Start()
    {
        SetCursor();
        var player = GameObject.FindWithTag("Player").transform;
        camera.Follow = player.transform;
        camera.LookAt = player.transform;
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
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            pauseScreen.SetActive(true);
        }
    }

    public void StartNewGame()
    {
        generator.GetComponent<MapCreator>().CreateMap();
    }
}
