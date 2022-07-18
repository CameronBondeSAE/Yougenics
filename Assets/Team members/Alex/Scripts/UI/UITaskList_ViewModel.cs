using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UITaskList_ViewModel : MonoBehaviour
{
    public Toggle toggleOutOfEnergy;
    public TaskList taskList;

    public void Update()
    {
        
        toggleOutOfEnergy.isOn = taskList.outOfEnergy;
    }
}
