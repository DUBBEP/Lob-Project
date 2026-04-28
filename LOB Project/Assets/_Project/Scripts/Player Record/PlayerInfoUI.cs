using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerInfoUI : MonoBehaviour
{
    [SerializeField] private TMP_Text playerInfo;

    private void Awake() => playerInfo = GetComponent<TMP_Text>();

    public void fillPlayerInfo(PlayerRecord item, int rank = 0)
    {
        playerInfo.text = $"{rank} <pos=10%>{item.username} <pos=75%>{item.score}";
    }
}
