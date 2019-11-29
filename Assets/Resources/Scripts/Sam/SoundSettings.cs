using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;


public class SoundSettings : MonoBehaviour
{
    private AudioSource audioSource;
    private int countLoop = 0;
    private float nextTurn = 0.0f;
    private float songPlayedDuration = 0.0f;

    public float beforeStart = 0.0f;
    public float repeat = 0.0f;
    public int randomMin = 0;
    public int randomMax = 0;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        beforeStart += Time.time;
    }

    private bool SongIsPlaying()
    {
        if (Time.time > songPlayedDuration)
        {
            return false;
        }
        return true;
    }

    // Return a number between 0 and the variable max
    public int RandomNumber(int min, int max)
    { return UnityEngine.Random.Range(min, max); }

    private void Play()
    {
        songPlayedDuration = Time.time + audioSource.time;
        audioSource.Play();
        countLoop += 1;
    }

    private void Update()
    {
        if (countLoop <= repeat)
        {
            if (Time.time > beforeStart)
            {
                if (!SongIsPlaying())
                {
                    Play();
                }
                beforeStart += (float)RandomNumber(randomMin, randomMax);
            }
        }
    }
}