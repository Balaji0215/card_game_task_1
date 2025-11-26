
using System.Collections.Generic;
using UnityEngine;


public class Card_game : MonoBehaviour{
  
[Header("Enter Card Data")]
public cardDatabase carddata = new cardDatabase();

[System.Serializable]
public class cardDatabase
{ 
 public List<Card_data> datalist = new List<Card_data>();
}

[System.Serializable]
public class Card_data
{
    public string cardName;
    public int cost;
    public int power;

    public string Ability;

    public int value;
}
void Awake() {
      Loaddata();
    }


   void Start() {

   
    }

 void Update()
    {
         if(Input.GetKeyDown(KeyCode.S)){


            Savedata();
         }

        if (Input.GetKeyDown(KeyCode.L))
        {
            
        
            Loaddata();
         
       }

    }
    
    public void Savedata(){

    string  datalist = JsonUtility.ToJson(carddata);

    string  path = Application.persistentDataPath + "/carddata.json";

    Debug.Log(path);

     System.IO.File.WriteAllText(path, datalist);

     Debug.Log("Data Saved");

 }



public void Loaddata(){

    string path = Application.persistentDataPath + "/carddata.json";

    string datalist = System.IO.File.ReadAllText(path);

    carddata = JsonUtility.FromJson<cardDatabase>(datalist);

    Debug.Log("Data Loaded");

    printcard();


}




void printcard()
    {

     foreach( var cards in carddata.datalist)
        {
            Debug.Log("Card Name:" + cards.cardName + " Cost:" + cards.cost + " Power:" + cards.power);
        }     

   


    }
            
}
  