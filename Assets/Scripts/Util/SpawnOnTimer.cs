using System.Collections;
using UnityEngine;

namespace Util
{
    public class SpawnOnTimer : MonoBehaviour
    {
        [SerializeField] private ObjectPool pool;
        [SerializeField] private float interval = 1f;
        [SerializeField] private Transform initialPosition;

        private Vector3 _spawnPos;

        private void Start()
        {
            _spawnPos = initialPosition.position;

            StartCoroutine(CoSpawn());
        }

        private IEnumerator CoSpawn()
        {
            while (isActiveAndEnabled)
            {
                yield return new WaitForSeconds(interval);;

                pool.Retreive(_spawnPos);
            }
        }
    }
}