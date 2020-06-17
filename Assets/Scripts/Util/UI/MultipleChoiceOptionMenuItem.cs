using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace Util.UI
{
    public class MultipleChoiceOptionMenuItem : MonoBehaviour
    {
        [SerializeField] private Button leftButton;
        [SerializeField] private Button rightButton;
        [SerializeField] private TextMeshProUGUI option1;
        [SerializeField] private TextMeshProUGUI option2;

        [SerializeField] private string[] options;
        [SerializeField] private float tweenTime;
        [SerializeField] private IntUnityEvent onChoiceSelection;

        private int _idx = 0;

        // todo(chris) this will fail if called while one of the transitions is ongoing, no need to make more robust yet but will need ot at some point
        public void SetOptions(string[] newOptions, int activeOption = 0)
        {
            if (newOptions.Length < (activeOption + 1) || activeOption < 0)
            {
                Debug.LogError($"invalid options, size={newOptions.Length}, active = {activeOption}");
                return;
            }

            options = newOptions;
            _idx = activeOption;
            option1.text = options[activeOption];
        }

        public void LeftClicked()
        {
            StartCoroutine(CoLeftClicked());
        }

        private IEnumerator CoLeftClicked()
        {
            leftButton.interactable = false;

            _idx = (_idx + 1) % options.Length;

            var option1EndPos = leftButton.transform.position.x;
            var option1StartPos = option1.transform.position.x;
            var option2EndPos = option1StartPos;
            var option2StartPos = rightButton.transform.position.x;

            yield return StartCoroutine(TweenOptions(option1StartPos, option1EndPos, option2StartPos, option2EndPos));


            leftButton.interactable = true;

            onChoiceSelection.Invoke(_idx);
        }

        public void RightClicked()
        {
            StartCoroutine(CoRightClicked());
        }

        private IEnumerator CoRightClicked()
        {
            rightButton.interactable = false;

            --_idx;
            if (_idx < 0) _idx = options.Length - 1;


            var option1EndPos = rightButton.transform.position.x;
            var option1StartPos = option1.transform.position.x;
            var option2EndPos = option1StartPos;
            var option2StartPos = leftButton.transform.position.x;

            yield return StartCoroutine(TweenOptions(option1StartPos, option1EndPos, option2StartPos, option2EndPos));

            rightButton.interactable = true;
            

            onChoiceSelection.Invoke(_idx);
        }

        private IEnumerator TweenOptions(float startPos1, float endPos1, float startPos2, float endPos2)
        {
            option2.text = options[_idx];

            var startTime = Time.unscaledTime;
            var endTime = Time.unscaledTime + tweenTime;

            var colour1 = option1.color;
            var colour2 = option2.color;

            while (Time.unscaledTime < endTime)
            {
                var currentTime = (Time.unscaledTime - startTime) / tweenTime;
                option1.transform.position = new Vector3(
                    Tween.SmoothStart2(startPos1, endPos1, currentTime),
                    option1.transform.position.y, option1.transform.position.z
                );

                option2.transform.position = new Vector3(
                    Tween.SmoothStart2(startPos2, endPos2, currentTime),
                    option2.transform.position.y, option2.transform.position.z
                );

                colour1.a = Tween.SmoothStart4(1, 0, currentTime);
                colour2.a = Tween.SmoothStart4(0, 1, currentTime);

                option1.color = colour1;
                option2.color = colour2;

                yield return null;
            }

            option1.text = option2.text;
            option1.color = option2.color;
            option1.transform.position = option2.transform.position;

            option2.color = new Color(option2.color.r, option2.color.g,option2.color.b, 0);
        }
    }
}