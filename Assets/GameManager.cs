using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class GameManager : MonoBehaviour
{
    public enum CurrentPlayer { Poule, Lapin, None }// pour pas que le joueur joue les deux perso
    public CurrentPlayer WhoIsPlayer;
    

    public enum GameStat { ReStart, Draw, Result, Other }// Etat actuel du combat -> LE MEME POUR TOUT LES JOUEURS 
    public GameStat CurrentStat;

    public GameObject PlayerPoule;
    public GameObject PlayerPouleGrid;
    public GameObject[] T_PlayerPouleGrid;
    public GameObject PlayerLapin;
    public GameObject PlayerLapinGrid;
    public GameObject[] T_PlayerLapinGrid;

    public TextMeshProUGUI WhoIsPlayerTxT;
    public TextMeshProUGUI PaTXT;


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

        }
        else if(CurrentStat == GameStat.Draw)
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
        else if (CurrentStat == GameStat.Result)
        {
            PlayerLapinGrid.SetActive(false);
            PlayerPouleGrid.SetActive(false);
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
        // POUR LA POULE
        if (Input.GetMouseButtonDown(0))
        {
            if (WhoIsPlayer == CurrentPlayer.Poule)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hitInfo;
                if (Physics.Raycast(ray, out hitInfo))
                {
                    if (hitInfo.collider.gameObject.tag == "Grid")
                    {
                        if (PlayerPoule.GetComponent<PlayerControl>().ActionPoint > 0 && hitInfo.collider.gameObject.GetComponent<SpriteRenderer>().color != Color.black)
                        {
                            Debug.Log("Clic sur " + hitInfo.collider.gameObject.name + "Devient Noir");
                            hitInfo.collider.gameObject.GetComponent<SpriteRenderer>().color = Color.black;
                            PlayerPoule.GetComponent<PlayerControl>().ActionPoint--;
                        }
                        else if (PlayerPoule.GetComponent<PlayerControl>().ActionPoint < 3 && hitInfo.collider.gameObject.GetComponent<SpriteRenderer>().color != Color.white)
                        {
                            Debug.Log("Clic sur " + hitInfo.collider.gameObject.name + "Devient Blanc");
                            hitInfo.collider.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
                            PlayerPoule.GetComponent<PlayerControl>().ActionPoint++;
                        }
                        else
                        {
                            Debug.Log("Plus de PA ! ");
                        }
                    }
                }
            }
            //POUR LE LAPIN 
            if (WhoIsPlayer == CurrentPlayer.Lapin)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hitInfo;
                if (Physics.Raycast(ray, out hitInfo))
                {
                    if (hitInfo.collider.gameObject.tag == "Grid")
                    {
                        if (PlayerLapin.GetComponent<PlayerControl>().ActionPoint > 0 && hitInfo.collider.gameObject.GetComponent<SpriteRenderer>().color != Color.black)
                        {
                            Debug.Log("Clic sur " + hitInfo.collider.gameObject.name + "Devient Noir (lapin)");
                            hitInfo.collider.gameObject.GetComponent<SpriteRenderer>().color = Color.black;
                            PlayerLapin.GetComponent<PlayerControl>().ActionPoint--;
                        }
                        else if (PlayerLapin.GetComponent<PlayerControl>().ActionPoint < 3 && hitInfo.collider.gameObject.GetComponent<SpriteRenderer>().color != Color.white)
                        {
                            Debug.Log("Clic sur " + hitInfo.collider.gameObject.name + "Devient Blanc (lapin)");
                            hitInfo.collider.gameObject.GetComponent<SpriteRenderer>().color = Color.white;
                            PlayerLapin.GetComponent<PlayerControl>().ActionPoint++;
                        }
                        else
                        {
                            Debug.Log("Plus de PA ! ");
                        }
                    }
                }
            }
        }
        if ((PlayerPoule.GetComponent<PlayerControl>().ActionPoint == 0) && (PlayerLapin.GetComponent<PlayerControl>().ActionPoint == 0))
        {
            CurrentStat = GameStat.Result;
        }
    }

    void resultPhase()
    {
        Debug.Log("resultPhase");
    }

}
