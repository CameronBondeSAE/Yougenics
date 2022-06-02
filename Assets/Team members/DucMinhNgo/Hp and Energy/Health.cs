using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public float Hp;
    // Start is called before the first frame update
    void Start()
    {
        HP0();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void HP0()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            Hp -= 10;
        }
    }

}
