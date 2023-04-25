using Manager;
using UnityEngine;
using UnityEngine.UI;

public enum GameState
{
    Start,Intro,Transition
    
}
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
  
    public CameraController cameraController;
    public Character character;
    public GameState gameState;
    [Space]
    public Slider slider;
    public float sliderValue;
    [Space] public GameObject[] StateObject;
    [Space] public GameObject Arm;
    [Space] public RectTransform finger;
    public RectTransform clickFinger;
    public Canvas canvas;
    private Camera cam;
   
    private void Awake()
    { 
        Instance = this;
    }
    private void Update()
    {
        slider.value = sliderValue;
        GameStateFNC();
        FingerMouse();
    }

    private bool isMoved;
    void GameStateFNC()
    {
        switch (gameState)
        {
            case GameState.Start :
                if (!isMoved)
                {
                    foreach (var VARIABLE in StateObject)
                    {
                        VARIABLE.SetActive(true);
                    }
                    isMoved = false;
                }
                ToolManager.Instance.UpdateTool();
                break;
            case GameState.Intro :
                if(!isMoved)
                {
                    cameraController.CameraMoved(3, 1);
                    character.Moved();
                    isMoved = true;
                }
                break;
            case  GameState.Transition:
                if (isMoved)
                {
                    cameraController.CameraMoved(2,2);
                    isMoved = false;
                }
                foreach (var VARIABLE in StateObject)
                {
                    VARIABLE.SetActive(false);
                }
                float x = cameraController.CameraDistance(2);
                if ( x<= 0.1f)
                {
                    Arm.SetActive(false);
                    gameState = GameState.Start;
                }
                break;
        }
    }

    void FingerMouse()
    { cam = null;
        if (canvas.renderMode == RenderMode.ScreenSpaceCamera)
            cam = canvas.worldCamera;
        Vector3 positiion = Input.mousePosition;
        Vector2 position = RectTransformUtility.WorldToScreenPoint(cam, positiion);
        finger.position = position;
        if (Input.GetMouseButton(0))
        {  clickFinger.position = position;
            finger.gameObject.SetActive(false);
            clickFinger.gameObject.SetActive(true);
            
        }
        else if (Input.GetMouseButtonUp(0))
        {
            finger.gameObject.SetActive(true);
            clickFinger.gameObject.SetActive(false);
        }

    }
}
