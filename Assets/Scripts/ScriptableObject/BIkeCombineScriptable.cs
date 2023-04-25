using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "BikeCombineScriptable", menuName = "BikeCombineScriptable" )]
public class BIkeCombineScriptable : ScriptableObject
{
    [Serializable]
    public struct BikePartPosition
    {
        public string partName;
        public int partIndex;
        public Vector3 position;
        public MeshRenderer meshRenderer;
    }
   public BikePartPosition[] bikePartPositions;
}
