using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
 * Lance le splash screen animé du logo de Mind-Depths
 */
public class Intro : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(launch_animation());
    }

    private IEnumerator launch_animation()
    {
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene("Game");
    }
}