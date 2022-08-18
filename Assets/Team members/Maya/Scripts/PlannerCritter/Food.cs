using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Maya
{
	[RequireComponent(typeof(Energy))]
	public class Food : MonoBehaviour, IEdible, IItem
	{
		public float scaleFactor;
		Energy       energy;

		public ItemInfo info;

		void Awake()
		{
			energy                    =  GetComponent<Energy>();
			energy.EnergyChangedEvent += EnergyOnEnergyChangedEvent;
			energy.EnergyAmount.Value =  Random.Range(10f, 25f);
		}

		void EnergyOnEnergyChangedEvent(float amount)
		{
			float energyAmountValue = Mathf.Clamp(energy.EnergyAmount.Value * scaleFactor, 0.01f, 9999f);
			gameObject.transform.DOScale(new Vector3(energyAmountValue, energyAmountValue, energyAmountValue), 1f).SetEase(Ease.OutElastic);

			if (energy.EnergyAmount.Value <=0)
			{
				Destroy(gameObject, 1f);
				// TODO: Networking
			}
		}

		public float GetEnergyAmount()
		{
			return energy.EnergyAmount.Value;
		}

		public float EatMe(float energyRemoved)
		{
			energy.ChangeEnergy(-energyRemoved);

			return energyRemoved;
		}

		public void SpawnedAsNormal()
		{
		}

		public ItemInfo GetInfo()
		{
			return info;
		}
	}
}