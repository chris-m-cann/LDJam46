using System;
using UnityEngine;
using Util;

namespace CatBall
{
    public class ToggleControlViews : MonoBehaviour
    {
        [SerializeField] private GameObject mouse;
        [SerializeField] private GameObject controller;

        private void Update()
        {
            if (InputEx.IsControllerConnected())
            {
                mouse.SetActive(false);
                controller.SetActive(true);
            }
            else
            {
                mouse.SetActive(true);
                controller.SetActive(false);

            }
        }
    }
}