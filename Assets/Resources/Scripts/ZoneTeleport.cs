using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneTeleport : MonoBehaviour
{
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (Input.GetKey(KeyCode.A))
            {
                Teleporter._instance.HideIndication();
                Teleporter._instance.TeleportPlayer();
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Teleporter._instance.ShowIndication();
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Teleporter._instance.HideIndication();
        }
    }
}
