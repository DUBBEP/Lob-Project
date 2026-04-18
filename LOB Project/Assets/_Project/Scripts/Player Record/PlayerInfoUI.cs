using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerInfoUI : MonoBehaviour
{
    [SerializeField] public TMP_Text playerInfo;

    public void fillPlayerInfo(PlayerRecord item)
    {
        playerInfo.text = $"{item.username} <pos=75%>{item.score}";
    }
}
