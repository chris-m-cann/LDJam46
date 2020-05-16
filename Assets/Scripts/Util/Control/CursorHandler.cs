using System;
using UnityEngine;

namespace Util.Control
{
    public class CursorHandler: MonoBehaviour
    {
        [SerializeField] private CursorLockMode lockMode = CursorLockMode.None;
        [SerializeField] private Texture2D cursor;
        [SerializeField] private Vector2 cursorHotspot = Vector2.zero;
        [SerializeField] private bool cursorVisible = true;

        public void SetCursorVisible(bool visible)
        {
            cursorVisible = visible;
            ResetCursor();
        }

        private void Start()
        {
            ResetCursor();
        }

        private void ResetCursor()
        {
            Cursor.visible = cursorVisible;
            Cursor.lockState = lockMode;
            Cursor.SetCursor(cursor, cursorHotspot, CursorMode.Auto);
        }

        private void OnMouseEnter()
        {
            ResetCursor();
        }

        private void OnMouseExit()
        {
            var cache = cursor;
            cursor = null;
            ResetCursor();
            cursor = cache;
        }
    }
}