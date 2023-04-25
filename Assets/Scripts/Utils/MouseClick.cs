 
using UnityEngine;

namespace Utils
{
    public class MouseClick : MonoBehaviour
    {
        
    
        public Camera cam;
        [Space]
        public bool mouseSingleClick;
        [Space]
        public Color paintColor;
    
        public float radius = 1;
        public float strength = 1;
        public float hardness = 1;
    
        public   bool click;
    
        public  Vector3 HitPosition{ private set; get; }
        private Vector3 startPoint, lastPoint;
 
        public Transform Painter()
        {   
            click = mouseSingleClick ? Input.GetMouseButtonDown(0) : Input.GetMouseButton(0);
            if (click)
            {
                Vector3 position = Input.mousePosition;
                Ray ray = cam.ScreenPointToRay(position);
                RaycastHit hit;
               
                if (Physics.Raycast(ray, out hit, 100.0f))
                {
                    Debug.DrawRay(ray.origin, hit.point - ray.origin, Color.red);
                    HitPosition = hit.point;
                    startPoint = HitPosition;
                    if (hit.transform.CompareTag("Untagged")) return hit.transform;
                    Paintable p = hit.collider.GetComponent<Paintable>();
                    if (p != null)
                    {
                        
                        PaintManager.instance.paint(p, hit.point, radius, hardness, strength, paintColor);
                    }

                    if (startPoint != lastPoint)
                    {
                        GameManager.Instance.sliderValue += 0.003f;
                        lastPoint = startPoint;
                    }
                    return hit.transform;
                }
            }
            return null;
        }

    
    }
}
