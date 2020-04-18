using UnityEngine;
using Util;

namespace CatBall
{
    public class PlayerDeath : MonoBehaviour, IHurtable
    {
        public void Hurt()
        {
            SceneManagerEx.ReloadScene();
        }
    }
}