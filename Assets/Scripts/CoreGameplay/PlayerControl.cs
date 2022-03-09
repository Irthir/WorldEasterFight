using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public int Life = 3;
    public int ActionPoint = 3;
    public GameObject[] T_LifeBar;
    public SpriteRenderer[] T_LifeBarRenderer;
    public bool ThisPlayerGetHit = false;
    public bool ThisPlayerGetHeal = false;
    // Start is called before the first frame update
    void Start()
    {
        /*foreach (Transform child in LifeBar)
        {
            //LifeBarSpritListTransform.Add(child.transform);
            //LifeBarSpritListTransform[i] = child.transform;
            i++;
        }

        i = 0;

        foreach (SpriteRenderer child in LifeBar)
        {
            //LifeBarSpritListTransform.Add(child.transform);
            LifeBarSpritListSprite[i] = child.spriterenderer;
            i++;
        }*/

    }

    // Update is called once per frame
    void Update()
    {
        if (ThisPlayerGetHit)
        {
            HitPlayer();
            ThisPlayerGetHit = false;
        }
        if(ThisPlayerGetHeal) {
            HealPlayer();
            ThisPlayerGetHeal = false;
        }

    }

    void HitPlayer()
    {
        T_LifeBar[Life - 1].SetActive(false);
        Life--;

        if (Life == 2) 
        {
            Debug.Log("Yellow");
            T_LifeBar[0].GetComponent<SpriteRenderer>().color = Color.yellow;
            T_LifeBar[1].GetComponent<SpriteRenderer>().color = Color.yellow;
        }
        else if (Life == 1)
        {
            Debug.Log("red");

            T_LifeBar[0].GetComponent<SpriteRenderer>().color = Color.red;
        }

    }


    void HealPlayer() {
        T_LifeBar[Life].SetActive(true);
        Life++;

        if(Life == 3) {
            Debug.Log("Green");
            T_LifeBar[0].GetComponent<SpriteRenderer>().color = Color.green;
            T_LifeBar[1].GetComponent<SpriteRenderer>().color = Color.green;
            T_LifeBar[2].GetComponent<SpriteRenderer>().color = Color.green;
        } else if(Life == 2) {
            Debug.Log("Yellow");
            T_LifeBar[0].GetComponent<SpriteRenderer>().color = Color.yellow;
            T_LifeBar[1].GetComponent<SpriteRenderer>().color = Color.yellow;
        } else if(Life == 1) {
            Debug.Log("red");
            T_LifeBar[0].GetComponent<SpriteRenderer>().color = Color.red;
        }
    }

}
