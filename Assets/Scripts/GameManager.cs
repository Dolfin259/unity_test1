using System.Collections;
 using System.Collections.Generic;
 using UnityEngine;
 public class GameManager : MonoBehaviour
 {
     public static GameManager instance = null;
     public int stageNum;
     public int continueNum;

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
 }