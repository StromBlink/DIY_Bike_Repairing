using DG.Tweening;
using Manager;
using UnityEngine;

namespace Controller
{
    public class CombiningStateController : MonoBehaviour
    {
        public static CombiningStateController Instance;
        [SerializeField]  private Transform  cameraMain;
        public ParticleSystem confetti;

        public BIkeCombineScriptable bIkeCombineScriptable;
        public Transform[] positions;
        public Transform[] positionsRuby;
        public GameObject bikeRuby;
   
 
        private Transform _hitTransform;
        private Transform _targetTransform;
        private int _index;
        private float _countDown;
        private int _length;
        private float _addition;
        private float _fillDone;
        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {   bikeRuby.SetActive(true);     
            //to save parts
            _length = bIkeCombineScriptable.bikePartPositions.Length;
            _addition = 1f / 4;
            for (int i = 0; i < _length; i++)
            {   
                bIkeCombineScriptable.bikePartPositions[i].partName=positionsRuby[i].name;
                bIkeCombineScriptable.bikePartPositions[i].partIndex=i;
                bIkeCombineScriptable.bikePartPositions[i].position = positionsRuby[i].position;
            }
        
        }

        public void UpdateCombining()
        {
            ClickMouse();
            _fillDone = GameManager.Instance.sliderValue;
            if (_fillDone > 1)
            {
                foreach (var varıable in CleanerStateController.Instance. bikeParts)
                {
                    if (varıable.TryGetComponent<MeshCollider>(out MeshCollider meshCollider))
                        meshCollider.convex = false;
                }
                confetti.Play();
                
                UIManager.Instance.SliderDomove(4);
                UIManager.Instance.nextButtonGameObject.SetActive(true);
            }
        }

        void ClickMouse()
        {
            bool _countdown = CountDown(1);
            if (!_countdown)return;
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 position = Input.mousePosition;
                Ray ray = cameraMain.GetComponent<Camera>().ScreenPointToRay(position);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, 100.0f))
                {
                    Debug.DrawRay(ray.origin, hit.point - ray.origin, Color.red);
                    if (hit.transform.CompareTag("Bikepart"))
                    {
                        if (hit.transform.TryGetComponent<Rigidbody>(out  Rigidbody rb))
                        {
                       
                            rb.isKinematic = true;
                            rb.useGravity = false;
                        }
                        _hitTransform = hit.transform;
                        for (int i = 0; i < positions.Length; i++)
                        {
                            if (_hitTransform == positions[i])
                            {
                                _index = i;
                                _targetTransform= positionsRuby[i];
                            }
                        }
                    }
                }
            }
        
            if (Input.GetMouseButton(0))
            {
                Vector3 position = Input.mousePosition;
                Ray ray = cameraMain.GetComponent<Camera>().ScreenPointToRay(position);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, 100.0f))
                {
                    Debug.DrawRay(ray.origin, hit.point - ray.origin, Color.red);
                    if (!hit.transform.CompareTag("Bikepart")) return;
                    _hitTransform.position = PositionBlocked(hit.point);
                    Vector3 vector3 = ForObjectPosition(_index);
                     
                    float x = Vector3.Distance(vector3, _hitTransform.position);

                    if (!(x < 1)) return;
                    _countDown = 0;
                    DoMovePart(_hitTransform,_targetTransform);
                    GameManager.Instance.sliderValue += _addition;
                }
            }
        }

        private bool CountDown(float time)
        {
            _countDown += Time.deltaTime;
            if (_countDown>time)
            {
                return true;
            }

            return false;
        }

        private  Vector3 ForObjectPosition(int index)
        {
            Vector3 x;
            x = bIkeCombineScriptable.bikePartPositions[index].position;
            return x;
        }

        private  Vector3 PositionBlocked(Vector3 _Position)
        {
            float x = _Position.x;
            float z = _Position.z;
            Vector3 s = new Vector3(x, 4.587963104248047f, z);
            return s;
        }

        void DoMovePart(Transform one,Transform two)
        { one.tag = "BikeBody";
            _targetTransform.gameObject.SetActive(false);
            Vector3 vector3 = _targetTransform.position;
            Quaternion quaternion = _targetTransform.rotation;
            one.DOMove(vector3, 1);
            one.DORotateQuaternion(quaternion, 1).OnComplete((() =>
            {
                _targetTransform = null;
                _hitTransform = null;
            }));
       
        }
    
    }
}
