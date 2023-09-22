using UnityEngine;

namespace LoveTriangle {

	public class ObjectPoolLibraryCommon : MonoSingleton<ObjectPoolLibraryCommon> {

        public enum PoolType { NULL, MONEY_VFX, SMOKE_EXPLOSION_WHITE, HAPPY_EMOJI, SAD_EMOJI, SMOKE_EXPLOSION_LARGE, SPARKLE_ORANGE, UPGRADE_VFX, FLOATING_MONEY };

        public ObjectPoolManager[] objectPoolers;

		public Canvas uiCanvas;
		public Camera cam;
		public ObjectPoolManager GetObjectPooler(PoolType poolerType) {
			if (objectPoolers == null || objectPoolers.Length <= 0)
				return null;
			return objectPoolers[(int)poolerType];
		}
	}
}