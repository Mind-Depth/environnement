using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Charge l'ensemble des ressources graphiques et sonores qui vont être utilisées dans le jeu.
 */
public class ResourcesManager : MonoBehaviour {

    public static ResourcesManager _instance = null;
    public GameObject[] assets;

    private void Start()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this);
        } else if (_instance != this)
        {
            Destroy(_instance);
        }
    }
}
