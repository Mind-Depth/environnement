using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
public class ActiveLightIntro : MonoBehaviour
{
    public GameObject[] lightMaterial;

    public GameObject[] light;

    public PostProcessVolume ppv;
    public PostProcessProfile ppp;
    public PostProcessProfile pppSave;
    public bool ActiveLight = true;
    // Start is called before the first frame update
    void Start()
    {
        if (pppSave)
        {
            ppv = ResourcesManager._instance.getPPV();
            ppv.profile = ppp;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ActiveLights() {
            ppv = ResourcesManager._instance.getPPV();
            ppv.profile = pppSave;
            for (int i = 0; i < light.Length; i++) {
                light[i].SetActive(ActiveLight);
            }
            for (int i = 0; i < lightMaterial.Length; i++) {
                lightMaterial[i].SetActive(ActiveLight);
            }
    }
}
