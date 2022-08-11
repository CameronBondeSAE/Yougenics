using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

public class CSharpJobLauncher : MonoBehaviour
{
	// Start is called before the first frame update
	void Start()
	{
		// Complete an arbitrary number of jobs
		NativeArray<JobHandle> handles = new NativeArray<JobHandle>(100, Allocator.Temp);

		for (int i = 0; i < 10; i++)
		{
			CamJob    camJob     = new CamJob();
			JobHandle jobHandle1 = camJob.Schedule();

			handles[i] = jobHandle1;
		}

		JobHandle.CompleteAll(handles);
		
		handles.Dispose();
	}

	void DoAThing()
	{
		float answer = 0;

		for (int i = 0; i < 10000000; i++)
		{
			answer += Mathf.Sqrt(i) + Mathf.PerlinNoise(i * 1.24f, 0);
		}

		Debug.Log("I did something! : " + answer);
	}
}