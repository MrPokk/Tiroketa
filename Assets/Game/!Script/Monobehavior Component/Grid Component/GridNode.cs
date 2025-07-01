using UnityEngine;

namespace Game._Script.GridComponent
{
    public struct GridNode
    {
        public GridNode(Vector2Int index) : this()
        {
            Index = index;
            Type = TypeNode.Empty;
        }
        
        public enum TypeNode
        {
            Empty = 1,
            Full = 2,
        }
        public TypeNode Type;
        public Vector2Int Index;
    }
}
