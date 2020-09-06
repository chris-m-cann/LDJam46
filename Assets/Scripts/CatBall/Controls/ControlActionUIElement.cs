using UnityEngine;

namespace CatBall.Controls
{
    public class ControlActionUIElement : MonoBehaviour
    {
        private CanvasRenderer _renderer;

        private void Awake()
        {
            _renderer = GetComponent<CanvasRenderer>();
        }

        // private void Start()
        // {
        //     if (_renderer != null)
        //         _renderer.sor = "Background";
        //     else
        //     {
        //         Debug.Log("no mesh renderer");
        //     }
        // }
    }
}
