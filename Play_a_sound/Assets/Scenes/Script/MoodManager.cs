using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Collections;

namespace Sam
{
    public static class Mood
    {
        public const float HAPPYNESS  = 80;
        public const float EXITEMENT  = 60;
        public const float CYNICAL    = 40;
        public const float FRUSTRATED = 20;
        public const float ANGER      = 0;
    }

    public class MoodManager : MonoBehaviour
    {
        private float mood = 100.0f;
        public int    computingThreshold = 2;

        public MoodManager(float mood)
        {
            this.mood = mood;
        }

        public string GetMoodName()
        {
            if (this.mood > Mood.HAPPYNESS)
            {
                return "happy";
            } else if (this.mood > Mood.EXITEMENT && this.mood < Mood.HAPPYNESS)
            {
                return "exitement";
            } else if (this.mood > Mood.CYNICAL && this.mood < Mood.EXITEMENT)
            {
                return "cynical";
            } else if (this.mood > Mood.FRUSTRATED && this.mood < Mood.CYNICAL)
            {
                return "frustrated";
            } else if (this.mood > Mood.ANGER && this.mood < Mood.FRUSTRATED)
            {
                return "anger";
            }
            return "cynical";
        }

        public float GetMoodValue()
        {
            return this.mood;
        }

        public void SetMood(float mood)
        {
            if (mood > -1 && mood < 101.0f)
            {
                this.mood = mood;
            } else if (mood < 0)
            {
                this.mood = 0.0f;
            } else if (mood > 100.0f)
            {
                this.mood = 100.0f;
            }
        }

        private float ComputeGap(float newValue, float oldValue)
        {
            return newValue - oldValue;
        }

        public float ComputeMood(List<History> histories)
        {
            float result = 0;

            if (histories.Count > computingThreshold)
            {
                for (int i = histories.Count - (computingThreshold); i < histories.Count; i++)
                {
                   result += ComputeGap(histories[i].fearLevelDelta, histories[i - 1].fearLevelDelta);
                }
                this.SetMood(this.mood + (result / computingThreshold));
            }
           
            return mood;
        }

    } 
}