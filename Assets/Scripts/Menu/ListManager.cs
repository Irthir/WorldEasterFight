using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EmptyList()
    {
        if (gameObject.transform != null)
        {
            foreach (Transform child in gameObject.transform)
            {
                DestroyRecursively(child.gameObject);
            }
        }
    }

    private void DestroyRecursively(GameObject gameObject)
    {
        foreach (Transform child in gameObject.transform)
        {
            DestroyRecursively(child.gameObject);
        }

        GameObject.Destroy(gameObject);
    }
}
