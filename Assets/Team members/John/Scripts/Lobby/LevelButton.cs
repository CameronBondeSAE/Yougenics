using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelButton : MonoBehaviour
{
    public void SetSceneToLoad()
    {
        LobbyUIManager.instance.sceneToLoad = GetComponentInChildren<TMPro.TMP_Text>().text;
    }
}
