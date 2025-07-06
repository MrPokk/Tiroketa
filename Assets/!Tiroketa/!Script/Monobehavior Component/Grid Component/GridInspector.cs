using System;
using UnityEngine;
namespace Game._Script.GridComponent
{
    public class GridInspector : MonoBehaviour
    {
        [field: SerializeField] public Vector2Int Size { get; private set; }
        [field: SerializeField] public float CellSize { get; private set; }
        public Vector2 PositionOrigin => transform.position;

        [Header("Gizmos Settings")]
        [SerializeField] private Color GridColor = Color.white;
        [SerializeField] private bool DrawCellCoordinates = false;


        private void OnValidate()
        {
            if (Size.x < 0 || Size.y < 0)
                Size = new Vector2Int(Math.Abs(Size.x), Math.Abs(Size.y));
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = GridColor;

            for (int x = 0; x <= Size.x; x++)
            {
                Vector3 start = PositionOrigin + new Vector2(x * CellSize, 0);
                Vector3 end = PositionOrigin + new Vector2(x * CellSize, Size.y * CellSize);
                Gizmos.DrawLine(start, end);
            }

            for (int y = 0; y <= Size.y; y++)
            {
                Vector3 start = PositionOrigin + new Vector2(0, y * CellSize);
                Vector3 end = PositionOrigin + new Vector2(Size.x * CellSize, y * CellSize);
                Gizmos.DrawLine(start, end);
            }


            if (DrawCellCoordinates)
            {
                var style = new GUIStyle
                {
                    normal = { textColor = GridColor },
                    fontSize = Mathf.FloorToInt(CellSize * 0.5f),
                };

                for (int x = 0; x < Size.x; x++)
                {
                    for (int y = 0; y < Size.y; y++)
                    {
                        Vector3 pos = PositionOrigin + (new Vector2(x, y) * CellSize) + new Vector2(CellSize, CellSize) / 2;
                        UnityEditor.Handles.Label(pos, $"[{x},{y}]", style);
                    }
                }
            }
        }
    }
}
