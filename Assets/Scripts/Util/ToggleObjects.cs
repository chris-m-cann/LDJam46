using System;
using Cinemachine;
using UnityEngine;

namespace Util
{
    public class ToggleObjects : MonoBehaviour
    {
        [SerializeField] private string button = "Toggle";
        [SerializeField] private GameObject startsOn;
        [SerializeField] private GameObject startsOff;

        private void Awake()
        {
            startsOn.SetActive(true);
            startsOff.SetActive(false);
        }

        private void Update()
        {
            if (Input.GetButtonDown(button))
            {
                startsOn.SetActive(false);
                startsOff.SetActive(true);
            }


            if (Input.GetButtonUp(button))
            {
                startsOn.SetActive(true);
                startsOff.SetActive(false);
            }


        }

        private void Toggle()
        {
            if (!startsOff.activeSelf)
            {
                startsOn.SetActive(false);
                startsOff.SetActive(true);
            }
            else
            {
                startsOn.SetActive(true);
                startsOff.SetActive(false);
            }
        }
    }
}