using UnityEngine;

namespace PainterControllers
{
    public class CollisionPainter : MonoBehaviour{
        public Color paintColor;
    
        public float radius = 1;
        public float strength = 1;
        public float hardness = 1;

        private void OnCollisionStay(Collision other) {
            Paintable p = other.collider.GetComponent<Paintable>();
            if(p != null){
                Vector3 pos = other.contacts[0].point;
                PaintManager.instance.paint(p, pos, radius, hardness, strength, paintColor);
            }
        }

        private void OnTriggerStay(Collider other)
        {
            Paintable p = other.GetComponent<Paintable>();
            if(p != null){
           
                Vector3 pos =other.ClosestPoint(transform.position);
                PaintManager.instance.paint(p, pos, radius, hardness, strength, paintColor);
            }
        }
    }
}
