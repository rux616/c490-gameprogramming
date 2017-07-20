/*--------------------------------------------------------------------------------------------------
 * Author:      Dr. Vrajitoru (Primary)
 * Author:      Dan Cassidy (Supplemental)
 * Date:        2015-09-24
 * Assignment:  Homework 4
 * Source File: BallControl.cs
 * Language:    C#
 * Course:      CSCI-C 490, Game Programming and Design, TuTh 17:30
 * Purpose:     Controls the ball and actions associated with it.
--------------------------------------------------------------------------------------------------*/
using UnityEngine;

public class BallControl : MonoBehaviour
{
    // Define default minimum and maximum speed of the ball.
    public float minSpeed = 5, maxSpeed = 25;

    // Array which holds references to the reserve balls.
    public GameObject[] reserve;

    // The number of reserve balls.
    int spareCount;

    // Initial position of the ball.
    Vector3 initialPos = new Vector3(0f, 0f, 0f);

    /*----------------------------------------------------------------------------------------------
     * Name:    SpareCount
     * Type:    Property
     * Purpose: Allows public read-only access to the number of reserve balls left.
    ----------------------------------------------------------------------------------------------*/
    public int SpareCount
    {
        get { return spareCount; }
    }

    /*----------------------------------------------------------------------------------------------
     * Name:    Reset
     * Type:    Method
     * Purpose: Resets the ball and all reserves.
     * Input:   Nothing.
     * Output:  Nothing.
    ----------------------------------------------------------------------------------------------*/
    public void Reset()
    {
        // Reset the number of reserves and make sure their GameObjects are enabled.
        spareCount = reserve.Length;
        foreach (GameObject reserveBall in reserve)
            reserveBall.SetActive(true);

        // Reactivates the ball.
        gameObject.SetActive(true);

        // Sets the ball position and speed.
        ResetBall();
    }

    /*----------------------------------------------------------------------------------------------
     * Name:    AverageSpeed
     * Type:    Method
     * Purpose: Averages the speed between the paddle (player) and the ball if the player is moving.
     * Input:   Collision2D colInfo, contains collision information between the ball and player.
     * Output:  Nothing.
    ----------------------------------------------------------------------------------------------*/
    void AverageSpeed(Collision2D colInfo)
    {
        //average the velocity over x between the player and the ball if the player is moving
        float velX = GetComponent<Rigidbody2D>().velocity.x;
        if (colInfo.collider.GetComponent<Rigidbody2D>().velocity.x != 0)
        {
            velX = velX / 2 + colInfo.collider.GetComponent<Rigidbody2D>().velocity.x / 3;
            if (velX >= 0)
                velX = Mathf.Clamp(velX, minSpeed, maxSpeed);
            else
                velX = Mathf.Clamp(velX, -maxSpeed, -minSpeed);
        }
        // then the same over y
        float velY = GetComponent<Rigidbody2D>().velocity.y;
        if (colInfo.collider.GetComponent<Rigidbody2D>().velocity.y != 0)
        {
            velY = velY / 2 + colInfo.collider.GetComponent<Rigidbody2D>().velocity.y / 3;
            if (velY >= 0)
                velY = Mathf.Clamp(velY, minSpeed, maxSpeed);
            else
                velY = Mathf.Clamp(velY, -maxSpeed, -minSpeed);
        }
        GetComponent<Rigidbody2D>().velocity = new Vector2(velX, velY);
    }

    /*----------------------------------------------------------------------------------------------
     * Name:    LoseBall
     * Type:    Method
     * Purpose: Lose a ball if it hits the ground.
     * Input:   Nothing.
     * Output:  Nothing.
    ----------------------------------------------------------------------------------------------*/
    void LoseBall()
    {
        // Check if there are any spare balls left.
        if (spareCount > 0)
        {
            // Decrement the number of spares, deactivate one of said spares, and reset the ball
            // position and velocity.
            spareCount--;
            reserve[spareCount].SetActive(false);
            ResetBall();
        }
        else
        {
            // No reserves left, so the game is lost.  Alert the GameControl script.
            GameObject.Find(Constants.GameControllerObjectName).
                GetComponentInChildren<GameControl>().GameOver(win: false);
        }
    }

    /*----------------------------------------------------------------------------------------------
     * Name:    OnCollisionEnter2D
     * Type:    Method
     * Purpose: Handles collisions between this object and another.
     * Input:   Collision2D colInfo, contains details about this collision.
     * Output:  Nothing.
    ----------------------------------------------------------------------------------------------*/
    void OnCollisionEnter2D(Collision2D colInfo)
    {
        // Check if the ball is colliding with the player.
        if (colInfo.collider.tag == "Player")
        {
            //average the velocity over x between the player and the ball
            AverageSpeed(colInfo);
        }

        // Check if the ball is colliding with the "ground" object.
        else if (colInfo.collider.tag == "Ground")
        {
            // Bye-bye ball.
            LoseBall();
        }
    }

    /*----------------------------------------------------------------------------------------------
     * Name:    ResetBall
     * Type:    Method
     * Purpose: Resets the ball with the initial position and velocity.
     * Input:   Nothing.
     * Output:  Nothing.
    ----------------------------------------------------------------------------------------------*/
    void ResetBall()
    {
        transform.position = initialPos; //recover the initial position
        GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 0f);
        int randomNumber = Random.Range(0, 2);
        if (randomNumber == 0)
        { // go left or right
            GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(30f, 50f), 10));
        }
        else
        {
            GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-50f, -30f), 10));
        }
    }

    /*----------------------------------------------------------------------------------------------
     * Name:    Start
     * Type:    Method
     * Purpose: Perform some one-time initialization tasks.
     * Input:   Nothing.
     * Output:  Nothing.
    ----------------------------------------------------------------------------------------------*/
    void Start()
    {
        // save the initial position
        initialPos = transform.position;
        // set the speed
        ResetBall();
        // find out how many spare balls we have
        spareCount = reserve.Length;
    }

}
