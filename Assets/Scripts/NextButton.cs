using System.Collections;
using System.Collections.Generic;
using Manager;
using UnityEngine;

public class NextButton : MonoBehaviour
{   
    public RectTransform glowNextbutton;

    // Update is called once per frame
    void Update()
    {
        GlowNexbuttonTurnUp();
    }
    void GlowNexbuttonTurnUp()
    {
        glowNextbutton.Rotate(new Vector3(0,0,Time.deltaTime*30));
    }

   public void NextClik()
    {
        switch (ToolManager.Instance.stateScene)
            {
                
                case StateScene.Default :
                
                    MousePainter.Instance.UpdatePainter();
                    break;
                case StateScene.Cleaner :
              
                    ToolManager.Instance.stateScene=StateScene.Combining;
                    break;
                case StateScene.Combining :
                    ToolManager.Instance.stateScene=StateScene.RepairingSaStateScene;
                    break;
                case StateScene.RepairingSaStateScene :
                    ToolManager.Instance.stateScene=StateScene.PartPainter;
                    break;
                case  StateScene.PartPainter:
                    
                    break;
            }
    }
}
