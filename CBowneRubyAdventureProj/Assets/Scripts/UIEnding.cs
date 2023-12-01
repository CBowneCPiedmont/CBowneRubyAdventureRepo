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

    public AudioClip WinAudio;
    public AudioClip FailAudio;
    public ParticleSystem WinStars;
    
    public void GameEnd(bool Dead = false)
    {
        (Dead ? OverScreen : CompleteScreen).SetActive(true);
        GetComponent<AudioSource>().PlayOneShot(Dead ? FailAudio : WinAudio); //Carl: Added Win/Loss Sound
        Camera.main.GetComponentInChildren<AudioSource>().Stop(); //Carl: Turns off main music upon Win/Loss. Yes I'm aware this method of getting the audio is bizarre, but its less annoying then making a field.
        WinStars.gameObject.SetActive(!Dead); //Carl: Added Code for Winning Particle Effect
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
