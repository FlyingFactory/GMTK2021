using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static partial class Utils {

    public static Battle.UnitFacing GetFacing(Vector3 displacement, bool EightDirection = true) {
        float eulerY = Quaternion.LookRotation(displacement, Vector3.up).eulerAngles.y;
        return GetFacing(eulerY, EightDirection);
    }

    public static Battle.UnitFacing GetFacing(float eulerY, bool EightDirection = true) {
        //sanity check
        eulerY = eulerY % 360f;
        if (eulerY < 0) eulerY += 360f;

        if (EightDirection) {
            if (eulerY < 22.5) return Battle.UnitFacing.NorthWest;
            else if (eulerY < 67.5) return Battle.UnitFacing.North;
            else if (eulerY < 112.5) return Battle.UnitFacing.NorthEast;
            else if (eulerY < 157.5) return Battle.UnitFacing.East;
            else if (eulerY < 202.5) return Battle.UnitFacing.SouthEast;
            else if (eulerY < 247.5) return Battle.UnitFacing.South;
            else if (eulerY < 292.5) return Battle.UnitFacing.SouthWest;
            else if (eulerY < 337.5) return Battle.UnitFacing.West;
            else return Battle.UnitFacing.NorthWest;
        }
        else {
            if (eulerY < 90) return Battle.UnitFacing.North;
            else if (eulerY < 180) return Battle.UnitFacing.East;
            else if (eulerY < 270) return Battle.UnitFacing.South;
            else return Battle.UnitFacing.West;
        }
    }
}
