using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Legacy {

    public class CreditsScreen : MonoBehaviour {

        public static CreditsScreen creditsScreen = null;
        private float time = 0;
        private bool _isActive = false;
        public bool isActive {
            get { return _isActive; }
            set {
                if (value) {
                    time = 0;
                }
                else {
                    AdjustColorAlpha(creditsBG, 0);
                    exiting = false;
                }
                _isActive = value;
            }
        }
        private bool _exiting = false;
        private bool exiting {
            get { return _exiting; }
            set {
                _exiting = value;
                if (value) {
                    time = 0;

                    AdjustColorAlpha(group_0_logo, 0);
                    AdjustColorAlpha(group_1_0, 0);
                    AdjustColorAlpha(group_1_1, 0);
                    AdjustColorAlpha(group_1_2, 0);
                    AdjustColorAlpha(group_1_3, 0);
                    AdjustColorAlpha(group_2_0, 0);
                    AdjustColorAlpha(group_2_1, 0);
                    AdjustColorAlpha(group_2_2, 0);
                    AdjustColorAlpha(group_3_0, 0);
                    AdjustColorAlpha(group_3_1, 0);
                    AdjustColorAlpha(group_4_0, 0);
                }
            }
        }

        private float cutIn = 1f;
        private float group0in = 0.5f;
        private float group0hold = 3f;
        private float group0out = 0.5f;
        private float group1in = 0.5f;
        private float group1hold = 4f;
        private float group1out = 0.5f;
        private float group2in = 0.5f;
        private float group2hold = 5f;
        private float group2out = 0.5f;
        private float group3in = 0.5f;
        private float group3hold = 4f;
        private float group3out = 0.5f;
        private float group4in = 0.5f;
        private float group4hold = 3f;
        private float group4out = 0.5f;
        private float cutOut = 1f;

        private Image creditsBG;
        private Image group_0_logo;
        private Text group_1_0;
        private Image group_1_1;
        private Text group_1_2;
        private Text group_1_3;
        private Text group_2_0;
        private Text group_2_1;
        private Text group_2_2;
        private Text group_3_0;
        private Text group_3_1;
        private Text group_4_0;

        private void Awake() {
            creditsScreen = this;

            group0in += cutIn;
            group0hold += group0in;
            group0out += group0hold;
            group1in += group0out;
            group1hold += group1in;
            group1out += group1hold;
            group2in += group1out;
            group2hold += group2in;
            group2out += group2hold;
            group3in += group2out;
            group3hold += group3in;
            group3out += group3hold;
            group4in += group3out;
            group4hold += group4in;
            group4out += group4hold;

            creditsBG = GetComponent<Image>();
            group_0_logo = transform.GetChild(0).GetChild(0).GetComponent<Image>();
            group_1_0 = transform.GetChild(1).GetChild(0).GetComponent<Text>();
            group_1_1 = transform.GetChild(1).GetChild(1).GetComponent<Image>();
            group_1_2 = transform.GetChild(1).GetChild(2).GetComponent<Text>();
            group_1_3 = transform.GetChild(1).GetChild(3).GetComponent<Text>();
            group_2_0 = transform.GetChild(2).GetChild(0).GetComponent<Text>();
            group_2_1 = transform.GetChild(2).GetChild(1).GetComponent<Text>();
            group_2_2 = transform.GetChild(2).GetChild(2).GetComponent<Text>();
            group_3_0 = transform.GetChild(3).GetChild(0).GetComponent<Text>();
            group_3_1 = transform.GetChild(3).GetChild(1).GetComponent<Text>();
            group_4_0 = transform.GetChild(4).GetChild(0).GetComponent<Text>();
        }

        private void Update() {

            if (Input.GetKeyDown(KeyCode.Escape)) exiting = true;

            if (exiting) {
                if (time < cutOut) {
                    AdjustColorAlpha(creditsBG, 1 - time / cutOut);
                }
                else {
                    isActive = false;
                }
                time += Time.deltaTime;
            }
            else if (isActive) {
                if (time < cutIn) {
                    AdjustColorAlpha(creditsBG, time / cutIn);
                }
                else if (time < group0in) {
                    AdjustColorAlpha(group_0_logo, (time - cutIn) / (group0in - cutIn));
                }
                else if (time < group0hold) {
                    AdjustColorAlpha(group_0_logo, 1);
                }
                else if (time < group0out) {
                    AdjustColorAlpha(group_0_logo, 1 - (time - group0hold) / (group0out - group0hold));
                }
                else if (time < group1in) {
                    AdjustColorAlpha(group_0_logo, 0);
                    AdjustColorAlpha(group_1_0, (time - group0out) / (group1in - group0out));
                    AdjustColorAlpha(group_1_1, (time - group0out) / (group1in - group0out));
                    AdjustColorAlpha(group_1_2, (time - group0out) / (group1in - group0out));
                    AdjustColorAlpha(group_1_3, (time - group0out) / (group1in - group0out));
                }
                else if (time < group1hold) {
                    AdjustColorAlpha(group_1_0, 1);
                    AdjustColorAlpha(group_1_1, 1);
                    AdjustColorAlpha(group_1_2, 1);
                    AdjustColorAlpha(group_1_3, 1);
                }
                else if (time < group1out) {
                    AdjustColorAlpha(group_1_0, 1 - (time - group1hold) / (group1out - group1hold));
                    AdjustColorAlpha(group_1_1, 1 - (time - group1hold) / (group1out - group1hold));
                    AdjustColorAlpha(group_1_2, 1 - (time - group1hold) / (group1out - group1hold));
                    AdjustColorAlpha(group_1_3, 1 - (time - group1hold) / (group1out - group1hold));
                }
                else if (time < group2in) {
                    AdjustColorAlpha(group_1_0, 0);
                    AdjustColorAlpha(group_1_1, 0);
                    AdjustColorAlpha(group_1_2, 0);
                    AdjustColorAlpha(group_1_3, 0);
                    AdjustColorAlpha(group_2_0, (time - group1out) / (group2in - group1out));
                    AdjustColorAlpha(group_2_1, (time - group1out) / (group2in - group1out));
                    AdjustColorAlpha(group_2_2, (time - group1out) / (group2in - group1out));
                }
                else if (time < group2hold) {
                    AdjustColorAlpha(group_2_0, 1);
                    AdjustColorAlpha(group_2_1, 1);
                    AdjustColorAlpha(group_2_2, 1);
                }
                else if (time < group2out) {
                    AdjustColorAlpha(group_2_0, 1 - (time - group2hold) / (group2out - group2hold));
                    AdjustColorAlpha(group_2_1, 1 - (time - group2hold) / (group2out - group2hold));
                    AdjustColorAlpha(group_2_2, 1 - (time - group2hold) / (group2out - group2hold));
                }
                else if (time < group3in) {
                    AdjustColorAlpha(group_2_0, 0);
                    AdjustColorAlpha(group_2_1, 0);
                    AdjustColorAlpha(group_2_2, 0);
                    AdjustColorAlpha(group_3_0, (time - group2out) / (group3in - group2out));
                    AdjustColorAlpha(group_3_1, (time - group2out) / (group3in - group2out));
                }
                else if (time < group3hold) {
                    AdjustColorAlpha(group_3_0, 1);
                    AdjustColorAlpha(group_3_1, 1);
                }
                else if (time < group3out) {
                    AdjustColorAlpha(group_3_0, 1 - (time - group3hold) / (group3out - group3hold));
                    AdjustColorAlpha(group_3_1, 1 - (time - group3hold) / (group3out - group3hold));
                }
                else if (time < group4in) {
                    AdjustColorAlpha(group_3_0, 0);
                    AdjustColorAlpha(group_3_1, 0);
                    AdjustColorAlpha(group_4_0, (time - group3out) / (group4in - group3out));
                }
                else if (time < group4hold) {
                    AdjustColorAlpha(group_4_0, 1);
                }
                else if (time < group4out) {
                    AdjustColorAlpha(group_4_0, 1 - (time - group4hold) / (group4out - group4hold));
                }
                else {
                    exiting = true;
                }
                time += Time.deltaTime;
            }
        }

        private void AdjustColorAlpha(Image target, float alpha) {
            target.color = new Color(target.color.r, target.color.g, target.color.b, alpha);
        }
        private void AdjustColorAlpha(Text target, float alpha) {
            target.color = new Color(target.color.r, target.color.g, target.color.b, alpha);
        }
    }

}