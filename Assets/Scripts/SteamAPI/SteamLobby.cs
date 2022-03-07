using UnityEngine;
using System.Collections;
using Steamworks;

public class SteamLobby : MonoBehaviour
{
    public static SteamLobby Instance;

    public Steamworks.CSteamID steamIDLobby = (Steamworks.CSteamID)0;

    public string sJoueur = "";

    private void Awake()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
