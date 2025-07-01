using Engine_Component.Utility.Interfaces;
using UnityEngine;

namespace Game._Script.GridComponent
{
    public class GridModel : IInitializable
    {
        public GridPresenter Presenter { get; private set; }
        
        public Vector2Int Size { get; private set; }
        public float CellSize { get; private set; }
        public GridNode[,] Array { get; private set; }
        public Vector2 PositionOrigin { get; private set; }

        public GridModel(GridPresenter presenter,Vector2Int size, float cellSize, Vector2 positionOrigin)
        {
            Presenter = presenter;
            
            Size = size;
            CellSize = cellSize;

            PositionOrigin = positionOrigin;
            Array = new GridNode[size.x, size.y];
        }

        public void Init()
        {
            for (int x = 0; x < Size.x; x++)
            {
                for (int y = 0; y < Size.y; y++)
                {
                    Array[x, y] = new GridNode(new(x,y));
                }
            }
        }
    }
}