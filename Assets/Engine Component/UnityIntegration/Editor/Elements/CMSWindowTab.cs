#if UNITY_EDITOR
using UnityEditor;

namespace Engine_Component.UnityIntegration.Editor
{
    public abstract class CMSWindowTab
    {
        public virtual void OnEnable(EditorWindow editorWindow) { }
        public virtual void OnSelectionChange() { }
        public abstract void Draw();
    }
}
#endif
