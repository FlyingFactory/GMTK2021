using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle {

    public abstract class EnemyUnit : BaseUnit {

        public virtual Action fireAction => null;
        public float aggroDistance = 10;

        public virtual float fireDistance => 5;
        public virtual float fireCooldown => 2;
        public float fireCounter = 0;

        public bool activated = false;

        // general enemy behaviors should go here
        private void FixedUpdate() {

            if (fireCounter > 0) fireCounter -= Time.fixedDeltaTime;

            if (!activated) {
                if ((PlayerUnit.player.transform.position - transform.position).magnitude <= aggroDistance) {
                    activated = true;
                }
                else {
                    return;
                }
            }

            if ((PlayerUnit.player.transform.position - transform.position).magnitude > fireDistance) {
                this.Move((PlayerUnit.player.transform.position - transform.position).normalized * MoveSpeed * Time.fixedDeltaTime);
            }
            else if (fireCounter <= 0) {
                this.fireAction();
                this.fireCounter = this.fireCooldown;
            }
        }
    }

}