using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
public class WallCrush : MonoBehaviour
{
    public float speed = 0.2f;
    public GameObject[] wallLeft;
    public GameObject[] wallRight;

    public GameObject[] lightMaterial;

    public GameObject[] light;

    public Transform pointEndLeft;
    public Transform pointEndRight;

    public PostProcessVolume ppv;
    public PostProcessProfile ppp;

    private bool move = false;

    // Update is called once per frame
    void Update()
    {
        if (move) {
            GoAhead();
        }
    }

    private void GoAhead() {
        for (int i = 0; i < wallLeft.Length; i++) {
            wallLeft[i].transform.position = new Vector3(wallLeft[i].transform.position.x, wallLeft[i].transform.position.y, wallLeft[i].transform.position.z + (pointEndLeft.position.z - wallLeft[i].transform.position.z) * Time.deltaTime * speed);
            wallRight[i].transform.position = new Vector3(wallRight[i].transform.position.x, wallRight[i].transform.position.y, wallRight[i].transform.position.z + (pointEndRight.position.z - wallRight[i].transform.position.z) * Time.deltaTime * speed);
        }
    }

    private void OnTriggerEnter(Collider other) {
        Debug.Log("wuut");
        Debug.Log(other.tag);
        if (other.tag == "Player") {
            ppv = ResourcesManager._instance.getPPV();
            ppv.profile = ppp;
            move = true;
            for (int i = 0; i < light.Length; i++) {
                light[i].SetActive(true);
            }
            for (int i = 0; i < lightMaterial.Length; i++) {
                lightMaterial[i].SetActive(true);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player") {
            move = false;
/*             for (int i = 0; i < light.Length; i++) {
                light[i].SetActive(false);
            }
            for (int i = 0; i < lightMaterial.Length; i++) {
                lightMaterial[i].SetActive(false);
            } */
        }
    }
}
