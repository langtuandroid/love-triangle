using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace LoveTriangle {

	[AddComponentMenu("PogoPeople/Custom/Game Object Recycler", 102)]
	public class GameObjectRecycler : MonoSingleton<GameObjectRecycler> {

		/// <summary>
		/// Key: Prefab instance id
		/// Value: HashSet of object container
		/// </summary>
		private Dictionary<int, HashSet<GameObject>> _masterContainer;

		/// <summary>
		/// Key: Instance id of instance
		/// Value: Instance id of reference prefab which is also the key for the container
		/// </summary>
		private Dictionary<int, int> _instanceToContainerLookup;


		public override void Init() {
			_masterContainer = new Dictionary<int, HashSet<GameObject>>();
			_instanceToContainerLookup = new Dictionary<int, int>();
		}

		/// <summary>
		/// Gets the container for the prefab. If container does not exist, create one
		/// </summary>
		private HashSet<GameObject> ContainerForPrefab(GameObject go) {
			if (_masterContainer.ContainsKey(go.GetInstanceID())) {
				return _masterContainer[go.GetInstanceID()];
			}
			else {
				Debug.Log("No container for prefab. Creating a new one!");
				HashSet<GameObject> container = new HashSet<GameObject>();
				_masterContainer.Add(go.GetInstanceID(), container);
				return container;
			}
		}

		/// <summary>
		/// Acquire the specified instance and store it to its respective recycler.
		/// Specified instance must be created by the recycler.
		/// </summary>
		public void Acquire(GameObject instance) {
			if (_instanceToContainerLookup.ContainsKey(instance.GetInstanceID())) {
				int refId = _instanceToContainerLookup[instance.GetInstanceID()];
				HashSet<GameObject> container = _masterContainer[refId];
				instance.SetActive(false);

				instance.transform.SetParent(transform, false);

				container.Add(instance);
			}
			else {
				Debug.LogWarning("The game object " + instance.name + " is not spawned by the recycler!");
			}
		}

		/// <summary>
		/// Release an instance of specified prefab.
		/// Search the container if it contains instance of the prefab.
		/// If the container is empty, instantiate a new one.
		/// </summary>
		public GameObject Release(GameObject prefab) {
			HashSet<GameObject> container = ContainerForPrefab(prefab);
			foreach (GameObject go in container) {
				//LoggerTool.Log("Found an instance! Returning to requestor...", LoggerTool.LOG_TYPE.UTILIY);
				go.SetActive(true);
				container.Remove(go);
				return go;
			}

			// Container was empty. Instantiate a new one
			//LoggerTool.Log("Container is empty. Instantiate a new one!", LoggerTool.LOG_TYPE.UTILIY);
			var obj = GameObject.Instantiate(prefab) as GameObject;
			_instanceToContainerLookup.Add(obj.GetInstanceID(), prefab.GetInstanceID());
			return obj;
		}

		/// <summary>
		/// Number of instances of a pooling prefab.
		/// </summary>
		public int InstanceCount(GameObject prefab) {
			if (_masterContainer.ContainsKey(prefab.GetInstanceID())) {
				return _masterContainer[prefab.GetInstanceID()].Count;
			}
			else {
				return 0;
			}
		}

	}
}