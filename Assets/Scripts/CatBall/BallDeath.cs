using UnityEngine;
using UnityEngine.SceneManagement;
using Util;

namespace CatBall
{
    public class BallDeath : MonoBehaviour, IHurtable
    {
        public void Hurt()
        {
            SceneManagerEx.ReloadScene();
        }
    }
}