using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GotoTitle : MonoBehaviour
{
    public void OnClickButton()
    {
        SceneManager.LoadScene("Title");
    }

}
