using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static partial class Utils {

    public static Vector3 GetMouseVectorXZ(Vector3 position) {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Vector3 pos = ray.origin - ray.direction * (ray.origin.y / ray.direction.y);
        return (pos - position);
    }
}