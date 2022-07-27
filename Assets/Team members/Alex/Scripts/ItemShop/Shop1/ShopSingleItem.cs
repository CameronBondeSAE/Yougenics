using Alex;
using UnityEngine;

class ShopSingleItem : MonoBehaviour
{
	private IItem      itemInterface; // Unity doesn't show interfaces in inspector
	public  GameObject itemToSpawn;   // So we'll use normal GameObject (the trouble is you can drag ANYTHING now, we need to check for the interface)

	public  Transform  spawningPoint;
	public  Spawner    spawner;
	public  Button     button;
	

	void Start()
	{
		button.buttonPressedEvent += SpawnItem;
	}

	public void SpawnItem()
	{
		spawner.SpawnSingle(itemToSpawn, spawningPoint.position, spawningPoint.rotation);
	}
}