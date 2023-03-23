using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionScene : MonoBehaviour
{
    public void OnSceneTransitionToMain()
    {
        FadeIOManager.instance.FadeOutToIn();
        SceneManager.LoadScene("Main1");
    }

    public void OnSceneTransitionToBoss()
    {
        FadeIOManager.instance.FadeOutToIn();
        SceneManager.LoadScene("BossStage");
    }

    public void OnSceneTransitionToTitle()
    {
        FadeIOManager.instance.FadeOutToIn();
        SceneManager.LoadScene("Title");
    }

    public void OnSceneTransitionToGameOver()
    {
        FadeIOManager.instance.FadeOutToIn();
        SceneManager.LoadScene("GameOver");
    }
}
