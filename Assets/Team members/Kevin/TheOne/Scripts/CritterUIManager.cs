using System.Collections;
using System.Collections.Generic;
using Kev;
using UnityEngine;
using UnityEngine.UI;

public class CritterUIManager : MonoBehaviour
{
    public GameObject critterPrefab;
    public CritterBase critterBase;
    public RawImage rawImagePrefab;
    public List<Texture> iconStates;

    public int currentState;
    
    // Start is called before the first frame update
    void Awake()
    {
        //Texture texture = rawImagePrefab.GetComponent<RawImage>().texture;
        critterBase = critterPrefab.GetComponent<CritterBase>();
    }

    // Update is called once per frame
    void Update()
    {
        if (critterBase.isPatrolling)
        {
            currentState = 0;
        }

        if (critterBase.isChasing)
        {
            currentState = 1;
        }

        if (critterBase.isEating)
        {
            currentState = 2;
        }

        if (critterBase.isMating)
        {
            currentState = 3;
        }

        if (critterBase.isSleeping)
        {
            currentState = 4;
        }

        rawImagePrefab.GetComponent<RawImage>().texture = iconStates[currentState];
    }
}
