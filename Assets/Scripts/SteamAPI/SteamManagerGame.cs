/******************************************************************************\
* Fichier       : SteamManagerGame.cs
*
* Classe        : SteamManagerGame
* Description   : Manager de gestion des opérations réseau durant le jeu.
*
* Créateur      : Romain Schlotter
\*******************************************************************************/
/*******************************************************************************\
* 09-03-2022   : Rendu du projet
\*******************************************************************************/

using UnityEngine;
using Steamworks;
using UnityEngine.SceneManagement;
using System.Text;

public class SteamManagerGame : ASteamManager
{
    // Start is called before the first frame update
    void Start()
    {
        if (GameObject.Find("SteamLobby") == null)
        {
            Debug.Log("Erreur session non trouvée.");
            stopCallbacks();
            SceneManager.LoadScene("Menu");
        }
        else
        {
            Debug.Log("Session trouvée.");
        }
    }

    private void OnEnable()
    {
        base.OnEnable();
        if (SteamManager.Initialized)
        {
            m_LobbyChatUpdate = null;
            m_LobbyChatUpdate = Callback<LobbyChatUpdate_t>.Create(OnLobbyChatUpdate);
            m_LobbyChatMsg = null;
            m_LobbyChatMsg = Callback<LobbyChatMsg_t>.Create(OnLobbyChatMsg);
        }
    }

    /*______________________________________LES ÉVÈNEMENTS !!!______________________________________*/

    //BUT: Évènement de réception d'un message du lobby.
    new private void OnLobbyChatMsg(LobbyChatMsg_t pCallback)
    {
        if (SteamManager.Initialized)
        {
            Steamworks.EChatEntryType eChatEntryType;
            string sMessage;
            byte[] bytes = new byte[4096];
            Steamworks.CSteamID steamIDSender;
            SteamMatchmaking.GetLobbyChatEntry((Steamworks.CSteamID)pCallback.m_ulSteamIDLobby, (int)pCallback.m_iChatID, out steamIDSender, bytes, 4096, out eChatEntryType);
            sMessage = Encoding.Default.GetString(bytes);
            Debug.Log("Message reçu du lobby : " + sMessage);

            Debug.Log("ID Notre : "+ (Steamworks.CSteamID)SteamUser.GetSteamID());
            Debug.Log("ID Envoie : "+ steamIDSender);

            if (steamIDSender != (Steamworks.CSteamID)SteamUser.GetSteamID())
            //Si nous ne sommes pas sur une attaque nous avons envoyé.
            {
                Debug.Log("Écriture de l'attaque reçue du joueur adverse.");
                AttaqueV3 attaqueV3 = AttaqueV3.FromString(sMessage);

                GameManager gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
                if (gameManager!=null)
                {
                    gameManager.ReceptionAttaqueReseau(attaqueV3);
                }
                else
                {
                    Debug.Log("Erreur gameManager null.");
                }
                //Appeler le GameManager pour lui envoyer l'info de l'attaque adverse.
            }
        }
    }

    //BUT: Évènement d'entrée et de sortie du lobby.
    new private void OnLobbyChatUpdate(LobbyChatUpdate_t pCallback)
    {
        if (SteamManager.Initialized)
        {
            string sMessage = "";
            switch ((Steamworks.EChatMemberStateChange)pCallback.m_rgfChatMemberStateChange)
            {
                case Steamworks.EChatMemberStateChange.k_EChatMemberStateChangeEntered:
                    sMessage = "Erreur quelqu'un est entré dans le lobby.";
                    break;
                case Steamworks.EChatMemberStateChange.k_EChatMemberStateChangeLeft:
                    sMessage = "Erreur quelqu'un a quitté le lobby.";
                    endLobby();
                    break;
                case Steamworks.EChatMemberStateChange.k_EChatMemberStateChangeDisconnected:
                    sMessage = "Erreur quelqu'un s'est déconnecté.";
                    endLobby();
                    break;
                case Steamworks.EChatMemberStateChange.k_EChatMemberStateChangeKicked:
                    sMessage = "Erreur quelqu'un a été exclu.";
                    endLobby();
                    break;
                case Steamworks.EChatMemberStateChange.k_EChatMemberStateChangeBanned:
                    sMessage = "Erreur quelqu'un a été banni.";
                    endLobby();
                    break;
                default:
                    sMessage = "Erreur quelque chose s'est passé.";
                    break;
            }

            Debug.Log(sMessage);
        }
    }

    /*______________________________________LES ACTIONS !!!______________________________________*/

    //BUT : Envoyer une action au 
    public bool sendActionToSteamNetwork(AttaqueV3 aAction)
    {
        if (SteamManager.Initialized)
        {
            SendMessageToLobby(aAction.ToString());
            return true;
        }
        else
        {
            return false;
        }
    }

    //BUT : Mettre fin au lobby.
    public void endLobby()
    {
        LeaveLobby();
        //Redirection de fin de jeu.
        SceneManager.LoadScene("TitleScreen");
    }
}
