using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Legacy {

    public enum UnitSpriteGroupID {
        Player,
        Enemy1,

        Last
    }

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

    public enum UnitAnimState {
        Walk,
        Stand,

        Last
    }

    public enum UnitCommand {
        MoveNorth,
        MoveNorthEast,
        MoveEast,
        MoveSouthEast,
        MoveSouth,
        MoveSouthWest,
        MoveWest,
        MoveNorthWest,

        Stand,

        Last
    }

    public enum Team {
        Boss,
        Evil,

        Player
    }

    public class Unit : MonoBehaviour {

        public static Unit Player;
        public static List<Unit> ActiveUnits = new List<Unit>();

        public static Vector3[] MoveVectors = new Vector3[8] {
        Vector3.Normalize(new Vector3(1, 0, 1)),
        new Vector3(1, 0, 0),
        Vector3.Normalize(new Vector3(1, 0, -1)),
        new Vector3(0, 0, -1),
        Vector3.Normalize(new Vector3(-1, 0, -1)),
        new Vector3(-1, 0, 0),
        Vector3.Normalize(new Vector3(-1, 0, 1)),
        new Vector3(0, 0, 1),
    };
        public static string[] AnimNames = new string[16] {
        "characterNorth",
        "characterNorthEast",
        "characterEast",
        "characterSouthEast",
        "characterSouth",
        "characterSouthWest",
        "characterWest",
        "characterNorthWest",
        "idleNorth",
        "idleNorthEast",
        "idleEast",
        "idleSouthEast",
        "idleSouth",
        "idleSouthWest",
        "idleWest",
        "idleNorthWest",
    };

        [System.NonSerialized] public const float HALF_ROOT_2 = 0.70710678118654752440084436210485f; // sqrt(2) / 2, for a diagonal of 1 unit
        public UnitFacing Facing = UnitFacing.SouthWest;
        [System.NonSerialized] public UnitAnimState AnimState = UnitAnimState.Stand;
        [System.NonSerialized] public bool isUnderForcedControl = false;
        [System.NonSerialized] public UnitCommand Command = UnitCommand.Stand;
        [System.NonSerialized] public float MoveSpeed = 8f;

        [System.NonSerialized] public SpriteRenderer spriteRenderer;
        [System.NonSerialized] public BoxCollider boxCollider;
        [System.NonSerialized] new public Rigidbody rigidbody;

        public Animator animator;

        private void Awake() {
            spriteRenderer = GetComponent<SpriteRenderer>();
            boxCollider = GetComponent<BoxCollider>();
            rigidbody = GetComponent<Rigidbody>();
            TimeManager.TimeStepEvent += TimeStep;

            animator = GetComponentInChildren<Animator>();

            if (!ActiveUnits.Contains(this)) ActiveUnits.Add(this);
        }
        private void OnDestroy() {
            if (ActiveUnits.Contains(this)) ActiveUnits.Remove(this);
        }

        public void MoveCommand(UnitCommand unitCommand) {
            Command = unitCommand;
        }

        public void TimeStep(float dt) {

            if (!isUnderForcedControl) {
                RaycastHit hit;
                UnitCommand modifiedCommand = UnitCommand.Stand;

                if ((int)Command < 8) {
                    if (!rigidbody.SweepTest(MoveVectors[(int)Command], out hit, MoveSpeed * dt, QueryTriggerInteraction.Ignore)) {
                        modifiedCommand = Command;
                    }

                    else if (!rigidbody.SweepTest(((int)Command == 7 ? MoveVectors[0] : MoveVectors[(int)Command + 1]),
                             out hit, MoveSpeed * dt, QueryTriggerInteraction.Ignore)) {
                        if ((int)Command == 7) modifiedCommand = (UnitCommand)0;
                        else modifiedCommand = (UnitCommand)((int)Command + 1);
                    }

                    else if (!rigidbody.SweepTest(((int)Command == 0 ? MoveVectors[7] : MoveVectors[(int)Command - 1]),
                             out hit, MoveSpeed * dt, QueryTriggerInteraction.Ignore)) {
                        if ((int)Command == 0) modifiedCommand = (UnitCommand)7;
                        else modifiedCommand = (UnitCommand)((int)Command - 1);
                    }
                }

                if ((int)modifiedCommand < 8) {
                    transform.Translate(MoveVectors[(int)modifiedCommand] * MoveSpeed * dt);
                    Facing = (UnitFacing)(int)Command;
                }

                if ((int)modifiedCommand < 8) {
                    if (!animator.GetCurrentAnimatorStateInfo(0).IsName(AnimNames[(int)modifiedCommand])) {
                        animator.Play(AnimNames[(int)modifiedCommand]);
                    }
                    AnimState = UnitAnimState.Walk;
                }
                else {
                    if (!animator.GetCurrentAnimatorStateInfo(0).IsName(AnimNames[8 + (int)Facing])) {
                        animator.Play(AnimNames[8 + (int)Facing]);
                    }
                    AnimState = UnitAnimState.Stand;
                }

                rigidbody.velocity = Vector3.zero;
            }
            else {
                if (!animator.GetCurrentAnimatorStateInfo(0).IsName(AnimNames[(int)AnimState * 8 + (int)Facing])) {
                    animator.Play(AnimNames[(int)AnimState * 8 + (int)Facing]);
                }
            }
        }

        public void Rotate(int amount) {
            int newFacing = ((int)Facing + amount) % 8;
            if (newFacing < 0) newFacing += 8;
            Facing = (UnitFacing)(newFacing);
        }
    }

}