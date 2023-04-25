 using UnityEngine;
using DG.Tweening;
 using Utils;

 public class WeldingMachine : MouseClick
{
    public static WeldingMachine Instance;
  
    [Space] public Transform weldingMachine;
    public GameObject weldingobject;
    public ParticleSystem weldingEffect;

    private float _countDown;
    private void Awake()
    {
        Instance = this;
    }
  public void UpdateWelding()
  {
      _countDown = Time.deltaTime + _countDown;
      Transform hitpointTransform = Painter();
      
      if(hitpointTransform ==null || _countDown<1)return;
      _countDown = 0;
      if (hitpointTransform .CompareTag("BikeBody") || hitpointTransform.transform.CompareTag("WeldObject"))
      { 
          weldingMachine.position = HitPosition;
          weldingEffect.Play();
          GameObject temp;
          temp= Instantiate(weldingobject,  HitPosition, Quaternion.identity);
          temp.transform.SetParent(hitpointTransform);
          temp.GetComponent<MeshRenderer>().material.DOColor(Color.gray, 2);
          
         if(hitpointTransform.transform.CompareTag("BikeBody"))
         {
             temp.tag = "WeldObject";
             GameManager.Instance.sliderValue += 0.11f;
         }
         else if(hitpointTransform.transform.CompareTag("WeldObject"))
         {
             temp.tag = "WeldObject_2";
             GameManager.Instance.sliderValue += 0.1f;
         }
         
         return;
      }
      else if(Input.GetMouseButtonUp(0)) weldingEffect.Stop();

  }
}
