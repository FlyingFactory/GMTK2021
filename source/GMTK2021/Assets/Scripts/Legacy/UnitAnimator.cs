using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Legacy {

    public class UnitAnimator : MonoBehaviour {

        // Note: the row number corresponds to UnitFacing + 8 * UnitAnimState.
        public const int numberOfRows = 16;
        [SerializeField] private int movementFramesPerRow = 6;
        [System.NonSerialized] public float movementFPS = 8;
        [SerializeField] private int idleFramesPerRow = 2;
        [System.NonSerialized] public float idleFPS = 2;

        private float time = 0;
        private int current = 0;

        [System.NonSerialized] public MeshRenderer meshRenderer;
        [System.NonSerialized] public Unit unit;

        private void Start() {
            meshRenderer = GetComponent<MeshRenderer>();
            meshRenderer.material.SetTextureScale("_MainTex", new Vector2(1f / 8, 1f / 16));
            unit = GetComponentInParent<Unit>();
            NextFrame();
        }

        private void Update() {
            //if (GlobalData.GameState == GameStates.Normal) {
                time += Time.fixedDeltaTime;
                if (unit.AnimState == UnitAnimState.Walk) {
                    if (time > 1f / movementFPS) {
                        time -= 1f / movementFPS;
                        current += 1;
                    }
                    if (current >= movementFramesPerRow) current = 0;
                }
                else {
                    if (time > 1f / idleFPS) {
                        time -= 1f / idleFPS;
                        current += 1;
                    }
                    if (current >= idleFramesPerRow) current = 0;
                }
                NextFrame();
            //}
        }

        private void NextFrame() {
            if (unit.AnimState == UnitAnimState.Walk)
                meshRenderer.material.SetTextureOffset("_MainTex", new Vector2((float)current / 8, 15f / 16f - ((float)unit.Facing / 16)));
            else
                meshRenderer.material.SetTextureOffset("_MainTex", new Vector2((float)current / 8, 7f / 16f - ((float)unit.Facing / 16)));
        }

        public void Change() {
            current = 0;
            NextFrame();
        }
    }

}