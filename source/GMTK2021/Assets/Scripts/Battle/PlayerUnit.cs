using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle {

    public class PlayerUnit : BaseUnit {

        public static PlayerUnit player;

        public bool carrying = true;
        public PlayerPassiveUnit battery;

        public float energy = 1;
        public float energyCost = 0.1f;
        public float energyRate = 0.2f;

        public float fireRate = 4;
        private float fireCounter = 0;
        public Projectile bullet;

        public float dashCooldown = 1;
        private float dashCounter = 0;
        private float dashRemainingDuration = 0;
        public float dashDuration = 0.2f;
        private Vector3 dashDirection = Vector3.forward;
        public float dashSpeed = 10;

        public float throwCooldown = 0.5f;
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
            if (dashRemainingDuration > 0) {
                this.Move(dashDirection * dashSpeed * Time.fixedDeltaTime);
                dashRemainingDuration -= Time.fixedDeltaTime;
            }
            else {
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
            }
            
            if (!carrying) {
                if (Input.GetAxis("Fire1") > 0.1f && fireCounter <= 0 && energy >= energyCost) {
                    Projectile b = Instantiate(bullet);
                    b.Init(transform.position + 0.5f * Vector3.up, 10, 8 * Utils.GetMouseVectorXZ(transform.position).normalized, "PlayerProjectile");
                    energy -= energyCost;
                    fireCounter = 1f / fireRate;
                }
            }
            else {
                energy = Mathf.Min(1, energy + energyRate * Time.fixedDeltaTime);
            }

            if (Input.GetAxis("Jump") > 0.1f && dashCounter <= 0) {
                dashCounter = dashCooldown;
                dashRemainingDuration = dashDuration;
                dashDirection = inputVector;
                if (dashDirection.magnitude < Mathf.Epsilon) {
                    dashDirection = Utils.GetMouseVectorXZ(transform.position).normalized;
                }
            }

            if (Input.GetAxis("Fire2") > 0.1f && throwCounter <= 0) {
                throwCounter = throwCooldown;
                if (carrying) {
                    this.battery.InitThrow(Utils.GetMouseVectorXZ(transform.position).normalized * 4);
                    this.carrying = false;
                }
                else {
                    if ((this.battery.transform.position - transform.position).magnitude < 1) {
                        this.battery.Pickup();
                        this.carrying = true;
                    }
                }
            }
        }

        public override void DealDamage(float damage) {
            Health -= damage;
            if (Health <= 0) {
                // spawn death animation
                gameObject.SetActive(false);
            }
        }
    }

}
