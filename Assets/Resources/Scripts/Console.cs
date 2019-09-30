using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Console : MonoBehaviour
{
    public Text console;
    public static Console _instance = null;
    public string prompt = "[Mind Depths]> ";
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

    public void Display(bool active)
    {
        console.gameObject.SetActive(active);
    }

    public void AddLog(string log)
    {
        console.text += prompt + log + "\n";
    }

    public void ClearLog()
    {
        console.text = "";
    }
}
