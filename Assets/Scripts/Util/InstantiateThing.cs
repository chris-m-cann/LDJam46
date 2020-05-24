using UnityEngine;

namespace Util
{
    public class InstantiateThing : MonoBehaviour
    {
        public void InstantiateAtPos(GameObject go)
        {
            Instantiate(go, transform.position, transform.rotation);
        }
    }
}