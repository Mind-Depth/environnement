using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 
 * Classe qui gère le lancement des évènements choisis par la partie génération.
 */
public abstract class Event : MonoBehaviour
{
    public abstract void ExecuteEvent(GenerationMessage message);
}
