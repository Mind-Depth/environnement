using UnityEngine;

public class RotableEntity : MonoBehaviour
{
    float rotationProgress = 0f;
    Quaternion originalRotation;
    Quaternion targetRotation;
    bool isRotating = false;

    void SetTargetRotation(Quaternion target)
    {
        isRotating = true;
        rotationProgress = 0;
        originalRotation = transform.rotation;
        targetRotation = target;
    }

    protected void StopRotation()
    {
        isRotating = false;
    }

    protected void RotateToNormal(Vector3 normal)
    {
        SetTargetRotation(Quaternion.FromToRotation(transform.up, normal) * transform.rotation);
    }

    protected void RotateSideWay(float angle)
    {
        SetTargetRotation(transform.rotation * Quaternion.Euler(0, angle, 0));
    }

    protected void RotateTo2DProjection(Vector3 direction)
    {
        SetTargetRotation(Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z)));
    }

    protected bool UpdateRotation(float speed)
    {
        if (!isRotating)
            return false;
        rotationProgress += Time.deltaTime * speed;
        transform.rotation = Quaternion.Lerp(originalRotation, targetRotation, rotationProgress);
        isRotating = rotationProgress < 1;
        return true;
    }
}
