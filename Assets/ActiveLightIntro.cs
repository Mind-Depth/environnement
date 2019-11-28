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
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ActiveLights() {
            ppv = ResourcesManager._instance.getPPV();
            ppv.profile = ppp;
            for (int i = 0; i < light.Length; i++) {
                light[i].SetActive(true);
            }
            for (int i = 0; i < lightMaterial.Length; i++) {
                lightMaterial[i].SetActive(true);
            }
    }
}
