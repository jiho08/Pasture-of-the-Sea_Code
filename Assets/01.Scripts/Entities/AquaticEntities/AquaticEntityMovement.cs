using Code.Entities;
using Code.ETC;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Code.AquaticEntities
{
    public class AquaticEntityMovement : MonoBehaviour, IEntityComponent
    {
        [Header("Movement Settings")]
        [SerializeField] private float maxSpeed = 5f;
        [SerializeField] private float minSpeed = 1f;
        [SerializeField] private float rotationSpeed = 2f;
        [SerializeField] private float hungrySpeedMultiplier = 1.5f;
        public float directionChangeInterval = 2f;
        
        [Header("Avoidance Settings")]
        [SerializeField] private float avoidanceDistance = 0.1f;
        [SerializeField] private LayerMask obstacleLayer;
        
        public Vector3 TargetPosition { get; set; }
        
        private readonly Collider[] _obstacleCashingArray = new Collider[20];
        
        private AquaticEntity _aquaticEntity;
        
        public void Initialize(Entity entity)
        {
            _aquaticEntity = entity as AquaticEntity;
        }
        
        public void SetRandomTargetPosition()
        {
            var angle = Random.Range(0f, 2f * Mathf.PI);
            var radius = Random.Range(0f, MapManager.Instance.mapSize.x / 2f);
            
            var x = radius * Mathf.Cos(angle);
            var z = radius * Mathf.Sin(angle);
            var y = Random.Range(-MapManager.Instance.mapSize.y / 2f, MapManager.Instance.mapSize.y / 2f);

            TargetPosition = MapManager.Instance.transform.position + new Vector3(x, y, z);
        }

        public void MoveTowardsTarget(Vector3 targetPos)
        {
            var count = Physics.OverlapSphereNonAlloc(_aquaticEntity.transform.position, avoidanceDistance, _obstacleCashingArray, obstacleLayer);

            if (count > 0)
            {
                var avoidanceDir = Vector3.zero;
                
                for (var i = 0; i < count; ++i)
                {
                    var obstacle = _obstacleCashingArray[i];
                    
                    if (obstacle == null)
                        continue;

                    Vector3 dirFromObstacle;
                    
                    if (obstacle is MeshCollider { convex: false })
                        dirFromObstacle = (_aquaticEntity.transform.position - obstacle.transform.position).normalized;
                    else
                    {
                        var closest = obstacle.bounds.ClosestPoint(_aquaticEntity.transform.position);
                        dirFromObstacle = (_aquaticEntity.transform.position - closest).normalized;
                    }
                    
                    avoidanceDir += dirFromObstacle;
                }
                
                if (avoidanceDir != Vector3.zero)
                {
                    var avoidanceRotation = Quaternion.LookRotation(avoidanceDir.normalized);
                    _aquaticEntity.transform.rotation = Quaternion.Slerp(_aquaticEntity.transform.rotation, avoidanceRotation, rotationSpeed * Time.deltaTime);
                }
            }
            else
            {
                var direction = targetPos - transform.position;
                var targetRotation = Quaternion.LookRotation(direction);
                _aquaticEntity.transform.rotation = Quaternion.Slerp(_aquaticEntity.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

                var distanceToTarget = direction.magnitude;
                var speedMultiplier = Mathf.Clamp01(distanceToTarget / avoidanceDistance);
                var speed = Mathf.Lerp(minSpeed, maxSpeed, speedMultiplier);
                
                if (_aquaticEntity.IsHungry) // 먹이를 먹으러 갈때만 빨라야 할 것 같으면 수정하기
                    speed *= hungrySpeedMultiplier;
                
                _aquaticEntity.Rigid.MovePosition(_aquaticEntity.transform.position + _aquaticEntity.transform.forward * (speed * Time.deltaTime));
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, avoidanceDistance);
        }
    }
}