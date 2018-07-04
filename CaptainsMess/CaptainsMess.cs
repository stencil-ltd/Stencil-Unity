using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class CaptainsMess : MonoBehaviour
{
    public string broadcastIdentifier = "CM";
    public int minPlayers = 2;
    public int maxPlayers = 4;
    public CaptainsMessPlayer playerPrefab;
    public float countdownDuration = 3; // Wait for this many seconds after people are ready before starting the game
    public CaptainsMessListener listener;
    public bool verboseLogging = false;
    public bool useDebugGUI = true;
    public bool forceServer = false; 

    public CaptainsMessNetworkManager NetworkManager { get; private set; }

    public void Awake()
    {
        ValidateConfig();

        // Create network manager
        NetworkManager = (Instantiate(Resources.Load("CaptainsMessNetworkManager")) as GameObject).GetComponent<CaptainsMessNetworkManager>();
        if (NetworkManager != null)
        {
            //networkManager.logLevel = 0;

            NetworkManager.name = "CaptainsMessNetworkManager";
            NetworkManager.runInBackground = false; // runInBackground is not recommended on iOS
            NetworkManager.broadcastIdentifier = broadcastIdentifier;
            NetworkManager.minPlayers = minPlayers;
            NetworkManager.SetMaxPlayers(maxPlayers); //Setting maxPlayers and maxConnections
            NetworkManager.allReadyCountdownDuration = countdownDuration;
            NetworkManager.forceServer = forceServer;

            // I'm just using a single scene for everything
            NetworkManager.offlineScene = "";
            NetworkManager.onlineScene = "";

            NetworkManager.playerPrefab = playerPrefab.gameObject;
            NetworkManager.listener = listener;
            NetworkManager.verboseLogging = verboseLogging;

            // Optionally create Debug GUI
            if (useDebugGUI) {
                NetworkManager.GetComponent<CaptainsMessDebugGUI>().enabled = true;
            }
        }
        else
        {
            Debug.LogError("#CaptainsMess# Error creating network manager");
        }
    }

    public void ValidateConfig()
    {
        if (broadcastIdentifier == "Spaceteam" && !Application.identifier.Contains("com.sleepingbeastgames"))
        {
            Debug.LogError("#CaptainsMess# You should pick a unique Broadcast Identifier for your game", this);
        }
        if (playerPrefab == null)
        {
            Debug.LogError("#CaptainsMess# Please pick a Player prefab", this);
        }
        if (listener == null)
        {
            Debug.LogError("#CaptainsMess# Please set a Listener object", this);
        }
    }

    public void Update()
    {
        if (NetworkManager == null)
        {
            NetworkManager = FindObjectOfType(typeof(CaptainsMessNetworkManager)) as CaptainsMessNetworkManager;
            NetworkManager.listener = listener;

            if (NetworkManager.verboseLogging) {
                Debug.Log("#CaptainsMess# !! RECONNECTING !!");
            }
        }
    }

    public List<CaptainsMessPlayer> Players()
    {
        return NetworkManager.LobbyPlayers();
    }

    public CaptainsMessPlayer LocalPlayer()
    {
        return NetworkManager.localPlayer;
    }

    public void AutoConnect()
    {
        NetworkManager.InitNetworkTransport();
        NetworkManager.minPlayers = minPlayers;
        NetworkManager.AutoConnect();
    }

    public void StartHosting()
    {
        NetworkManager.InitNetworkTransport();
        NetworkManager.minPlayers = minPlayers;
        NetworkManager.StartHosting();
    }

    public void StartJoining()
    {
        NetworkManager.InitNetworkTransport();
        NetworkManager.minPlayers = minPlayers;
        NetworkManager.StartJoining();
    }

    public void Cancel()
    {
        NetworkManager.Cancel();
        NetworkManager.ShutdownNetworkTransport();
    }

    public bool AreAllPlayersReady()
    {
        return NetworkManager.AreAllPlayersReady();
    }

    public float CountdownTimer()
    {
        return NetworkManager.allReadyCountdown;
    }

    public void StartLocalGameForDebugging()
    {
        NetworkManager.InitNetworkTransport();
        NetworkManager.minPlayers = 1;
        NetworkManager.StartLocalGameForDebugging();
    }

    public bool IsConnected()
    {
        return NetworkManager.IsConnected();
    }

    public bool IsHost()
    {
        return NetworkManager.IsHost();
    }

    public void FinishGame()
    {
        NetworkManager.FinishGame();
    }

    public void SetForceServer(bool fs)
    {
        forceServer = fs;
        NetworkManager.forceServer = fs;
    }

    public void SetPrivateTeamKey(string key)
    {
        NetworkManager.SetPrivateTeamKey(key);
    }

    public int HighestConnectedVersion()
    {
        return NetworkManager.HighestConnectedVersion();
    }
}
