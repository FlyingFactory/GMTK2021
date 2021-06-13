using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle {

    public class Enemy2 : EnemyUnit {
        public override Action fireAction => Fire;
        public Projectile bullet = null;

        private void Start() {
            MaxHealth = 30;
            Health = 30;
            MoveSpeed = 2;
        }

        public void Fire() {
            if (bullet != null) {
                Projectile b = Instantiate(bullet);
                b.Init(transform.position + 0.5f * Vector3.up, 10, 6 * (PlayerUnit.player.transform.position - transform.position).normalized, "EnemyProjectile");
            }
        }
    }

}