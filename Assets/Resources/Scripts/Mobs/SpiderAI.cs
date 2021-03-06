﻿using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SpiderAI : RotableEntity
{
    enum State
    {
        IDLE,
        TURNING,
        WALKING,
    }

    Animator anim;
    int layerMask;

    float walkingSpeed;
    float groundHovering;

    State state;
    float stateDuration;

    float hoverTime = 0f;
    bool isRotating = false;
    float rotationProgress = 0f;
    Quaternion originalRotation;
    Quaternion targetRotation;
    List<Transform> raycastSources = new List<Transform>(2);

    public Vector3 radius = new Vector3(4.6f, 1.5f, 5f);

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

    private float maxHoverTime = 3f;

    public Transform frontRaycastSource;
    public Transform backRaycastSource;
    public Transform leftRaycastSource;
    public Transform rightRaycastSource;

    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
        layerMask = ~(1 << gameObject.layer);
        radius = Vector3.Scale(radius, transform.localScale);
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

    void SetTargetRotation(Quaternion target)
    {
        isRotating = true;
        rotationProgress = 0;
        originalRotation = transform.rotation;
        targetRotation = target;
    }

    void RotateToNormal(Vector3 normal)
    {
        SetTargetRotation(Quaternion.FromToRotation(transform.up, normal) * transform.rotation);
    }

    //TODO
    //void Rotate()
    //{
    //    isRotating
    //    Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
    //}

    void RotateSideWay(float angle)
    {
        SetTargetRotation(transform.rotation * Quaternion.Euler(0, angle, 0));
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

    bool FrontDownwardRaycast(out RaycastHit hit, float dist=-1)
    {
        return Physics.Raycast(frontRaycastSource.position, -frontRaycastSource.up, out hit, dist < 0 ? groundHovering : dist, layerMask);
    }

    bool BackDownwardRaycast(out RaycastHit hit)
    {
        return Physics.Raycast(backRaycastSource.position, -backRaycastSource.up, out hit, groundHovering, layerMask);
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

        bool frontRaycast = FrontDownwardRaycast(out RaycastHit front);
        bool backRaycast = BackDownwardRaycast(out RaycastHit back);

        // Rectify rotation on light slopes
        if (frontRaycast)
        {
            hoverTime = 0;
            if (!backRaycast || back.distance < front.distance)
                RotateToNormal(front.normal);
            else if (front.distance < back.distance)
                RotateToNormal(back.normal);
            return;
        }

        // Prevent spider from floating in the air
        hoverTime += Time.deltaTime;
        if (hoverTime > maxHoverTime)
        {
            if (FrontDownwardRaycast(out hit, Mathf.Infinity))
            {
                transform.position = hit.point + frontRaycastSource.up * groundHovering;
                transform.rotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
            }
            else
                Destroy(gameObject);
            return;
        }

        // Rotate in order to pass an edge
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
