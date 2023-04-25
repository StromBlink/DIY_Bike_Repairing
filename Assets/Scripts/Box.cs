 
using DG.Tweening;
using Unity.Mathematics;
using UnityEngine;
 

public class Box : MonoBehaviour
{
    public static Box Instance;
    
    
    public Transform cameraMain;
    [Space] public Transform cover_1;
    public Transform cover_2;
    public Transform cover_3;
    public Transform cover_4;
    [Space] public float amount;
    public float amount_2;
   
    [Space] public float cover_4_z = -135.875f;
    public float cover_3_x = -135.875f;
    public float cover_2_z = 135.875f;
    public float cover_1_x = -135.875f;
    [Space] 
    public GameObject readyCleanerBikeParts;
    [Space] 
    public GameObject boxCollider_1;
    public GameObject boxCollider_2;
    public GameObject boxCollider_3;
    public GameObject boxCollider_4;
   
    [Space] 
    private float _countdown;
    private int _counter;

    private void Awake()
    {
        Instance = this;
    }

    public void UpdateOpening()
    {
        _countdown += Time.deltaTime;

        cover_4.transform.localRotation =
            Quaternion.Lerp(cover_4.transform.localRotation,
                quaternion.EulerXYZ(new Vector3(0, 0, cover_4_z + amount * 0.01f)), Time.deltaTime*5);
        cover_2.transform.localRotation =
            Quaternion.Lerp(cover_2.transform.localRotation,
                quaternion.EulerXYZ(new Vector3(0, 0, cover_2_z - amount * 0.01f)), Time.deltaTime*5);
        
        cover_3.transform.localRotation =
            Quaternion.Lerp(cover_3.transform.localRotation,
                quaternion.EulerXYZ(new Vector3(cover_3_x + amount_2 * 0.01f, 0, 0)), Time.deltaTime*5);
        cover_1.transform.localRotation =
            Quaternion.Lerp(cover_1.transform.localRotation,
                quaternion.EulerXYZ(new Vector3(cover_1_x - amount_2 * 0.01f, 0, 0)), Time.deltaTime*5);
        
        if (Input.GetMouseButtonDown(0) && _countdown > 0.1f)
        {GameManager.Instance.sliderValue += 1/30f;
            amount += 20;
            _countdown = 0;
            if (amount > 90)
            {
                amount_2 += 30;
                if (amount > 250)
                {
                    GameManager.Instance.sliderValue = 1;
                    amount = 250;
                    amount_2 = 250;
                }
            }
        }
    } 
    private bool _iscalled;
    public void Moved()
    {   if(_iscalled)return;
        transform.DOMove(transform.position - Vector3.forward * 6, 1);
        _iscalled = true;
    }

  public  void CamMoved()
    {   
        Vector3 position = transform.position;
        cameraMain.DOMove(position + Vector3.up * 6, 2).OnComplete(() =>
        {
            cameraMain.DOLookAt(position, 1f) ;
        });
        cameraMain.DOLookAt(position, 0.5f).SetDelay(0.5f).OnComplete(() => { cameraMain.DOLookAt(position, 1); });
    }

    public void CameraFinalMoved()
    { 
        boxCollider_1.SetActive(false);
        boxCollider_2.SetActive(false);
        boxCollider_3.SetActive(false);
        boxCollider_4.SetActive(false);
        transform.DOMove(transform.position + Vector3.forward * 6, 2).SetDelay(1).OnComplete(() =>
        {    readyCleanerBikeParts.SetActive(true);
            NewCameraLocation();
            gameObject.SetActive(false);
        });
    }

    void NewCameraLocation()
    {
        cameraMain.DOMove(new Vector3(8.7664423f,9.34508419f,-0.446346223f), 1);
        cameraMain.DOLocalRotateQuaternion(new Quaternion(0.285875678f, 0.64170891f, -0.291224122f, 0.649363816f), 1);
    }

  public  void OnclickSelect()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 position = Input.mousePosition;
            Ray ray = cameraMain.GetComponent<Camera>().ScreenPointToRay(position);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100.0f))
            {
                Debug.DrawRay(ray.origin, hit.point - ray.origin, Color.red);
                if (hit.transform.CompareTag("Bikepart"))
                { if (hit.transform.TryGetComponent<Rigidbody>(out  Rigidbody rb))
                    {
                        rb.isKinematic = true;
                        rb.useGravity = false;
                    }
                    hit.transform.GetComponent<MeshCollider>().enabled = false;
                    hit.transform.DOMove(hit.transform.position + Vector3.forward * 3 + Vector3.up * 5, 2);
                    GameManager.Instance.sliderValue+=0.091f;
                }
            }
        }
    }
}
