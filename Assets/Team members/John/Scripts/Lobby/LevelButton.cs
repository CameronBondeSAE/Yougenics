using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace John
{
    public class LevelButton : MonoBehaviour
    {
        public string myLevel;

        public void SetSceneToLoad()
        {
            LobbyUIManager.instance.sceneToLoad = myLevel;
        }
    }
}