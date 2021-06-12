using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle {

    public class PlayerUnit : BaseUnit {
        //public override float Health { get; set; }

        private void FixedUpdate() {

            //Debug.Log(Input.GetAxis("Horizontal") + ", " + Input.GetAxis("Vertical"));

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

        }
    }

}
