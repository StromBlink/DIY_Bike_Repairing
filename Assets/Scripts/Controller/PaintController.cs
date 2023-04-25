using DG.Tweening;
using Manager;
using Unity.Mathematics;
using UnityEditor.Experimental;
using UnityEngine;
public class PaintController : MonoBehaviour
{
    [Space] public ParticleSystem confetti;
    public Transform targetTransformBucket;
    public Transform bucket;
    public GameObject paint;
    [Space] private Vector3 _bucketDefaultPosition;
    private Quaternion _bucketDefaultQuartion;

    [Space]

    public Transform paintTank;
    public Transform targetpaintTank;
    public Camera cam;
    private CameraController _cameraController;
    private SkinnedMeshRenderer _skinnedMeshRenderer;
    private ParticlesController _particlesController;
    [Space] 
    
    public Transform paintTankReferance;
    public Transform paintTankReferance_1;
    public Transform paintTankReferance_2;
    public Transform paintTankdefaultReferance;
    [Space] public Transform airBrush;
    public ParticleSystem spray;
    
    [Space] public bool paintTime;
    public Transform platform;
    [Space] public Transform stickerCube;

    private void Start()
    {
        _cameraController = cam.GetComponent<CameraController>();
        _bucketDefaultPosition = bucket.position;
        _bucketDefaultQuartion = bucket.rotation;
        
        _skinnedMeshRenderer = paint.GetComponent<SkinnedMeshRenderer>();
     
        
    }

    public void Painting()
    {
        if (!paintTime) return;
        platform.Rotate(new Vector3(0,Time.deltaTime*10,0));

            Vector3 position = Input.mousePosition;
            Ray ray = cam.ScreenPointToRay(position);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100.0f))
            {    
                Debug.DrawRay(ray.origin, hit.point - ray.origin, Color.red);
                airBrush.transform.position = Block(hit.point);
            }
        
        if (Input.GetMouseButtonDown(0))
        {
             spray.Play();
        }
        else if(Input.GetMouseButtonUp(0))
        {
            spray.Stop();
        }
        MousePainter.Instance.UpdatePainter();
    }
    Vector3 Block(Vector3 input)
    {
        Vector3 position =input;
        float x = input.x;
        float y = input.y;
        float z = input.z;
        
        
        z = Mathf.Clamp(z,   -2.3f,-2.5f);
        position = new Vector3(x, y, z);
        return   position;
    }
    public void PaintFlow()
    {
        Sequence mySequence =DOTween.Sequence();
        float x = 100;
        mySequence.Append(airBrush.DOMove(airBrush.transform.position+Vector3.forward, .8f));
       
        mySequence.Append(bucket.DOMove(targetTransformBucket.position, 1));
        mySequence.Join(paintTank.DOMove(targetpaintTank.position, 0.8f));

        mySequence.Append(bucket.DORotateQuaternion(targetTransformBucket.rotation, 1));
        mySequence.Join(paintTank.DORotateQuaternion(targetpaintTank.rotation, 0.8f));

        Transform _paint = paintTank.transform.GetChild(0);

        mySequence.Append(_paint.DOLocalMove(paintTankReferance.localPosition, 1).OnUpdate((() =>
        { paint.SetActive(true);
           if(x>0)
           {
               x -= Time.deltaTime * 300;
           }
           _skinnedMeshRenderer.SetBlendShapeWeight(1,x);
        } )));
        mySequence.Join(_paint.DOScale(paintTankReferance.localScale, .8f));
        
        mySequence.Append(_paint.DOLocalMove(paintTankReferance_1.localPosition, .8f));
        mySequence.Join(_paint.DOScale(paintTankReferance_1.localScale, .8f));
        
        mySequence.Append(_paint.DOLocalMove(paintTankReferance_2.localPosition, 1f));
        mySequence.Join(_paint.DOScale(paintTankReferance_2.localScale, 1f).OnComplete((() =>
        {
            _skinnedMeshRenderer.SetBlendShapeWeight(1,100);
        })));

        mySequence.Append(bucket.DORotateQuaternion(_bucketDefaultQuartion,.8f));
        mySequence.Append(bucket.DOMove(_bucketDefaultPosition, 2));

        mySequence.Append(paintTank.DOMove(paintTankdefaultReferance.position+Vector3.forward, .8f));
        mySequence.Join(paintTank.DORotateQuaternion(paintTankdefaultReferance.transform.rotation, .8f).OnComplete(() =>
        { _cameraController.CameraMoved(6,2);
            paintTime = true;
        }));

        mySequence.Append(
            airBrush.DORotateQuaternion(new Quaternion(-0.0443353802f, -0.703177094f, 0.0115677342f, 0.70953685f), 1).OnComplete((
                () =>
                {
                    UIManager.Instance.PaintBike();
                })));
    }

    private float _counter;
   public void StickerPuck()
    {
        Vector3 position = Input.mousePosition;
        Ray ray = cam.ScreenPointToRay(position);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100.0f))
        {
            _counter += Time.deltaTime;
            Debug.DrawRay(ray.origin, hit.point - ray.origin, Color.red);
            stickerCube.position = hit.point;
            if (Input.GetMouseButtonDown(0) && _counter>1)
            {
                float x =GameManager. Instance.sliderValue;
                _counter = 0;
                if (x > 1)
                {
                    ToolManager.Instance.stateScene = StateScene.Final;
                }
                x += 0.1f;
                GameManager.Instance.sliderValue = x;
                Instantiate(stickerCube, hit.point, stickerCube.rotation).SetParent(hit.transform);
            }
        }
    }
}
