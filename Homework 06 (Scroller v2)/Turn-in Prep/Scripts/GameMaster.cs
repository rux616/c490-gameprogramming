/*--------------------------------------------------------------------------------------------------
 * Author:      Dan Cassidy
 * Date:        2015-10-07
 * Assignment:  Homework 6
 * Source File: GameMaster.cs
 * Language:    C#
 * Course:      CSCI-C 490, Game Programming and Design, TuTh 17:30
 * Purpose:     Controls the general behavior of the overall game.
--------------------------------------------------------------------------------------------------*/
using UnityEngine;
using UnityEngine.UI;

public class GameMaster : MonoBehaviour
{
    public GameObject friendRef = null;
    public GameObject foeRef = null;
    public int level = 0;

    GameObject moon = null;
    TextMesh scoreText = null;
    TextMesh infoText = null;
    GameObject newGameButton = null;
    Text friendsLeftText = null;
    PlayerControl player = null;

    GameObject[] friendArray = null;
    GameObject[] foeArray = null;

    int score = -1;
    int friendsLeft = -1;
    bool levelStarted = false;
    bool levelEnded = false;

    /*----------------------------------------------------------------------------------------------
     * Name:    AddPoints
     * Type:    Method
     * Purpose: Add points to the score.
     * Input:   int pointsToAdd, the number of points to add.
     * Output:  Nothing.
    ----------------------------------------------------------------------------------------------*/
    public void AddPoints(int pointsToAdd)
    {
        // Update the score, then update the score display.
        score += pointsToAdd;
        UpdateScoreDisplay();
    }

    /*----------------------------------------------------------------------------------------------
     * Name:    FriendPickup
     * Type:    Method
     * Purpose: Handle picking up a friend in the level.
     * Input:   Nothing.
     * Output:  Nothing.
    ----------------------------------------------------------------------------------------------*/
    public void FriendPickup()
    {
        if (level == 2)
        {
            // Picked up a friend, decrement the counter.
            friendsLeft--;
            UpdateScoreDisplay();
        }
    }

    /*----------------------------------------------------------------------------------------------
     * Name:    Reset
     * Type:    Method
     * Purpose: Reset the game.
     * Input:   Nothing.
     * Output:  Nothing.
    ----------------------------------------------------------------------------------------------*/
    public void Reset()
    {
        // Load Level 1.
        Application.LoadLevel(Constants.LevelNameL1);
    }


    /*----------------------------------------------------------------------------------------------
     * Name:    CheckEndLevel
     * Type:    Method
     * Purpose: Check for the end of the level based on which level is active.
     * Input:   Nothing.
     * Output:  Nothing.
    ----------------------------------------------------------------------------------------------*/
    void CheckEndLevel()
    {
        // Check for the end of the level based on which level is active.
        switch (level)
        {
            case 1:
                // If the score reaches 0, the level is over.
                if (score == 0)
                    LevelOver();
                break;

            case 2:
                // If the number of friends level reaches 0, the level is over.
                if (friendsLeft == 0 || player.health <= 0)
                    LevelOver();
                break;

            default:
                break;
        }
    }

    /*----------------------------------------------------------------------------------------------
     * Name:    LevelOver
     * Type:    Method
     * Purpose: Handles ending the level.
     * Input:   Nothing.
     * Output:  Nothing.
    ----------------------------------------------------------------------------------------------*/
    void LevelOver()
    {
        // Deactivate all friend objects.
        foreach (GameObject friend in friendArray)
            friend.SetActive(false);

        // Deactivate all foe objects.
        foreach (GameObject foe in foeArray)
            foe.SetActive(false);

        switch (level)
        {
            case 1:
                // Mark the level as ended.
                levelEnded = true;

                // Display level ending text.
                infoText.text = Constants.LevelCompleteTextL1;
                break;

            case 2:
                // Activate the moon and the new game button.
                moon.SetActive(true);
                newGameButton.SetActive(true);

                // Display appropriate level (or game) ending text.
                if (player.health <= 0)
                    infoText.text = Constants.GameOverText;
                else
                    infoText.text = Constants.LevelCompleteTextL2;

                break;
        }
    }

    /*----------------------------------------------------------------------------------------------
     * Name:    Start
     * Type:    Method
     * Purpose: Performs one-time initialization tasks.
     * Input:   Nothing.
     * Output:  Nothing.
    ----------------------------------------------------------------------------------------------*/
    void Start()
    {
        // Set and cache the object references.
        moon = GameObject.Find(Constants.MoonObjectName);
        newGameButton = GameObject.Find(Constants.NewGameButtonObjectName);
        scoreText = GameObject.Find(Constants.ScoreTextObjectName).GetComponent<TextMesh>();
        infoText = GameObject.Find(Constants.InfoTextObjectName).GetComponent<TextMesh>();
        player = GameObject.Find(Constants.PlayerObjectName).GetComponent<PlayerControl>();

        // Deactivate the moon and new game button.
        moon.SetActive(false);
        newGameButton.SetActive(false);

        // Start the level.
        StartLevel();
    }

    /*----------------------------------------------------------------------------------------------
     * Name:    StartLevel
     * Type:    Method
     * Purpose: Handle level specific initialization tasks.
     * Input:   Nothing.
     * Output:  Nothing.
    ----------------------------------------------------------------------------------------------*/
    void StartLevel()
    {
        switch (level)
        {
            case 1:
                // Instantiate the friend objects.
                friendArray = new GameObject[Constants.NumFriendsL1];
                for (int friendNum = 0; friendNum < Constants.NumFriendsL1; friendNum++)
                    friendArray[friendNum] = Instantiate(friendRef) as GameObject;

                // Instantiate the foe objects.
                foeArray = new GameObject[Constants.NumFoesL1];
                for (int foeNum = 0; foeNum < Constants.NumFoesL1; foeNum++)
                    foeArray[foeNum] = Instantiate(foeRef) as GameObject;

                // Set the initial score.
                score = Constants.InitialScoreL1;
                UpdateScoreDisplay();

                // Set the info text.
                infoText.text = Constants.InfoTextL1;

                break;

            case 2:
                // Set and cache level specific object references.
                friendsLeftText = GameObject.Find(Constants.FriendsLeftTextObjectName).
                    GetComponent<Text>();

                // Instantiate the friend objects.
                friendArray = new GameObject[Constants.NumFriendsL2];
                for (int friendNum = 0; friendNum < Constants.NumFriendsL2; friendNum++)
                    friendArray[friendNum] = Instantiate(friendRef) as GameObject;

                // Instantiate the foe objects.
                foeArray = new GameObject[Constants.NumFoesL2];
                for (int foeNum = 0; foeNum < Constants.NumFoesL2; foeNum++)
                    foeArray[foeNum] = Instantiate(foeRef) as GameObject;

                // Set the initial scores.
                score = Constants.InitialScoreL2;
                friendsLeft = Constants.InitialFriendsLeftL2;
                UpdateScoreDisplay();

                // Set the info text.
                infoText.text = Constants.InfoTextL2;

                break;
        }
    }

    /*----------------------------------------------------------------------------------------------
     * Name:    Update
     * Type:    Method
     * Purpose: Performs checks every frame.
     * Input:   Nothing.
     * Output:  Nothing.
    ----------------------------------------------------------------------------------------------*/
    void Update()
    {
        if (!levelStarted && Input.anyKey)
        {
            // Wait until a key press to hide the info text.
            infoText.text = string.Empty;
            levelStarted = true;
        }
        else if (levelEnded && Input.GetKeyDown(KeyCode.Return))
        {
            switch (level)
            {
                case 1:
                    // Level 1 complete, but wait until enter is pressed to load level 2.
                    Application.LoadLevel(Constants.LevelNameL2);
                    break;
            }
        }
        else
        {
            // If nothing else, check for end-of-level conditions.
            CheckEndLevel();
        }
    }

    /*----------------------------------------------------------------------------------------------
     * Name:    UpdateScoreDisplay
     * Type:    Method
     * Purpose: Updates the score display.
     * Input:   Nothing.
     * Output:  Nothing.
    ----------------------------------------------------------------------------------------------*/
    void UpdateScoreDisplay()
    {
        // Update the score text.
        scoreText.text = Constants.ScoreText + score.ToString();

        if (level == 2)
            friendsLeftText.text = friendsLeft.ToString() + Constants.LeftText;
    }
}
