using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MathsTests : MonoBehaviour
{
	public Vector3 test;

	// Start is called before the first frame update
	void Start()
	{
	}

	// Update is called once per frame
	void Update()
	{
		// Task: Add Debug.Draw

		// Use something like for each value. Some are float so make a use something like 'new Vector3(0,value,0)'.
		//   Debug.DrawLine(new Vector3(0,0,0), value goes here);

		Debug.Log("sqrMagnitude = " + test.sqrMagnitude);
		Debug.DrawLine(new Vector3(0, 0, 0), new Vector3(0, test.sqrMagnitude, 0));

		Debug.Log("normalized = " + test.normalized);
		Debug.Log("magnitude = " + test.magnitude);

		Debug.Log(Vector3.Angle(Vector3.forward, test));
		Debug.Log(Vector3.Cross(Vector3.up, test));
		Debug.Log(Vector3.Dot(Vector3.up, test));

		Debug.Log(Vector3.ProjectOnPlane(Vector3.up, test));
		Debug.Log(Vector3.Project(Vector3.up, test));


		// Short
		Debug.DrawLine(new Vector3(0, 0, 0), new Vector3(0, test.sqrMagnitude, 0));

		
		
		
		

		Vector3 startPoint = new Vector3(0, 0, 0);
		Vector3 endPoint   = Vector3.Cross(Vector3.up, test);

		Debug.DrawLine(startPoint, endPoint);

		
		
		
		
		
		

		// Short version all on one line
		// Note: If my Drawline function WANTS a Vectors and my Reflect function RETURNS a Vector3, you can run it directly inside the function
		Debug.DrawLine(new Vector3(0, 0, 0), Vector3.Reflect(Vector3.up, test));


		// Long version
		// Beginners tend to make more variable for each part as it's clearer.
		Vector3 reflect       = Vector3.Reflect(Vector3.up, test);
		Vector3 startPosition = new Vector3(0, 0, 0);

		Debug.DrawLine(startPosition, reflect);
	}
}