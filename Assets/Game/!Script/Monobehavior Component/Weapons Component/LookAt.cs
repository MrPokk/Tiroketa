using Game._Script.Interaction;
using UnityEngine;

namespace Game._Script.GunComponent
{
    public class LookAt : MonoBehaviour
    {
        [SerializeField]
        private float _rotationRadius = 1f; // Расстояние от родителя
        [SerializeField]
        private float _rotationSpeed = 10f; // Скорость вращения

        private Transform ParentTransform => transform.parent;

        private void Start()
        {
            if (ParentTransform != null)
                return;

            Debug.LogError("This object must be a child of another object!");
            enabled = false;
        }

        private void Update()
        {
            LookProcess();
        }

        private void LookProcess()
        {
            var mouseWorldPos = ControlInteraction.GetMousePositionWorld();
            var parentPos = ParentTransform.position;

            var direction = (mouseWorldPos - parentPos).normalized;

            var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            var targetRotation = Quaternion.Euler(0, 0, angle);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime * 5f);

            transform.position = parentPos + direction * _rotationRadius;
        }

        private void OnDrawGizmos()
        {
            if (!ParentTransform)
                return;

            Gizmos.color = Color.red;

            DrawCircle(ParentTransform.position, _rotationRadius, 16);

            Gizmos.DrawLine(ParentTransform.position, transform.position);
        }

        private void DrawCircle(Vector2 center, float radius, int segments)
        {
            var angle = 0f;
            var angleStep = 360f / segments;
            var lastPoint = center + new Vector2(radius, 0);

            for (int i = 1; i <= segments; i++)
            {
                angle += angleStep;
                var nextPoint = center + new Vector2(
                    Mathf.Cos(Mathf.Deg2Rad * angle) * radius,
                    Mathf.Sin(Mathf.Deg2Rad * angle) * radius
                );
                Gizmos.DrawLine(lastPoint, nextPoint);
                lastPoint = nextPoint;
            }
        }
    }
}
