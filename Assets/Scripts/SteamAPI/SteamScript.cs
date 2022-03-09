/******************************************************************************\
* Fichier       : SteamScript.cs
*
* Classe        : SteamScript
* Description   : Script de test quant à l'API Steam pour Unity.
*
* Créateur      : Romain Schlotter
* 
* Notes			: Script non utilisé dans la vie du jeu.
\*******************************************************************************/
/*******************************************************************************\
* 09-03-2022   : Rendu du projet
\*******************************************************************************/

//#define TESTDOC

using UnityEngine;
using System.Collections;
using Steamworks;


public class SteamScript: MonoBehaviour
{
#if TESTDOC
	//Tests Doc
	private Callback<GameOverlayActivated_t> m_GameOverlayActivated;
	//Test Doc
	private CallResult<NumberOfCurrentPlayers_t> m_NumberOfCurrentPlayers;
#endif

	private Callback<GameConnectedFriendChatMsg_t> m_GameConnectedFriendChatMsg;

	private void OnEnable()
	{
		if (SteamManager.Initialized)
		{
		#if TESTDOC
			//Test Doc
			string name = SteamFriends.GetPersonaName();
			Debug.Log(name);

			//Test Doc
			m_GameOverlayActivated = Callback<GameOverlayActivated_t>.Create(OnGameOverlayActivated);
			//Test Doc
			m_NumberOfCurrentPlayers = CallResult<NumberOfCurrentPlayers_t>.Create(OnNumberOfCurrentPlayers);
		#endif

			SteamFriends.SetListenForFriendsMessages(true);
			m_GameConnectedFriendChatMsg = Callback<GameConnectedFriendChatMsg_t>.Create(OnMessageReceived);
		}
		else
		{
			Debug.Log("Erreur dans l'initialisation de Steam.");
		}
	}

	private void Update()
	{
		#if TESTDOC
		//Test Doc
		if (Input.GetKeyDown(KeyCode.Space))
		{
			SteamAPICall_t handle = SteamUserStats.GetNumberOfCurrentPlayers();
			m_NumberOfCurrentPlayers.Set(handle);
			Debug.Log("Called GetNumberOfCurrentPlayers()");
		}
		#endif
	}

	//BUT : Évènement de réception d'un message Steam.
	private void OnMessageReceived(GameConnectedFriendChatMsg_t pCallback)
	{

		if (SteamManager.Initialized)
		{
			Steamworks.EChatEntryType entry = Steamworks.EChatEntryType.k_EChatEntryTypeInvalid;
			string sMessage = "Prout";
			SteamFriends.GetFriendMessage(pCallback.m_steamIDUser, pCallback.m_iMessageID, out sMessage, 4096, out entry);

			if (entry == Steamworks.EChatEntryType.k_EChatEntryTypeChatMsg)
			{
				Debug.Log(entry);
				Debug.Log("Message avant traitement : " + sMessage);
				MessageReceivedCallbackAutoReply(pCallback, sMessage);
			}
		}
	}

	//BUT : Répondre à un message Steam en répondant la même chose que ce qui est reçu.
	private void MessageReceivedCallbackAutoReply(GameConnectedFriendChatMsg_t pCallback, string sMessage)
	{
		SteamFriends.ReplyToFriendMessage(pCallback.m_steamIDUser, sMessage);
		Debug.Log("Message reçu");
		Debug.Log(pCallback);
		Debug.Log("ID Message : " + pCallback.m_iMessageID);
		Debug.Log("ID Utilisateur : " + pCallback.m_steamIDUser);
		Debug.Log("Nom Utilisateur : " + SteamFriends.GetFriendPersonaName(pCallback.m_steamIDUser));
		Debug.Log("Message : " + sMessage);
	}



#if TESTDOC
	//Test Doc
	private void OnGameOverlayActivated(GameOverlayActivated_t pCallback)
	{
		if (pCallback.m_bActive != 0)
		{
			Debug.Log("Steam Overlay has been activated");
			//Faire pause
		}
		else
		{
			Debug.Log("Steam Overlay has been closed");
			//Enlever la pause
		}
	}
	//Test Doc
	private void OnNumberOfCurrentPlayers(NumberOfCurrentPlayers_t pCallback, bool bIOFailure)
	{
		if (pCallback.m_bSuccess != 1 || bIOFailure)
		{
			Debug.Log("There was an error retrieving the NumberOfCurrentPlayers.");
		}
		else
		{
			Debug.Log("The number of players playing your game: " + pCallback.m_cPlayers);
		}
	}
#endif
}