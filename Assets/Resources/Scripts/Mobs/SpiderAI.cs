using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SpiderAI : MonoBehaviour
{
    enum State
    {
        IDLE,
        TURNING,
        WALKING,
    }

    Animator anim;
    Vector3 radius;
    int layerMask;

    float walkingSpeed;
    float groundHovering;

    State state;
    float stateDuration;

    bool isRotating = false;
    float rotationProgress = 0f;
    Quaternion originalRotation;
    Quaternion targetRotation;
    List<Transform> raycastSources = new List<Transform>(2);

    public float minWalkDuration = 10f;
    public float maxWalkDuration = 20f;

    public float minIdleDuration = 5;
    public float maxIdleDuration = 10;

    public float minTurnAngle = -90;
    public float maxTurnAngle = 90;

    public float turningSpeed = 1f;

    public float walkingSpeedFactor = 1f;
    public float groundHoveringFactor = 2f;

    public float upwardRotationSpeed = 1f;
    public float downwardRotationSpeed = 1f;

    public Transform frontRaycastSource;
    public Transform backRaycastSource;
    public Transform leftRaycastSource;
    public Transform rightRaycastSource;

    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
        layerMask = ~(1 << gameObject.layer);
        radius = GetComponentInChildren<BoxCollider>().bounds.size / 2;
        walkingSpeed = radius.z * walkingSpeedFactor;
        groundHovering = radius.y * groundHoveringFactor;
        raycastSources.Add(frontRaycastSource);
        raycastSources.Add(backRaycastSource);
        raycastSources.Add(leftRaycastSource);
        raycastSources.Add(rightRaycastSource);
        transform.RotateAround(transform.position, transform.up, Random.Range(0, 360));
        if (Random.value < 0.5f)
            Walk();
        else
            Stop();
    }

    void RotateToNormal(Vector3 normal)
    {
        isRotating = true;
        rotationProgress = 0;
        originalRotation = transform.rotation;
        targetRotation = Quaternion.FromToRotation(transform.up, normal) * originalRotation;
    }

    void RotateSideWay(float angle)
    {
        isRotating = true;
        rotationProgress = 0;
        originalRotation = transform.rotation;
        targetRotation = transform.rotation * Quaternion.Euler(0, angle, 0);
    }

    void DownwardRaycast(out List<float> distances)
    {
        distances = new List<float>(raycastSources.Count);
        foreach (Transform raycastSource in raycastSources)
            if (Physics.Raycast(raycastSource.position, -raycastSource.up, out RaycastHit hit, groundHovering, layerMask))
                distances.Add(hit.distance);
    }

    bool FrontRaycast(out RaycastHit hit)
    {
        return Physics.Raycast(frontRaycastSource.position, frontRaycastSource.forward, out hit, radius.z, layerMask);
    }

    bool BackRaycast(out RaycastHit hit)
    {
        return Physics.Raycast(backRaycastSource.position, backRaycastSource.forward, out hit, radius.z, layerMask);
    }

    bool LeftRaycast(out RaycastHit hit)
    {
        return Physics.Raycast(leftRaycastSource.position, leftRaycastSource.forward, out hit, radius.x, layerMask);
    }

    bool RightRaycast(out RaycastHit hit)
    {
        return Physics.Raycast(rightRaycastSource.position, rightRaycastSource.forward, out hit, radius.x, layerMask);
    }

    void UpdateRotation(float speed)
    {
        rotationProgress += Time.deltaTime * speed;
        transform.rotation = Quaternion.Lerp(originalRotation, targetRotation, rotationProgress);
        isRotating = rotationProgress < 1;
    }

    void UpdateWalkingRotation()
    {
        // Lerp transform
        if (isRotating)
            UpdateRotation(upwardRotationSpeed);

        // Find obstacle
        if (FrontRaycast(out RaycastHit hit))
        {
            RotateToNormal(hit.normal);
            return;
        }

        RaycastHit front, back;
        bool frontRaycast = Physics.Raycast(frontRaycastSource.position, -frontRaycastSource.up, out front, groundHovering, layerMask);
        bool backRaycast = Physics.Raycast(backRaycastSource.position, -backRaycastSource.up, out back, groundHovering, layerMask);

        // Rectify rotation on light slopes
        if (frontRaycast)
        {
            if (!backRaycast || back.distance < front.distance)
                RotateToNormal(front.normal);
            else if (front.distance < back.distance)
                RotateToNormal(back.normal);
            return;
        }

        isRotating = false;
        transform.RotateAround(transform.position, transform.right, Time.deltaTime * downwardRotationSpeed);
    }

    void UpdateWalkingPosition()
    {
        // Walk
        transform.position += transform.forward * Time.deltaTime * walkingSpeed;

        // Stick to surfaces
        DownwardRaycast(out List<float> distances);
        if (distances.Count > 0)
            transform.position -= transform.up * (distances.Min() - radius.y);
    }

    void Stop()
    {
        state = State.IDLE;
        anim.SetBool("isTurning", false);
        anim.SetBool("isWalking", false);
        stateDuration = Random.Range(minIdleDuration, maxIdleDuration);
    }

    void Turn()
    {
        float orientation = Random.Range(minTurnAngle, maxTurnAngle);
        state = State.TURNING;
        anim.SetBool("isTurning", true);
        anim.SetBool("isWalking", false);
        anim.SetFloat("orientation", orientation);
        RotateSideWay(orientation);
    }

    void Walk()
    {
        state = State.WALKING;
        anim.SetBool("isTurning", false);
        anim.SetBool("isWalking", true);
        stateDuration = Random.Range(minWalkDuration, maxWalkDuration);
    }

    void UpdateIdle()
    {
        if (stateDuration < 0f)
            Turn();
    }

    void UpdateTurning()
    {
        if (isRotating)
            UpdateRotation(turningSpeed);
        else
            Walk();
    }

    void UpdateWalking()
    {
        UpdateWalkingRotation();
        UpdateWalkingPosition();
        if (stateDuration > 0f)
            return;

        // Check if obstructed
        DownwardRaycast(out List<float> distances);
        if (distances.Count < 4 ||
            FrontRaycast(out RaycastHit front) ||
            BackRaycast(out RaycastHit back) ||
            LeftRaycast(out RaycastHit left) ||
            RightRaycast(out RaycastHit right))
            return;

        Stop();
    }

    void Update()
    {
        stateDuration -= Time.deltaTime;
        switch (state)
        {
            case State.IDLE: UpdateIdle(); break;
            case State.TURNING: UpdateTurning(); break;
            case State.WALKING: UpdateWalking(); break;
        }
    }
}
