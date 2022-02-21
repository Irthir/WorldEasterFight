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

	private void OnMessageReceived(GameConnectedFriendChatMsg_t pCallback)
	{
		Debug.Log("Message reçu");
		Debug.Log(pCallback);
		Debug.Log("ID Message " + pCallback.m_iMessageID);
		Debug.Log("ID Utilisateur " + pCallback.m_steamIDUser);
		//EChatEntryType entry = EChatEntryTypeInvalid;
		string sMessage="Prout";
		//SteamFriends.GetFriendMessage(pCallback.m_steamIDUser, pCallback.m_iMessageID, out sMessage, 4096, out entry);
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