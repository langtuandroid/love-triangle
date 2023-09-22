using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lofelt.NiceVibrations;
public static class TapticPlayer {
    public static bool IsOn { set; get; } = true;

    public static void PlayTapticLight(){
        if(IsOn){
            HapticPatterns.PlayPreset(HapticPatterns.PresetType.LightImpact);
        }
    }
    public static void PlayTapticMedium(){
        if(IsOn){
            HapticPatterns.PlayPreset(HapticPatterns.PresetType.MediumImpact);
        }
    }
}
