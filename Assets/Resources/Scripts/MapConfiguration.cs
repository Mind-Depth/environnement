using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MapConfiguration : MonoBehaviour
{
    public string id;
    public Fear fear;
    public bool generic;
    public int intro = -1;
    public string description;

    [Serializable]
    public class CategoriesConfiguration
    {
        public int use_min;
        public int use_max;
        public ModelConfiguration.Type type;
    }
    public List<CategoriesConfiguration> categories_config = new List<CategoriesConfiguration>();
}
