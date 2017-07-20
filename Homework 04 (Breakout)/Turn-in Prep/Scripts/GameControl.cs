/*--------------------------------------------------------------------------------------------------
 * Author:      Dan Cassidy
 * Date:        2015-09-24
 * Assignment:  Homework 4
 * Source File: GameControl.cs
 * Language:    C#
 * Course:      CSCI-C 490, Game Programming and Design, TuTh 17:30
 * Purpose:     This script serves as the main coordinator for the different actions that need to
 *              be taken by the various different scripts.
--------------------------------------------------------------------------------------------------*/
using UnityEngine;
using System.Collections;

public class GameControl : MonoBehaviour
{
    // GameObject references used for command and control.
    private GameObject ballObject = null;
    private GameObject endTextObject = null;
    private GameObject scoreTextObject = null;
    private GameObject newGameButtonObject = null;

    // ArrayList of bricks to keep track of however many there are.
    private ArrayList bricks = new ArrayList();

    // Score tracker.
    private int score = 0;

    /*----------------------------------------------------------------------------------------------
     * Name:    AddPoints
     * Type:    Method
     * Purpose: Add points to the game score.
     * Input:   int numPointsToAdd, contains the number of points to add to the score.
     * Output:  Nothing.
    ----------------------------------------------------------------------------------------------*/
    public void AddPoints(int numPointsToAdd)
    {
        // Add points to the score and then update the score display.
        score += numPointsToAdd;
        UpdateScoreDisplay();
    }

    /*----------------------------------------------------------------------------------------------
     * Name:    GameOver
     * Type:    Method
     * Purpose: End the game.
     * Input:   bool win, contains whether the game was won (true) or lost (false).
     * Output:  Nothing.
    ----------------------------------------------------------------------------------------------*/
    public void GameOver(bool win = false)
    {
        // Game is over. Regardless of whether it was a win or a loss, disable the ball and enable
        // the New Game button.
        ballObject.SetActive(false);
        newGameButtonObject.SetActive(true);

        // Check if the game was a win or a loss.
        if (win)
        {
            // Add a bonus to the score based on how many spare balls remain and update the score
            // display to reflect the new score.
            score += ballObject.GetComponent<BallControl>().SpareCount * Constants.PointsPerBall;
            UpdateScoreDisplay();

            // Show the appropriate text for winning.
            endTextObject.GetComponent<TextMesh>().text = Constants.GameOverWinText;
        }
        else
        {
            // Show the appropriate text for losing.
            endTextObject.GetComponentInChildren<TextMesh>().text = Constants.GameOverLossText;
        }
    }

    /*----------------------------------------------------------------------------------------------
     * Name:    RegisterBrick
     * Type:    Method
     * Purpose: Registers a brick's reference to be used for game resets.
     * Input:   GameObject brickToRegister, the reference to the brick that will be registered.
     * Output:  Nothing.
    ----------------------------------------------------------------------------------------------*/
    public void RegisterBrick(GameObject brickToRegister)
    {
        // Add the brick reference to the bricks object.
        bricks.Add(brickToRegister);
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
        // Disable the New Game button.
        newGameButtonObject.SetActive(false);

        // Remove text from the game over display.
        endTextObject.GetComponent<TextMesh>().text = string.Empty;

        // Reset the score and update the display.
        score = 0;
        UpdateScoreDisplay();

        // Reset all the bricks.
        foreach (GameObject brick in bricks)
            brick.GetComponent<BrickControl>().Reset();
    }

    /*----------------------------------------------------------------------------------------------
     * Name:    UpdateScoreDisplay
     * Type:    Method
     * Purpose: Updates the score display.
     * Input:   Nothing.
     * Output:  Nothing.
    ----------------------------------------------------------------------------------------------*/
    private void UpdateScoreDisplay()
    {
        // Update the score text.
        scoreTextObject.GetComponent<TextMesh>().text = Constants.ScoreText + score.ToString();
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
        // Get and save (cache) the references for the different GameObjects.
        ballObject = GameObject.Find(Constants.BallObjectName);
        endTextObject = GameObject.Find(Constants.EndTextObjectName);
        scoreTextObject = GameObject.Find(Constants.ScoreTextObjectName);
        newGameButtonObject = GameObject.Find(Constants.NewGameButtonObjectName);

        // Set up the game.
        Reset();
    }
}
