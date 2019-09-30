using UnityEngine;

public class RandomTools
{
    public static float Gaussian()
    {
        int i = 0;
        float v1, v2, s;
        do
        {
            ++i;
            v1 = 2.0f * Random.Range(0f, 1f) - 1.0f;
            v2 = 2.0f * Random.Range(0f, 1f) - 1.0f;
            s = v1 * v1 + v2 * v2;
        } while (s >= 1.0f || s == 0f);
        return v1 * Mathf.Sqrt((-2.0f * Mathf.Log(s)) / s) / 3;
    }

    public static float Gaussian(float mean, float standard_deviation)
    {
        return mean + Gaussian() * standard_deviation;
    }

    public static float ClampedGaussian(float mean, float standard_deviation)
    {
        return Mathf.Clamp(Gaussian(mean, standard_deviation), 0, 1);
    }
}
