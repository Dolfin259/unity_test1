using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public GameObject Player = player;
    PlayerScript  PlayerCtrl;

    void Start()
    {
     player = GameObject.Find("Player");   
     PlayerCtrl = GetComponent<Player>();
    }

    void Update()
    {
        
    }
}
