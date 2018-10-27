using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movements : MonoBehaviour {

    // vitesse de deplacement de l'object
    int speed = 10;
    float waiter = 0; // seconds;
    float change_dir = 6; // seconds;
    float air = -1.0f;

    // trajectoire
    Vector3 target;

    // L'état actuel de l'objet (se deplace sur un mur, le sol ...)
    State object_state = State.GROUND;

    public enum State
    {
        GROUND,
        WALLX,
        WALLX_INV,
        WALLZ,
        WALLZ_INV, //invert
        CEILING
    }

    public Movements(State state)
    {
        object_state = state;
    }


    // Obtenir une nouvelle direction de déplacement
    public Vector3 GetNewTarget(Vector3 position)
    {
        Vector3 new_target = new Vector3(position.x, position.y, position.z);
        int x = Random.Range(0, 80);
        int y = Random.Range(0, 80);
        int z = Random.Range(-50, 50);
        switch (object_state)
        {
            case State.GROUND:
                new_target.x = x;
                new_target.z = z;
                break;
            case State.WALLZ:
                new_target.y = y;
                new_target.z = z;
                break;
            case State.WALLZ_INV:
                new_target.y = y;
                new_target.z = z;
                break;
            case State.WALLX:
                new_target.x = x;
                new_target.y = y;
                break;
            case State.WALLX_INV:
                new_target.x = x;
                new_target.y = y;
                break;
            case State.CEILING:
                new_target.x = x;
                new_target.z = z;
                break;
        }
        return new_target;
    }

    public void RotateObject(Vector3 positionItem)
    {
        switch (object_state) {
            case State.GROUND:
                transform.eulerAngles = new Vector3(0, 0, 0);
                transform.position = new Vector3(transform.position.x, positionItem.y, transform.position.z);
                break;
            case State.CEILING:
                transform.eulerAngles = new Vector3(180, 0, 0);
                transform.position = new Vector3(transform.position.x, positionItem.y, transform.position.z);
                break;
            case State.WALLZ:
                Debug.Log("WallZ");
                transform.eulerAngles = new Vector3(0, 0, -90);
                transform.position = new Vector3(positionItem.x, transform.position.y, transform.position.z);
                break;
            case State.WALLX:
                Debug.Log("WallX");
                transform.eulerAngles = new Vector3(-90, 0, 0);
                transform.position = new Vector3(transform.position.x, transform.position.y, positionItem.z);
                break;
            case State.WALLX_INV:
                Debug.Log("WallX_I");
                transform.eulerAngles = new Vector3(90, 0, 0);
                transform.position = new Vector3(transform.position.x, transform.position.y, positionItem.z);
                break;
            case State.WALLZ_INV:
                Debug.Log("WallZ_I");
                transform.eulerAngles = new Vector3(0, 0, 90);
                transform.position = new Vector3(positionItem.x, transform.position.y, transform.position.z);
                break;
        }
        Debug.Log(transform.eulerAngles);
    }

    void Start () {
        target = GetNewTarget(transform.position);
        Debug.Log(target);
	}

    bool has_jumped = false;

    void goDown()
    {
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        GetComponent<Rigidbody>(). AddForce(Vector3.down * 1000);
        air = 1.0f;
        has_jumped = false;
    }

	void Update () {
        if (waiter > 0)
            waiter -= Time.deltaTime;
        if (change_dir > 0)
            change_dir -= Time.deltaTime;
        if (air > 0)
            air -= Time.deltaTime;
        if (has_jumped && air < 0)
            goDown();
        if (air < 0)
            Jump();
    }

    public void Move() {
        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, target, step);
        if (Vector3.Equals(transform.position, target) || change_dir < 0) {
            target = GetNewTarget(transform.position);
            change_dir = 6;
        }
    }

    void Jump()
    {
        if (!has_jumped)
        {
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            GetComponent<Rigidbody>().AddForce(Vector3.up * 1500);
            air = 0.2f;
            has_jumped = true;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (waiter > 0)
            return;
        State tag;
        switch (collision.gameObject.tag)
        {
            case "WallX":
                tag = State.WALLX;
                break;
            case "WallX_Inv":
                tag = State.WALLX;
                break;
            case "WallZ":
                tag = State.WALLX;
                break;
            case "WallZ_Inv":
                tag = State.WALLX;
                break;
            case "Ceiling":
                tag = State.CEILING;
                break;
            case "Ground":
                tag = State.GROUND;
                break;
            default:
                tag = State.GROUND;
                break;
        }
        if (tag == object_state)
            return;
        Debug.Log(collision.gameObject.tag);
        object_state = tag;
        waiter = 0.5f;
        RotateObject(collision.gameObject.transform.position);
        target = GetNewTarget(transform.position);
    }
}
