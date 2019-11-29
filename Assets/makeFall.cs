using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class makeFall : MonoBehaviour
{
    private bool move = false;
    public float speed = 24f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!move)
        return ;
        transform.position += Vector3.up * Time.deltaTime * speed;
        speed += 0.5f;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Player") {
            Sam.TriggerManager._instance.UpdateTriggerEvents();
            move = true;
        }
    }
}
