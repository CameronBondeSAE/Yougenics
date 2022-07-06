using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraModel : MonoBehaviour
{
    public PlayerModel target;
    public float heightOffset;

    [HideInInspector]
    public float mouseX, mouseY;
    float xRotation;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        transform.SetParent(target.transform);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = target.gameObject.transform.position;

        //Setting x rotation to match mouse direction + locking it at 90 angle
        xRotation -= mouseY * target.lookSensitivity;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
    }
}
