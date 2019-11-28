using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Sam
{
    public enum SamLyricStates : int { NO_TALK, CALL_TO_ACTION, AMBIANCES };
    public enum OrderStates : int { NO_ORDER, ORDER_HAS_BEEN_GIVEN, ORDER_HAS_BEEN_DONE };
    public enum MindStates : int { HELPER, PLOT_TWIST, PSYCHOPATHE };
    public enum GameStates : int { INTRODUCTION, PLAY_MODE, INTER_STATE };

   
    public class States
    {
        SamLyricStates samLyricState;
        OrderStates orderState;
        MindStates mindState;
        GameStates gameState;

        List<String> samLyricStatesValues = new List<String> { "NO_TALK", "CALL_TO_ACTION", "AMBIANCES" };
        List<String> orderStatesValues = new List<String> { "NO_ORDER", "ORDER_HAS_BEEN_GIVEN", "ORDER_HAS_BEEN_DONE" };
        List<String> mindStatesValues = new List<String> { "HELPER", "PLOT_TWIST", "PSYCHOPATHE" };
        List<String> gameStatesValues = new List<String> { "INTRODUCTION", "PLAY_MODE", "INTER_STATE" };

        public States()
        {
            this.samLyricState = SamLyricStates.NO_TALK;
            this.orderState = OrderStates.NO_ORDER;
            this.mindState = MindStates.HELPER;
            this.gameState = GameStates.INTRODUCTION;
        }

        // Setters

        public void SetSamLyricState(SamLyricStates state)
        { this.samLyricState = state; }

        public void SetOrderState(OrderStates state)
        { this.orderState = state; }

        public void SetMindState(MindStates state)
        { this.mindState = state; }

        public void SetGameState(GameStates state)
        { this.gameState = state; }

        // Getters

        public SamLyricStates GetSamLyricState()
        { return this.samLyricState; }

        public OrderStates GetOrderState()
        { return this.orderState; }

        public MindStates GetMindState()
        { return this.mindState; }

        public GameStates GetGameState()
        { return this.gameState; }

        public String GetSamLyricStateValue()
        { return this.samLyricStatesValues[(int)this.samLyricState]; }

        public String GetOrderStatesValue()
        { return this.orderStatesValues[(int)this.orderState]; }

        public String GetMindStatesValue()
        { return this.mindStatesValues[(int)this.mindState]; }

        public String GetGameStatesValue()
        { return this.gameStatesValues[(int)this.gameState]; }

        // Override

        public override string ToString()
        {
            return this.GetSamLyricStateValue() + " " +
                this.GetOrderStatesValue() + " " +
                this.GetMindStatesValue() + " " +
                this.GetGameStatesValue();
        }
    }
}