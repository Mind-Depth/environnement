using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportFall : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player") {
            Teleporter._instance.TeleportPlayer();
        }
    }
}
