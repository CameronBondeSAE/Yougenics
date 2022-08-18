using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Luke
{
	public class LukeBumper : MonoBehaviour
	{
		[SerializeField] private Rigidbody _rb;
		[SerializeField] private Transform _parentTransform;
		[SerializeField] private Transform _myTransform;
        public float acceleration;
		public float turnSpeed;
        public float size = 0.6f;
        public Vector3 forward;
        public List<GameObject> antennae;
		
		// Start is called before the first frame update
		void Start()
		{
			_rb = GetComponentInParent<Rigidbody>();
			_myTransform = transform;
			_parentTransform = _myTransform.parent;
            Critter parent = GetComponentInParent<Critter>();
            acceleration = parent.acceleration*2;
            turnSpeed = parent.turnSpeed*7f;
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
				foreach (GameObject t in antennae)
				{
					t.SetActive(false);
				}
				_rb.AddForce(hitInfo.normal*acceleration, ForceMode.Acceleration);
				_rb.AddRelativeTorque(new Vector3(0,turnSpeed,0), ForceMode.Acceleration);
			}
			else
			{
				foreach (GameObject t in antennae)
				{
					t.SetActive(true);
				}
			}
		}
	}
}