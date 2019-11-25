using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Sam
{
    [Serializable]
    public class Line
    {
        public string   name;
        public float    duration;
        public string   mood;
        public string   fear;
        public string   callToAction;
    }

    [Serializable]
    public class MindStateLines
    {
        public List<Line> helper;
        public List<Line> plot_twist;
        public List<Line> psychopathe;
    }

    [Serializable]
    public class SamLines
    {
        public MindStateLines mindStateLines;
        public List<Line> ambiances;
        public List<Line> introduction;
    }
}
