using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapTeleporter : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TeleportPlayer() {
        Teleporter._instance.TeleportPlayer();
    }
    public void ShowIndication(){
        Teleporter._instance.ShowIndication();
    }
    public void HideIndication() {
        Teleporter._instance.HideIndication();
    }
    public void SamTrigger()
    {
        Sam.TriggerManager._instance.UpdateTriggerEvents();
    }
}
