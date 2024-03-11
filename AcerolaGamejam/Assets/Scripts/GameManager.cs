using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public enum GameState
    {
        Preview = 0,
        Gameplay
    }
    public static GameState gameState;

    public float timeModifierForPreview = 1.0f;
    public GameObject PreviewGo;
    public GameObject GameplayGo;
    public int currentLevel = 1;

    private float timer = 0.0f;
    private float timeModifier = 1.0f;
    private VoronoidGenerator voroGen;
    private LevelConfiguration levelConf;

    //== GAMEPLAY VARIABLES == //

    public GameObject windupGo;
    public float windupBetweenLevels = 3.0f;

    private TMP_Text windupText;
    private bool levelWindedUp = false;
    private int gameplayCounter = 0;

    private float testTimer = 0.0f;

    void Start()
    {
        PreviewGo.SetActive(false);
        //gameState = GameState.Gameplay;
        voroGen = GameplayGo.GetComponent<VoronoidGenerator>();
        levelConf = GetComponent<LevelConfiguration>();
        windupText = windupGo.GetComponent<TMP_Text>();
    }

    void ShowWindowupCounter(float currentTime)
    {
        int displayTime = (int)(windupBetweenLevels - currentTime +1);
        windupText.SetText(displayTime.ToString());
    }

    void SwitchToGameplay()
    {
        PreviewGo.SetActive(false);
        gameState = GameState.Gameplay;
        timeModifier = 1.0f;
    }

    void SwitchToPreview()
    {
        PreviewGo.SetActive(true);
        gameState = GameState.Preview;
        timeModifier = timeModifierForPreview;
    }
    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if(!levelWindedUp)
        {
            ShowWindowupCounter(timer);
            if (timer >= windupBetweenLevels)
            {
                levelWindedUp = true;
                windupGo.SetActive(false);

                if (gameplayCounter == 0)
                {
                    //going into preview
                    SwitchToPreview();
                }
                else if(gameplayCounter == 1)
                {
                    //going into gameplay
                    SwitchToGameplay();
                }
                timer = 0.0f;

            }
        }
        else if (levelWindedUp)
        {
            if (gameState == GameState.Preview)
            {
                if (timer >= levelConf.Level1Duration)
                {
                    //enter gameplay
                    SwitchToGameplay();
                    windupGo.SetActive(true);
                    levelWindedUp = false;
                    gameplayCounter = 1;
                    timer = 0.0f;
                    return;
                }
            }
            else if (gameState == GameState.Gameplay)
            {
                //gameplay
                testTimer += Time.deltaTime;
                //win condition
                if (testTimer > 3.0f)
                {
                    testTimer = 0.0f;
                    Debug.Log("Won level " + currentLevel);
                    currentLevel++;

                    windupGo.SetActive(true);
                    levelWindedUp = false;
                    gameplayCounter = 0;
                    timer = 0.0f;
                    return;
                }
            }
            voroGen.UpdateAndShowTextures(levelConf.Level(currentLevel, timer * timeModifier), gameState);

        }
    }
}
