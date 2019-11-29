using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SoundSettingsVariable {
    public float beforeStart;
    public float repeat;
    public int randomMin;
    public int randomMax;
    public bool loop;
}
public class IntroSoundManager : MonoBehaviour
{
    public GameObject SoundSettingsPrefab;
    public AudioClip[] SoundClips;
    public List<SoundSettings> SoundSettingsList = new List<SoundSettings>();
    public SoundSettingsVariable[] SoundSettingsVariables;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < SoundClips.Length; i++) {
            GameObject tmp = Instantiate(SoundSettingsPrefab, transform);
            SoundSettingsList.Add(tmp.GetComponent<SoundSettings>());
            SoundSettingsList[i].audioSource.clip = SoundClips[i];
            SoundSettingsList[i].soundSettingsVariables = SoundSettingsVariables[i];
            SoundSettingsList[i].soundSettingsVariables.beforeStart += Time.time;
            SoundSettingsList[i].audioSource.loop = SoundSettingsVariables[i].loop;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
