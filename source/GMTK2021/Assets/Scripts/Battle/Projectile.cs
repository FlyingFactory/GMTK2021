using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle {

    public class Projectile : MonoBehaviour {

        public float lifespan = 10f;
        public float damage = 10;
        public Vector3 velocity = Vector2.zero;

        public void Init(Vector3 initPosition, float damage, Vector3 velocity, string layer) {
            this.transform.position = initPosition;
            this.damage = damage;
            this.velocity = velocity;
            gameObject.layer = LayerMask.NameToLayer(layer);
        }

        private void FixedUpdate() {
            transform.position += velocity * Time.fixedDeltaTime;
        }

        private void OnTriggerEnter(Collider other) {
            BaseUnit b = other.GetComponentInParent<BaseUnit>();
            if (b) {
                b.DealDamage(this.damage);
            }
            Destroy(gameObject);
        }
    }

}