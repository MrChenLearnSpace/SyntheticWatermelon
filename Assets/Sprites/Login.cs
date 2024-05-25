using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Login : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Screen.SetResolution(540, 960, false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public  void OnStartClick()
    {
        //跳转到游戏场景
        SceneManager.LoadScene("PlayGame");
    }
}
