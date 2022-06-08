using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Creature : MonoBehaviour
{
    public Energy energyScript;

    public bool isHungry;
    //public bool foodFound;
    public Food foodObj;
    public float rotateSpeed;
    public float moveSpeed;
    public bool atFood;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void CheckEnergy()
    {
        if (energyScript.energyAmount <= energyScript.energyMax * 0.5f)
        {
            isHungry = true;
        }
        else
        {
            isHungry = false;
        }
    }
    
    public bool FindFood()
    {
        RaycastHit hit;
        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward * 100));
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, 100))
        {
            if (hit.collider.GetComponent<Food>() != null)
            {
                Debug.Log("THERES DA FOOD");
                return true;
            }
        }
        else
        {
            transform.Rotate(new Vector3(transform.rotation.eulerAngles.x,
                transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z) *rotateSpeed);
            Debug.Log("WHERE DA FOOD??!??!");

        }
        return false;

    }

    public void GoToFood()
    {
        transform.Translate(Vector3.forward * moveSpeed);
        
        //collider check for touching food
        //att food = true
        
        
    }

    private void OnTriggerEnter(Collider other)
    
    {
        if (other.GetComponent<Food>() != null)
        {
            Debug.Log("OOOH FOOD");
            atFood = true;
        }
        else
        {
            atFood = false;
        }
    }
    

    public void EatFood()
    {
        //if atfood is true
        if (atFood)
        {
            Debug.Log("YUM YUM YUM");
            energyScript.energyAmount += 50;
        }
    }
}

    
