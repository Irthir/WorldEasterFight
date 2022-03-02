using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;

public class JoinLobby : MonoBehaviour
{
    public Steamworks.CSteamID cSteamID;
    //public SteamMenu steamMenu;

    // Start is called before the first frame update
    void Start()
    {
        //steamMenu = GameObject.Find("SteamMenuManager").GetComponent<SteamMenu>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Join()
    {
        //steamMenu.JoinLobby(cSteamID);
    }
}