using System;
using System.Collections.Generic;
using UnityEngine;

public enum Fear
{
    Arachnophobia,
    Vertigo,
    Nyctophobia,
    Claustrophobia
};

[Serializable]
public class EnvironmentMessage
{
    public enum Type
    {
        Terminate,
        Initialize,
        RequestRoom,
    };

    public Type type;
    public string message;
}

[Serializable]
public class GenerationMessage
{
    public enum Type
    {
        Terminate,
        Initialize,
        RoomConfiguration,
        Start,
        Quit,
    };

    [Serializable]
    public class ModelGroup
    {
        public ModelConfiguration.Type type;
        public List<string> modelIds;
    }

    public Type type;
    public string message;
    public string mapId;
    public List<string> eventIds;
    public List<ModelGroup> modelGroups;
    public Fear fear;
    public float fearIntensity;
}
