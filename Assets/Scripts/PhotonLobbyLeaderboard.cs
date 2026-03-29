using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class PhotonLobbyLeaderboard : MonoBehaviourPunCallbacks
{
    [Header("Drag your TextMeshPro here")]
    public TextMeshPro leaderboardText;

    // Keeps players in join order
    private List<Player> playerList = new List<Player>();

    void Start()
    {
        UpdatePlayerList();
        UpdateLeaderboard();
    }

    // Called when joining a room
    public override void OnJoinedRoom()
    {
        UpdatePlayerList();
        UpdateLeaderboard();
    }

    // Called when a player joins
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        playerList.Add(newPlayer);
        UpdateLeaderboard();
    }

    // Called when a player leaves
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        playerList.Remove(otherPlayer);
        UpdateLeaderboard();
    }

    // Rebuild player list in correct order
    void UpdatePlayerList()
    {
        playerList.Clear();

        foreach (Player p in PhotonNetwork.PlayerList)
        {
            playerList.Add(p);
        }

        // Sort by ActorNumber (join order)
        playerList.Sort((a, b) => a.ActorNumber.CompareTo(b.ActorNumber));
    }

    // Update TextMeshPro text
    void UpdateLeaderboard()
    {
        if (leaderboardText == null) return;

        StringBuilder sb = new StringBuilder();

        for (int i = 0; i < playerList.Count; i++)
        {
            sb.AppendLine(playerList[i].NickName);
        }

        leaderboardText.text = sb.ToString();
    }
}