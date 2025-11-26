using UnityEngine;
using Unity.Netcode;
using TMPro;
public class PlayerNetwork : NetworkBehaviour
{
    public static PlayerNetwork LocalPlayer;
    public GameObject cardPrefab; 
      public NetworkCard nc;
     public HandUI_manger handUI_;// network card prefab

    public override void OnNetworkSpawn()
    {
        if (IsOwner)
            LocalPlayer = this;

        // Manually spawn cards for the player (host)
        if (IsServer)
            SpawnCardsForPlayer(OwnerClientId);
    }

    public void SpawnCardsForPlayer(ulong clientId)
    {
        var db = FindObjectOfType<Card_game>();
        HandUI_manger ui = FindObjectOfType<HandUI_manger>();
        if (db == null) return;

        foreach (var data in db.carddata.datalist)
        {
            GameObject go = Instantiate(cardPrefab);

            TextMeshProUGUI[] txts = go.GetComponentsInChildren<TextMeshProUGUI>();

             if (txts.Length >= 5)
        {
            txts[0].text = data.cardName;
            txts[1].text = data.cost.ToString();
            txts[2].text = data.power.ToString();
            txts[3].text = data.Ability;
            txts[4].text = data.value.ToString();
        }

            NetworkObject netObj = go.GetComponent<NetworkObject>();

            NetworkCard nc = go.GetComponent<NetworkCard>();

            netObj.SpawnWithOwnership(clientId);

            nc.SetData(data);

             ui.AssignNetworkCard(data.cardName, nc);

             LinkCardToUIClientRpc(data.cardName, netObj.NetworkObjectId);
           
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void MoveCardServerRpc(ulong cardId)
    {
        var netObj = NetworkManager.Singleton.SpawnManager.SpawnedObjects[cardId];
        HandUI_manger ui = FindObjectOfType<HandUI_manger>();

        Transform board = (netObj.OwnerClientId == 0)
            ? ui.targetParent
            : ui.targetParent_3;

        netObj.TrySetParent(board);
        MoveCardClientRpc(cardId);
    }

    [ClientRpc]
    void MoveCardClientRpc(ulong cardId)
    {
        if (NetworkManager.Singleton.SpawnManager.SpawnedObjects.TryGetValue(cardId, out NetworkObject netObj))
        {
            // Optional: visual update for all clients
        }
    }

 [ClientRpc]
void LinkCardToUIClientRpc(string cardName, ulong networkId)
{
    NetworkObject netObj = NetworkManager.Singleton.SpawnManager.SpawnedObjects[networkId];
    if (netObj == null) return;

    NetworkCard nc = netObj.GetComponent<NetworkCard>();
    HandUI_manger ui = FindObjectOfType<HandUI_manger>();
    ui.AssignNetworkCard(cardName, nc);
}


}
