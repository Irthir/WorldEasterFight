using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public List<Transform> LifeBarSpritList;


    public int Life = 3;
    public int ActionPoint = 3;
    public Transform LifeBar;
    public bool ThisPlayerGetHit = false;
    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform child in LifeBar)
        {
            LifeBarSpritList.Add(child.transform);
        }
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
        Destroy(LifeBarSpritList[LifeBarSpritList.Count - 1].gameObject);
        LifeBarSpritList.RemoveAt(LifeBarSpritList.Count - 1);
        Life--;

        if (Life == 2) 
        {
            Debug.Log("Yellow");
            LifeBarSpritList[0].GetComponent<SpriteRenderer>().color = Color.yellow;
            LifeBarSpritList[1].GetComponent<SpriteRenderer>().color = Color.yellow;
        }
        else if (Life == 1)
        {
            Debug.Log("red");

            LifeBarSpritList[0].GetComponent<SpriteRenderer>().color = Color.red;
        }

    }
}
