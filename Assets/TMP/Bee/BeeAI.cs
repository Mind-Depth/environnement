using UnityEngine;

public class BeeAI : RotableEntity
{
    Animator anim;

    bool flying;
    float stateDuration;

    bool hasTarget;
    Vector3 target;

    public float minIdleDuration = 0f;
    public float maxIdleDuration = 0f;

    public float minFlyDuration = 1f;
    public float maxFlyDuration = 1f;

    public float takeOffUnitAngleLerp = 0.6f;

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

    void UpdateFlying()
    {
        if (!hasTarget)
        {
            hasTarget = true;
            target = Random.onUnitSphere;
            target = Quaternion.Lerp(Quaternion.identity, Quaternion.FromToRotation(target, transform.up), takeOffUnitAngleLerp) * target;
            // Distance ?
            Debug.DrawLine(transform.position, transform.position + target * 100, Color.black, 1f);
        }
        else
        {
//            Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        }
        // Do stuff

        if (stateDuration > 0f)
            return;

        // Check if landing is possible
        Stop();
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
