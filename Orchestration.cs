using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Classe qui, via la liste d'assets défini par "AssetsDecision", place les différents éléments.
 * Choix de l'ambiance lumineuse.
 * Choix de l'ambiance sonore.
 */
public class Orchestration {

    Map map = null;
    public Orchestration() {

    }

    public void ChooseMapAssets()
    {
        map = AssetsList._instance.GetFirstItem();
    }

    /* Envoie de la Map au DisplayManager pour l'afficher */
    public void SendToDisplayManager()
    {
        DisplayManager._instance.SetMap(map);
    }

    /* Vérification et validation des assets de la Map */
    public void Update()
    {
        this.ChooseMapAssets();
        if (map != null)
        {
            this.SendToDisplayManager();
            AssetsList._instance.RemoveElement(map);
            map = null;
        }
    }
}
