using Unity.Netcode;
using Unity.Collections;
using UnityEngine;

public class NetworkCard : NetworkBehaviour
{
    public NetworkVariable<FixedString64Bytes> CardName = new();
    public NetworkVariable<int> Cost = new();
    public NetworkVariable<int> Power = new();
    public NetworkVariable<FixedString64Bytes> Ability = new();
    public NetworkVariable<int> Value = new();

   
    public void SetData(Card_game.Card_data data)
    {
        CardName.Value = data.cardName;
        Cost.Value = data.cost;
        Power.Value = data.power;
        Ability.Value = data.Ability;
        Value.Value = data.value;
    }
}
