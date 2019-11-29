using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Collections;
using UnityEngine.SceneManagement;

namespace Sam
{
    public class TriggerManager : MonoBehaviour
    {
        public GameObject spwanerPrefab = null;
        public static TriggerManager _instance = null;

        private List<string> mood = new List<string>();
        private List<string> rooms = new List<string>();
        private List<string> currentFear = new List<string>();
        private List<History> histories = new List<History>();

        private string[]        samLines;
        private List<Line>      samLinesObject;
        public string           lang;

        private SamLinesManager samLineManager;
        private MoodManager     moodIntroduction;
        private HistoryManager  historyManager;
        private AudioSource     audioSource;
        private string          currenMood;
        private List<Line>      samIntroductionLines;

        public float startTime = 0.0f;
        public float targetTime = 5.0f;

        public float startAmbianceLine = 10.0f;
        public float startPrez = 1.0f;

        public int intervalTurn = 1;
        private float nextTurn = 0.0f;
        private int fearLevel = 0;
        public OrderStates orderState = OrderStates.NO_ORDER;
        public SamLyricStates samLyricStates = SamLyricStates.NO_TALK;
        public GameStates gameStates = GameStates.INTRODUCTION;
        private States states;
        private Room currentRoom;
        private Room oldRoom;
        private MoodManager moodPlayMode;
        private MoodHelperManager moodHelperManager;

        public MindStates mindStates = MindStates.PSYCHOPATHE;
        private MindStates oldMindStates = MindStates.HELPER;

        private float userFeelingVariation = 0;

        private int stepClaustro = 0;

        void Awake()
        {
            // Singleton
            if (_instance == null)
            {
                _instance = this;
            }
            else if (_instance != this)
            {
                Destroy(this);
            }

            audioSource = GetComponent<AudioSource>();
            samLineManager = new SamLinesManager(audioSource, lang);

            moodIntroduction = new MoodManager(50.0f);
            moodPlayMode = new MoodManager(50.0f);
            moodHelperManager = new MoodHelperManager(100.0f);
            states = new States();
            currentRoom = new Room();
            currentRoom.SetRoomName("none");
            oldRoom = new Room();
            oldRoom.SetRoomName("none");
            states.SetMindState(MindStates.HELPER);
        }

        // Return a number between 0 and the variable max
        public int RandomNumber(int max)
        { return UnityEngine.Random.Range(0, max); }

        private void ConfigureSoundFirstRoom()
        {
            samLineManager.AddToPipe(samLineManager.FindIntroductionByName("intro_allumage_de_la_radio"));
            samLineManager.PausePipe(3.0f);
            samLineManager.AddToPipe(samLineManager.FindIntroductionByName("depeche_toi_sors_d_ici_il_arrive"));
            samLineManager.PausePipe(3.0f);
            samLineManager.AddToPipe(samLineManager.FindIntroductionByName("vas_au_fond_et_sors_de_suite"));
            samLineManager.PausePipe(1.5f);
            samLineManager.AddToPipe(samLineManager.FindIntroductionByName("la_porte_au_fond_sors_de_la"));
            samLineManager.PausePipe(1.0f);
            samLineManager.AddToPipe(samLineManager.FindIntroductionByName("sors_de_la_putain"));
        }

        private void ConfigureSoundSecondRoom()
        {
            samLineManager.AddToPipe(samLineManager.FindIntroductionByName("okay_ca_c_est_regle"));
            samLineManager.PausePipe(2.0f);
            samLineManager.AddToPipe(samLineManager.FindIntroductionByName("je_m_appelle_sam_1"));
            samLineManager.AddToPipe(samLineManager.FindIntroductionByName("premiere_chose_a_faire_rallumer_la_lumiere"));
        }

        private void ConfigureSoundSecondRoomAfterCta()
        {
            if (states.GetMindState() == MindStates.HELPER)
            {
                samLineManager.AddToPipe(samLineManager.FindIntroductionByCTA("lever"));
                samLineManager.PausePipe(1.0f);
                samLineManager.AddToPipe(samLineManager.FindIntroductionByName("premiere_chose_a_faire_passer_dans_une_nouvelle_salle"));
                samLineManager.AddToPipe(samLineManager.FindIntroductionByName("avance_quoi_qu_il_arrive"));
                samLineManager.AddToPipe(samLineManager.FindIntroductionByName("quoi_que_tu_puisses_voir"));
            }
        }

        private void ConfigureSoundClaustrophobiaRoom()
        {
            if (states.GetMindState() == MindStates.HELPER)
            {
                samLineManager.PausePipe(3.0f);
                samLineManager.AddToPipe(samLineManager.FindHelperByName("bouge_plus_les_murs_vont_arreter_de_bouger"));
                samLineManager.PausePipe(1.5f);
                samLineManager.AddToPipe(samLineManager.FindHelperByName("reste_immobile_3"));
                samLineManager.PausePipe(1.0f);
                samLineManager.AddToPipe(samLineManager.FindHelperByName("ah_merde_c_est_pas_cette_salle"));
            }
            else if (states.GetMindState() == MindStates.PSYCHOPATHE)
            {
                samLineManager.PausePipe(3.0f);
                samLineManager.AddToPipe(samLineManager.FindPsychopatheByName("tu_sais_que_ca_va_etre_galere_pour_nettoyer"));
                samLineManager.PausePipe(1.5f);
                samLineManager.AddToPipe(samLineManager.FindPsychopatheByName("fais_moi_confiance_ne_bouge_pas"));
                samLineManager.PausePipe(1.0f);
                samLineManager.AddToPipe(samLineManager.FindPsychopatheByName("il_y_a_bien_ta_tete_qui_va_bloquer_l_avancement_des_murs"));
                samLineManager.PausePipe(2.0f);
                samLineManager.AddToPipe(samLineManager.FindPsychopatheByName("ah_mais_merde_on_dirait_quelqu_un_qui_va_mourrir"));
                samLineManager.PausePipe(3.0f);
                samLineManager.AddToPipe(samLineManager.FindPsychopatheByName("on_arrive_a_ma_partie_preferee"));
            }
        }

        private void ConfigureSoundVertigoRoom()
        {
            if (states.GetMindState() == MindStates.HELPER)
            {
                samLineManager.PausePipe(3.0f);
                samLineManager.AddToPipe(samLineManager.FindHelperByName("ne_prend_pas_la_passerelle_2"));
                samLineManager.PausePipe(2.0f);
                samLineManager.AddToPipe(samLineManager.FindHelperByName("ok_tkt_pas"));
                samLineManager.PausePipe(2.0f);
                samLineManager.AddToPipe(samLineManager.FindHelperByName("aller_je_sais_que_tu_peux_le_faire"));
                samLineManager.PausePipe(3.0f);
                samLineManager.AddToPipe(samLineManager.FindHelperByName("il_faut_que_tu_sortes_d_ici"));
                samLineManager.PausePipe(4.0f);
                samLineManager.AddToPipe(samLineManager.FindHelperByName("tu_sautes_ou_je_t_y_pousse"));
                samLineManager.PausePipe(4.0f);
                samLineManager.AddToPipe(samLineManager.FindHelperByName("saute_putain"));
            } else if (states.GetMindState() == MindStates.PSYCHOPATHE)
            {
                samLineManager.PausePipe(3.0f);
                samLineManager.AddToPipe(samLineManager.FindPsychopatheByName("douce_melodie_des_os_s_ecraser_sur_le_sol"));
                samLineManager.PausePipe(3.0f);
                samLineManager.AddToPipe(samLineManager.FindPsychopatheByName("il_y_a_deja_tes_petits_camarades_en_bas"));
                samLineManager.PausePipe(3.0f);
                samLineManager.AddToPipe(samLineManager.FindPsychopatheByName("je_t_en_supplie_saute_ce_erait_tellement_bon"));
                samLineManager.PausePipe(3.0f);
                samLineManager.AddToPipe(samLineManager.FindPsychopatheByName("saute_dans_ce_putain_de_trou"));
                samLineManager.PausePipe(3.0f);
                samLineManager.AddToPipe(samLineManager.FindPsychopatheByName("tu_m_as_fais_confiance_jusqu_ici_et_tu_arretes"));

            }
        }

        private void ConfigureSoundVertigoRoomCTA()
        {
            if (states.GetMindState() == MindStates.HELPER)
            {
                // response unnecessary.
            }
        }

        private void ConfigureSoundArachnophobiaRoom()
        {
            if (states.GetMindState() == MindStates.HELPER)
            {
                samLineManager.PausePipe(3.0f);
                samLineManager.AddToPipe(samLineManager.FindHelperByName("ouvre_la_trappe"));
                samLineManager.PausePipe(1.5f);
                samLineManager.AddToPipe(samLineManager.FindHelperByName("ouvre_cte_trappe"));
                samLineManager.PausePipe(1.0f);
                samLineManager.AddToPipe(samLineManager.FindHelperByName("fais_ce_que_je_te_dis"));
                samLineManager.PausePipe(2.0f);
                samLineManager.AddToPipe(samLineManager.FindHelperByName("fais_ce_que_je_te_dis_tu_veux_mourrir"));
            } else if (states.GetMindState() == MindStates.PSYCHOPATHE)
            {
                samLineManager.PausePipe(3.0f);
                samLineManager.AddToPipe(samLineManager.FindPsychopatheByName("des_que_ca_a_pris_gout_a_la_viande"));
                samLineManager.PausePipe(1.5f);
                samLineManager.AddToPipe(samLineManager.FindPsychopatheByName("tu_pourras_pas_fuir_toute_ta_vie"));
                samLineManager.PausePipe(1.0f);
                samLineManager.AddToPipe(samLineManager.FindPsychopatheByName("creve"));
                samLineManager.PausePipe(2.0f);
                samLineManager.AddToPipe(samLineManager.FindPsychopatheByName("tu_kiffes_hein"));
                samLineManager.PausePipe(3.0f);
                samLineManager.AddToPipe(samLineManager.FindPsychopatheByName("oh_oui_j_adoooore"));
            }
        }

        private void ConfigureSoundNyctophobiaRoom()
        {
            if (states.GetMindState() == MindStates.HELPER)
            {
                samLineManager.PausePipe(3.0f);
                samLineManager.AddToPipe(samLineManager.FindHelperByName("dans_cette_salle_il_faut_rester_discret"));
                samLineManager.PausePipe(1.0f);
                samLineManager.AddToPipe(samLineManager.FindHelperByName("tu_devrais_trouver_un_interupteur"));
                samLineManager.PausePipe(2.0f);
                samLineManager.AddToPipe(samLineManager.FindHelperByName("il_va_falloir_eteindre_la_lumiere"));
                samLineManager.PausePipe(3.0f);
                samLineManager.AddToPipe(samLineManager.FindHelperByName("fais_moi_confiance"));
                samLineManager.PausePipe(2.0f);
                samLineManager.AddToPipe(samLineManager.FindHelperByName("il_faut_que_tu_eteignes_depeche_toi"));
                samLineManager.PausePipe(2.0f);
                samLineManager.AddToPipe(samLineManager.FindHelperByName("fais_moi_confiance_enerve"));
                samLineManager.PausePipe(3.0f);
                samLineManager.AddToPipe(samLineManager.FindHelperByName("fais_ce_que_je_te_dis_enerve"));
            } else if (states.GetMindState() == MindStates.PSYCHOPATHE)
            {
                
                samLineManager.PausePipe(2.0f);
                samLineManager.AddToPipe(samLineManager.FindPsychopatheByName("attention_de_ne_pas_marcher_sur_olivier"));
                samLineManager.PausePipe(2.0f);
                samLineManager.AddToPipe(samLineManager.FindPsychopatheByName("attention_derriere_toi_2"));
                samLineManager.PausePipe(3.0f);
                samLineManager.AddToPipe(samLineManager.FindPsychopatheByName("au_dela_du_cadavre_ca_sent_comme_dans_la_cave"));
            }
        }

        private void ConfigureSoundNyctophobiaRoomCTA()
        {
            if (states.GetMindState() == MindStates.HELPER)
            {
                samLineManager.AddToPipe(samLineManager.FindHelperByName("aller_on_continue"));
            }
        }

        private void ConfigureSoundArachnophobiaRoomCTA()
        {
            if (states.GetMindState() == MindStates.HELPER)
            {
                samLineManager.AddToPipe(samLineManager.FindHelperByName("alors_ca_a_marche"));
            }
        }

        private void ConfigureSoundClaustrophobiaRoomAfterCta()
        {
            if (stepClaustro == 0)
            {
                samLineManager.AddToPipe(samLineManager.FindHelperByName("bouge_plus"));
                stepClaustro += 1;
            } else if (stepClaustro == 1)
            {
                samLineManager.AddToPipe(samLineManager.FindHelperByName("je_t_ai_dis_de_pas_bouger"));
                stepClaustro += 1;
            } else if (stepClaustro == 2)
            {
                samLineManager.AddToPipe(samLineManager.FindHelperByName("bouuuuge_pluuus"));
                stepClaustro += 1;
                states.SetMindState(MindStates.PSYCHOPATHE);
            }
        }


        private void Update()
        {
            // TODO: Correctly call all SamLinesManager functions.
            // TODO: Modify mood with mood Manager and pass new moods in SamLinesManager class. 
            if (Time.time > nextTurn) {
                if (states.GetGameState() == GameStates.INTRODUCTION)
                {
                    Debug.Log("INTRODUCTION IS PLAYING.");
                } else if (states.GetGameState() == GameStates.PLAY_MODE)
                {
                    if (userFeelingVariation == 0) {
                        userFeelingVariation = moodIntroduction.GetUserFeelingVariation(Time.time);
                        moodPlayMode.SetUserFeelingVariation(userFeelingVariation);
                    }
                    moodPlayMode.ComputeMood(fearLevel, currentRoom.GetTimeSpent());
                }
                nextTurn += 1;
            }
            if (oldMindStates != mindStates)
            {
                states.SetMindState(mindStates);
                oldMindStates = mindStates;
            }
            if (!samLineManager.SongIsRunning())
            {
                samLineManager.PlayPipe();
            }
        }

        public void UpdateFear(float newFearLevel)
        {
            if ((int)newFearLevel != fearLevel)
            {
                fearLevel = (int)newFearLevel;
                moodIntroduction.IncrementUserFeelingChangement();
                //Debug.Log("DebugLog/ [SAM] received a fear level of " + fearLevel.ToString());
                //Console._instance.AddLog("ConsoleInstance/ [SAM] received a fear level of " + fearLevel.ToString());
            }
        }
        public void SpiderFearTrigger()
        {
            GameObject spiderSpwaner = Instantiate(spwanerPrefab, new Vector3(RandomNumber(200), 0, 0), Quaternion.identity);
            Spawner spSpwaner = spiderSpwaner.GetComponent<Spawner>();
            spSpwaner.minCount = 15;
            spSpwaner.maxCount = 20;
            spSpwaner.Activate(1);
        }

        public void UpdateRoomConfig(List<SamTags> tags, Fear fearType, float fearIntensity)
        {
            string msg = "[SAM] received a room config room " + fearType.ToString() + " and intensity " + fearIntensity.ToString();
            Debug.Log("UpdateRoomConfig -- " + msg);
            if (states.GetGameState() != GameStates.PLAY_MODE) {
                states.SetGameState(GameStates.PLAY_MODE);
            }
            currentRoom = new Room(fearType.ToString(), fearIntensity, fearType, tags, Time.time);
            if (currentRoom.GetRoomName() == "first_room")
            {
                states.SetGameState(GameStates.INTRODUCTION);
                samLineManager.CleanPipe();
                ConfigureSoundFirstRoom();
            }
            else if (currentRoom.GetRoomName() == "second_room")
            {
                states.SetGameState(GameStates.INTRODUCTION);
                samLineManager.CleanPipe();
                ConfigureSoundSecondRoom();
            }
            else if (currentRoom.GetRoomName() == "Claustrophobia")
            {
                samLineManager.CleanPipe();
                ConfigureSoundClaustrophobiaRoom();
            } else if (currentRoom.GetRoomName() == "Arachnophobia")
            {
                samLineManager.CleanPipe();
                ConfigureSoundArachnophobiaRoom();
            } else if (currentRoom.GetRoomName() == "Vertigo")
            {
                samLineManager.CleanPipe();
                ConfigureSoundVertigoRoom();
            } else if (currentRoom.GetRoomName() == "Nyctophobia")
            {
                samLineManager.CleanPipe();
                ConfigureSoundNyctophobiaRoom();
            }
            //samLineManager.CleanPipe();
            //Console._instance.AddLog("ConsoleInstance -- " + msg);
        }

        public void UpdateTriggerEvents()
        {
            string msg = "[SAM] received a event is trigger ";
            Debug.Log("UpdateTriggerEvents -- " + msg);
            if (currentRoom.GetRoomName() == "second_room")
            {
                samLineManager.CleanPipe();
                ConfigureSoundSecondRoomAfterCta();
            }
            if (currentRoom.GetRoomName() == "Claustrophobia") {
                samLineManager.CleanPipe();
                ConfigureSoundClaustrophobiaRoomAfterCta();
            } else if (currentRoom.GetRoomName() == "Arachnophobia")
            {
                samLineManager.CleanPipe();
                ConfigureSoundArachnophobiaRoomCTA();
            } else if (currentRoom.GetRoomName() == "Vertigo")
            {
                samLineManager.CleanPipe();
                ConfigureSoundVertigoRoomCTA();
            } else if (currentRoom.GetRoomName() == "Nyctophobia")
            {
                samLineManager.CleanPipe();
                ConfigureSoundNyctophobiaRoomCTA();
            }
            //Console._instance.AddLog("ConsoleInstance --" + msg);
        }
    }
}
