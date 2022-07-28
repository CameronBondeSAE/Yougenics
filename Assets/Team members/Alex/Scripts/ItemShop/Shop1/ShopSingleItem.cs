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
	public bool beingBuilt;
	public bool canSpawn = true;


	void Start()
	{
		itemInfo = itemToSpawn.GetComponent<IItem>().GetInfo();
		button.buttonPressedEvent += SpawnItem;
	}

	public void SpawnItem()
	{
		Wrapper();
	}

	public void Wrapper()
	{
		if (beingBuilt == false)
		{
			StartCoroutine(StartBuild());
		}
	}
	

	public IEnumerator StartBuild()
	{
		if (canSpawn)
		{
			canSpawn = false;
			beingBuilt = true;
			yield return new WaitForSeconds(itemInfo.buildTime);
			spawner.SpawnSingle(itemToSpawn, spawningPoint.position +  new Vector3(0, itemInfo.height,0), spawningPoint.rotation);
			canSpawn = true;
			beingBuilt = false;
		}
	}
}