using UnityEngine;

namespace LoveTriangle {

	public class ProjectilePoolLibraryCommon : MonoSingleton<ProjectilePoolLibraryCommon> {

		public enum PoolType { NULL, FIRE_BALL, ICE_BALL };

		public ObjectPoolManager[] objectPoolers;

		public ObjectPoolManager GetObjectPooler(PoolType poolerType) {
			if (objectPoolers == null || objectPoolers.Length <= 0)
				return null;
			return objectPoolers[(int)poolerType];
		}
	}
}