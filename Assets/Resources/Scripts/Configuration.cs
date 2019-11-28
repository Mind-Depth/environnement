using System;
using System.IO;
using UnityEngine;

[Serializable]
public class Configuration
{
    /* Fields */
    [Serializable]
    public class _generated
    {
        public string maps;
        public string models;
        public string events;
        public string enums;
    }
    public _generated generated;
    [Serializable]
    public class _connection
    {
        public int chunk_size;
        public string server_to_client;
        public string client_to_server;
        public string environment;
    }
    public _connection connection;

    /* Static Load */
    public static string last_root;
    public static Configuration Load(string root)
    {
        last_root = root;
        return JsonUtility.FromJson<Configuration>(File.ReadAllText(root + "\\main.json"));
    }
}
