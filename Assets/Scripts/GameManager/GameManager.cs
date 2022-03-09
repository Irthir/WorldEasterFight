using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using TMPro;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public enum CurrentPlayer { Poule, Lapin, None }// pour pas que le joueur joue les deux perso
    public CurrentPlayer WhoIsPlayer;

    public enum GameStat { ReStart, Draw, Wait, Fight, Result }// Etat actuel du combat -> LE MEME POUR TOUT LES JOUEURS 
    public GameStat CurrentStat;

    public int PouleAction = -1;
    public int LapinAction = -1;

    public GameObject PlayerPoule;
    public GameObject PlayerPouleGrid;
    public SpriteRenderer[] T_PlayerPouleGrid;
    public GameObject PlayerLapin;
    public GameObject PlayerLapinGrid;
    public SpriteRenderer[] T_PlayerLapinGrid;

    public int[] T_PouleDrawing = { -1, -1, -1 };
    public int[] T_LapinDrawing = { -1, -1, -1 };

    public GameObject ResultGrid;
    public SpriteRenderer[] T_ResultGrid;

    public TextMeshProUGUI WhoIsPlayerTxT;
    public TextMeshProUGUI PaTXT;
    public TextMeshProUGUI ResultTXT;

    public Animator animPoule;
    public Animator animLapin;

    public int[,] T_Drawings = {
        //Attaques
        {0, 1, 2 },         //Attaque haute
        {0, 3, 6 },         //Attaque gauche
        {2, 5, 8 },         //Attaque droite
        {6, 7, 8 },         //Attaque basse
        //Parades
        {0, 2, 4 },         //Parade haute
        {0, 4, 6 },         //Parade gauche
        {2, 4, 8 },         //Parade droite
        {4, 6, 8 },         //Parade basse
        //Esquives
        {1, 3, 5 },         //Esquive haute
        {1, 3, 7 },         //Esquive gauche
        {1, 5, 7 },         //Esquive droite
        {3, 5, 7 },         //Esquive basse
        //Soin
        {0, 4, 8 },         //Soin diagonal haut-gauche
        {1, 4, 7 },         //Soin vertical
        {2, 4, 6 },         //Soin diagonal haut-droite
        {3, 4, 5 }          //Soin horizontal
    };

    //Booléen à valider une fois la réception d'une attaque faite, et à invalider une fois la phase passée.
    private bool bReceptionReseau = false;
    //Référence au manager réseau
    SteamManagerGame steamManagerGame=null;

    // Start is called before the first frame update
    void Start()
    {
        if (SteamLobby.Instance.sJoueur == "Lapin")
        {
            Debug.Log("Joueur = Lapin");
            WhoIsPlayer = CurrentPlayer.Lapin;
        }
        else if (SteamLobby.Instance.sJoueur == "Poule")
        {
            Debug.Log("Joueur = Poule");
            WhoIsPlayer = CurrentPlayer.Poule;
        }

        //Mise en place du manager de réseau.
        steamManagerGame = GameObject.Find("SteamManagerGame").GetComponent<SteamManagerGame>();
        if (steamManagerGame == null)
        {
            Debug.Log("Erreur lors de la récupération du manager réseau.");
        }

        CurrentStat = GameStat.Draw;
    }

    // Update is called once per frame
    void Update()
    {
        int i;

        switch (CurrentStat)
        {
            case GameStat.ReStart:
                CurrentStat = GameStat.Draw;
                PlayerPoule.GetComponent<PlayerControl>().ActionPoint = 3;
                PlayerLapin.GetComponent<PlayerControl>().ActionPoint = 3;
                ResultGrid.SetActive(false);
                animPoule.SetBool("Attaque", false);
                animPoule.SetBool("Esquive", false);
                animPoule.SetBool("Parade", false);
                animPoule.SetBool("Soin", false);
                animLapin.SetBool("Attaque", false);
                animLapin.SetBool("Esquive", false);
                animLapin.SetBool("Parade", false);
                animLapin.SetBool("Soin", false);


                for (i = 0; i < T_PlayerPouleGrid.Length; i++)
                {
                    //Debug.Log("Couleur avant : " + T_PlayerPouleGrid[i].color);
                    if(T_PlayerPouleGrid[i].color == Color.white) {
                        T_PlayerPouleGrid[i].color = Color.white;
                    } else {
                        T_PlayerPouleGrid[i].color = Color.gray;
                    }

                    if (T_PlayerLapinGrid[i].color == Color.white) {
                        T_PlayerLapinGrid[i].color = Color.white;
                    } else {
                        T_PlayerLapinGrid[i].color = Color.gray;
                    }
                    
                    //Debug.Log("Couleur apres : " + T_PlayerPouleGrid[i].color);
                }
                break;

            case GameStat.Draw:
                drawPhase();

                if (WhoIsPlayer == CurrentPlayer.Poule) {
                    WhoIsPlayerTxT.SetText("Vous êtes : Poule");
                    PaTXT.SetText("Point d'action : " + PlayerPoule.GetComponent<PlayerControl>().ActionPoint);
                    PlayerLapinGrid.SetActive(false);
                    PlayerPouleGrid.SetActive(true);

                } else if (WhoIsPlayer == CurrentPlayer.Lapin) {
                    WhoIsPlayerTxT.SetText("Vous êtes : Lapin");
                    PaTXT.SetText("Point d'action : " + PlayerLapin.GetComponent<PlayerControl>().ActionPoint);
                    PlayerLapinGrid.SetActive(true);
                    PlayerPouleGrid.SetActive(false);

                }
                break;
            case GameStat.Wait:
                //Attente de la réception d'une réponse réseau.
                if (bReceptionReseau==true)
                {
                    CurrentStat = GameStat.Fight;
                    bReceptionReseau = false;
                }
                break;
            case GameStat.Fight:
                resultPhase();
                CurrentStat = GameStat.Result;
                bReceptionReseau = false;
                break;
        }



        if (WhoIsPlayer == CurrentPlayer.None)
        {
            WhoIsPlayerTxT.SetText("ERREUR : AUCUN PERSONNAGE");
            WhoIsPlayerTxT.color = Color.red;
        }
    }

    void drawPhase()
    {

        if (Input.GetMouseButtonDown(0))
        {
            // POUR LA POULE
            if (WhoIsPlayer == CurrentPlayer.Poule)
            {
                PouleAction = playerDrawing(PlayerPoule, T_PlayerPouleGrid, out T_PouleDrawing);

            }
            //POUR LE LAPIN 
            if (WhoIsPlayer == CurrentPlayer.Lapin)
            {
                LapinAction = playerDrawing(PlayerLapin, T_PlayerLapinGrid, out   T_LapinDrawing);
            }
        }


        //Changement d'état quand le joueur en cours n'a plus de PA
        /*if (WhoIsPlayer == CurrentPlayer.Lapin && (PlayerLapin.GetComponent<PlayerControl>().ActionPoint == 0))
        {

            CurrentStat = GameStat.Wait;
        }

        if (WhoIsPlayer == CurrentPlayer.Poule && (PlayerPoule.GetComponent<PlayerControl>().ActionPoint == 0))
        {

            CurrentStat = GameStat.Wait;
        }*/

        //Changement d'état quand les deux joueurs on plus de PA
        /*if ((PlayerPoule.GetComponent<PlayerControl>().ActionPoint == 0) && (PlayerLapin.GetComponent<PlayerControl>().ActionPoint == 0))
        {
            CurrentStat = GameStat.Fight;
        }*/
    }

    int playerDrawing(GameObject PlayerObject, SpriteRenderer[] T_PlayerGrid, out int[] T_PlayerDrawing)
    {
        int i;
        int numDrawing;
        int typeDrawing = -1;
        int j = 0;
        int[] T_BaseDrawing = { -1, -1, -1 };
        T_PlayerDrawing = T_BaseDrawing;

        AttaqueV3 PlayerAttack;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo))
        {
            if (hitInfo.collider.gameObject.tag == "Grid")
            {
                if (PlayerObject.GetComponent<PlayerControl>().ActionPoint > 0 && hitInfo.collider.gameObject.GetComponent<SpriteRenderer>().color == Color.white)
                {

                    //Debug.Log("Clic sur " + hitInfo.collider.gameObject.name + ": Devient Noir");
                    hitInfo.collider.gameObject.GetComponent<SpriteRenderer>().color = Color.black;
                    PlayerObject.GetComponent<PlayerControl>().ActionPoint--;

                    //Validation du dessin
                    if (PlayerObject.GetComponent<PlayerControl>().ActionPoint <= 0)
                    {
                        for (i = 0; i < T_PlayerGrid.Length; i++)
                        {
                            if (T_PlayerGrid[i].color == Color.black)
                            {
                                T_PlayerDrawing[j] = i;
                                j++;
                            }
                        }

                        numDrawing = ValidDrawing(T_PlayerDrawing);

                        if (numDrawing != -1)
                        {
                            typeDrawing = (numDrawing / 4) + 1;
                            Debug.Log("Dessin n°" + numDrawing);
                            PlayerAttack = new AttaqueV3(typeDrawing, T_PlayerDrawing[0], T_PlayerDrawing[1], T_PlayerDrawing[2]);
                            //WhoIsPlayer = CurrentPlayer.Lapin;


                            //Envoie de l'action au réseau.
                            if (steamManagerGame.sendActionToSteamNetwork(PlayerAttack))
                            {
                                Debug.Log("Réussite de l'envoie de l'attaque réseau.");
                                //Passage en phase d'attente de réponse réseau.
                                CurrentStat = GameStat.Wait;
                            }
                            else
                            {
                                Debug.Log("Échec de l'envoi de l'attaque réseau.");
                            }
                        }
                        else
                        {
                            Debug.Log("Dessin invalide");
                            PlayerObject.GetComponent<PlayerControl>().ActionPoint = 3;
                            for(i=0; i<T_PlayerGrid.Length; i++) {
                                if (T_PlayerGrid[i].color != Color.gray) {
                                    T_PlayerGrid[i].color = Color.white;
                                }
                            }
                        }

                    }
                }
                else if (PlayerObject.GetComponent<PlayerControl>().ActionPoint < 3 && hitInfo.collider.gameObject.GetComponent<SpriteRenderer>().color == Color.black)
                {
                    //Debug.Log("Clic sur " + hitInfo.collider.gameObject.name + "Devient Blanc");
                    hitInfo.collider.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
                    PlayerObject.GetComponent<PlayerControl>().ActionPoint++;
                }
                else
                {
                    Debug.Log("Cette case ne peut pas etre cochée !");
                }
            }
        }

        return typeDrawing;
    }

    void resultPhase()
    {
        int i;
        
        for(i=0; i<T_ResultGrid.Length; i++) {
            T_ResultGrid[i].color = Color.white;
        }

        Debug.Log("resultPhase");

        for(i=0; i<T_ResultGrid.Length; i++) {
            if(T_PlayerPouleGrid[i].color == Color.black) {
                if(T_PlayerLapinGrid[i].color == Color.black) {
                    T_ResultGrid[i].color = Color.yellow;
                } else {
                    T_ResultGrid[i].color = Color.red;
                }
            } else {
                if (T_PlayerLapinGrid[i].color == Color.black) {
                    T_ResultGrid[i].color = Color.green;
                }
            }

            if(T_PlayerPouleGrid[i].color == Color.gray) {
                T_PlayerPouleGrid[i].color = Color.white;
            }
            if (T_PlayerLapinGrid[i].color == Color.gray)
            {
                T_PlayerLapinGrid[i].color = Color.white;
            }

        }

        PlayerLapinGrid.SetActive(false);
        PlayerPouleGrid.SetActive(false);
        ResultGrid.SetActive(true);

        
        /*
        //Pour la poule 
        //Attaque :
        if (T_PlayerPouleGrid[0].color == Color.black && T_PlayerPouleGrid[1].color == Color.black && T_PlayerPouleGrid[2].color == Color.black)
        {
            Debug.Log("ATTAQUE 1 DE LA POULE !");
            PouleAction = Action.Attaque1;
         }
        else if (T_PlayerPouleGrid[2].color == Color.black && T_PlayerPouleGrid[5].color == Color.black && T_PlayerPouleGrid[8].color == Color.black)
        {
            Debug.Log("ATTAQUE 2 DE LA POULE !");
            PouleAction = Action.Attaque2;
            animPoule.SetBool("Attaque2", true);

        }
        //Defense : 
        else if (T_PlayerPouleGrid[8].color == Color.black && T_PlayerPouleGrid[7].color == Color.black && T_PlayerPouleGrid[6].color == Color.black)
        {
            Debug.Log("DEFENSE 1 DE LA POULE !");
            PouleAction = Action.Attaque2;
            animPoule.SetBool("Defense1", true);

        }
        else if (T_PlayerPouleGrid[0].color == Color.black && T_PlayerPouleGrid[3].color == Color.black && T_PlayerPouleGrid[6].color == Color.black)
        {
            Debug.Log("DEFENSE 2 DE LA POULE !");
            PouleAction = Action.Attaque2;
            animPoule.SetBool("Defense2", true);
        }

        //Pour le Lapin 
        //Attaque :
        if (T_PlayerLapinGrid[0].color == Color.black && T_PlayerLapinGrid[1].color == Color.black && T_PlayerLapinGrid[2].color == Color.black)
        {
            Debug.Log("ATTAQUE 1 DU LAPIN !");
            LapinAction = Action.Attaque1;

        }
        else if (T_PlayerLapinGrid[2].color == Color.black && T_PlayerLapinGrid[5].color == Color.black && T_PlayerLapinGrid[8].color == Color.black)
        {
            Debug.Log("ATTAQUE 2 DU LAPIN !");
            LapinAction = Action.Attaque2;
        }
        //Defense : 
        else if (T_PlayerLapinGrid[8].color == Color.black && T_PlayerLapinGrid[7].color == Color.black && T_PlayerLapinGrid[6].color == Color.black)
        {
            Debug.Log("DEFENSE 1 DU LAPINE !");
            LapinAction = Action.Defense1;
        }
        else if (T_PlayerLapinGrid[0].color == Color.black && T_PlayerLapinGrid[3].color == Color.black && T_PlayerLapinGrid[6].color == Color.black)
        {
            Debug.Log("DEFENSE 2 DU LAPIN !");
            LapinAction = Action.Defense2;
        }
        */


        StartCoroutine(TimeCoroutine());

    }
    IEnumerator TimeCoroutine()
    {
        int i;
        int nbCollide;
        bool tapePoule = false;
        bool tapeLapin = false;
        bool healPoule = false;
        bool healLapin = false;


        //Print the time of when the function is first called.

        //On compte le nombre de cases en commun entre les deux joueurs
        nbCollide = 0;
        for (i=0; i<T_ResultGrid.Length; i++) {
            if (T_ResultGrid[i].color == Color.yellow) {
                nbCollide++;
            }
        }

        Debug.Log("Nombre de collisons : " + nbCollide);

        //ACTION DE DEFENSE
        if (PouleAction == 2 ^ LapinAction == 2) {
            if (PouleAction == 2) {
                Debug.Log("Poule: Defense");
                animPoule.SetBool("Parade", true);
                if (nbCollide == 2) {
                    LapinAction = 0;
                    tapeLapin = true;
                } else {
                    PouleAction = 0;
                }
            } else {
                Debug.Log("Lapin: Defense");
                animLapin.SetBool("Parade", true);
                if (nbCollide == 2) {
                    PouleAction = 0;
                    tapePoule = true;
                } else {
                    LapinAction = 0;
                }
            }

            nbCollide = 0;
            
        }



        //ACTION D'ESQUIVE
        if(PouleAction == 3 || LapinAction == 3) {
            if(PouleAction == 3) {
                Debug.Log("Poule: Esquive");
                animPoule.SetBool("Esquive", true);
                if (nbCollide == 1) {
                    LapinAction = 0;
                }
                if(nbCollide == 2) {
                    healPoule = true;
                }
            }
            if(LapinAction == 3) {
                Debug.Log("Lapin: Esquive");
                animLapin.SetBool("Esquive", true);
                if (nbCollide == 1) {
                    PouleAction = 0;
                }
                if(nbCollide == 2) {
                    healLapin = true;
                }
            }

            if (LapinAction == 0 || PouleAction == 0) {
                nbCollide = 0;
            }
        }



        //ACTION D'ATTAQUE
        if(PouleAction == 1 || LapinAction == 1) {
            if(PouleAction == 1) {
                Debug.Log("Poule: Attaque");
                animPoule.SetBool("Attaque", true);
                if(nbCollide == 0) { 
                    tapeLapin = true;
                }
            }
            if(LapinAction == 1) {
                Debug.Log("Lapin: Attaque");
                animLapin.SetBool("Attaque", true);
                if (nbCollide == 0) { 
                    tapePoule = true;
                }
            }
        }



        //ACTION DE SOIN
        if(PouleAction == 4 || LapinAction == 4) {
            if(PouleAction == 4) {
                Debug.Log("Poule: Soin");
                animPoule.SetBool("Soin", true);
                if(nbCollide < 2) { 
                    healPoule = true;
                }
            }
            if(LapinAction == 4) {
                Debug.Log("Lapin: Soin");
                animLapin.SetBool("Soin", true);
                if (nbCollide < 2) { 
                    healLapin = true;
                }
            }
        }


        yield return new WaitForSeconds(2);

        for (i=0; i<T_ResultGrid.Length; i++) {
            if(LapinAction == 0) {
                if(T_ResultGrid[i].color == Color.yellow) {
                    T_ResultGrid[i].color = Color.red;
                }
                if (T_ResultGrid[i].color == Color.green) {
                    T_ResultGrid[i].color = Color.white;
                }
            } else {
                if (T_ResultGrid[i].color == Color.yellow) {
                    T_ResultGrid[i].color = Color.green;
                }
                if (T_ResultGrid[i].color == Color.red) {
                    T_ResultGrid[i].color = Color.white;
                }
            }
        }


        


        //yield on a new YieldInstruction that waits for 5 seconds.
        yield return new WaitForSeconds(2);
        
        if(tapeLapin) {
            Debug.Log("Degats sur le lapin");
            PlayerLapin.GetComponent<PlayerControl>().ThisPlayerGetHit = true;
        }
        if(tapePoule) {
            Debug.Log("Degats sur la poule");
            PlayerPoule.GetComponent<PlayerControl>().ThisPlayerGetHit = true;
        }
        if(healLapin) {
            Debug.Log("Soin sur le lapin");
            if (PlayerLapin.GetComponent<PlayerControl>().Life < 3) {
                PlayerLapin.GetComponent<PlayerControl>().ThisPlayerGetHeal = true;
            }
        }
        if(healPoule) {
            Debug.Log("Soins sur le lapin");
            if(PlayerPoule.GetComponent<PlayerControl>().Life < 3) {
                PlayerPoule.GetComponent<PlayerControl>().ThisPlayerGetHeal = true;
            }
        }


        yield return new WaitForSeconds(2);



        if (PlayerPoule.GetComponent<PlayerControl>().Life <= 0 || PlayerLapin.GetComponent<PlayerControl>().Life <= 0) {
            VictoryScene();
        } else {
            CurrentStat = GameStat.ReStart;
        }

        //After we have waited 5 seconds print the time again.
    }

    int ValidDrawing(int[] T_PlayerDrawing) {
        int i;
        int valid = -1;

        for (i = 0; i < 16; i++) {
            if (T_PlayerDrawing[0] == T_Drawings[i, 0] && T_PlayerDrawing[1] == T_Drawings[i, 1] && T_PlayerDrawing[2] == T_Drawings[i, 2]) {
                valid = i;
                break;
            }
        }

        return valid;
    }

    //BUT : Récupérer l'attaque en réseau et signaler que l'attaque est reçue.
    public void ReceptionAttaqueReseau(AttaqueV3 attaqueV3)
    {
        int i;

        if (bReceptionReseau==false) {
            if (WhoIsPlayer == CurrentPlayer.Poule) {
                //Le joueur est la poule, donc l'attaque reçue en réseau s'applique au lapin
                Debug.Log("Reception de l'attaque du lapin");
                LapinAction = attaqueV3.Type;
                T_LapinDrawing[0] = attaqueV3.Case1;
                T_LapinDrawing[1] = attaqueV3.Case2;
                T_LapinDrawing[2] = attaqueV3.Case3;

                for(i=0; i<T_PlayerLapinGrid.Length; i++) {
                    if(i == T_LapinDrawing[0] || i == T_LapinDrawing[1] || i == T_LapinDrawing[2]) {
                        T_PlayerLapinGrid[i].color = Color.black;
                    } else {
                        T_PlayerLapinGrid[i].color = Color.white;
                    }
                }

            } else if (WhoIsPlayer == CurrentPlayer.Lapin) {
                //Le joueur est le lapin, donc l'attaque reçue en réseau s'applique à la poule.
                Debug.Log("Reception de l'attaque de la poule");
                PouleAction = attaqueV3.Type;
                T_PouleDrawing[0] = attaqueV3.Case1;
                T_PouleDrawing[1] = attaqueV3.Case2;
                T_PouleDrawing[2] = attaqueV3.Case3;

                for(i=0; i<T_PlayerPouleGrid.Length; i++) {
                    if(i == T_PouleDrawing[0] || i == T_PouleDrawing[1] || i == T_PouleDrawing[2]) {
                        T_PlayerPouleGrid[i].color = Color.black;
                    } else {
                        T_PlayerPouleGrid[i].color = Color.white;
                    }
                }

            }

            bReceptionReseau = true;
        }
    }

    public void VictoryScene()
    {
        if (PlayerPoule.GetComponent<PlayerControl>().Life <= 0)
        {
            animLapin.SetBool("Victoire", true);
            Debug.Log("Victoire du lapin ! ");
            ResultTXT.SetText("VICTOIRE DU LAPIN !");
        }
        else if (PlayerLapin.GetComponent<PlayerControl>().Life <= 0)
        {
            animPoule.SetBool("Victoire", true);
            Debug.Log("Victoire de la poule ! ");
            ResultTXT.SetText("VICTOIRE DE LA POULE !");
        }
        else
        {
            Debug.Log("égalité, du coup double annim de victoire !");
            ResultTXT.SetText("VICTOIRE !");
            animPoule.SetBool("Victoire", true);
            animLapin.SetBool("Victoire", true);
        }

        StartCoroutine(EndGameCoroutine());

    }

        //BUT : Mettre fin au réseau, au jeu.
    void EndGame()
    {
        //Référence au manager réseau.
        steamManagerGame.endLobby();
    }

    IEnumerator EndGameCoroutine()
    {
        //yield on a new YieldInstruction that waits for 5 seconds.
        yield return new WaitForSeconds(5);
    }
}
