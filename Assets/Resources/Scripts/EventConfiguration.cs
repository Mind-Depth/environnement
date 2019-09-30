using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


/* 
 * Classe contenant les informations d'un événement.
 * Utilisée par le RessourceManager
 */

[Serializable]
public class EventConfiguration : MonoBehaviour
{
    public string id;
    public string description;
    public List<ModelConfiguration.Type> type_assets_needed = new List<ModelConfiguration.Type>(); /* la liste des assets qui sont nécessaire à l'event */
    public List<Fear> fears = new List<Fear>();
}
