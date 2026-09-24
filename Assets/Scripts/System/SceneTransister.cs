using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransister : MonoBehaviour
{
    public void LoadTitle()
    {
        SceneTransition("TitleScene");
    }

    public void LoadSkillBuild()
    {
        SceneTransition("SkillBuildScene");
    }

    public void LoadInGame()
    {
        SceneTransition("InGameScene");
    }

    private void SceneTransition(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
