using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Tanks;
using UnityEngine;

public class WallTest : MonoBehaviour
{
    private GameObject startGO;
    private GameObject endGO;
    private Bounds startBounds;
    private Bounds endBounds;
    
    [Button]
    public void RotateWall()
    {
        startGO = gameObject;
        startBounds = GetComponent<Collider>().bounds;
        StartCoroutine(RotateWallCoroutine());
    }
    
    public IEnumerator RotateWallCoroutine()
    {
        transform.Rotate(0,90,0,Space.Self);
        yield return new WaitForSeconds(0.1f);
        endGO = gameObject;
        endBounds = GetComponent<Collider>().bounds;
        
        WallUpdated();
    }
    
    
    public void WallUpdated()
    {
        Utilities.OnWorldObstacleUpdatedEvent(startGO, startBounds);
        Utilities.OnWorldObstacleUpdatedEvent(endGO, endBounds);
    }
    
    //need to store where you were
    //need to store where you end up
    //once moved, then call the update for start and end
}
