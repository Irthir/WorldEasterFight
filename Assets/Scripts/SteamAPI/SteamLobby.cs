/*******************************************************************************\
* Fichier       : SteamLobby.cs
*
* Classe        : SteamLobby
* Description   : Fonction stockant l'id du lobby
* Attribut      : static SteamLobby Instance : Stockage de l'instance de la classe pour la fonction du singleton.
*                 Steamworks.CSteamID steamIDLobby : ID du lobby sauvegard� qui passe entre les sc�nes.
*                 string sJoueur : Chaine de caract�re stockant le r�le du joueur en jeu.
*
* Cr�ateur      : Romain Schlotter
\*******************************************************************************/
/*******************************************************************************\
* 09-03-2022   : Rendu du projet
\*******************************************************************************/

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
