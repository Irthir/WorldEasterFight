using UnityEngine;
using System.Collections;
using Steamworks;
using UnityEngine.UI;
using System;
using System.Text;

public class SteamMenu : MonoBehaviour
{

	[SerializeField]
	private GameObject ButtonListContent;
	[SerializeField]
	private GameObject prefabJoinButton;

	private void Start()
    {
		//SteamLobby.Instance.steamIDLobby = (Steamworks.CSteamID)0;
	}

	private void OnEnable()
	{
		if (SteamManager.Initialized)
		{
			SteamMatchmaking.AddRequestLobbyListResultCountFilter(-1);
			SteamMatchmaking.AddRequestLobbyListStringFilter("Name","WorldEasterFight",Steamworks.ELobbyComparison.k_ELobbyComparisonEqual);
			SteamMatchmaking.RequestLobbyList();
		}
		else
		{
			Debug.Log("Erreur dans l'initialisation de Steam.");
		}
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			CreateLobby();
		}

		if (Input.GetKeyDown(KeyCode.R))
		{
			SteamMatchmaking.RequestLobbyList();
		}

		if (Input.GetKeyDown(KeyCode.Q))
		{
			LeaveLobby();
		}

		if (Input.GetKeyDown(KeyCode.M))
		{
			SendMessageToLobby("Patate !");
		}
	}

	//BUT : Quitter un lobby.
	private void LeaveLobby()
	{
		if (SteamLobby.Instance.steamIDLobby != (Steamworks.CSteamID)0)
		{
			if (SteamManager.Initialized)
			{
				SteamMatchmaking.LeaveLobby(SteamLobby.Instance.steamIDLobby);
			}
			SteamLobby.Instance.steamIDLobby = (Steamworks.CSteamID)0;
		}
	}

	//BUT : Créer un lobby.
	private void CreateLobby()
	{
		//En faire un Singleton
		LeaveLobby();
		hSteamAPICall = SteamMatchmaking.CreateLobby(Steamworks.ELobbyType.k_ELobbyTypePublic, 2);
	}

	//BUT : Rejoindre un lobby.
	public void JoinLobby(Steamworks.CSteamID idLobby)
	{
		//En faire un Singleton
		LeaveLobby();
		hSteamAPICall = SteamMatchmaking.JoinLobby(idLobby);
	}

	//BUT : Inviter un jouer à un lobby.
	public void InviteToLobby(Steamworks.CSteamID idFriend)
	{
		if (SteamLobby.Instance.steamIDLobby != (Steamworks.CSteamID)0)
		{
			if (SteamMatchmaking.InviteUserToLobby(SteamLobby.Instance.steamIDLobby, idFriend))
			{
				Debug.Log("Joueur invité au lobby !");
			}
			else
			{
				Debug.Log("Échec dans l'invitation au lobby !");
			}
		}
	}

	//BUT : Envoyer un message dans le lobby.
	public void SendMessageToLobby(string sMessage)
    {
		byte[] bytes = new byte[4096];
		bytes = Encoding.Default.GetBytes(sMessage);
		SteamMatchmaking.SendLobbyChatMsg(SteamLobby.Instance.steamIDLobby, bytes, 4096);
	}

}