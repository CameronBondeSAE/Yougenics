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
        public LayerMask layerMask;

        private float angle;
		
		// Start is called before the first frame update
		void Start()
		{
			_rb = GetComponentInParent<Rigidbody>();
			_myTransform = transform;
			_parentTransform = _myTransform.parent;
            Critter parent = GetComponentInParent<Critter>();
            acceleration = parent.acceleration;
            turnSpeed = parent.turnSpeed/parent.numberOfAntennae;
            angle = transform.rotation.eulerAngles.y;
            if (angle > 180) angle -= 360;
		}

		// Update is called once per frame
		void FixedUpdate()
        {
            float growingSize = size * _parentTransform.localScale.x;
            forward = _myTransform.forward;
            Vector3 position = _myTransform.position;
			Debug.DrawLine(position, position+forward*growingSize);
			if (Physics.Linecast(position, position+forward*growingSize, out RaycastHit hitInfo, layerMask))
			{
                _rb.AddForce(hitInfo.normal*(acceleration*(growingSize-hitInfo.distance)/growingSize), ForceMode.Acceleration);
                float normalAngle = Vector3.SignedAngle(-forward,hitInfo.normal, Vector3.up);
                float angleToWall = 90 + normalAngle + angle;
                if (angleToWall > 90) angleToWall -= 180;
				_rb.AddRelativeTorque(new Vector3(0,angleToWall*turnSpeed*((growingSize-hitInfo.distance)/growingSize/2f),0), ForceMode.Acceleration);
			}
		}
	}
}