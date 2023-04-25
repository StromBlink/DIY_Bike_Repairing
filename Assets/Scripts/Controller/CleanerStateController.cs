using System;
using DG.Tweening;
using Manager;
using Tools;
using UnityEngine;
using UnityEngine.Serialization;

namespace Controller
{
    public  enum MyEnum
    {   BoxOpened,
        BoxClosed,
        Brush,
        Wash
    }
    public class CleanerStateController : MonoBehaviour
    {
        public static CleanerStateController Instance;
        public ParticleSystem confetti;
        public GameObject bikeRuby;
        public MeshRenderer[] bikeParts;
        public MeshRenderer[] wheelParts;
        public Material mudReadyMaterial;
        public Material washReadyMaterial;
        public Material weldingReadyMaterial;
        public Material paintReadyMaterial;
        public Material wheelMaterial;
        public Material wheelCleanMaterial;
        public MyEnum myEnum;
        private float _fillDone;

        private void Awake()
        {
            Instance = this;
        }

        public void UpdateCleanerController()
        {
            _fillDone = GameManager.Instance.sliderValue;
            switch (_fillDone)
            {
                case >= 1 :
                { switch (myEnum)
                    {
                        case MyEnum.BoxClosed:
                            
                            Confetti();            
                            UIManager.Instance.SetIcon(1,0);
                          
                            CleanerBrusg.Instance.cleanerBrush.gameObject.SetActive(true);
                            CleanerBrusg.Instance.useIt = CleanerBrusg.Instance.cleanerBrush;
                            GameManager.Instance.sliderValue = 0;
                            Box.Instance.CamMoved();
                            myEnum = MyEnum.BoxOpened;
                            break;
                    
                        case MyEnum.BoxOpened:
                            foreach (var varıable in bikeParts)
                            {
                                varıable.material = mudReadyMaterial;
                                if (varıable.TryGetComponent<Paintable>(out Paintable paintable ))
                                   paintable.Start();
                                else
                                    varıable.gameObject.AddComponent<Paintable>();
                                varıable.tag = "Bikepart";
                                 


                            }
                            
                            Confetti();        
                            UIManager.Instance.SetIcon(2,0);
                            UIManager.Instance.SliderDomove(1);
                            Box.Instance.CameraFinalMoved();
                            GameManager.Instance.sliderValue = 0;
                            myEnum = MyEnum.Brush;
                            break;
                    
                        case MyEnum.Brush:
                            
                            Confetti();         
                            UIManager.Instance.SetIcon(4,0);
                            UIManager.Instance.SliderDomove(2);
                            CleanerBrusg.Instance.IsDone();
                            GameManager.Instance.sliderValue = 0;
                            foreach (var varıable in bikeParts)
                            {
                                varıable.material = washReadyMaterial;
                                varıable.GetComponent<Paintable>().Start();
                                if (varıable.TryGetComponent<MeshCollider>(out MeshCollider meshCollider))
                                    meshCollider.convex = true;
                            }
                            

                            foreach (var VARIABLE in wheelParts)
                            {
                                VARIABLE.material = wheelMaterial;
                                VARIABLE.GetComponent<Paintable>().Start();
                            }
                            myEnum = MyEnum.Wash;
                            break;
                        
                        case MyEnum.Wash:
                            Confetti();        
                            UIManager.Instance.SliderDomove(3);
                            CleanerBrusg.Instance.End();
                            UIManager.Instance.SetIcon(1,0);
                            GameManager.Instance.sliderValue = 0;
                            UIManager.Instance.nextButtonGameObject.SetActive(true);
                            foreach (var varıable in bikeParts)
                            {
                                varıable.material = weldingReadyMaterial;
                                varıable.GetComponent<Paintable>().Start();
                            }
                            foreach (var VARIABLE in wheelParts)
                            {
                                VARIABLE.material = wheelCleanMaterial;
                                VARIABLE.GetComponent<Paintable>().Start();
                            }
                            
                            break;
                    
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                    break;
                }
                case < 1 :
                {
                    switch (myEnum)
                    {
                        case MyEnum.BoxClosed:
                            Box.Instance.UpdateOpening();
                            break;
                        case MyEnum.BoxOpened:
                            Box.Instance.OnclickSelect();
                            break;
                        case MyEnum.Brush:
                            CleanerBrusg.Instance.UpdateCleaner();
                            break;
                        case MyEnum.Wash:
                            CleanerBrusg.Instance.UpdateCleaner();
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                    break;
                }
            }
        }

        public void ReadyPaint()
        {
            foreach (var varıable in bikeParts)
            {
                varıable.material = paintReadyMaterial;
                varıable.GetComponent<Paintable>().Start();
                Rigidbody rb = varıable.GetComponent<Rigidbody>();
                if (rb == null)
                {
                    rb=  varıable.gameObject.AddComponent<Rigidbody>();
                    rb.useGravity = false;
                    rb.isKinematic = true;
                }
            }
            foreach (var VARIABLE in wheelParts)
            {
                VARIABLE.material = wheelCleanMaterial;
                VARIABLE.GetComponent<Paintable>().Start();
            }
        }
        void Confetti()
        { confetti.Play();
            
        }
    }
}