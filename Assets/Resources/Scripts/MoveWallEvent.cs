using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveWallEvent : Event
{
    public Vector3 tranlate_direction;
    public float speed;
    private bool can_move;
    private bool start_event;
    private Vector3 mem_wall_position;


    private void Start()
    {
        can_move = false;
        start_event = false;
        speed = 10 / speed;
    }

    private void Update()
    {
        if (!can_move || !start_event)
            return;
        // check if translate is finished
        if (Vector3.Distance(transform.position, (mem_wall_position + tranlate_direction)) < 0.1f)
            return;
        transform.Translate(tranlate_direction * (Time.deltaTime / speed));
    }

    public override void ExecuteEvent(GenerationMessage data) {
        start_event = true;
    }

    private void OnCollisionStay(Collision collision)
    {
        can_move = false;
        ModelConfiguration model = collision.gameObject.GetComponent<ModelConfiguration>();
        if (model != null && model.type == ModelConfiguration.Type.Mob)
            can_move = true;
    }
}
