using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

#if UNITY_EDITOR || UNITY_EDITOR_64
using UnityEditor;
#endif

using Random = UnityEngine.Random;

public class Spawner : MonoBehaviour
{
	[System.Serializable]
	public class GroupInfo
	{
		public GameObject[] prefabs;

		[Tooltip("Use SpawnPoint prefab. If there's no spawnpoints, it'll use the transform position")]
		public Transform[] spawnPoints;

		public int countPerGroup;
		// public DayNightManager.DayPhase phaseTime;
	}

	public bool autoStart = false;

	public float radius = 5f;

	public  GroupInfo[] groupInfos;
	public  float       groundOffset;
	private GroupInfo   _currentGroupInfo;

	// add as many as you need for the animals needed to spawn


	public List<GameObject> spawned;

	private void Start()
	{
		if (autoStart) SpawnMultiple();

		// FindObjectOfType<DayNightManager>().PhaseChangeEvent += ChangePhase;
	}

	// public void ChangePhase(DayNightManager.DayPhase timeOfDay)
	public List<GameObject> SpawnMultiple()
	{
		for (int i = 0; i < groupInfos.Length; i++) //searches through all of wildLife aray
		{
			// if (spawnInfos[i].phaseTime == timeOfDay) //if the wildlife dayPhase inside the array matches current day phase
			{
				_currentGroupInfo = groupInfos[i];

				Transform randomTransform;
				for (int j = 0; j < _currentGroupInfo.countPerGroup; j++)
				{
					if (_currentGroupInfo.spawnPoints.Length <= 0)
						// Use my own GO
						randomTransform = transform;
					else
						randomTransform = _currentGroupInfo.spawnPoints[Random.Range(0, _currentGroupInfo.spawnPoints.Length)];

					Vector3 spawnPos = randomTransform.position;
					spawnPos = new Vector3(spawnPos.x, spawnPos.y, spawnPos.z);
					Vector3 randomSpot = Random.insideUnitCircle * radius;
					randomSpot.z = randomSpot.y; //hack, im sure there is an easier way to do this
					randomSpot.y = 0;
					// Debug.Log(randomSpot);
					GameObject randomPrefab =
						_currentGroupInfo.prefabs[Random.Range(0, _currentGroupInfo.prefabs.Length)];
					GameObject spawnedPrefab = SpawnSingle(randomPrefab, spawnPos + randomSpot, randomTransform.rotation);

					spawned.Add(spawnedPrefab);
				}
			}
		}

		return spawned;
	}

	public GameObject SpawnSingle(GameObject prefab, Vector3 pos, Quaternion rotation)
	{
		Vector3 randomSpot;
		GameObject spawnedPrefab = Instantiate(prefab, pos,
											   rotation);

		// Object must have NetworkObject component to work on clients
		spawnedPrefab.GetComponent<NetworkObject>()?.Spawn();

		
		spawnedPrefab.transform.position = Utilities.FindGroundHeight(spawnedPrefab.transform.position, groundOffset);
		
		return spawnedPrefab;
	}

	private void OnDrawGizmos()
	{
		if (_currentGroupInfo != null)
		{
			foreach (Transform spawnPoint in _currentGroupInfo.spawnPoints)
			{
#if UNITY_EDITOR || UNITY_EDITOR_64

				Handles.color = Color.green;
				Handles.zTest = UnityEngine.Rendering.CompareFunction.LessEqual;
				Handles.DrawWireDisc(spawnPoint.position, Vector3.up, radius);
#endif
			}
		}
	}
}