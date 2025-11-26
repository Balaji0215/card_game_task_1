using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.Netcode;
using System.Collections;
using System;

public class HandUI_manger : NetworkBehaviour
{
    [Header("UI Panels")]
    public Transform handpanel;      // Host hand
    public Transform handpanel_2;    // Client hand
    public Transform targetParent;   // Host board
    public Transform targetParent_3; // Client board

    [Header("UI & Database")]
    public GameObject uiCardPrefab;
     public GameObject CardPrefab_net;
    public Card_game.cardDatabase cardDatabase;
    public TextMeshProUGUI turn_text,Score_No;

     public Buttoncarddata selectedData;
    private Transform selectedCardTransform;
    private int turn = 0;

     private GameObject uiCard;
    
     public PlayerNetwork playernetwork;
    

    private void Start()
    {

     Card_game dbObj = FindObjectOfType<Card_game>();
    if (dbObj != null)
    {
        cardDatabase = dbObj.carddata;
        Debug.Log("HandUI using card database with " + cardDatabase.datalist.Count + " cards");
    }
        // Attach local cards when player joins
        NetworkManager.Singleton.OnClientConnectedCallback += (id) =>
        {
            if (id == NetworkManager.Singleton.LocalClientId)
                AttachLocalHandCards();
        };

          
            if(IsHost)AttachLocalHandCards();
    }



  

    

    // Only attach local player's hand cards
void AttachLocalHandCards()
{
    Transform hand = IsHost ? handpanel : handpanel_2;
     
    if (IsHost)
{
    handpanel_2.gameObject.SetActive(false);
    handpanel.gameObject.SetActive(true);

}
else
{
    handpanel_2.gameObject.SetActive(true);

    handpanel.gameObject.SetActive(false);
}

         
    
   
    foreach (Transform child in hand)
        Destroy(child.gameObject);
        
     
    foreach (var card in cardDatabase.datalist)
    {

        Debug.Log("working");
             uiCard = Instantiate(uiCardPrefab, hand);
             

       
        TextMeshProUGUI[] txts = uiCard.GetComponentsInChildren<TextMeshProUGUI>();
        if (txts.Length >= 5)
        {
            txts[0].text = card.cardName;
            txts[1].text = card.cost.ToString();
            txts[2].text = card.power.ToString();
            txts[3].text = card.Ability;
            txts[4].text = card.value.ToString();
        }

       
        Buttoncarddata data = uiCard.GetComponent<Buttoncarddata>();
        data.cardName = card.cardName;
        data.cost = card.cost;
        data.power = card.power;
        data.Ability = card.Ability;
        data.value = card.value;

        


        Button btn = uiCard.GetComponent<Button>();
        btn.onClick.AddListener(() => OnCardSelected(data));
    }
}
public void AssignNetworkCard(string cardName, NetworkCard networkCard)
    {
        
         StartCoroutine(TryAssign(cardName, networkCard));
        
    }


 private IEnumerator  TryAssign(string cardName, NetworkCard networkCard)
{
    Buttoncarddata uiCard = null;

    // Keep trying until UI card exists
    while (uiCard == null)
    {
        foreach (var card in FindObjectsOfType<Buttoncarddata>())
        {
            if (card.cardName == cardName)
            {
                uiCard = card;
                break;
            }
        }

        if (uiCard == null)
        {
            yield return null; 
            continue;
        }
    }

    uiCard.networkCard = networkCard;
    Debug.Log("Linked NetworkCard to UI: " + cardName);
}
   

    // Called when a card is clicked
  public void OnCardSelected(Buttoncarddata data)
{
     selectedData = data;

      

    if (data.networkCard == null)
    {
        Debug.LogError("This UI card has NO linked NetworkCard!");
        return;
    }  
 
      

    selectedCardTransform = data.networkCard.transform;

    Debug.Log("Selected card: " + data.cardName);

    
} 


    // Called when "Use Card" button is pressed
    public void OnUseSelectedCard()
    {
        if (turn == 6)
        {
            return;
        }
           inactive();
           UpdateScore();
       
        NetworkObject netObj = selectedCardTransform.GetComponent<NetworkObject>();
        
        turn += 1;

        

        turn_text.text = turn.ToString();
          
        if (netObj == null)
        {
            Debug.LogError("Selected card has no NetworkObject!");
            return;
        }

        PlayerNetwork.LocalPlayer.MoveCardServerRpc(netObj.NetworkObjectId);


        
        // Update turn
        

        // Clear selection
        selectedData = null;
        selectedCardTransform = null;
    }


       public void inactive(){
          

         selectedData.gameObject.SetActive(false);

        }
      




   private int totalScore = 0;

   public void UpdateScore()
{
     

    int score = selectedData.power + selectedData.value;
    
    totalScore += score;

     

    Score_No.text = totalScore.ToString();
   Debug.Log("Total Score: " + totalScore);
    
    
}



    
}
