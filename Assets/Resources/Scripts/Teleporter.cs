using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Teleporter : MonoBehaviour
{
    public static Teleporter _instance = null;
    public Transform player;
    public Text indication;

    private Vector3 old_player_position = Vector3.zero;
    private bool is_teleporting = false;

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

    private void Start()
    {
        indication.gameObject.SetActive(false);
    }

    public void TeleportPlayer()
    {
        if (!is_teleporting)
        {
            old_player_position = player.position;
            player.position = RoomManager._instance.pt_tp_player_waiting.position;
            is_teleporting = true;
            RoomManager._instance.RequestRoom();
        }
    }

    public void TeleportPlayerBack(float new_position_y)
    {
        is_teleporting = false;
        player.position = new Vector3(old_player_position.x, new_position_y + 1, old_player_position.z);
        player.GetComponent<Rigidbody>().velocity = Vector3.zero;
        player.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        player.GetComponent<Rigidbody>().Sleep();
    }

    public void ShowIndication()
    {
        indication.gameObject.SetActive(true);
    }

    public void HideIndication()
    {
        indication.gameObject.SetActive(false);
    }
}
