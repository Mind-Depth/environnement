using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeepMe : MonoBehaviour
{
    private bool toto = false;
    void Awake()
    {
        DontDestroyOnLoad(this);
    }

     void Update()
    {
        if (!toto) toto = true;
    }
}
