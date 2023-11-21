using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class UIEnding : Singleton<UIEnding>
{

    public GameObject CompleteScreen;
    public GameObject OverScreen;
    public GameObject RestartScreen;
    bool ended;
    
    public void GameEnd(bool Dead = false)
    {
        (Dead ? OverScreen : CompleteScreen).SetActive(true);
        RestartScreen.SetActive(true);
        ended = true;
        RubyController.instance.GameEnd();
    }
        
    public void Restart(bool requireEnd)
    {
    	if(requireEnd && !ended) return;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
