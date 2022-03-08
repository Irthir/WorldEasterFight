using UnityEngine;
using Steamworks;
using UnityEngine.SceneManagement;

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
			RequestLobby();
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
			m_LobbyEntered = Callback<LobbyEnter_t>.Create(OnLobbyEntered);
			m_LobbyChatUpdate = Callback<LobbyChatUpdate_t>.Create(OnLobbyChatUpdate);
		}
	}

	//BUT : G�rer les inputs pour le lobby.
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
		}

		if (Input.GetKeyDown(KeyCode.R))
		{
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

	/*______________________________________LES �V�NEMENTS !!!______________________________________*/

	//BUT : Mettre � jour le menu � la r�ception des lobbys
	new private void OnGetLobbyMatchList(LobbyMatchList_t pCallback)
	{
		if (SteamManager.Initialized)
		{
			Debug.Log("Nombre de Lobbys trouv�s : " + pCallback.m_nLobbiesMatching);
			if (ButtonListContent)
			{
				ButtonListContent.GetComponent<ListManager>().EmptyList();

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

			RequestLobby();
		}
	}

	//BUT : �v�nement d'entr�e dans un lobby.
	new private void OnLobbyEntered(LobbyEnter_t pCallback)
	{
		if (SteamManager.Initialized)
		{
			Debug.Log("Lobby rejoint : " + pCallback.m_ulSteamIDLobby);

			SteamLobby.Instance.steamIDLobby = (Steamworks.CSteamID)pCallback.m_ulSteamIDLobby;

			LogLobby();
		}
	}

	//BUT: �v�nement d'entr�e et de sortie du lobby.
	new private void OnLobbyChatUpdate(LobbyChatUpdate_t pCallback)
	{
		if (SteamManager.Initialized)
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

			LogLobby();
		}
	}

	/*______________________________________LES ACTIONS !!!______________________________________*/

	//BUT : R�cup�rer les lobbys de WorldEasterFight
	public void RequestLobby()
	{
		SteamMatchmaking.AddRequestLobbyListResultCountFilter(-1);
		SteamMatchmaking.AddRequestLobbyListStringFilter("Name", "WorldEasterFight", Steamworks.ELobbyComparison.k_ELobbyComparisonEqual);
		SteamMatchmaking.RequestLobbyList();
	}

	/*______________________________________LES LOGS !!!______________________________________*/

	//BUT : Afficher les logs du lobbys et lancer la sc�ne si le lobby est complet.
	private void LogLobby()
	{
		if (SteamManager.Initialized)
		{
			int nbJoueur = SteamMatchmaking.GetNumLobbyMembers(SteamLobby.Instance.steamIDLobby);

			Debug.Log("Nombre de joueurs présents dans le lobby " + nbJoueur + ".");

			for (int i = 0; i < nbJoueur; i++)
			{
				Steamworks.CSteamID UserID = SteamMatchmaking.GetLobbyMemberByIndex(SteamLobby.Instance.steamIDLobby, i);
				Debug.Log("Joueur " + (i + 1) + " :");

				string userName = SteamFriends.GetFriendPersonaName(UserID);
				Debug.Log("Nom d'utilisateur : " + userName);

				string oldUserName = SteamFriends.GetFriendPersonaNameHistory(UserID, 0);
				Debug.Log("Ancien nom d'utilisateur : " + oldUserName);
			}

			if (nbJoueur == 2)
			{
				setPlayer();
				stopCallbacks();
				SceneManager.LoadScene("Game");
			}
		}
	}

	//BUT : Mettre en place le r�le Lapin ou Poule.
	private void setPlayer()
	{
		if (SteamManager.Initialized)
		{
			if (SteamUser.GetSteamID() == SteamMatchmaking.GetLobbyOwner(SteamLobby.Instance.steamIDLobby))
			{
				SteamLobby.Instance.sJoueur = "Lapin";
			}
			else
			{
				SteamLobby.Instance.sJoueur = "Poule";
			}
		}
	}
	//BUT : Créer un lobby.
	new public void CreateLobby()
	{
		base.CreateLobby();
	}
}
