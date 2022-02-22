using UnityEngine;
using System.Collections;
using Steamworks;
using UnityEngine.UI;

public class SteamMenu : MonoBehaviour
{
	Steamworks.SteamAPICall_t hSteamAPICall;

	private Callback<LobbyMatchList_t> m_LobbyMatchList;
	private Callback<LobbyCreated_t> m_LobbyCreated;
	private Callback<LobbyEnter_t> m_LobbyEntered;

	[SerializeField]
	private GameObject ButtonListContent;
	[SerializeField]
	private GameObject prefabJoinButton;

	private void OnEnable()
	{
		if (SteamManager.Initialized)
		{
			SteamMatchmaking.AddRequestLobbyListResultCountFilter(-1);
			//SteamMatchmaking.AddRequestLobbyListStringFilter("JEU","WORLDEASTERFIGHT",Steamworks.ELobbyComparison.k_ELobbyComparisonEqual);

			SteamMatchmaking.RequestLobbyList();
			m_LobbyMatchList = Callback<LobbyMatchList_t>.Create(OnGetLobbyMatchList);
			m_LobbyCreated = Callback<LobbyCreated_t>.Create(OnLobbyCreated);
			m_LobbyEntered = Callback<LobbyEnter_t>.Create(OnLobbyEntered);
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
	}

	//BUT : Évènement de réception des lobbys Steam.
	private void OnGetLobbyMatchList(LobbyMatchList_t pCallback)
	{
		if (SteamManager.Initialized)
		{
			Debug.Log("Nombre de Lobbys trouvés : " + pCallback.m_nLobbiesMatching);
			for (int i = 0; i < pCallback.m_nLobbiesMatching; i++)
            {
				Steamworks.CSteamID idLobby = SteamMatchmaking.GetLobbyByIndex(i);
				//Debug.Log("Id " + i + " " + idLobby);
				//Steamworks.CSteamID idOwner = SteamMatchmaking.GetLobbyOwner(idLobby);
				/*string sName = SteamFriends.GetFriendPersonaName(idOwner);
				Debug.Log("Hôte : " + sName);*/
				string sName = SteamMatchmaking.GetLobbyData(idLobby, "Name");
				if (sName!="")
				{
					Debug.Log("Nom Lobby : " + sName);
					GameObject myJoinButton = Instantiate(prefabJoinButton, new Vector3(0,0,0),Quaternion.identity);
					myJoinButton.GetComponent<JoinLobby>().cSteamID = idLobby;
					myJoinButton.transform.GetChild(0).GetChild(0).gameObject.GetComponent<UnityEngine.UI.Text>().text = sName;
					myJoinButton.transform.SetParent(ButtonListContent.transform);
				}
			}
		}
	}

	//BUT : Évènement de création d'un lobby.
	private void OnLobbyCreated(LobbyCreated_t pCallback)
	{
		if (SteamManager.Initialized)
		{
			if (pCallback.m_eResult != Steamworks.EResult.k_EResultOK)
			{
				Debug.Log("Création du lobby échouée.");
				return;
			}

			Debug.Log("Lobby créé " + pCallback.m_ulSteamIDLobby);
		}
	}

	//BUT : Évènement d'entrée dans un lobby.
	private void OnLobbyEntered(LobbyEnter_t pCallback)
	{
		if (SteamManager.Initialized)
		{
			Debug.Log("Lobby rejoint : " + pCallback.m_ulSteamIDLobby);
		}
	}

	//BUT : Créer un lobby.
	private void CreateLobby()
    {
		hSteamAPICall = SteamMatchmaking.CreateLobby(Steamworks.ELobbyType.k_ELobbyTypePublic, 2);
	}

	//BUT : Rejoindre un lobby.
	public void JoinLobby(Steamworks.CSteamID idLobby)
	{
		hSteamAPICall = SteamMatchmaking.JoinLobby(idLobby);
	}
}