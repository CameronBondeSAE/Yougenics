using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelButton : MonoBehaviour
{
    public string myLevel;
    public void SetSceneToLoad()
    {
        LobbyUIManager.instance.sceneToLoad = myLevel;
    }
}
