using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MoneyPowerUp : PowerUp {

	public LoveTriangle.ObjectPoolLibraryCommon.PoolType vfxType;
	public SFXPlayer.SFX_TYPE sfxType;
	public override void DisplaVFX() {
		base.DisplaVFX();
		VFXDisplayer.DisplayVFX(vfxType, transform.position, 1f);
	}

    public override void GetPowerUp() {
        PlayerData.IncreaseCurrentLevelMoney((int)points);
        onPickedUp?.Invoke(this, PlayerData.CurrentLevelMoney);
		SFXPlayer.Instance.PlaySFX(sfxType);
        base.GetPowerUp();
	}
}
