using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tabtale.TTPlugins;
[DefaultExecutionOrder(-2000)]
public class TTPSDK : MonoBehaviour
{
    private void Awake()
    {
        TTPCore.Setup();
    }
}
