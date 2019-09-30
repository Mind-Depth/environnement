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
    public static string root = "..\\Generation\\config";
    public static string main = root + "\\main.json";
    public static Configuration Load()
    {
        return JsonUtility.FromJson<Configuration>(File.ReadAllText(main));
    }
}
