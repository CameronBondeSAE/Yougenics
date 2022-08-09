using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class WallTest : MonoBehaviour
{
    [Button]
    public void RotateWall()
    {
        StartCoroutine(RotateWallCoroutine());
    }
    
    public IEnumerator RotateWallCoroutine()
    {
        WallUpdated();
        yield return new WaitForSeconds(1f);
        transform.Rotate(0,90,0,Space.Self);
        yield return new WaitForSeconds(1f);
        WallUpdated();
    }
    
    
    public void WallUpdated()
    {
        Utilities.OnWorldObstacleUpdatedEvent(gameObject, GetComponent<Collider>().bounds);
    }
    
    //need to store where you were
    //need to store where you end up
    //once moved, then call the update for start and end
}
