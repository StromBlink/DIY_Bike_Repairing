using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Manager
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance;
        [Space] public Slider missionsSlider;
        [Space]
        public GameObject cleanerUI;
        public GameObject weldingUI;
        public GameObject paintingUI;
        public GameObject paintingUI_Airbrush;
        [Space]
        public Sprite[] icons;
        [Space] 
        public Image icon_Main;
        public Image icon_1;
        public Image icon_2;
        public Image icon_3;
        [Space] 
        public Image glowIcon_1;
        public Image glowIcon_2;
        public Image glowIcon_3;
        [Space]
        public GameObject nextButtonGameObject;

        [Space] public GameObject StickerUI;
        public MeshRenderer stickerMesh;
        public Material stickerMaterial;
        
        private void Awake()
        {
            Instance = this;
        } 
        public void MissionAccept(float time)
        {
            StartCoroutine(FNC(time));
        }

        public void MissionDeny()
        {
        
        }
        IEnumerator FNC(float time)
        {
        
            yield return new WaitForSeconds(time);
            GameManager.Instance.gameState = GameState.Transition;
            ToolManager.Instance.stateScene = StateScene.Cleaner;
        }

        public  void SetIcon( int iconIndex, int whatIcon)
        {
            switch (whatIcon)
            {   case 0:   
                    icon_Main.sprite = icons[iconIndex];
                    break;
                case 1:
                    icon_1.sprite = icons[iconIndex];
                    break;
                case 2:
                    icon_2.sprite = icons[iconIndex];
                    break;
                case 3:
                    icon_3.sprite = icons[iconIndex];
                    break;
            }
        }
        public void CleanerSetIcons()
        {
            SetIcon(0,0);
            SetIcon(0,1);
            SetIcon(2,2);
            SetIcon(4,3);
        }

        public void SliderDomove(float index)
        {
            missionsSlider.DOValue(index, 3);
            switch (index)
            {
                case 1:
                    glowIcon_1.enabled = true;
                    break;
                case 2:
                    glowIcon_2.enabled = true;
                    break;
                case 3:
                   glowIcon_3.enabled = true;
                   break;
            }
        }

        public void SetSticker(GameObject _button)
        {
            Image image = _button.GetComponent<Image>();
            Material material  = stickerMesh.material = Instantiate(stickerMaterial);
            material.SetTexture("Texture2D_abfdaea01b0848f5872e4cce686041a5",image.mainTexture);
        }

        public void PaintBike()
        {
            GameManager.Instance.sliderValue=0;
            paintingUI.SetActive(false);
            paintingUI_Airbrush.SetActive(true);
        }
    }
}
