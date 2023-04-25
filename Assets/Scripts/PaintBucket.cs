using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
 

public class PaintBucket : MonoBehaviour
{
     public ParticleSystem ps;
     public ParticleSystem particuleSplash;
      public ParticlesController collisionPainter;
     [Space]
     public ParticleSystem ps_2;
     public ParticleSystem particuleSplash_2;
     public ParticlesController collisionPainter2;
     [Space] 
     public Bucket bucket;
     [Space]
     public Transform paintBucket;
     public Transform paintBucket_2;
     private int _index;
     [Space] public ParticleSystem airbrushParticule; 
     public Material paintMaterial_2;
     public ParticlesController _AribrushPainterController;
 

     public  void SetParticuleColor( GameObject _gameObject)
     {
         Color _newcolor = _gameObject.GetComponent<Image>().color;
         float x = ps.main.duration+3;
         if(_index==0)
         {  paintBucket.transform.DORotateQuaternion(new Quaternion(-0.80582422f, -0.0620778911f, 0.0858634636f, 0.582598627f), 1).OnComplete((
                 () =>
                 {ps.Play();
             
                     var main = ps.main;
                     main.startColor = new ParticleSystem.MinMaxGradient( _newcolor);
                     var main_2 = particuleSplash.main;
                     main_2.startColor = new ParticleSystem.MinMaxGradient( _newcolor);
                     collisionPainter.paintColor = _newcolor;
                     
                 }));
             paintBucket.DOLocalMove(new Vector3(-4.47535324f,15.0922928f,7.78000021f), 5).SetDelay(x);

         }
         else if(_index==1)
         {
             paintBucket_2.transform
                 .DORotateQuaternion(new Quaternion(0.116514958f,0.58454299f,0.801964819f,0.0398272946f), 1)
                 .OnComplete((
                     () =>
                     {
                         ps_2.Play();

                         var main = ps_2.main;
                         main.startColor = new ParticleSystem.MinMaxGradient(_newcolor);
                         var main_2 = particuleSplash_2.main;
                         main_2.startColor = new ParticleSystem.MinMaxGradient(_newcolor);
                         collisionPainter2.paintColor = _newcolor;
                     }));
             paintBucket_2.DOLocalMove(new Vector3(-2.43000007f,15.0954933f,-15.0608263f), 5).SetDelay(x);
         }
         else
         {
             return;
         }

         bucket.SetColors(_newcolor, _index);
         _index++;
       _gameObject.SetActive(false);
     }

     public void setAirbrush( GameObject _gameObject)
     { 
         Color _newcolor = _gameObject.GetComponent<Image>().color;
         
         var main = airbrushParticule.main;
         main.startColor = new ParticleSystem.MinMaxGradient( _newcolor);
         collisionPainter.paintColor = _newcolor;
         paintMaterial_2.SetColor("_BaseColor",_newcolor);
         paintMaterial_2.SetColor("_VoronoiColor",_newcolor);
         _AribrushPainterController.paintColor = _newcolor;
     }
}
