using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FogChanger {
    public static void ChageFog(Level.EnvironmentType type) {
        RenderSettings.fogColor = Level.fogColors[(int)type];
    }
}
