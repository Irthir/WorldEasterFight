using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;

public class JoinLobby : MonoBehaviour
{
    public Steamworks.CSteamID cSteamID;
    public SteamManagerMenu steamMenu;

    // Start is called before the first frame update
    void Start()
    {
        steamMenu = GameObject.Find("SteamMenuManager").GetComponent<SteamManagerMenu>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Join()
    {
        steamMenu.JoinLobby(cSteamID);
    }
}