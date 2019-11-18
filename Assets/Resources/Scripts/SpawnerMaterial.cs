using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpawnerMaterial : MonoBehaviour
{
    public List<GameObject> objects = new List<GameObject>();

    public void Activate(float power)
    {
        objects.OrderBy(x => Guid.NewGuid()).FirstOrDefault();
        float count = Mathf.Lerp(0, objects.Count, power);
        for (int i = 0; i < count; ++i)
        {
            objects[i].SetActive(true);
        }
    }
}
