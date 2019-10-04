using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

/*
 * Classe qui, via la liste d'assets défini par la partie génération, place les différents éléments.
 * Choix de l'ambiance lumineuse.
 * Choix de l'ambiance sonore.
 */
public class Orchestration : MonoBehaviour
{ 
    public static Orchestration _instance = null;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this);
        }
        else if (_instance != this)
        {
            Destroy(this);
        }
    }

    public void GenerateNewMap(GenerationMessage message)
    {
        Console._instance.AddLog("Generate map.");
        GameObject map = Instantiate(ResourcesManager._instance.maps_dic[message.mapId], Vector3.zero, Quaternion.identity, null);
        Dictionary<ModelConfiguration.Type, List<string>> models_received = new Dictionary<ModelConfiguration.Type, List<string>>();
        List<Spawner> spawner = new List<Spawner>();

        foreach (GenerationMessage.ModelGroup model in message.modelGroups)
        {
            models_received.Add(model.type, model.modelIds);
        }
        ReplaceAssets(models_received, map.transform, spawner);
        if (spawner.Count != 0)
        {
            spawner.OrderBy(x => Guid.NewGuid()).FirstOrDefault();
            float limit = spawner.Count * message.fearIntensity;
            limit = Mathf.Clamp(limit, 1.0f, spawner.Count);
            for (int i = 0; i < limit; i++)
            {
                spawner[i].Activate(message.fearIntensity);
            }
        }
        RoomManager._instance.MapIsReady(map);
    }

    public void ReplaceAssets(Dictionary<ModelConfiguration.Type, List<string>> models_received, Transform parent, List<Spawner> spawner)
    {
        List<Transform> children = new List<Transform>();
        foreach (Transform child in parent)
        {
            children.Add(child);
        }
        foreach (Transform child in children)
        {
            if (child.transform.childCount > 0)
                ReplaceAssets(models_received, child, spawner);
            TemplateTypeModel config = child.GetComponent<TemplateTypeModel>();
            if (config != null)
            {
                ModelConfiguration.Type type = child.GetComponent<TemplateTypeModel>().typeObject;
                string id_asset = models_received[type].OrderBy(x => Guid.NewGuid()).FirstOrDefault();
                if (id_asset == "")
                    return;
                GameObject obj = Instantiate(ResourcesManager._instance.assets_dic[id_asset], child.transform.position, child.transform.rotation, parent.transform);
                obj.transform.localScale = child.localScale;
                if (type == ModelConfiguration.Type.Mob)
                    spawner.Add(obj.GetComponent<Spawner>());
                Destroy(child.gameObject);
            }
        }
    }
}
