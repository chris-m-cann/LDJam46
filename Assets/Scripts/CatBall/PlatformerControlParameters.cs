using System;
using UnityEngine;

namespace CatBall
{
    [CreateAssetMenu(menuName = "Custom/ControlParameters")]
    public class PlatformerControlParameters : ScriptableObject
    {
        [Header("Jump")]
        [Tooltip("height you will jump to if you just feather the button")]
        public float minJumpHeight;
        [Tooltip("height you will jump to if you hold button until the apex")]
        public float maxJumpHeight;
        public float timeToPeak;
        public float timeBackDown;
        [Tooltip("amoun of time after walking of the edge of a platform that we will still register a jump")]
        public float coyoteTime;
        [Tooltip("amount of time before landing on a platform that we will still register a jump")]
        public float graceTime;
        [Tooltip("This along with the timeToPeak and timeBackDown control our max foot speed")]
        public float maxJumpWidth;


        [Header("Ground Control")]
        public float timeToMaxSpeed;
        public float timeBackFromMaxToRest;

        [Header("Air Control")]
        public float timeToMaxSpeedInAir;
        public float timeToBackFromMaxSpeedInAir;
        public float maxFallSpeed;

        [Space]
        [Tooltip("Layers we consider as 'ground'")]
        public LayerMask groundMask;

        // Todo(chris) update this to a full C# event so we can have multiple subscribers
        [NonSerialized] public Action onUpdate;

        private void OnValidate()
        {
            onUpdate?.Invoke();
        }
    }
}