using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldPowerUp : PowerUp {
	public override void DisplaVFX() {
		base.DisplaVFX();
		VFXDisplayer.DisplayVFX(LoveTriangle.ObjectPoolLibraryCommon.PoolType.SMOKE_EXPLOSION_WHITE, transform.position, 1f);
	}
}
