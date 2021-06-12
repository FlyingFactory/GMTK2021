using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Battle {

    public class PlayerPassiveUnit : BaseUnit {
        private void Start() {
            PlayerUnit.player.battery = this;
        }
    }

}