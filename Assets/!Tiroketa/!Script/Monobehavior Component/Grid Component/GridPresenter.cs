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
            if (IsWithinGrid(indexNode))
            {
                positionValue = new Vector3(indexNode.x, indexNode.y, 0) * _gridModel.CellSize + (Vector3)_gridModel.PositionOrigin;
                return true;
            }

            positionValue = Vector3.negativeInfinity;
            return false;
        }

        public bool TryGetPositionInGrid(Vector3 objectPosition, out Vector2Int positionValue)
        {
            if (!IsWithinGrid(ConvertingPosition(objectPosition)))
            {
                positionValue = Vector2Int.one * int.MinValue;
                return false;
            }

            int X = Mathf.FloorToInt((objectPosition - (Vector3)_gridModel.PositionOrigin).x / _gridModel.CellSize);
            int Y = Mathf.FloorToInt((objectPosition - (Vector3)_gridModel.PositionOrigin).y / _gridModel.CellSize);
            positionValue = new Vector2Int(X, Y);
            return true;
        }
        
        public void SetValueInGrid(Vector2Int index, T value)
        {
            if (IsWithinGrid(index))
            {
                _gridModel.Array[index.x, index.y] = value;
            }
        }
        
        public void SetValueInGrid(Vector3 positionWorld, T value)
        {
            SetValueInGrid(ConvertingPosition(positionWorld),value);
        }

        public T GetNodeByIndex(Vector2Int index)
        {
            return _gridModel.Array[index.x, index.y];
        }

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
        
        public bool IsWithinGrid(Vector2Int indexNode)
        {
            return indexNode.x >= 0 && indexNode.x < _gridModel.Size.x && indexNode.y >= 0 && indexNode.y < _gridModel.Size.y;
        }
        
        public bool IsWithinGrid(Vector2 positionWorld)
        {
            return IsWithinGrid(ConvertingPosition(positionWorld));
        }
        
        public bool IsWithinGrid(Vector3 positionWorld)
        {
            return IsWithinGrid(ConvertingPosition(positionWorld));
        }
    }
}
