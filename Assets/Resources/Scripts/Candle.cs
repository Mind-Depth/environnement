using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Candle : MonoBehaviour
{
    // Start is called before the first frame update

    public Light candle;
    private Color color;
    private float range;
    private float intensity;
    void Start()
    {
        color = candle.color;
        range = candle.range;
        intensity = candle.intensity;
        StartCoroutine(CandleLight());
    }

    private IEnumerator CandleLight()
    {
        while (true)
        {
            candle.range = range + Random.Range(-0.5f, 0.5f);
            candle.intensity = intensity + Random.Range(-0.07f, 0.07f);
            candle.color = new Color(color.r, color.g + Random.Range(-0.03f, 0.03f), color.b);
            yield return new WaitForSeconds(Random.Range(0.05f, 0.2f));
        }
    }
}
