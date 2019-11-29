using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontMoveSam : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Sam.TriggerManager._instance.UpdateTriggerEvents();
        }
    }
}
