using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;
using System;
using UnityEngine.Rendering.PostProcessing;

/*
 * Charge l'ensemble des ressources graphiques et sonores qui vont être utilisées dans le jeu.
 */
public class ResourcesManager : MonoBehaviour {

    public static ResourcesManager _instance = null;
    public PostProcessVolume PPV;
    public GameObject[] assets;
    public GameObject[] maps;
    public GameObject[] events;

    /* The string represents the ID of the asset */
    public Dictionary<string, GameObject> assets_dic = new Dictionary<string, GameObject>();
    public Dictionary<string, GameObject> maps_dic = new Dictionary<string, GameObject>();
    public Dictionary<string, GameObject> events_dic = new Dictionary<string, GameObject>();

    private int current_id = 0;
    private void Awake()
    {
        /* Create instance */
        if (_instance == null)
        {
            _instance = this;
        }
        else if (_instance != this)
        {
            Destroy(_instance);
        }
    }

    public PostProcessVolume getPPV() {
        return PPV;
    }
    void CreateModelFiles()
    {
        /* Crée le dossier model */
        string dir = Configuration.last_root + "\\" + Manager.configuration.generated.models;
        Directory.CreateDirectory(dir);

        ModelConfiguration asset_informations;
        foreach (GameObject asset in assets)
        {
            /* Récupération des informations du model. */
            asset_informations = asset.GetComponent<ModelConfiguration>();
            asset_informations.id = System.Guid.NewGuid().ToString();
            Debug.Log(asset.name + " has id : " + asset_informations.id);
            assets_dic.Add(asset_informations.id, asset);

            /* Creation des dossiers du path si ces derniers sont inexistants.
            ** Creation du fichier.*/
            FileInfo file_models = new FileInfo(dir + "\\" + asset_informations.id + ".json");
            file_models.Directory.Create();

            /* Données à ajouter en format JSON.*/
            string json = JsonUtility.ToJson(asset_informations);
            File.WriteAllText(file_models.FullName, json);
        }
        Debug.Log("Files : models files created.");
    }

    void CreateEventFiles()
    {
        /* Crée le dossier event */
        string dir = Configuration.last_root + "\\" + Manager.configuration.generated.events;
        Directory.CreateDirectory(dir);

        EventConfiguration event_information;
        foreach (GameObject ev in events)
        {
            /* Récupération des informations de l'event. */
            event_information = ev.GetComponent<EventConfiguration>();
            event_information.id = System.Guid.NewGuid().ToString();
            events_dic.Add(event_information.id, ev);

            /* Creation des dossiers du path si ces derniers sont inexistants.
            ** Creation du fichier.*/
            FileInfo file_events = new FileInfo(dir + "\\" + event_information.id + ".json");
            file_events.Directory.Create();

            /* Données à ajouter en format JSON.*/
            string json = JsonUtility.ToJson(event_information);
            File.WriteAllText(file_events.FullName, json);
    } 
        Debug.Log("Files : events files created.");
    }

    void CreateMapFiles()
    {
        /* Crée le dossier map */
        string dir = Configuration.last_root + "\\" + Manager.configuration.generated.maps;
        Directory.CreateDirectory(dir);

        MapConfiguration map_config;
        foreach (GameObject map in maps)
        {
            /* Récupération des informations de la map. */
            map_config = map.GetComponent<MapConfiguration>();
            map_config.id = System.Guid.NewGuid().ToString();
            maps_dic.Add(map_config.id, map);
            FileInfo file_maps = new FileInfo(dir + "\\" + map_config.id + ".json");
            file_maps.Directory.Create();
            string json = JsonUtility.ToJson(map_config);
            File.WriteAllText(file_maps.FullName, json);
        }
        Debug.Log("Files : maps files created.");
    }

    void CreateEnumFiles()
    {
        string json = JsonUtility.ToJson(EnumLists.instance);
        string file = new FileInfo(Configuration.last_root + "\\" + Manager.configuration.generated.enums).FullName;
        File.WriteAllText(file, json);
        Debug.Log("Files : enums file created.");
    }

    public void CreateFiles()
    {
        CreateModelFiles();
        Console._instance.AddLog("Files : models files created.");
        CreateMapFiles();
        Console._instance.AddLog("Files : maps files created.");
        CreateEventFiles();
        //Console._instance.AddLog("Files : events files created.");
        CreateEnumFiles();
        Console._instance.AddLog("Files : enums file created.");
    }

    public void DeleteFiles()
    {
        string[] generated =
        {
            Manager.configuration.generated.models,
            Manager.configuration.generated.maps,
            Manager.configuration.generated.events,
            Manager.configuration.generated.enums
        };
        foreach (string s in generated)
        {
            string path = Configuration.last_root + "\\" + s;
            if (Directory.Exists(path))
                Directory.Delete(path, true);
            else if (File.Exists(path))
                File.Delete(path);
        }
    }
}
