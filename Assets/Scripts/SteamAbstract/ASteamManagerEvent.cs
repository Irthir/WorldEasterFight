using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;
using System.Text;

abstract class ASteamManagerEvent : MonoBehaviour
{
	/*______________________________________LES �V�NEMENTS !!!______________________________________*/

	private Steamworks.SteamAPICall_t hSteamAPICall;

	private Callback<LobbyMatchList_t> m_LobbyMatchList;
    private Callback<LobbyCreated_t> m_LobbyCreated;
	private Callback<LobbyEnter_t> m_LobbyEntered;
	private Callback<GameConnectedFriendChatMsg_t> m_GameConnectedFriendChatMsg;
	private Callback<LobbyChatUpdate_t> m_LobbyChatUpdate;
	private Callback<LobbyChatMsg_t> m_LobbyChatMsg;
	private Callback<LobbyDataUpdate_t> m_LobbyDataUpdate;
	private Callback<LobbyInvite_t> m_LobbyInvite;

	void OnEnable()
	{
	    m_LobbyMatchList = Callback<LobbyMatchList_t>.Create(OnGetLobbyMatchList);
	    m_LobbyCreated = Callback<LobbyCreated_t>.Create(OnLobbyCreated);
	    m_LobbyEntered = Callback<LobbyEnter_t>.Create(OnLobbyEntered);
	    m_LobbyChatUpdate = Callback<LobbyChatUpdate_t>.Create(OnLobbyChatUpdate);
	    m_LobbyChatMsg = Callback<LobbyChatMsg_t>.Create(OnLobbyChatMsg);
	    m_LobbyDataUpdate = Callback<LobbyDataUpdate_t>.Create(OnLobbyDataUpdate);
	    m_LobbyInvite = Callback<LobbyInvite_t>.Create(OnLobbyInvite);
	    SteamFriends.SetListenForFriendsMessages(true);
	    m_GameConnectedFriendChatMsg = Callback<GameConnectedFriendChatMsg_t>.Create(OnMessageReceived);
	}

	//BUT : �v�nement de r�ception des lobbys Steam.
	void OnGetLobbyMatchList(LobbyMatchList_t pCallback)
	{
		if (SteamManager.Initialized)
		{
			Debug.Log("Nombre de Lobbys trouv�s : " + pCallback.m_nLobbiesMatching);
		}
	}

	//BUT : �v�nement de cr�ation d'un lobby.
	void OnLobbyCreated(LobbyCreated_t pCallback)
	{
		if (SteamManager.Initialized)
		{
			if (pCallback.m_eResult != Steamworks.EResult.k_EResultOK)
			{
				Debug.Log("Cr�ation du lobby �chou�e.");
				return;
			}

			Debug.Log("Lobby cr�� " + pCallback.m_ulSteamIDLobby);

			SteamLobby.Instance.steamIDLobby = (Steamworks.CSteamID)pCallback.m_ulSteamIDLobby;
		}
	}

	//BUT : �v�nement d'entr�e dans un lobby.
	void OnLobbyEntered(LobbyEnter_t pCallback)
	{
		if (SteamManager.Initialized)
		{
			Debug.Log("Lobby rejoint : " + pCallback.m_ulSteamIDLobby);
		}
	}

	//BUT : �v�nement de r�ception d'un message Steam.
	void OnMessageReceived(GameConnectedFriendChatMsg_t pCallback)
	{
		if (SteamManager.Initialized)
		{
			Steamworks.EChatEntryType entry = Steamworks.EChatEntryType.k_EChatEntryTypeInvalid;
			string sMessage = "";
			SteamFriends.GetFriendMessage(pCallback.m_steamIDUser, pCallback.m_iMessageID, out sMessage, 4096, out entry);

			if (entry == Steamworks.EChatEntryType.k_EChatEntryTypeChatMsg)
			{
				Debug.Log("Message re�u.");
				Debug.Log(sMessage);
			}
		}
	}

	//BUT: �v�nement d'entr�e et de sortie du lobby.
	void OnLobbyChatUpdate(LobbyChatUpdate_t pCallback)
	{
		string sMessage = "";
		switch ((Steamworks.EChatMemberStateChange)pCallback.m_rgfChatMemberStateChange)
		{
			case Steamworks.EChatMemberStateChange.k_EChatMemberStateChangeEntered:
				sMessage = "est entr� dans le lobby.";
				break;
			case Steamworks.EChatMemberStateChange.k_EChatMemberStateChangeLeft:
				sMessage = "a quitt� le lobby.";
				break;
			case Steamworks.EChatMemberStateChange.k_EChatMemberStateChangeDisconnected:
				sMessage = "s'est d�connect�.";
				break;
			case Steamworks.EChatMemberStateChange.k_EChatMemberStateChangeKicked:
				sMessage = "a �t� exclu.";
				break;
			case Steamworks.EChatMemberStateChange.k_EChatMemberStateChangeBanned:
				sMessage = "a �t� banni.";
				break;
			default:
				sMessage = ".";
				break;
		}

		string sNom = SteamFriends.GetFriendPersonaName((Steamworks.CSteamID)pCallback.m_ulSteamIDMakingChange);

		Debug.Log(sNom + " " + sMessage);
	}

	//BUT: �v�nement de r�ception d'un message du lobby.
	void OnLobbyChatMsg(LobbyChatMsg_t pCallback)
	{
		Steamworks.EChatEntryType eChatEntryType;
		string sMessage;
		byte[] bytes = new byte[4096];
		Steamworks.CSteamID steamIDSender;
		SteamMatchmaking.GetLobbyChatEntry((Steamworks.CSteamID)pCallback.m_ulSteamIDLobby, (int)pCallback.m_iChatID, out steamIDSender, bytes, 4096, out eChatEntryType);
		sMessage = Encoding.Default.GetString(bytes);
		Debug.Log("Message re�u du lobby : " + sMessage);
	}

	//BUT: �v�nement de mise � jour des donn�es du lobby.
	void OnLobbyDataUpdate(LobbyDataUpdate_t pCallback)
	{
		string sLobby = pCallback.m_ulSteamIDLobby.ToString();
		string sMember = pCallback.m_ulSteamIDMember.ToString();
		if (pCallback.m_bSuccess == 1)
		{
			Debug.Log("Le lobby " + sLobby + " a �t� modifi� par le membre " + sMember + " avec succ�s.");
		}
		else
		{
			Debug.Log("Le lobby " + sLobby + " a �t� modifi� par le membre " + sMember + " sans succ�s.");
		}
	}

	//BUT: �v�nement de r�ception d'une invitation au lobby.
	void OnLobbyInvite(LobbyInvite_t pCallback)
	{
		string sNom = SteamFriends.GetFriendPersonaName((Steamworks.CSteamID)pCallback.m_ulSteamIDUser);
		string sLobby = pCallback.m_ulSteamIDLobby.ToString();
		string sGame = pCallback.m_ulGameID.ToString();
		Debug.Log(sNom + " vous a invit� au lobby : " + sLobby + " sur le jeu : " + sGame + ".");
	}

	/*______________________________________LES ACTIONS !!!______________________________________*/

	//BUT : Quitter un lobby.
	void LeaveLobby()
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

	//BUT : Cr�er un lobby.
	void CreateLobby(Steamworks.SteamAPICall_t hSteamAPICall)
	{
		//En faire un Singleton
		LeaveLobby();
		hSteamAPICall = SteamMatchmaking.CreateLobby(Steamworks.ELobbyType.k_ELobbyTypePublic, 2);
	}

	//BUT : Rejoindre un lobby.
	public void JoinLobby(Steamworks.CSteamID idLobby, Steamworks.SteamAPICall_t hSteamAPICall)
	{
		//En faire un Singleton
		LeaveLobby();
		hSteamAPICall = SteamMatchmaking.JoinLobby(idLobby);
	}

	//BUT : Inviter un jouer � un lobby.
	public void InviteToLobby(Steamworks.CSteamID idFriend)
	{
		if (SteamLobby.Instance.steamIDLobby != (Steamworks.CSteamID)0)
		{
			if (SteamMatchmaking.InviteUserToLobby(SteamLobby.Instance.steamIDLobby, idFriend))
			{
				Debug.Log("Joueur invit� au lobby !");
			}
			else
			{
				Debug.Log("�chec dans l'invitation au lobby !");
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
