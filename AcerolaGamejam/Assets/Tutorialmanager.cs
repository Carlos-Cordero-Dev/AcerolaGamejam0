using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Tutorialmanager : MonoBehaviour
{
    private float timer = 0.0f;

    public float tutorialDuration = 7.0f;
    void Start()
    {
        timer = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if(timer >= tutorialDuration)
        {
            timer = 0.0f;
            SceneManager.LoadScene(2);
        }
    }
}
