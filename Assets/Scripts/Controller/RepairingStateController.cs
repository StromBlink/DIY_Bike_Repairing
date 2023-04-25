using System;
using DG.Tweening;
using Manager;
using UnityEngine;

namespace Controller
{
    public enum RepairingState
    {
        Welding,
        Dinging
    }
    public class RepairingStateController : MonoBehaviour
    {
        public static RepairingStateController Instance;
        public RepairingState repairingState;
        public ParticleSystem confetti;
        [Space] public GameObject WeldingMachineObject;
         public GameObject GrindingMachineObject;
        public GameObject[] flours;
        
        private void Awake()
        {
            Instance = this;
        }

        public void UpdateRepairing()
        {
            float x = GameManager.Instance.sliderValue;
            if (x<1)
            {
                switch (repairingState)
                {
                    case RepairingState.Welding:
                        WeldingMachine.Instance.UpdateWelding();
                        break;
                    case RepairingState.Dinging:
                        GrindingMachine.Instance.UpdateGrinding();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            else
            {
                switch (repairingState)
                {
                    case RepairingState.Welding:
                        WeldingMachineObject.SetActive(false);
                        Confetti();
                        repairingState = RepairingState.Dinging;
                        GameManager.Instance.sliderValue = 0;
                        foreach (var VARIABLE in flours)
                        {
                            VARIABLE.SetActive(true);
                        }
                        break;
                    case RepairingState.Dinging:
                        GrindingMachineObject.SetActive(true);
                        Confetti();
                        CleanerStateController.Instance.ReadyPaint();
                       UIManager.Instance.nextButtonGameObject.SetActive(true);
                        GameManager.Instance.sliderValue = 0;
                        
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        void Confetti()
        { confetti.Play();
            
        }
    }
}