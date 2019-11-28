using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneTeleport : MonoBehaviour
{

    public void ShowIndication() {
            Teleporter._instance.ShowIndication();
    }

    public void TeleportPlayer() {
            Teleporter._instance.HideIndication();
            Teleporter._instance.TeleportPlayer();
    }


/*     private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Hand")
        {
            if (Input.GetKey(KeyCode.A) || Input.GetJoystickNames("Trigger"))
            {
                Teleporter._instance.HideIndication();
                Teleporter._instance.TeleportPlayer();
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Hand")
        {
            Teleporter._instance.ShowIndication();
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Hand")
        {
            Teleporter._instance.HideIndication();
        }
    } */
}
