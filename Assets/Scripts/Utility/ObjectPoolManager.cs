/* Object Pooler by Aaron Aranas
 * 
 * attach this script to a gameobject, that game object will be the parent of the pools objects
 * make sure to have a reference on your pool manager to get access of the pool objects
 * 
 * you can also have many poolManager on your scene as long as you have different references. ex. ObjectPoolManager bulletPool, ObjectPoolManager obstaclePool etc......
 * you should create different gameobject on different pool eg. bullets, obstacles and have different gameObjects on the scene.
 * 
 * 
 * perfect combination is you know the least number of your pool (set it to initialCountOnScene in inspector) and check canGrow to have flexibility
 * */

using UnityEngine;

namespace LoveTriangle {

	public class ObjectPoolManager : MonoBehaviour {

		public GameObject[] objectToPool;
		public int maxCount = 0;

		public GameObject GiveGameObject() {
			int randObject = UnityEngine.Random.Range(0, objectToPool.Length);
			if (maxCount > 0) {
				var count = GameObjectRecycler.instance.InstanceCount(objectToPool[randObject]);
				if (count >= maxCount)
					return null;
			}
			return GameObjectRecycler.instance.Release(objectToPool[randObject]);
		}

		public void ReturnObject(GameObject instance) {
            GameObjectRecycler.instance.Acquire(instance);
		}
	}

}