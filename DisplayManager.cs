using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayManager : MonoBehaviour {

    public static DisplayManager _instance = null;
    private Map map_to_display = null;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this);
        } else if (_instance != this) {
            Destroy(this);
        }
    }

    public void SetMap(Map map)
    {
        map_to_display = map;
    }

    void Update()
    {
        if (map_to_display != null) {
            Instantiate(map_to_display.assets[0]);
            map_to_display = null; // has been instanciated.
        }
    }
}
