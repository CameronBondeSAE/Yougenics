using Alex;
using UnityEngine;
using UnityEngine.UI;


public class TaskList : MonoBehaviour
{
    //private bool toggle;
    public Player player;
    public bool outOfEnergy;


    // Update is called once per frame
    void Update()
    {
        OutOfEnergy();
    }

    public void OutOfEnergy()
    {
        if (player.energy.EnergyAmount.Value <= 0)
        {
            outOfEnergy = true;
        }
    }

}
