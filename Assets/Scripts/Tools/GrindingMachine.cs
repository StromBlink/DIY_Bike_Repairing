 
using UnityEngine;
using Utils;

public class GrindingMachine : MouseClick
{
    public static GrindingMachine Instance;
    
    [Space]
    public Transform grindingMachine;
    public Transform machineSprial;
    public ParticleSystem grindingEffect;
    
    private void Awake()
    {
        Instance = this;
    }
     public void UpdateGrinding()
    {
        GrindingCleaner();

    }

    void GrindingCleaner()
    { machineSprial.Rotate(new Vector3(0,0,-300)*Time.deltaTime);
        Transform hitpointTransform = Painter();
        
        if(hitpointTransform==null)return;
        grindingMachine.position = HitPosition;
        if (!hitpointTransform.CompareTag("BikeBody"))
        { 
            grindingEffect.Stop();
        }
        else if(Input.GetMouseButtonDown(0))
        { 
            grindingEffect.Play();
            
        }
        if (hitpointTransform.CompareTag("WeldObject") ||hitpointTransform.CompareTag("WeldObject_2"))
        {  grindingEffect.Play();
            Destroy(hitpointTransform.gameObject);
        }
    }
}
