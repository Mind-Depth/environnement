using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

// Loading dynamically a sound and play it.

/*
// Sources :
// yield key word https://www.infoworld.com/article/3122592/my-two-cents-on-the-yield-keyword-in-c.html
// Comments norms https://docs.microsoft.com/en-us/dotnet/csharp/codedoc
// AudioSource https://docs.unity3d.com/2018.3/Documentation/ScriptReference/AudioSource.html
// AudioClip https://docs.unity3d.com/2018.3/Documentation/ScriptReference/AudioClip.html
// Inspiration https://www.youtube.com/watch?v=9gAHZGArDgU
// String.Format method https://docs.microsoft.com/en-us/dotnet/api/system.string.format?view=netframework-4.8
*/


public class PlayLoadSound : MonoBehaviour
{
    private string[] samLines;

    private AudioSource  source;
    public  string       soundPath;

	
    void Awake()
    {
        string resourcesPath = Application.dataPath + "/Resources";

        samLines = new String[] { "GrabMeABeer", "TheWall", "Fail" };
        source = GetComponent<AudioSource>();
        soundPath = resourcesPath + "/SamLines/";
    }

    // Load one of the pick up lines of sam
    private void GetSound(string lineToPlay)
    {
        if (!File.Exists(soundPath + lineToPlay + ".mp3"))
            Debug.LogError(": " + soundPath + lineToPlay + ".mp3" + " : Do not exist");

        source.clip = Resources.Load<AudioClip>("SamLines/" + lineToPlay);
    }

    // Play the last Same line loaded
    private void PlayAudioFile()
    {
        source.Play();
    }

    // Return a number between 0 and the variable max
    public int RandomNumber(int max)
    {
        return UnityEngine.Random.Range(0, max);
    }

    void Update()
    {
		if (Input.GetKeyDown(KeyCode.Space)) {
            int lineToPlay = RandomNumber(samLines.Length);
            
            GetSound(samLines[lineToPlay]);
            PlayAudioFile();
        }
    }
}
