using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Legacy {

    public class UnitPainter : MonoBehaviour {

        public static bool isTunnel = false;
        public static bool isSafehouse = false;
        public static GameObject[] UnitPrefabs = new GameObject[(int)UnitSpriteGroupID.Last];

        [SerializeField] private float playerSpawnX = 0f;
        [SerializeField] private float playerSpawnZ = 0f;
        [SerializeField] private float playerMoveSpeedFactor = 1f;
        [SerializeField] private bool useBrightPlayerSprite = false;
        [SerializeField] private bool tunnel = false;
        [SerializeField] private bool safehouse = false;

        private void Awake() {
            if (!tunnel) UnitPrefabs[(int)UnitSpriteGroupID.Player] = Resources.Load<GameObject>("UnitPrefabs/Player");
            else UnitPrefabs[(int)UnitSpriteGroupID.Player] = Resources.Load<GameObject>("UnitPrefabs/Player with hostage");
            SpawnUnits();
            isTunnel = tunnel;
            isSafehouse = safehouse;
        }

        private void Start() {
            //GameData.gameData.checkPoint_x = playerSpawnX;
            //GameData.gameData.checkPoint_z = playerSpawnZ;
        }

        private void SpawnUnits() {
            if (Unit.Player != null) {
                Destroy(Unit.Player);
                Unit.Player = null;
            }
            GameObject player = Instantiate(UnitPrefabs[(int)UnitSpriteGroupID.Player]);
            Unit.Player = player.GetComponent<Unit>();
            player.transform.position = new Vector3(0, player.transform.position.y, 0);
            Camera.main.transform.SetParent(player.transform);
            player.transform.position = new Vector3(playerSpawnX, player.transform.position.y, playerSpawnZ);
            Unit.Player.MoveSpeed *= playerMoveSpeedFactor;
            if (useBrightPlayerSprite) Unit.Player.GetComponentInChildren<MeshRenderer>().material = Resources.Load<Material>("Sprites/Player/Materials/GALLERY_Spritesheet");

            if (tunnel) {
                GameObject backblocker = new GameObject();
                backblocker.AddComponent<BoxCollider>().size = new Vector3(1, 1, 1);
                backblocker.transform.position = Unit.Player.transform.position + new Vector3(0, 0, 0.515f + player.GetComponent<BoxCollider>().bounds.size.z / 2);
                //backblocker.AddComponent<RetreatBlocker>();
            }
        }
    }

}