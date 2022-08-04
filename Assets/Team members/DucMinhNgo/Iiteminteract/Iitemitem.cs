using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Minh
{
    public class Iitemitem : MonoBehaviour, IItem, IInteractable
    {
        public GameObject m;
        public ItemInfo itemInfo;
        public void SpawnedAsNormal()
        {
            throw new System.NotImplementedException();
        }

        public ItemInfo GetInfo()
        {
            return itemInfo;
        }
        public void Interact()
        {
            DoThing();
        }

        public void DoThing()
        {
            m.GetComponent<Renderer> ().material.color = Color.yellow;
            Debug.Log("Do thing");
        }
    }
}

