using UnityEngine;
using UnityEngine.SceneManagement;
public class DebugUtility : BaseInteraction, IEnterInUpdate
{
    public void Update(float timeDelta)
    {
        if (Input.GetKeyDown(KeyCode.R))
            SceneManager.LoadScene(sceneBuildIndex: 0);
    }
}
