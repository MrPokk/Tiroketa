using UnityEngine;

namespace Game._Script.GridComponent
{
    public class GridModel<T>
    {
        public GridPresenter<T> Presenter { get; private set; }
        public Vector2Int Size { get; private set; }
        public float CellSize { get; private set; }
        public T[,] Array { get; private set; }
        public Vector2 PositionOrigin { get; private set; }

        public GridModel(GridPresenter<T> presenter,Vector2Int size, float cellSize, Vector2 positionOrigin)
        {
            Presenter = presenter;
            
            Size = size;
            CellSize = cellSize;

            PositionOrigin = positionOrigin;
            Array = new T[size.x, size.y];
        }
    }
}