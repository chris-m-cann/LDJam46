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
        [Header("Hide Mouse")]
        [Tooltip("set timer <= 0 to disable this feature")]
        [SerializeField] private float cursorInvisibleAfter = 3f;
        [Tooltip("in pixels")]
        [SerializeField] private short minMouseMove = 2;

        private float _hideCursor = 0f;
        private Vector2 _lastMousePos;
        public void SetCursorVisible(bool visible)
        {
            cursorVisible = visible;
            ResetCursor();
        }

        private void Start()
        {
            ResetCursor();
            _hideCursor = Time.unscaledTime + cursorInvisibleAfter;
            _lastMousePos = Input.mousePosition;
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

        private void UpdateHideCursor()
        {
            bool hasMoved = Vector2.Distance(_lastMousePos, Input.mousePosition) > minMouseMove;
            _lastMousePos = Input.mousePosition;

            if (cursorVisible)
            {
                if (hasMoved)
                {
                    _hideCursor = Time.unscaledTime + cursorInvisibleAfter;
                } else if (_hideCursor < Time.unscaledTime)
                {
                    cursorVisible = false;
                    Cursor.visible = cursorVisible;
                }
            }
            else
            {
                if (hasMoved)
                {
                    _hideCursor = Time.unscaledTime + cursorInvisibleAfter;
                    cursorVisible = true;
                    Cursor.visible = cursorVisible;
                }
            }
        }
        private void Update()
        {
            if (cursorInvisibleAfter > 0)
            {
                UpdateHideCursor();
            }
        }
    }
}