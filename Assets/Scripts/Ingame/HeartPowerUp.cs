using System.Collections;
using System;
using UnityEngine;

public class HeartPowerUp : PowerUp {

	public override void DisplaVFX() {
		base.DisplaVFX();
		VFXDisplayer.DisplayVFX(LoveTriangle.ObjectPoolLibraryCommon.PoolType.MONEY_VFX, transform.position, 1f);
	}

    public override void GetPowerUp() {
        TapticPlayer.PlayTapticMedium();
        onPickedUp?.Invoke(this, points);
        base.GetPowerUp();
	}
}