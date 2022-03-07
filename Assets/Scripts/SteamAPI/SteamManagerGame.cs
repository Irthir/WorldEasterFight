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

    // Update is called once per frame
    void Update()
    {
        
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
            AttaqueV3 attaqueV3 = AttaqueV3.FromString(sMessage);
            //Appeler le GameManager pour lui envoyer l'info de l'attaque adverse.
        }
    }

    /*______________________________________LES ACTIONS !!!______________________________________*/

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
}
