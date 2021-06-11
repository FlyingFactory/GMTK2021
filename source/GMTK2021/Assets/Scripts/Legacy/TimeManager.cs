using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Legacy {

    public interface I_TimeStep {
        void TimeStep(float dt);
    }

    public class TimeManager : MonoBehaviour {

        public float GameTimeSpeed = 1f;

        public delegate void TimeStep(float dt);
        public static event TimeStep TimeStepEvent;

        public static float dt = 0.02f;

        private void FixedUpdate() {
            if (TimeStepEvent != null && GlobalData.GameState == GameStates.Normal) {
                dt = GameTimeSpeed * Time.fixedDeltaTime;
                TimeStepEvent(dt);
            }
        }

        public static void Reset() {
            TimeStepEvent = null;
        }
    }

}