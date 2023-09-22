using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedPowerUp : PowerUp {
	public override void DisplaVFX() {
		base.DisplaVFX();
		VFXDisplayer.DisplayVFX(LoveTriangle.ObjectPoolLibraryCommon.PoolType.MONEY_VFX, transform.position, 1f);
	}
}
