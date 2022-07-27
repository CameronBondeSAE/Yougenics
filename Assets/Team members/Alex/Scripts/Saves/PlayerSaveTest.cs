using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;

public class PlayerSaveTest : MonoBehaviour
{
    public PlayerProfile playerProfile;
    public TMP_Text text;

    private void Start()
    {
        string myData = JsonUtility.ToJson(playerProfile, true);
        
        // Save to disc
        PlayerPrefs.SetString("Settings", myData);

        // Load from DISC
        string loadedMessage = PlayerPrefs.GetString("Settings");
        
        
        JsonUtility.FromJsonOverwrite(loadedMessage, myData);
        Debug.Log(myData);

        text.text = myData;
        
        Path.Combine(Application.persistentDataPath, "PlayerProfile.json");
    }
}