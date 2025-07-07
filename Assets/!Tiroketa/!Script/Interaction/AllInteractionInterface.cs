using _Tiroketa._Script._Cms_Content.View;

namespace Game._Script.Interaction
{
        public interface IColliderInteraction
        {
                public void UpdateCollider(ViewCollision source, ViewCollision collision){}
                public void EnterCollider(ViewCollision source, ViewCollision collision){}
                public void ExitCollider(ViewCollision source, ViewCollision collision){}
        }
}
