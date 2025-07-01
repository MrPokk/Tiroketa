
namespace Engine_Component.Utility.Interfaces
{
    public interface IRoot
    {
        protected internal void PreStartGame();
        protected internal void UpdateGame( float timeDelta);
        protected internal void PhysicUpdateGame(float timeDelta);
        protected internal void StoppedGame();
    }
}
