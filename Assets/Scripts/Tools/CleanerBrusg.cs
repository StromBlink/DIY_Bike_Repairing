using UnityEngine;
using UnityEngine.Serialization;
using Utils;

namespace Tools
{
    public class CleanerBrusg : MouseClick
    {
        public static CleanerBrusg Instance;

        [Space] 
    
        public bool isWaterHose;
        [Space]
        public Transform useIt;
        public Transform cleanerBrush;
        public Transform waterHose;
        public ParticleSystem mudparticuleEffect;
        private void Awake()
        {
            Instance = this;
        }
 
        public void UpdateCleaner()
        { 
            GrindingCleaner();
        }
    
        void GrindingCleaner()
        {   Transform hitpointTransform = Painter();
            if(hitpointTransform==null)return;
            useIt.position = HitPosition;
            Block();
            useIt.LookAt(HitPosition);
            if (!hitpointTransform.CompareTag("Bikepart"))
            {
                mudparticuleEffect.Stop();
            }
            if (hitpointTransform.CompareTag("Bikepart"))
            {
                mudparticuleEffect.Play();
            }
            else if(Input.GetMouseButtonUp(0))  mudparticuleEffect.Stop();
        }

        void Block()
        {   if(!isWaterHose) return;
            Vector3 position = useIt.position;
            float x = position.x;
            x = Mathf.Clamp(x, -50, 10f);
            position = new Vector3(x, 6f, position.z);
            useIt.position = position;
        }

        public void IsDone()
        {
            useIt = waterHose;
            waterHose.gameObject.SetActive(true);
            cleanerBrush.gameObject.SetActive(false);
            isWaterHose = true;
        }

        public void End()
        { waterHose.gameObject.SetActive(false);
            cleanerBrush.gameObject.SetActive(false);
       
        }
    }
}
