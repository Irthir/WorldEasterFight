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
            CheckLifeBar();
            ThisPlayerGetHit = false;
        }

    }

    void CheckLifeBar()
    {
        T_LifeBar[Life - 1].SetActive(false);
        Life--;

        if (Life == 2) 
        {
            Debug.Log("Yellow");
            /*LifeBarSpritListTransform[0].GetComponent<SpriteRenderer>().color = Color.yellow;
            LifeBarSpritListTransform[1].GetComponent<SpriteRenderer>().color = Color.yellow;*/
        }
        else if (Life == 1)
        {
            Debug.Log("red");

            //LifeBarSpritListTransform[0].GetComponent<SpriteRenderer>().color = Color.red;
        }

    }
}
