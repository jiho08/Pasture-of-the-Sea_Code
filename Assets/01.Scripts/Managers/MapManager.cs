using Code.Core;
using UnityEngine;

namespace Code.ETC
{
    public class MapManager : MonoSingleton<MapManager>
    {
        public Vector3 mapSize;

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(transform.position, mapSize);
        }
    }
}