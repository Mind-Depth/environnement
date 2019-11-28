using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Collections;
using System.Linq;

namespace Sam
{
    public class MoodHelpingManager : MonoBehaviour
    {
        public int computingThreshold = 2;

        float mood = 100.0f;
        float sinceLastSetMood = 0;

        public MoodHelpingManager(float mood)
        {
            this.mood = mood;
        }

        public string GetMoodName()
        {
            if (this.mood > MoodHelping.NORMAL)
            {
                return "normal";
            } else if (this.mood > MoodHelping.NERVOUS && this.mood < MoodHelping.NORMAL)
            {
                return "nervous";
            } else if (this.mood > MoodHelping.FEARED && this.mood < MoodHelping.NERVOUS)
            {
                return "feared";
            }
            return "nervous";
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

        public void ComputeMood(OrderStates state)
        {
            if (state == OrderStates.ORDER_HAS_BEEN_GIVEN)
            {
                this.mood -= 1;
            }
            else if (state == OrderStates.ORDER_HAS_BEEN_DONE)
            {
                this.mood = 100;
            }
        }
    } 
}