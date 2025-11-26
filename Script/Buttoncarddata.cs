using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class Buttoncarddata : MonoBehaviour
{  
    public string cardName;
    public int cost;
    public int power;
    public string Ability;
    public int value;

    public NetworkCard networkCard;

    void Start()
    {
       
    }

    public void PrintData()
    {
        Debug.Log("CARD SELECTED:");
        Debug.Log("Name: " + cardName);
        Debug.Log("Cost: " + cost);
        Debug.Log("Power: " + power);
        Debug.Log("Ability: " + Ability);
    }
}

