using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Tests : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // Keep a reference to the thread
        Thread newThread = new Thread(LongRunningFunction);
        newThread.Start();

        // Just start a thread without a reference
        // new Thread(LongRunningFunction).Start();

    }

    void LongRunningFunction()
    {
        Debug.Log("Hi from thread!");
    }
}
