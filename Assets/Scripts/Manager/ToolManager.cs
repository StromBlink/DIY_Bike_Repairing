using Controller;
using UnityEngine;

namespace Manager
{
    public enum StateScene
    {
        Default,
        Cleaner,
        RepairingSaStateScene,
        Combining,
        Grinding,
        PartPainter,
        Sticker,
        Final
    
    }
    public class ToolManager : MonoBehaviour
    {   enum MyEnum
        {
            statStart,
            statEnd, 
        }

        public static ToolManager Instance;
        private MyEnum _myEnum;
        public StateScene stateScene;
        public CameraController cameraController;
        public GameManager gameManager;
        public UIManager uIManager;
        public PaintController paintCOntroller;
        [Space] 
        public GameObject cleaner;
        public GameObject combining;
        public GameObject repairing;
        public GameObject partPainter;
        
        private void Awake()
        {
            Instance = this;
        }

        public void UpdateTool()
        {
            WhichState();
        }

        public void SetState(int x)
        {
            switch (x)
            {
                case 0:
                    stateScene = StateScene.RepairingSaStateScene;
                    break;
                case 1:
                    stateScene = StateScene.Grinding;
                    break;
            }
        }  
        void WhichState()
        {
        
            switch (stateScene)
            {
                
                case StateScene.Default :
                
                    MousePainter.Instance.UpdatePainter();
                    break;
                case StateScene.Cleaner :
              
                    if(MyEnum.statStart==_myEnum)
                    {   cleaner.SetActive(true);
                        uIManager.CleanerSetIcons();
                        gameManager.sliderValue = 0;
                        uIManager.cleanerUI.SetActive(true);
                        _myEnum = MyEnum.statEnd;
                    }
                    CleanerStateController.Instance.UpdateCleanerController();
                    break;
                case StateScene.Combining :
                    if(MyEnum.statEnd==_myEnum)
                    {   combining.SetActive(true);
                        uIManager.SetIcon(1,0);
                        gameManager.sliderValue = 0;
                        cameraController.CameraMoved(5,2);
                        _myEnum = MyEnum.statStart;
                    }
                    CombiningStateController.Instance.UpdateCombining();
                    break;
                case StateScene.RepairingSaStateScene :
                    if (_myEnum == MyEnum.statStart)
                    {   repairing.SetActive(true);
                        uIManager.SetIcon(6,0);
                        gameManager.sliderValue = 0;
                        _myEnum = MyEnum.statEnd;
                        cameraController.CameraMoved(0,2);
                    }
                    RepairingStateController.Instance.UpdateRepairing();
                    break;
                case  StateScene.PartPainter:
                    if (_myEnum == MyEnum.statEnd)
                    { 
                        partPainter.SetActive(true);
                        uIManager.SetIcon(5,0);
                        gameManager.sliderValue = 0;
                        uIManager.paintingUI.SetActive(true);
                        cameraController.CameraMoved(1,2);
                        _myEnum = MyEnum.statStart;
                    }
                    paintCOntroller.Painting();
                    break;
                case  StateScene.Sticker:
                    
                    UIManager.Instance.StickerUI.SetActive(true);
                    paintCOntroller.StickerPuck();
                    break;
                case  StateScene.Final:
                    
                    
                    break;
            }
        } 
    }
}