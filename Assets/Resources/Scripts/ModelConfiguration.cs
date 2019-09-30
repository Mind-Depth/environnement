using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ModelConfiguration : MonoBehaviour {
    public enum Type
    {
        Ground,
        Ceiling,
        Wall,
        Light,
        LightWall,
        Mob,
        Decoration,
        DecorationWall,
    };
    public Type type;
    public string id;
    public string description;
    //public int fearIntensity;
    public List<Fear> fears = new List<Fear>();
}
