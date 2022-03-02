using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;

public class SteamManagerMenu : ASteamManager
{
	[SerializeField]
	private GameObject ButtonListContent;
	[SerializeField]
	private GameObject prefabJoinButton;

	//BUT : Faire la premi�re recherche pour l'affichage, et mettre en place les conditions de recherche.
	private void Start()
	{
		if (SteamManager.Initialized)
		{
			SteamLobby.Instance.steamIDLobby = (Steamworks.CSteamID)0;

			SteamMatchmaking.AddRequestLobbyListResultCountFilter(-1);
			SteamMatchmaking.AddRequestLobbyListStringFilter("Name", "WorldEasterFight", Steamworks.ELobbyComparison.k_ELobbyComparisonEqual);
			SteamMatchmaking.RequestLobbyList();
		}
		else
		{
			Debug.Log("Erreur dans l'initialisation de Steam.");
		}
	}

	//BUT : Initialisation des �v�nements.
	new protected void OnEnable()
	{
		base.OnEnable();
		if (SteamManager.Initialized)
		{
			m_LobbyMatchList = Callback<LobbyMatchList_t>.Create(OnGetLobbyMatchList);
			m_LobbyCreated = Callback<LobbyCreated_t>.Create(OnLobbyCreated);
		}
	}

	//BUT : G�rer les inputs pour le lobby.
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

	//BUT : Mettre � jour le menu � la r�ception des lobbys
	new private void OnGetLobbyMatchList(LobbyMatchList_t pCallback)
	{
		if (SteamManager.Initialized)
		{
			Debug.Log("Nombre de Lobbys trouv�s : " + pCallback.m_nLobbiesMatching);
			ButtonListContent.GetComponent<ListManager>().EmptyList();

			Debug.Log("Prout");

			for (int i = 0; i < pCallback.m_nLobbiesMatching; i++)
			{
				Steamworks.CSteamID idLobby = SteamMatchmaking.GetLobbyByIndex(i);
				//Debug.Log("Id " + i + " " + idLobby);
				//Steamworks.CSteamID idOwner = SteamMatchmaking.GetLobbyOwner(idLobby);
				/*string sName = SteamFriends.GetFriendPersonaName(idOwner);
				Debug.Log("H�te : " + sName);*/
				string sName = SteamMatchmaking.GetLobbyData(idLobby, "Name");
				if (sName != "")
				{
					Debug.Log("Nom Lobby : " + sName);
					GameObject myJoinButton = Instantiate(prefabJoinButton, new Vector3(0, 0, 0), Quaternion.identity);
					myJoinButton.GetComponent<JoinLobby>().cSteamID = idLobby;
					myJoinButton.transform.GetChild(0).GetChild(0).gameObject.GetComponent<UnityEngine.UI.Text>().text = sName;
					myJoinButton.transform.SetParent(ButtonListContent.transform);
				}
			}
		}
	}


	//BUT : Cr�er le lobby et mettre en place les donn�es.
	new private void OnLobbyCreated(LobbyCreated_t pCallback)
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

			if (SteamMatchmaking.SetLobbyData(SteamLobby.Instance.steamIDLobby, "Name", "WorldEasterFight"))
			{
				Debug.Log("Mise en place des donn�es du lobby.");
			}
			else
			{
				Debug.Log("Erreur dans la mise en place des donn�es du lobby.");
			}

		}
	}
}
