using System.Collections;
 using System.Collections.Generic;
 using UnityEngine;
 public class GameManager : MonoBehaviour
 {
     public static GameManager instance = null;
    
    [Header("現在のステージ")] public int stageNum;
    [Header("現在の復帰位置")]public int continueNum;
    [HideInInspector] public bool isGameOver = false;
    [HideInInspector] public bool isStageClear = false;

    private AudioSource audiosource = null;


     private void Awake()
     {
          if (instance == null)
          {
              instance = this;
              DontDestroyOnLoad(this.gameObject);
          }
          else
          {
              Destroy(this.gameObject);
          }
     }

     private void Start()
     {
        audiosource = GetComponent<AudioSource>();
     }

     public void PlaySE(AudioClip clip)
     {
        if(audiosource != null)
        {
            audiosource.PlayOneShot(clip);
        }
        else
        {
            Debug.Log("オーディオソースが設定されていません");
        }
     }
 }