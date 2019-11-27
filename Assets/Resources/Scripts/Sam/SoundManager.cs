using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

// Deserialise JSON https://stackoverflow.com/questions/36239705/serialize-and-deserialize-json-and-json-array-in-unity

namespace Sam
{
    public class SoundManager
    {
        public string pathFolder;
        public string resourcePath = Application.dataPath + "/Resources/";

        private AudioSource source;

        public SoundManager(AudioSource s,string pathFolder)
        {
            this.source = s;
            this.pathFolder = pathFolder;
        }

        public string GetPathFolder()
        {
            return this.pathFolder;
        }

        public void SetPathFolder(string pathFolder)
        {
            this.pathFolder = pathFolder;
        }

        public AudioSource GetAudioSource()
        {
            return this.source;
        }

        public void SetAudioSource(AudioSource s)
        {
            this.source = s;
        }

        // Load one of the pick up lines of sam
        public void LoadSound(string lineToPlay)
        {
            if (!File.Exists(this.resourcePath + this.pathFolder + lineToPlay + ".wav"))
                Debug.LogError(":LoadSound: " + this.resourcePath + this.pathFolder + lineToPlay + ".wav" + " : Do not exist");
            
            source.clip = Resources.Load<AudioClip>(this.pathFolder + lineToPlay);
        }


        // Play the last Same line loaded
        public void PlaySound()
        {
            source.Play();
        }

    }
}
