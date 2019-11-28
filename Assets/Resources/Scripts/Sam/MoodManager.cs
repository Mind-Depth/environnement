using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Collections;
using System.Linq;

namespace Sam
{
    public class MoodManager : MonoBehaviour
    {
        public int computingThreshold = 2;

        float mood = 100.0f;
        float sinceLastSetMood = 0;
        int countUserFeelingChangement = 0;
        float userFeelingVariation = 0;

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

        public void IncrementUserFeelingChangement()
        {
            this.countUserFeelingChangement += 1;
        }

        public float GetUserFeelingVariation(float timeSpentInGame)
        {
            float userFeelingVariation = 0;

            if (timeSpentInGame != 0)
            {
                userFeelingVariation = (this.countUserFeelingChangement / timeSpentInGame) * 100;
            }
            return userFeelingVariation;
        }

        public void SetUserFeelingVariation(float variation)
        {
            userFeelingVariation = variation;
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

        public void ComputeMood(float fearLevel, float timeSinceRoomStarted)
        {
            float variation = userFeelingVariation / 100;
            if (variation != 0) {
                if (fearLevel == 0)
                {
                    SetMood(this.mood - ((1 * variation) + variation) * (Time.time - timeSinceRoomStarted));
                }
                else if (fearLevel == 1)
                {
                    SetMood(this.mood + ((1 * variation) + variation) * (Time.time - timeSinceRoomStarted));
                }
            } else {
                if (fearLevel == 0)
                {
                    SetMood(this.mood - 1);
                }
                else if (fearLevel == 1)
                {
                    SetMood(this.mood + 1);
                }
            }
        }
    } 
}