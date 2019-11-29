using UnityEngine;

public class AutoPlayerScale : MonoBehaviour
{
    public float defaultHeight = 1.8f;
    void Start()
    {
        transform.localScale = new Vector3(1, defaultHeight / transform.localPosition.y, 1);
    }
}
