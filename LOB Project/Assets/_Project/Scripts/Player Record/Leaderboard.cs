using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Unity.Services.Analytics;
using UnityEditor.Search;
using UnityEngine;
using System.Linq;

public class Leaderboard : MonoBehaviour
{
    [SerializeField] private BackendPlayerRecordConnector apiManager;

    public GameObject leaderboardContainer;
    public GameObject playerInfo;

    public int displayCount;
    private int counter;

    private void Start()
    {
        pullRecords();
    }

    //call API at start of scene to pull player records 
    private async void pullRecords()
    {
        List<PlayerRecord> items = await apiManager.GetIndexAsync();
        int rank = 0;

        foreach (PlayerRecord item in items)
        {
            rank++;
            Debug.Log($"#{rank} Username: {item.username} Score: {item.score}");
        }

        sortPlayerScores(items);
        addToLeaderboard(items);
        
        int playerRank = GetPlayerRank(items);

        if (playerRank >= 0)
        {
            AddNewRecord(items, playerRank);
        }
    }

    //sort by scores high to low
    private void sortPlayerScores(List<PlayerRecord> unsortedPLayerRecords)
    {
        unsortedPLayerRecords.Sort((a, b) => b.score.CompareTo(a.score));
    }

    //add container for each top # highest scores and their usernames 
    //delete old displayed player records
    //add a cap to number of player records displayed
    private void addToLeaderboard(List<PlayerRecord> sortedPlayerRecords)
    {
        foreach (Transform child in leaderboardContainer.transform)
        {
            Destroy(child.gameObject);
        }

        for (counter = 0; counter < displayCount; counter++)
        {
            AddNewRecord(sortedPlayerRecords, counter);
        }
    }

    private void AddNewRecord(List<PlayerRecord> sortedPlayerRecords, int index)
    {
        Debug.Log($"passed index was {index}");

        PlayerRecord item = sortedPlayerRecords[index];

        GameObject playerInfoContainer = Instantiate(playerInfo, leaderboardContainer.transform);
        PlayerInfoUI playerInfoUI = playerInfoContainer.GetComponentInChildren<PlayerInfoUI>();

        if (playerInfoUI != null)
        {
            if (index == 0)
                playerInfoUI.fillPlayerInfo(item, index + 1, Color.yellow);
            else if (index == 1)
                playerInfoUI.fillPlayerInfo(item, index + 1, Color.blue);
            else if (index == 2)
                playerInfoUI.fillPlayerInfo(item, index + 1, Color.red);
            else if (item.username == AuthManager.GetUsername())
                playerInfoUI.fillPlayerInfo(item, index + 1, Color.green);
            else
                playerInfoUI.fillPlayerInfo(item, index + 1);
        }
    }

    private int GetPlayerRank(List<PlayerRecord> sortedPlayerRecords)
    {
        string playerName = AuthManager.GetUsername();
        for (int i = 0; i < sortedPlayerRecords.Count; i++)
        {
            if (playerName == sortedPlayerRecords[i].username)
            {
                if (i <= 9)
                    return -1;
                else
                    return i;
            }
        }

        return -1;
    }
}
