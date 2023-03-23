using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FadeIOManager : MonoBehaviour
{

    [SerializeField] CanvasGroup canvasGroup;
    

    //シングルトンのコード開始
    static public FadeIOManager instance;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    //シングルトン終わり
    }
    
    //フェードイン、アウトの実装
    public void FadeIn()
    {
        canvasGroup.DOFade(0,2f);
    }

    public void FadeOut()
    {
        canvasGroup.DOFade(1,1f);
    }

    public void FadeOutToIn()
    {
        canvasGroup.DOFade(1,1f).OnComplete(FadeIn);
    }
}

