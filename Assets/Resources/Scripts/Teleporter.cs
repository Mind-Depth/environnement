using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;

public class Teleporter : MonoBehaviour
{
    public PostProcessVolume ppv;
    public PostProcessProfile pppSave;
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
    public PostProcessVolume getPPV() {
        return ppv;
    }
    public void TeleportPlayer()
    {
        if (!is_teleporting)
        {
            is_teleporting = true;
            ppv.profile = pppSave;
            RoomManager._instance.RequestRoom();
            indication.gameObject.SetActive(false);
        }
    }

    public void TeleportPlayerBack(float new_position_y)
    {
        is_teleporting = false;
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
