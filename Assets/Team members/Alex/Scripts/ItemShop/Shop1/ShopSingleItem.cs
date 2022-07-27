using Alex;
using UnityEngine;

public class ShopSingleItem : MonoBehaviour
{
	public  ItemInfo   itemInterface; // Unity doesn't show interfaces in inspector
	public  GameObject itemToSpawn;   // So we'll use normal GameObject (the trouble is you can drag ANYTHING now, we need to check for the interface)
	public  Transform  spawningPoint;
	public  Spawner    spawner;
	public  Button     button;
	

	void Start()
	{

		itemInterface = itemToSpawn.GetComponent<IItem>().GetInfo();
		//itemHeight = itemInterface.height;
		button.buttonPressedEvent += SpawnItem;
	}

	public void SpawnItem()
	{
		spawner.SpawnSingle(itemToSpawn, spawningPoint.position +  new Vector3(0, itemInterface.height,0), spawningPoint.rotation);
	}
}