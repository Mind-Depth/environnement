using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Mesh initialMesh = this.GetComponent<MeshFilter>().mesh;
        for (int i = 0; i < initialMesh.triangles.Length; i += 3) {
            Debug.Log(initialMesh.triangles[i] + " " + initialMesh.triangles[i + 1] + " " + initialMesh.triangles[i + 2]);
        }
    }
}
