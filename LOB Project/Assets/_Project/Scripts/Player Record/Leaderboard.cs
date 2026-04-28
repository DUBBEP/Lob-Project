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
            PlayerRecord item = sortedPlayerRecords[counter];

            GameObject playerInfoContainer = Instantiate(playerInfo, leaderboardContainer.transform);
            PlayerInfoUI playerInfoUI = playerInfoContainer.GetComponentInChildren<PlayerInfoUI>();

            if (playerInfoUI != null)
            {
                playerInfoUI.fillPlayerInfo(item, counter + 1);
            }
        }
    }
    private void Start()
    {
        pullRecords();
    }

}
