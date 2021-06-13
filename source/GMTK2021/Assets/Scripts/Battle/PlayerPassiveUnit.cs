using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle {

    public class PlayerPassiveUnit : BaseUnit {

        private bool stopped = false;
        private Vector3 velocity;
        private float gravity = 8f;

        private void Start() {
            PlayerUnit.player.battery = this;
            this.Pickup();
        }

        public void InitThrow(Vector3 direction) {
            transform.position = PlayerUnit.player.transform.position + Vector3.up * 0.8f;
            this.gameObject.SetActive(true);
            this.stopped = false;
            this.velocity = new Vector3(direction.x, 1f, direction.z);
        }

        public void Pickup() {
            this.gameObject.SetActive(false);
        }

        private void FixedUpdate() {
            if (!stopped) {
                this.Move(velocity * Time.fixedDeltaTime);
                this.velocity += Vector3.down * gravity * Time.fixedDeltaTime;
                if (transform.position.y <= 0.1f) {
                    stopped = true;
                }
            }
        }
    }

}