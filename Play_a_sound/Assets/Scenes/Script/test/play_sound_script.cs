using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// yield key word https://www.infoworld.com/article/3122592/my-two-cents-on-the-yield-keyword-in-c.html



public class play_sound_script : MonoBehaviour
{
	public AudioClip sound;
	private AudioSource source;

	public float volLowRange = .5f;
	public float volHighRange = 1.0f;
    // Start is called before the first frame update
    void Awake()
    {
		source = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
		if (Input.GetKeyDown(KeyCode.Space)) {
			Debug.Log("salut");
			source.Play();
		}
    }
}
