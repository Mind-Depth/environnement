using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAHead : MonoBehaviour
{

    private Vector3 save;
    public float speed = 1000f;
    public Transform Player;
    // Start is called before the first frame update
    void Start()
    {
        save = Player.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Player.position != save) {
            transform.position = new Vector3((transform.position.x - save.x) * speed, (transform.position.y - save.y) * speed / 2, (transform.position.z - save.z) * speed);
            save = Player.position;
        }
    }
}
