using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Util
{
    public class ObjectPool : MonoBehaviour
    {
        [SerializeField] private GameObject pooledObject;
        [SerializeField] private bool isPooledObjectAPrefab = false;
        [SerializeField] private int poolSize = 10;

        private Queue<GameObject> _q;

        private void Start()
        {
            _q = new Queue<GameObject>();

            var size = poolSize;
            if (!isPooledObjectAPrefab)
            {
                pooledObject.transform.parent = transform;
                --size;
            }


            for (int i = 0; i < size; i++)
            {
                var obj = Instantiate(pooledObject, transform);
                obj.SetActive(false);
                _q.Enqueue(obj);
            }
        }

        public GameObject Retreive(Vector3 position)
        {
            if (_q.Count == 0)
            {
                return null;
            }
            else
            {
                var obj = _q.Dequeue();
                obj.transform.position = position;
                obj.SetActive(true);
                return obj;
            }
        }

        public void Return(GameObject obj)
        {
            if (obj != null)
            {
                obj.SetActive(false);
                _q.Enqueue(obj);
            }
        }
    }
}