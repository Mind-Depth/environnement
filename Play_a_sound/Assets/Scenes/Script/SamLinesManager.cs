using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Collections;

namespace Sam
{
    public class SamLinesManager
    {
        private string[]        samLines;
        private string          language;
        

        private SamLinesJson    samLinesJson;
        private Lines           lines;
        private List<Line>      samLinesObject;


        private AudioSource     audioSource;
        private SoundManager    soundManager;


        // Mood Sam Line
        private List<Line> happyLines = new List<Line>();
        private List<Line> exitementLines = new List<Line>();
        private List<Line> cynicalLines = new List<Line>();
        private List<Line> frustratedLines = new List<Line>();
        private List<Line> angerLines = new List<Line>();

        public SamLinesManager(AudioSource audioSource, string language)
        {
            this.audioSource = audioSource;
            this.language = language;

            samLinesJson = new SamLinesJson(this.language);
            lines = samLinesJson.LoadJSONLines();
            Debug.Log(lines.presentation);
            samLinesObject = samLinesJson.GetLines();
           
            soundManager = new SoundManager(audioSource, "SamLines/" + this.language + "/");

        }

        public Lines GetSamLines()
        {
            return this.lines;
        }

        public Line GetSamPrez()
        {
            return this.lines.presentation;
        }

        public void SetLinesByMood(List<Line> samLines)
        {
            Debug.Log("SetLinesByMood > " + samLines[0].name);

            for (int i = 0; i < samLines.Count; i++)
            {
                if (samLines[i].mood == "happy") // Attention il y en a une sur toi ahahah!
                {
                    this.happyLines.Add(samLines[i]);
                }
                else if (samLines[i].mood == "exitement") //Tu te sent bien là hu huhu...
                {
                    this.exitementLines.Add(samLines[i]);
                }
                else if (samLines[i].mood == "cynical") // Elles vont pas te manger tu sais... Enfin ..
                {
                    this.cynicalLines.Add(samLines[i]);
                }
                else if (samLines[i].mood == "frustrated") // C'est sur que si tu restes dans ton coin aussi !
                {
                    this.frustratedLines.Add(samLines[i]);
                }
                else if (samLines[i].mood == "anger") // Tu fais le malin ! mais change de salle pour voir !
                {
                    this.angerLines.Add(samLines[i]);
                }
            }
        }

        public string GetLanguage()
        { return this.language; }

        public void SetLanguage(string language)
        {
            this.language = language;
            soundManager.SetPathFolder("SamLines/" + this.language + "/");
        }

        public AudioSource GetAudioSource()
        { return audioSource; }

        public void SetAudioSource(AudioSource audioSource)
        {
            this.audioSource = audioSource;
            soundManager.SetAudioSource(this.audioSource);
        }

        public string[] GetSamLinesName()
        {
            /*Debug.Log("samelinesJSON Object = " + samLinesJson);*/
            samLines = samLinesJson.LinesToStringArray(lines);
            
            return samLines;
        }

        public List<Line> GetLinesByMood(string mood)
        {
            if (mood == "happy") // Attention il y en a une sur toi ahahah!
            {
                return this.happyLines;
            } else if (mood == "exitement") // Tu te sent bien là hu huhu...
            {
                return this.exitementLines;
            } else if (mood == "cynical") // Elles vont pas te manger tu sais... Enfin ..
            {
                return this.cynicalLines;
            } else if (mood == "frustrated") // C'est sur que si tu restes dans ton coin aussi !
            {
                return this.frustratedLines;
            } else if (mood == "anger") // Tu fais le malin ! mais change de salle pour voir !
            {
                return this.angerLines;
            }
            return this.cynicalLines;
        }

        public List<Line> GetSamLinesObject()
        {
            return this.samLinesObject;
        }

        /*public Line FindSamLineObjectByName(string name)
        {
            Line samLineObject ;

            for (int i = 0; i < lines.lines.lenght; i++)
            {
                Debug.Log("--- " + lines + " ---");
            }
            return samLineObject;
        }
        */
        // Use the sound manager for load and play the sound asked in parameter.
        public void Play(string songToPlay)
        {
            soundManager.LoadSound(songToPlay);
            soundManager.PlaySound();
        }
    }

}
