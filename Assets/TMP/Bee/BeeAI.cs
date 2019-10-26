using UnityEngine;

public class BeeAI : RotableEntity
{
    Animator anim;

    bool flying;
    float stateDuration;

    bool hasTarget;
    Vector3 target;

    public float minIdleDuration = 5f;
    public float maxIdleDuration = 5f;

    public float minFlyDuration = 30f;
    public float maxFlyDuration = 30f;

    public float takeOffUnitAngleLerp = 0.5f;

    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
        transform.RotateAround(transform.position, transform.up, Random.Range(0, 360));
        if (Random.value < 0.5f)
            Fly();
        else
            Stop();
    }

    void Stop()
    {
        flying = false;
        anim.SetBool("fly", flying);
        stateDuration = Random.Range(minIdleDuration, maxIdleDuration);
    }

    void Fly()
    {
        flying = true;
        hasTarget = false;
        anim.SetBool("fly", flying);
        stateDuration = Random.Range(minFlyDuration, maxFlyDuration);
    }

    void UpdateIdle()
    {
        if (stateDuration < 0f)
            Fly();
    }

    void FindTarget(Vector3 direction)
    {
        hasTarget = true;
        target = Random.onUnitSphere;
        target = Quaternion.Lerp(Quaternion.identity, Quaternion.FromToRotation(target, direction), takeOffUnitAngleLerp) * target;
        //TODO
        //raycast
        // if collide, dont go
        // else random distance < collision
        RotateTo2DProjection(target);
        Debug.DrawLine(transform.position, transform.position + target * 20, Color.black, 1f);
    }

    void GoToTarget()
    {
        //if (transform.position == target)
        if (Random.value < 0.02)
        {
            FindTarget(transform.forward);
            return;
        }

        /*
        Vector3 direction = target - transform.position;
        Vector3 movment = direction.normalized * Time.deltaTime * 20; // TODO speed
        if (movment.magnitude > direction.magnitude)
            movment = direction;
        transform.position += movment;
        */
        transform.position += Vector3.ProjectOnPlane(target, transform.right) * Time.deltaTime * 100; //TODO speed
    }

    void Land()
    {
        // Check is somewhere to stop
        //Physics.OverlapSphere;
        //Collider.ClosestPoint
        // Else FindTarget
        Stop();
        return;
    }

    void UpdateFlying()
    {
        UpdateRotation(1f); //TODO Speed
        /*if (stateDuration < 0f)
            Land();
        else*/ if (hasTarget)
            GoToTarget();
        else
            FindTarget(transform.up);
    }

    void Update()
    {
        stateDuration -= Time.deltaTime;
        if (flying)
            UpdateFlying();
        else
            UpdateIdle();
    }
}
