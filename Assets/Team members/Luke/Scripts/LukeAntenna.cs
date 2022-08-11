using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Luke
{
	public class LukeAntenna : MonoBehaviour
	{
		[SerializeField] private Rigidbody _rb;
		[SerializeField] private Transform _transform;
		public Vector3 direction;
		public float acceleration;
		public float turnSpeed;
		
		// Start is called before the first frame update
		void OnEnable()
		{
			_rb = GetComponent<Rigidbody>();
			_transform = GetComponent<Transform>();
		}

		// Update is called once per frame
		void FixedUpdate()
		{
			Debug.DrawLine(_transform.position, _transform.TransformDirection(direction));
			if (Physics.Linecast(_transform.position, _transform.TransformDirection(direction), out RaycastHit hitInfo))
			{
				float mag = Vector3.Magnitude(direction);
				_rb.AddForce(-direction*(acceleration*(mag-hitInfo.distance)/mag));
				_rb.AddRelativeTorque(new Vector3(0,Mathf.Sign(direction.x)*turnSpeed*((mag-hitInfo.distance)/mag),0));
			}
		}
	}
}