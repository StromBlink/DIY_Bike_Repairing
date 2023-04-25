 
using System;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Rendering;

public class CameraController : MonoBehaviour

{
    [Serializable]
   public struct MyStruct
    {
        public string explanation;
        public Vector3 cameraPosition;
        public Vector4 cameraRotation;
    }
    
    public MyStruct[] MyStructs;
    

    public void CameraMoved(Index x,float time)
    {
        
            transform.DOMove(MyStructs[x].cameraPosition, time);

            Quaternion quaternion = new Quaternion(MyStructs[x].cameraRotation.x, MyStructs[x].cameraRotation.y,
                MyStructs[x].cameraRotation.z, MyStructs[x].cameraRotation.w);
            transform.DORotateQuaternion(quaternion, time);
    }

    public float CameraDistance(Index x)
    {
        float distance = Vector3.Distance(transform.position, MyStructs[x].cameraPosition);
        return distance;
    }

    public void ClickAcceptAnimationCatchBox(float time)
    {
        CameraMoved(4,time);
    }

}
