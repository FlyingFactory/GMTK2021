using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

        [SerializeField]
        public float fadeRate = 4f; //Used to adjust image fade speed

        private UnitFacing currentSel;

        public Button buttonUp, buttonDown, buttonLeft, buttonRight;

        public Animator animator = null;

        public float MaxHealth = 100;
        public float Health = 100;
        public float MoveSpeed = 5;
        public Rigidbody MovementCollider { get; set; }

        public UnitFacing AnimFacing = UnitFacing.South;
        public bool AnimMove = false;
        public bool AnimShoot = false;

        private void Start()
        {
            currentSel = UnitFacing.South;

            
        }

        private void Update()
        {
            if (animator != null) {
                if (Input.GetKeyDown(KeyCode.W)) animator.SetTrigger("Direction up");
                if (Input.GetKeyDown(KeyCode.S)) animator.SetTrigger("Direction down");
                if (Input.GetKeyDown(KeyCode.D)) animator.SetTrigger("Direction rigth");
                if (Input.GetKeyDown(KeyCode.A)) animator.SetTrigger("Direction left");
            }
        }

        public void Move(Vector3 displacement) {
            RaycastHit hit;
            if (MovementCollider == null || !MovementCollider.SweepTest(displacement, out hit, displacement.magnitude, QueryTriggerInteraction.Ignore)) {
                transform.position += displacement;
            }
        }

        public virtual void DealDamage(float damage) {
            Health -= damage;
            if (Health <= 0) {
                // spawn death animation
                Destroy(gameObject);
            }
        }
    }

}
