using UnityEngine;
using DG.Tweening;
using PainterControllers;
    

public class Bucket : MonoBehaviour 
{ 
    public Transform bucket_transform;
    public Material paintMaterial;
    public Material paintMaterial_2;
    public Material paintMaterial_3;
    public ParticleSystem spray;
    private ParticlesController _particlesController;
    [Space]
    public string floatPropertyName;
    public string  floatPropertyName_2;
    [Space]
    public float time;
    [Space]
    public float floatValue;
    public float targetValue;
    [Space]
    public float floatValue_2;
    public float targetValue_2;
    [Space] public PaintBucket paintBucket;
    /*private CollisionPainter _collisionPainter;*/
    public PaintController paintController;
    [Space] public Transform spoon;
    public Transform[] spoonPath;

    private void Start()
    {   _particlesController = spray.GetComponent<ParticlesController>();
        /*_collisionPainter = GetComponent<CollisionPainter>();*/
    }

    private void Update()
    { 
        paintMaterial.SetFloat(floatPropertyName,floatValue);
        paintMaterial.SetFloat(floatPropertyName_2,floatValue_2);
        bucket_transform.Rotate(0,-Time.deltaTime* floatValue_2,0,Space.Self);
        /*_collisionPainter.paintColor = paintMaterial.GetColor("_Color_1") + paintMaterial.GetColor("_Color_2");*/

    } 
    void MixColors()
    {
        DOTween.To (() => floatValue, x => floatValue = x, targetValue, time).SetDelay(2);
        DOTween.To (() => floatValue_2, x => floatValue_2 = x, targetValue_2, time).SetDelay(2).OnComplete((() =>
        {
            paintController.PaintFlow();
        }));
    }

  public  void SetColors(Color newColor,int x)
    {
        if(x==0)
        {
            paintMaterial.SetColor("_Color_2", newColor);
            
            transform.DOLocalMove(new Vector3(-0.00200000009f,0.230000004f,0.00300000003f), paintBucket.ps.main.duration).SetDelay(1.5f);
            transform.DOScale( new Vector3(0.398577243f,0.00304721599f,0.411480159f), paintBucket.ps.main.duration).SetDelay(1.5f);
        }
        else
        {
            SpoonPath();
            transform.DOLocalMove(new Vector3(-0.00200000009f, 0.416000009f, 0.00300000003f), paintBucket.ps_2.main.duration).SetDelay(1.5f);
                     transform.DOScale( new Vector3(0.45270738f, 0.00346105359f, 0.467362612f), paintBucket.ps_2.main.duration).SetDelay(1.5f);
            paintMaterial.SetColor("_Color_1", newColor);
            MixColors();
            Color _color = (paintMaterial.GetColor("_Color_1") + paintMaterial.GetColor("_Color_2")/2);
            paintMaterial_2.SetColor("_BaseColor",_color);
            paintMaterial_2.SetColor("_VoronoiColor",_color);
            paintMaterial_3.SetColor("_BaseColor", _color);
            paintMaterial_3.SetColor("_VoronoiColor", _color);
            _particlesController.paintColor = _color;
            
            var main = spray.main;
            main.startColor = new ParticleSystem.MinMaxGradient( _color);
        }
    }

  public void SpoonPath()
  {
      Vector3[] vector3s=new Vector3[spoonPath.Length];
      for (int i = 0; i <spoonPath.Length; i++)
      {
          vector3s[i] = spoonPath[i].position;
      }

      spoon.DOPath(vector3s, 1).SetEase(Ease.Linear).SetLoops(10, LoopType.Restart).OnComplete((() =>
              {
                  spoon.DOMove(spoon.position + (Vector3.up + Vector3.back) * 10, 5);
              }
          ));
  }
}

