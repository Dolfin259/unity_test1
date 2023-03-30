using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionScene : MonoBehaviour
{
    public static TransitionScene instance;

    public void Awake()
    {
        if(instance == null)
        {
        instance = this;
        }
    }

    public void OnSceneTransitionToPrologue()
    {
        FadeIOManager.instance.FadeOutToIn();
        SceneManager.LoadScene("Prologue");
        
    }

    public void OnSceneTransitionToMain()
    {
        FadeIOManager.instance.FadeOutToIn();
        SceneManager.LoadScene("Main1");
    }

     public void OnSceneTransitionToEpilogue()
    { 
        FadeIOManager.instance.FadeOutToIn();
        SceneManager.LoadScene("Epilogue"); 
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
