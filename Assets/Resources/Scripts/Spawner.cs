using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject entity;
    public int minCount;
    public int maxCount;
    public float minScale;
    public float maxScale;

    public void Activate(float power)
    {
        float count = Mathf.Lerp(minCount, maxCount, power);
        for (int i = 0; i < count; ++i)
        {
            float scale = Mathf.Lerp(minScale, maxScale, RandomTools.ClampedGaussian(power, power));
            Instantiate(entity, transform.position, transform.rotation).transform.localScale = new Vector3(scale, scale, scale);
        }
    }
}
