using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/*
 * Contient la liste de Map qui sont échangées entre l'Orchestration et l'AssetsDecision.
 */
public class AssetsList : MonoBehaviour {

    public static AssetsList _instance = null;
    private List<Map> object_list;

    public AssetsList() {
        object_list = new List<Map>();
        if (_instance == null) {
            _instance = this;
        }
    }

    public Map GetFirstItem()
    {
        Map ret = object_list.FirstOrDefault();
        return ret;
    }

    public void AddElement(Map map)
    {
        object_list.Add(map);
    }

    public void RemoveElement(Map map)
    {
        object_list.Remove(map);
    }
}
