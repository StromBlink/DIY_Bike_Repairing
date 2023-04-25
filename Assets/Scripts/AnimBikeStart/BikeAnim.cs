 
using UnityEngine;
using DG.Tweening;

public class BikeAnim : MonoBehaviour
{
    public Transform wheel_1;
    public Transform wheel_2;
    [Space] public Transform target;
    public Vector3 _rotation;
    public float _rotation_x;
    public float _multply;
    public bool finish;
    private void Update()
    {if(finish)return;
        WheelRotate();
    }

    public void WheelRotate()
    {
        _rotation_x += Time.deltaTime * _multply;
        _rotation.x = _rotation_x;
        wheel_1.Rotate(_rotation);
        wheel_2.Rotate(_rotation);
        DoMoveAnim();


    }
 
private bool inSide;
    void DoMoveAnim()
    {
        if (Vector3.Distance(target.position, transform.position)<4)
        {
            inSide = true;
            transform.DOMove(target.position, 4);
            DOTween.To(()=> _multply, x=> _multply = x, 0, 5).OnComplete((() =>
            {
                finish = true;
            }));
        }
        
    }
}
