using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle {

    public enum UnitFacing {
        North,
        NorthEast,
        East,
        SouthEast,
        South,
        SouthWest,
        West,
        NorthWest,
    }

    public class BaseUnit : MonoBehaviour {
        public virtual float Health { get; set; } = 100;
        public virtual float MoveSpeed { get; set; } = 5;
        public Rigidbody MovementCollider { get; set; }

        public UnitFacing AnimFacing = UnitFacing.South;
        public bool AnimMove = false;
        public bool AnimShoot = false;

        public void Move(Vector3 displacement) {
            RaycastHit hit;
            if (MovementCollider == null || !MovementCollider.SweepTest(displacement, out hit, displacement.magnitude, QueryTriggerInteraction.Ignore)) {
                transform.position += displacement;
            }
        }

    }

}
