using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeepMe : MonoBehaviour
{
    void Awake()
    {
        DontDestroyOnLoad(this);
    }
}
