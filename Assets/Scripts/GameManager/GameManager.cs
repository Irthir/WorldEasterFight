using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using TMPro;
public class GameManager : MonoBehaviour
{
    public enum CurrentPlayer { Poule, Lapin, None }// pour pas que le joueur joue les deux perso
    public CurrentPlayer WhoIsPlayer;


    public enum GameStat { ReStart, Draw, Fight, Result }// Etat actuel du combat -> LE MEME POUR TOUT LES JOUEURS 
    public GameStat CurrentStat;

    public enum Action { Aucune, Attaque1, Attaque2, Defense1, Defense2, Heal }
    public Action PouleAction;
    public Action LapinAction;

    public GameObject PlayerPoule;
    public GameObject PlayerPouleGrid;
    public SpriteRenderer[] T_PlayerPouleGrid;
    public GameObject PlayerLapin;
    public GameObject PlayerLapinGrid;
    public SpriteRenderer[] T_PlayerLapinGrid;

    public int[] T_PouleDrawing = { -1, -1, -1 };
    public int[] T_LapinDrawing = { -1, -1, -1 };

    public TextMeshProUGUI WhoIsPlayerTxT;
    public TextMeshProUGUI PaTXT;

    public Animator animPoule;

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


    // Start is called before the first frame update
    void Start()
    {
        CurrentStat = GameStat.Draw;

    }

    // Update is called once per frame
    void Update()
    {

        if (CurrentStat == GameStat.ReStart)
        {
            CurrentStat = GameStat.Draw;
            PlayerPoule.GetComponent<PlayerControl>().ActionPoint = 3;
            PlayerLapin.GetComponent<PlayerControl>().ActionPoint = 3;
            animPoule.SetBool("Attaque1", false);
            animPoule.SetBool("Attaque2", false);
            animPoule.SetBool("Defense1", false);
            animPoule.SetBool("Defense2", false);

            for (int i = 0; i < T_PlayerPouleGrid.Length; i++)
            {
                //Debug.Log("Couleur avant : " + T_PlayerPouleGrid[i].color);
                T_PlayerPouleGrid[i].color = Color.white;
                T_PlayerLapinGrid[i].color = Color.white;
                //Debug.Log("Couleur apres : " + T_PlayerPouleGrid[i].color);
            }



        }
        else if (CurrentStat == GameStat.Draw)
        {
            drawPhase();

            if (WhoIsPlayer == CurrentPlayer.Poule)
            {
                WhoIsPlayerTxT.SetText("Vous étes : Poule");
                PaTXT.SetText("Point d'action : " + PlayerPoule.GetComponent<PlayerControl>().ActionPoint);
                PlayerLapinGrid.SetActive(false);
                PlayerPouleGrid.SetActive(true);
            }
            else if (WhoIsPlayer == CurrentPlayer.Lapin)
            {
                WhoIsPlayerTxT.SetText("Vous étes : Lapin");
                PaTXT.SetText("Point d'action : " + PlayerLapin.GetComponent<PlayerControl>().ActionPoint);
                PlayerLapinGrid.SetActive(true);
                PlayerPouleGrid.SetActive(false);

            }
        }
        else if (CurrentStat == GameStat.Fight)
        {

            CurrentStat = GameStat.Result;
            resultPhase();

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
                playerDrawing(PlayerPoule, T_PlayerPouleGrid, out T_PouleDrawing);
            }
            //POUR LE LAPIN 
            if (WhoIsPlayer == CurrentPlayer.Lapin)
            {
                playerDrawing(PlayerLapin, T_PlayerLapinGrid, out   T_LapinDrawing);
            }
        }


        //Changement d'état quand les deux joueurs on plus de PA
        if ((PlayerPoule.GetComponent<PlayerControl>().ActionPoint == 0) && (PlayerLapin.GetComponent<PlayerControl>().ActionPoint == 0))
        {
            CurrentStat = GameStat.Fight;
        }
    }


    void playerDrawing(GameObject PlayerObject, SpriteRenderer[] T_PlayerGrid, out int[] T_PlayerDrawing)
    {
        int i;
        int numDrawing;
        int j = 0;
        int[] T_BaseDrawing = { -1, -1, -1 };
        T_PlayerDrawing = T_BaseDrawing;

        AttaqueV3 PlayerAttack

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo))
        {
            if (hitInfo.collider.gameObject.tag == "Grid")
            {
                if (PlayerObject.GetComponent<PlayerControl>().ActionPoint > 0 && hitInfo.collider.gameObject.GetComponent<SpriteRenderer>().color != Color.black)
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

                        if (numDrawing != -1) {
                            Debug.Log("Dessin n°" + numDrawing);
                            PlayerAttack.Case1 = T_PlayerDrawing[1];
                            PlayerAttack.Case2 = T_PlayerDrawing[2];
                            PlayerAttack.Case3 = T_PlayerDrawing[3];
                        } else {
                            Debug.Log("Dessin invalide");
                            PlayerObject.GetComponent<PlayerControl>().ActionPoint = 3;
                            for(i=0; i<T_PlayerGrid.Length; i++)
                            {
                                T_PlayerGrid[i].color = Color.white;
                            }
                        }

                    }
                }
                else if (PlayerObject.GetComponent<PlayerControl>().ActionPoint < 3 && hitInfo.collider.gameObject.GetComponent<SpriteRenderer>().color != Color.white)
                {
                    //Debug.Log("Clic sur " + hitInfo.collider.gameObject.name + "Devient Blanc");
                    hitInfo.collider.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
                    PlayerObject.GetComponent<PlayerControl>().ActionPoint++;
                }
                else
                {
                    Debug.Log("Plus de PA ! ");
                }
            }
        }
    }


    void resultPhase()
    {
        Debug.Log("resultPhase");

        //Pour la poule 
        //Attaque :
        if (T_PlayerPouleGrid[0].color == Color.black && T_PlayerPouleGrid[1].color == Color.black && T_PlayerPouleGrid[2].color == Color.black)
        {
            Debug.Log("ATTAQUE 1 DE LA POULE !");
            PouleAction = Action.Attaque1;
            animPoule.SetBool("Attaque1", true);
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


        StartCoroutine(TimeCoroutine());

    }
    IEnumerator TimeCoroutine()
    {
        //Print the time of when the function is first called.
        PlayerLapinGrid.SetActive(false);
        PlayerPouleGrid.SetActive(false);


        //yield on a new YieldInstruction that waits for 5 seconds.
        yield return new WaitForSeconds(2);
        if (PouleAction == Action.Attaque1 && LapinAction != Action.Defense1)
        {
            PlayerLapin.GetComponent<PlayerControl>().ThisPlayerGetHit = true;
        }
        else if (PouleAction == Action.Attaque2 && LapinAction != Action.Defense2)
        {
            PlayerLapin.GetComponent<PlayerControl>().ThisPlayerGetHit = true;
        }

        if (LapinAction == Action.Attaque1 && PouleAction != Action.Defense1)
        {
            PlayerPoule.GetComponent<PlayerControl>().ThisPlayerGetHit = true;
        }
        else if (LapinAction == Action.Attaque2 && PouleAction != Action.Defense2)
        {
            PlayerPoule.GetComponent<PlayerControl>().ThisPlayerGetHit = true;
        }
        CurrentStat = GameStat.ReStart;

        //After we have waited 5 seconds print the time again.
    }

    int ValidDrawing(int[] T_PlayerDrawing)
    {
        int i;
        int valid = -1;

        for (i = 0; i < 16; i++)
        {
            if (T_PlayerDrawing[0] == T_Drawings[i, 0] && T_PlayerDrawing[1] == T_Drawings[i, 1] && T_PlayerDrawing[2] == T_Drawings[i, 2])
            {
                valid = i;
                break;
            }
        }

        return valid;
    }

}
