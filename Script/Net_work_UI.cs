using UnityEngine;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine.UI;
using UnityEditor;

public class SimpleNetworkUI : MonoBehaviour
{
    public Button hostButton;
    public Button clientButton;
    public GameObject menuPanel;  

    public bool hostswitch = false;
    public bool client_switch = false;


    void Start()
    {
      
       
       
     
    }
      



void Awake(){


     hostButton.onClick.AddListener(() =>{
      Debug.Log("host");
     NetworkManager.Singleton.StartHost();
     hide();
     hostswitch = true;
     });



     clientButton.onClick.AddListener(() =>
     {
         
         Debug.Log("client");
         NetworkManager.Singleton.StartClient();
          hide();
          client_switch = true;
     });
        

}


public void hide()
    {
        menuPanel.SetActive(false);


    }
   


   



    
}
