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
    }

    [Serializable]
    public class Lines
    {
        public string   lang;
        public Line[]   lines;
        public Line presentation;
    }
}
