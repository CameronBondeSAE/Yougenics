using System.Collections;
using TMPro;
using UnityEngine;
using DG.Tweening;
using Kevin;


public class UINotifcations : MonoBehaviour
{
    public static UINotifcations instance;
    public TMP_Text notificationText;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        notificationText = GetComponent<TMP_Text>();
        StartCoroutine(sendNotification("Collect energy before the world drowns", 10));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NotificationText(string text, int time)
    {
        StartCoroutine(sendNotification(text, time));
    }

    public IEnumerator sendNotification(string text, int time)
    {
        notificationText.color = Color.white;
        notificationText.text = text;
        notificationText.DOFade(0, time);
        yield return new WaitForSeconds(time);
        notificationText.text = "";
        
    }
    
}
