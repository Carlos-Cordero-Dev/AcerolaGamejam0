using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public enum GameState
    {
        Preview = 0,
        Gameplay
    }
    public static GameState gameState;

    public GameObject PreviewGo;
    public GameObject GameplayGo;
    public GameObject PlayerGo;
    public GameObject CompletePlayerGo;
    public GameObject PreviewCameraGo;
    public int currentLevel = 1;
    public int maxLevel = 10;

    private float timer = 0.0f;
    private VoronoidGenerator voroGen;
    private LevelConfiguration levelConf;
    private PlayerTexLink playerTexLink;
    private TMP_Text windupText;
    private TMP_Text LostText;
    private TMP_Text WonText;
    private TMP_Text currentLvlText;
    public GameObject windupGo;
    public GameObject LostTxtGo;
    public GameObject WonTxtGo;
    public GameObject currentLvlTxtGo;

    private bool updateOnceTexture = false;

    //== GAMEPLAY VARIABLES == //

    public float windupBetweenLevels = 3.0f;

    private bool levelWindedUp = false;
    private int gameplayCounter = 0;

    private float surviveTimer = 0.0f;

    //== TESTING VARIABLES ==//
    public int initialLevel = 1;
    public bool playerInvicible = false;
    [HideInInspector]
    public bool playerDied = false;
    void Start()
    {
        currentLevel = initialLevel;
        PreviewGo.SetActive(false);
        //gameState = GameState.Gameplay;
        voroGen = GameplayGo.GetComponent<VoronoidGenerator>();
        levelConf = GetComponent<LevelConfiguration>();
        windupText = windupGo.GetComponent<TMP_Text>();
        LostText = LostTxtGo.GetComponent<TMP_Text>();
        LostTxtGo.SetActive(false);
        WonText = WonTxtGo.GetComponent<TMP_Text>();
        WonTxtGo.SetActive(false);
        currentLvlText = currentLvlTxtGo.GetComponent<TMP_Text>();

        playerTexLink = PlayerGo.GetComponent<PlayerTexLink>();

        DisableGameplay();
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
    }

    void SwitchToPreview()
    {
        DisableGameplay();
        PreviewGo.SetActive(true);
        gameState = GameState.Preview;
    }

    void DisableGameplay()
    {
        CompletePlayerGo.SetActive(false);
        PreviewCameraGo.SetActive(true);
    }

    void EnableGameplay()
    {
        PreviewCameraGo.SetActive(false);
        CompletePlayerGo.SetActive(true);

        PlayerGo.transform.position = levelConf.LevelSpawnpoint(currentLevel);
    }
    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if(!levelWindedUp)
        {
            if(!updateOnceTexture)
            {
                if (gameplayCounter == 1)
                {
                    //going into gameplay
                    EnableGameplay();
                }
                voroGen.UpdateAndShowTextures(levelConf.Level(currentLevel, 0.0f),gameState);
                updateOnceTexture = true;
            }
            ShowWindowupCounter(timer);
            if (timer >= windupBetweenLevels)
            {
                levelWindedUp = true;
                windupGo.SetActive(false);

                if (gameplayCounter == 0)
                {
                    //going into preview
                    LostTxtGo.SetActive(false);
                    WonTxtGo.SetActive(false);

                    SwitchToPreview();
                    updateOnceTexture = false;
                }
                else if(gameplayCounter == 1)
                {
                    //going into gameplay
                    SwitchToGameplay();
                    updateOnceTexture = false;
                }
                timer = 0.0f;

            }
        }
        else if (levelWindedUp)
        {
            if (gameState == GameState.Preview)
            {
                //print("preview time " + levelConf.PreviewTime(currentLevel, gameState));
                if (timer >= levelConf.PreviewTime(currentLevel, gameState))
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
                //====================================================================
                //gameplay
                surviveTimer += Time.deltaTime;

                if(playerTexLink.currentColor == Color.black && !playerInvicible)
                {
                    playerDied = true;
                }

                if(playerDied)
                {
                    playerDied = false;
                    surviveTimer = 0.0f;
                    //Debug.Log("lost level " + currentLevel);
                    LostTxtGo.SetActive(true);

                    DisableGameplay();
                    windupGo.SetActive(true);
                    levelWindedUp = false;
                    gameplayCounter = 0;
                    timer = 0.0f;
                    return;
                }

                //win condition
                //Debug.Log("level " + currentLevel + " duration " + levelConf.LevelDuration(currentLevel));
                if (surviveTimer > levelConf.LevelDuration(currentLevel))
                {
                    surviveTimer = 0.0f;
                    //Debug.Log("Won level " + currentLevel);
                    WonTxtGo.SetActive(true);
                    currentLevel++;
                    if (currentLevel > maxLevel)
                    {
                        //RESET GAME
                        playerDied = false;
                        currentLevel = 1;
                        currentLevel = initialLevel;
                        PreviewGo.SetActive(false);
                        DisableGameplay();
                        WonTxtGo.SetActive(false);
                        LostTxtGo.SetActive(false);
                        SceneManager.LoadScene(0);
                        return;
                    }

                    currentLvlText.SetText("Current Level: " + currentLevel);

                    DisableGameplay();
                    windupGo.SetActive(true);
                    levelWindedUp = false;
                    gameplayCounter = 0;
                    timer = 0.0f;
                    
                    
                    //return;
                }

                //====================================================================
            }
            voroGen.UpdateAndShowTextures(levelConf.Level(currentLevel, timer * 
                levelConf.TimeModifierForLevel(currentLevel,gameState)), gameState);

        }
    }
}
