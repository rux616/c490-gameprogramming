/*--------------------------------------------------------------------------------------------------
 * Author:      Dan Cassidy
 * Date:        2015-09-24
 * Assignment:  Homework 4
 * Source File: BrickControl.cs
 * Language:    C#
 * Course:      CSCI-C 490, Game Programming and Design, TuTh 17:30
 * Purpose:     Controls the behavior of the bricks.
--------------------------------------------------------------------------------------------------*/
using UnityEngine;

public class BrickControl : MonoBehaviour
{
    // Keep track of the number of bricks left and the object hosting the GameControl script.
    static int bricksLeft = 0;
    static GameObject gameController = null;

    // Color mapped to brick strength.  Goes from green (4 hits left) down to red (1 hit left).
    static Color[] StrengthColorMap =
    {
        new Color(1.0f, 0.0f, 0.0f),    // red
        new Color(1.0f, 0.5f, 0.0f),    // orange
        new Color(1.0f, 1.0f, 0.0f),    // yellow
        new Color(0.0f, 1.0f, 0.0f),    // green
    };

    // Keep track of the brick strength (how many hits are required to break it.
    int brickStrength = Constants.BrickStrengthDefault;

    /*----------------------------------------------------------------------------------------------
     * Name:    Reset
     * Type:    Method
     * Purpose: Resets the brick and randomizes its strength.
     * Input:   Nothing.
     * Output:  Nothing.
    ----------------------------------------------------------------------------------------------*/
    public void Reset()
    {
        // Check if this brick is not active.
        if (!gameObject.activeSelf)
        {
            // Reactivate this brick and increment the number of bricks left.
            gameObject.SetActive(true);
            bricksLeft++;
        }

        // Randomize the brick strength and change color accordingly.
        brickStrength = Random.Range(Constants.BrickStrengthMin, Constants.BrickStrengthMax + 1);
        ChangeColor();
    }

    /*----------------------------------------------------------------------------------------------
     * Name:    ChangeColor
     * Type:    Method
     * Purpose: Update the color of the brick.
     * Input:   Nothing.
     * Output:  Nothing.
    ----------------------------------------------------------------------------------------------*/
    void ChangeColor()
    {
        try
        {
            // Try to set the color of the brick based on the strength.
            GetComponent<SpriteRenderer>().color = StrengthColorMap[brickStrength - 1];
        }
        catch
        {
            // If something goes wrong, say for some reason brickStrength is out of bounds of
            // StrengthColorMap, set the brick to all white instead.
            GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f);
        }
    }

    /*----------------------------------------------------------------------------------------------
     * Name:    OnCollisionEnter2D
     * Type:    Method
     * Purpose: Handles collisions with other physics objects, specifically the ball.
     * Input:   Collision2D colInfo, contains the collision information for this collision.
     * Output:  Nothing.
    ----------------------------------------------------------------------------------------------*/
    void OnCollisionEnter2D(Collision2D colInfo)
    {
        // Check to make sure that the object colliding with this one is the ball.
        if (colInfo.collider.tag == "Ball")
        {
            // Check to see if the brick has enough strength to withstand the hit.
            if (brickStrength > Constants.BrickStrengthMin)
            {
                // Reduce the strength of the brick by 1 and update the color accordingly.
                brickStrength--;
                ChangeColor();
            }
            else
            {
                // The brick is broken.  Add the appropriate point value to the score and decrement
                // the number of bricks left.
                gameController.GetComponent<GameControl>().AddPoints(Constants.PointsPerBrick);
                bricksLeft--;

                // Check if the number of bricks left is now 0.
                if (bricksLeft == 0)
                {
                    // If no bricks are left, the game has been won.  Let GameControl know.
                    gameController.GetComponent<GameControl>().GameOver(win: true);
                }

                // This brick has broken, bringing much shame upon its family.  It must deactivate
                // itself in order to restore its honor.
                gameObject.SetActive(false);
            }
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
        // Check if the gameController object has been cached already, and if it hasn't, do so.
        if (gameController == null)
            gameController = GameObject.Find(Constants.GameControllerObjectName);

        // Register the brick with the Game Control script.
        gameController.GetComponent<GameControl>().RegisterBrick(gameObject);

        // Increment the number of bricks left.
        bricksLeft++;

        // Set up the brick strength.
        Reset();
    }
}
