using UnityEngine;

namespace Game._Script.GridComponent
{
    public class GridPresenter<T>
    {
        private readonly GridModel<T> _gridModel;
        private readonly GridInspector _gridInspector;

        public GridPresenter(GridInspector gridInspector)
        {
            _gridInspector = gridInspector;
            _gridModel = new GridModel<T>(this, gridInspector.Size, gridInspector.CellSize, gridInspector.PositionOrigin);
        }

        public bool TryGetPositionInGrid(Vector2Int indexNode, out Vector3 positionValue)
        {
            if (IsWithGrid(indexNode))
            {
                positionValue = new Vector3(indexNode.x, indexNode.y, 0) * _gridModel.CellSize + (Vector3)_gridModel.PositionOrigin;
                return true;
            }

            positionValue = Vector3.negativeInfinity;
            return false;
        }

        public bool TryGetPositionInGrid(Vector3 objectPosition, out Vector2Int positionValue)
        {
            if (!IsWithGrid(ConvertingPosition(objectPosition)))
            {
                positionValue = Vector2Int.one * int.MinValue;
                return false;
            }

            int X = Mathf.FloorToInt((objectPosition - (Vector3)_gridModel.PositionOrigin).x / _gridModel.CellSize);
            int Y = Mathf.FloorToInt((objectPosition - (Vector3)_gridModel.PositionOrigin).y / _gridModel.CellSize);
            positionValue = new Vector2Int(X, Y);
            return true;
        }

        public void SetValue(Vector2Int index, T value)
        {
            if (!IsWithGrid(index))
                return;

            _gridModel.Array[index.x, index.y] = value;
        }

        public void SetValue(Vector3 positionWorld, T value)
        {
            SetValue(ConvertingPosition(positionWorld), value);
        }

        public bool HasValue(Vector2Int index)
        {
            return GetValue(index) != null;
        }

        public bool HasValue(Vector3 positionWorld)
        {
            return GetValue(positionWorld) != null;
        }

        public T GetValue(Vector2Int index)
        {
            return IsWithGrid(index) ? _gridModel.Array[index.x, index.y] : default;
        }

        public T GetValue(Vector3 positionWorld) => GetValue(ConvertingPosition(positionWorld));

        public Vector3 ConvertingPosition(Vector2Int index)
        {
            return new Vector3(index.x, index.y, 0) * _gridModel.CellSize + (Vector3)_gridModel.PositionOrigin;
        }

        public Vector2Int ConvertingPosition(Vector3 worldPose)
        {
            int X = Mathf.FloorToInt((worldPose - (Vector3)_gridModel.PositionOrigin).x / _gridModel.CellSize);
            int Y = Mathf.FloorToInt((worldPose - (Vector3)_gridModel.PositionOrigin).y / _gridModel.CellSize);
            return new Vector2Int(X, Y);
        }

        public bool IsWithGrid(Vector2Int indexNode)
        {
            return indexNode.x >= 0 && indexNode.x < _gridModel.Size.x && indexNode.y >= 0 && indexNode.y < _gridModel.Size.y;
        }

        public bool IsWithGrid(Vector2 positionWorld)
        {
            return IsWithGrid(ConvertingPosition(positionWorld));
        }

        public bool IsWithGrid(Vector3 positionWorld)
        {
            return IsWithGrid(ConvertingPosition(positionWorld));
        }
    }
}
