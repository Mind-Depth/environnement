using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sam;
public class SamObject : MonoBehaviour
{
    public string name = "";
    public bool isTriggerable = false;
    // Start is called before the first frame update
    void Start()
    {
        TriggerManager._instance.UpdateTriggerEvents(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
