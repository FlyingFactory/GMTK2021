using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Legacy {

    public class ArcView : MonoBehaviour, I_TimeStep {

        public Team team = Team.Boss;
        public float Range;
        [Range(0, 360)] public float AngleWidth;
        private float detectedMax = 0.1f;
        private float detectedTime = 0;

        private LayerMask obstacleMask;

        [SerializeField] private bool drawArc = true;

        private bool isUnit;
        private Unit unit;
        [SerializeField] private float rotation = 0;
        private float catchupRate = 180;
        private float laggingAngle = 0;
        private const float triangleResolution = 1;
        private MeshFilter FOVMeshFilter;
        private Mesh FOVMesh;

        private bool inTimeStep = false;

        [SerializeField] private GameObject spotLight;
        [SerializeField] new private GameObject camera;

        [System.NonSerialized]
        public float[] FacingToUnityAngle = new float[] {
        45,
        90,
        135,
        180,
        235,
        270,
        315,
        0,
    };

        private void Start() {
            GameObject newEmpty = new GameObject();
            newEmpty.transform.SetParent(transform, false);
            newEmpty.transform.position = new Vector3(newEmpty.transform.position.x, 0.01f, newEmpty.transform.position.z);

            unit = transform.GetComponent<Unit>();
            isUnit = (unit != null);
            if (isUnit) laggingAngle = FacingToUnityAngle[(int)unit.Facing];

            FOVMeshFilter = newEmpty.AddComponent<MeshFilter>();
            MeshRenderer meshRenderer = newEmpty.AddComponent<MeshRenderer>();
            meshRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            meshRenderer.receiveShadows = false;

            FOVMesh = new Mesh();
            FOVMeshFilter.mesh = FOVMesh;

            meshRenderer.sortingOrder = team == Team.Boss ? -3 : -2;

            switch (team) {
                case Team.Boss:
                    obstacleMask = LayerMask.GetMask(new string[1] { "Blocks Both" });
                    meshRenderer.material = Resources.Load<Material>("Materials/BossTeamVision");
                    break;
                case Team.Evil:
                    obstacleMask = LayerMask.GetMask(new string[2] { "Blocks Both", "Blocks LOS" });
                    meshRenderer.material = Resources.Load<Material>("Materials/EvilTeamVision");
                    break;
                case Team.Player:
                    Destroy(this);
                    break;
            }

            if (!inTimeStep) {
                TimeManager.TimeStepEvent += TimeStep;
                inTimeStep = true;
            }
        }

        private void OnEnable() {
            if (!inTimeStep) TimeManager.TimeStepEvent += TimeStep;
            inTimeStep = true;
        }
        private void OnDisable() {
            if (inTimeStep) TimeManager.TimeStepEvent -= TimeStep;
            inTimeStep = false;
        }
        private void OnDestroy() {
            if (inTimeStep) TimeManager.TimeStepEvent -= TimeStep;
            inTimeStep = false;
        }

        public void TimeStep(float dt) {
            if (isUnit) {
                float f = FacingToUnityAngle[(int)unit.Facing];

                if (Mathf.Abs(laggingAngle - f - 360) < Mathf.Abs(laggingAngle - f)) laggingAngle -= 360;
                else if (Mathf.Abs(laggingAngle - f + 360) < Mathf.Abs(laggingAngle - f)) laggingAngle += 360;

                if (Mathf.Abs(laggingAngle - f) < catchupRate * dt) laggingAngle = f;
                else laggingAngle += dt * (laggingAngle < f ? catchupRate : -catchupRate);
            }

            Vector3 direction = new Vector3(Unit.Player.transform.position.x - transform.position.x, 0, Unit.Player.transform.position.z - transform.position.z);
            RaycastHit hit;

            if (direction.magnitude < Range
                && AngleWidth / 2 > Vector3.Angle(direction, ConvertToDirectionVector(rotation + laggingAngle))
                && !Physics.Raycast(new Vector3(transform.position.x, 0.1f, transform.position.z), direction, out hit, direction.magnitude, obstacleMask)) {
                // FUNCTIONAILTY HERE
                if (team == Team.Evil) {

                    detectedTime += dt;
                    if (detectedTime > detectedMax) {
                        //GameData.gameData.defeated = true;
                        //GlobalData.GameState = GameStates.Paused;
                        //UIGameEnd.UIgameEnd.gameObject.SetActive(true);
                    }
                }
                if (team == Team.Boss) {
                    //GameData.gameData.surveillanceStatus = true;
                }
            }
            else {
                detectedTime = 0;
            }
        }

        private void LateUpdate() {
            if (drawArc) DrawArc();
            if (UnitPainter.isSafehouse) {
                Vector3 lightRotation = new Vector3(0, laggingAngle, 0);
                spotLight.transform.localEulerAngles = lightRotation;
                camera.transform.localEulerAngles = lightRotation;
            }
        }


        private void DrawArc() {
            int baseTriangles = Mathf.CeilToInt(AngleWidth * triangleResolution);
            float centerAngle = AngleWidth / baseTriangles;
            List<Vector3> FOVLocations = new List<Vector3>();

            for (int i = 0; i < baseTriangles + 1; i++) {
                float angle = rotation + laggingAngle - (AngleWidth / 2) + (i * centerAngle);
                FOVLocations.Add(CheckLine(angle).endLocation);
            }

            Vector3[] vertices = new Vector3[baseTriangles + 2];
            int[] triangles = new int[baseTriangles * 3];

            vertices[0] = Vector3.zero;
            vertices[baseTriangles + 1] = FOVLocations[baseTriangles];

            for (int i = 0; i < baseTriangles; i++) {
                vertices[i + 1] = FOVLocations[i];
                triangles[i * 3] = 0;
                triangles[i * 3 + 1] = i + 1;
                triangles[i * 3 + 2] = i + 2;
            }

            FOVMesh.Clear();
            FOVMesh.vertices = vertices;
            FOVMesh.triangles = triangles;
            FOVMesh.RecalculateNormals();

        }

        // ===== Utility functions =====

        /// <summary>
        /// Converts a directional angle to a direction vector. North = zero, Clockwise = positive.
        /// </summary>
        private Vector3 ConvertToDirectionVector(float angle) {
            return new Vector3(Mathf.Sin(Mathf.Deg2Rad * angle), 0, Mathf.Cos(Mathf.Deg2Rad * angle));
        }

        private struct FOVLine {
            public bool obstructed;
            public float endDistance;
            public Vector3 endLocation;
            public float angle;

            public FOVLine(bool obstructed, float endDistance, Vector3 endLocation, float angle) {
                this.obstructed = obstructed;
                this.endDistance = endDistance;
                this.endLocation = endLocation;
                this.angle = angle;
            }
        }

        private FOVLine CheckLine(float angle) {
            Vector3 direction = ConvertToDirectionVector(angle);
            RaycastHit hit;

            if (Physics.Raycast(new Vector3(transform.position.x, 0.1f, transform.position.z), direction, out hit, Range, obstacleMask)) {
                return new FOVLine(true, hit.distance, new Vector3(hit.point.x - transform.position.x, 0, hit.point.z - transform.position.z), angle);
            }
            else {
                return new FOVLine(false, Range, Range * direction, angle);
            }
        }
    }

}