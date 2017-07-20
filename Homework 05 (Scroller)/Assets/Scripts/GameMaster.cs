/*--------------------------------------------------------------------------------------------------
 * Author:      Dan Cassidy
 * Date:        2015-10-01
 * Assignment:  Homework 5
 * Source File: GameMaster.cs
 * Language:    C#
 * Course:      CSCI-C 490, Game Programming and Design, TuTh 17:30
 * Purpose:     Controls the general behavior of the overall game.
--------------------------------------------------------------------------------------------------*/
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    public GameObject friendRef;
    public GameObject foeRef;

    GameObject moon = null;
    TextMesh scoreText = null;
    GameObject newGameButton = null;

    GameObject[] friendArray = new GameObject[Constants.NumFriends];
    GameObject[] foeArray = new GameObject[Constants.NumFoes];

    int score = Constants.InitialScore;

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

        // If the score reaches 0, the game is over.
        if (score == 0)
            GameOver();
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
        // Reset all the friends.
        foreach (GameObject friend in friendArray)
        {
            friend.SetActive(true);
            friend.GetComponent<CritterControl>().Reset();
        }

        // Reset all the foes.
        foreach (GameObject foe in foeArray)
        {
            foe.SetActive(true);
            foe.GetComponent<CritterControl>().Reset();
        }

        // Reset the score and score display.
        score = Constants.InitialScore;
        UpdateScoreDisplay();

        // Hide the moon and new game button.
        moon.SetActive(false);
        newGameButton.SetActive(false);
    }

    /*----------------------------------------------------------------------------------------------
     * Name:    GameOver
     * Type:    Method
     * Purpose: Handles ending the game.
     * Input:   Nothing.
     * Output:  Nothing.
    ----------------------------------------------------------------------------------------------*/
    void GameOver()
    {
        // Deactivate all friends.
        foreach (GameObject friend in friendArray)
            friend.SetActive(false);

        // Deactivate all foes.
        foreach (GameObject foe in foeArray)
            foe.SetActive(false);

        // Activate the moon and the new game button.
        moon.SetActive(true);
        newGameButton.SetActive(true);
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
        // Instantiate the friend objects.
        for (int friendNum = 0; friendNum < Constants.NumFoes; friendNum++)
            friendArray[friendNum] = Instantiate(friendRef) as GameObject;

        // Instantiate the foe objects.
        for (int foeNum = 0; foeNum < Constants.NumFoes; foeNum++)
            foeArray[foeNum] = Instantiate(foeRef) as GameObject;

        // Set and cache the object references.
        moon = GameObject.Find(Constants.MoonObjectName);
        newGameButton = GameObject.Find(Constants.NewGameButtonObjectName);
        scoreText = GameObject.Find(Constants.ScoreTextObjectName).GetComponent<TextMesh>();

        // Set the initial score.
        score = Constants.InitialScore;
        UpdateScoreDisplay();

        // Deactivate the moon and new game button.
        moon.SetActive(false);
        newGameButton.SetActive(false);
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
    }
}
