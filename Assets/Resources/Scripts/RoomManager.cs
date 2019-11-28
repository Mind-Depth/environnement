using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/* 
** Effectue les changements de salles pour mettre a jour les portails & leurs liaisons.
** Assure la communication de ces changements au niveau de la partie génération.
*/
public class RoomManager : MonoBehaviour
{
    public static RoomManager _instance = null;

    public GameObject current_map = null;
    public Transform pt_tp_player_waiting;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else if (_instance != this)
        {
            Destroy(this);
        }
    }

    public void RequestRoom()
    {
        Manager._instance.generation_client.SendMessage(new EnvironmentMessage { type = EnvironmentMessage.Type.RequestRoom });
    }

    public void MapIsReady(GameObject map)
    {
        Destroy(current_map.gameObject);
        current_map = map;
        Teleporter._instance.TeleportPlayerBack(map.GetComponent<MapInformations>().point_to_teleport.position.y);
    }
 }
