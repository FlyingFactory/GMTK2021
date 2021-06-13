using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle {

    public class PlayerUnit : BaseUnit {

        public static PlayerUnit player;

        public bool carrying = true;
        public PlayerPassiveUnit battery;

        public float fireRate;
        private float fireCounter = 0;

        public float dashCooldown;
        private float dashCounter = 0;

        private float throwCounter = 0; // to prevent instantly picking up and throwing in the same frame

        private void Awake() {
            if (player != null) {
                Destroy(player);
            }
            player = this;
        }

        private void FixedUpdate() {

            if (fireCounter > 0) fireCounter -= Time.fixedDeltaTime;
            if (dashCounter > 0) dashCounter -= Time.fixedDeltaTime;
            if (throwCounter > 0) throwCounter -= Time.fixedDeltaTime;

            // Moving
            Vector3 inputVector = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            if (inputVector.sqrMagnitude > Mathf.Epsilon) {
                if (inputVector.sqrMagnitude > 1) inputVector = inputVector / inputVector.magnitude;
                inputVector = Quaternion.Euler(new Vector3(0, 45, 0)) * inputVector;

                UnitFacing facing = Utils.GetFacing(inputVector);

                // set animation
                this.AnimFacing = facing;
                this.AnimMove = true;

                // move unit
                this.Move(inputVector * MoveSpeed * Time.fixedDeltaTime);
            }
            else {
                this.AnimMove = false;
            }

            // Shooting
            if (Input.GetAxis("Fire1") > 0.1f &&!carrying && fireCounter <= 0) {
                // fire and drain energy
            }

            if (Input.GetAxis("Jump") > 0.1f && dashCounter <= 0) {
                // dash
            }

            if (Input.GetAxis("Fire2") > 0.1f && throwCounter <= 0) {
                // either throw or pickup
            }
        }
    }

}
