using System.Collections;
using Alex;
using UnityEngine;

public class ShopSingleItem : MonoBehaviour
{
	public  ItemInfo   itemInfo; // Unity doesn't show interfaces in inspector
	public  GameObject itemToSpawn;   // So we'll use normal GameObject (the trouble is you can drag ANYTHING now, we need to check for the interface)
	public  Transform  spawningPoint;
	public  Spawner    spawner;
	public  Button     button;
	public bool canSpawn = true;


	void Start()
	{
		itemInfo = itemToSpawn.GetComponent<IItem>().GetInfo();
		button.buttonPressedEvent += SpawnItem;
	}

	public void SpawnItem()
	{
		StartCoroutine(StartBuild());
	}
	

	public IEnumerator StartBuild()
	{
		if (canSpawn)
		{
			//Can spawn set to false to prevent multiple objects being built at the same time
			canSpawn = false;

			//Waits for build time before creating object in scene
			yield return new WaitForSeconds(itemInfo.buildTime);
			spawner.SpawnSingle(itemToSpawn, spawningPoint.position +  new Vector3(0, itemInfo.height,0), spawningPoint.rotation);
			
			//After build is complete allows you to build a new object
			canSpawn = true;
		}
	}
}