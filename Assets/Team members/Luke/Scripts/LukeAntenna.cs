using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Luke
{
	public class LukeAntenna : MonoBehaviour
	{
		[SerializeField] private Rigidbody _rb;
		[SerializeField] private Transform _parentTransform;
		[SerializeField] private Transform _myTransform;
        public float acceleration;
		public float turnSpeed;
        public float size;
        public Vector3 forward;
		
		// Start is called before the first frame update
		void OnEnable()
		{
			_rb = GetComponentInParent<Rigidbody>();
			_parentTransform = GetComponentInParent<Transform>();
			_myTransform = GetComponent<Transform>();
            Critter parent = GetComponentInParent<Critter>();
            acceleration = parent.acceleration*2;
            turnSpeed = parent.turnSpeed*5;
            
        }

		// Update is called once per frame
		void FixedUpdate()
        {
            float growingSize = size * _parentTransform.localScale.x;
            forward = _myTransform.forward;
            Vector3 position = _myTransform.position;
			Debug.DrawLine(position, position+forward*growingSize);
			if (Physics.Linecast(position, position+forward*growingSize, out RaycastHit hitInfo))
			{
                _rb.AddForce(hitInfo.normal*(acceleration*(growingSize-hitInfo.distance)/growingSize), ForceMode.Acceleration);
                float angle = Vector3.SignedAngle(forward,_parentTransform.forward, Vector3.up);
				_rb.AddRelativeTorque(new Vector3(0,-Mathf.Sign(angle)*turnSpeed*((growingSize-hitInfo.distance)/growingSize),0), ForceMode.Acceleration);
			}
		}
	}
}