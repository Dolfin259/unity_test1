using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GotoMain : MonoBehaviour
{
    public void OnClickButton()
    {
        SceneManager.LoadScene("Main1");
    }

}
