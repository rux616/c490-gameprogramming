/*--------------------------------------------------------------------------------------------------
 * Author:      Dr. Dana Vrajitoru
 * Author:      Dan Cassidy
 * Date:        2015-10-01
 * Assignment:  Homework 5
 * Source File: PlayerControl.cs
 * Language:    C#
 * Course:      CSCI-C 490, Game Programming and Design, TuTh 17:30
 * Purpose:     Controls the different aspects of critters, including respawn and collision
 *              behavior.
--------------------------------------------------------------------------------------------------*/
using UnityEngine;

public class PlayerControl : MonoBehaviour
{

    public KeyCode moveUp, moveDown, moveLeft, moveRight;

    public float speedX = 0, speedY = 0;
    public bool linearMovement = true;

    public Camera mainCam;

    public static int score = 0;

    private Rigidbody2D rbody;

    /*----------------------------------------------------------------------------------------------
     * Name:    AdjustPosition
     * Type:    Method
     * Purpose: Make sure the player does not go off the screen.
     * Input:   Nothing.
     * Output:  Nothing.
    ----------------------------------------------------------------------------------------------*/
    void AdjustPosition()
    {
        Vector3 screenPos = mainCam.WorldToScreenPoint(transform.position);
        Vector3 topScreen = mainCam.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height,
            0f));
        Vector3 bottomScreen = mainCam.ScreenToWorldPoint(new Vector3(0f, 0f, 0f));
        // vertical adjustment
        if (screenPos.y > Screen.height)
            transform.position = new Vector3(transform.position.x, topScreen.y,
                transform.position.z);
        else if (screenPos.y < 0)
            transform.position = new Vector3(transform.position.x, bottomScreen.y,
                transform.position.z);
        // student: add some code for the horizontal
        if (screenPos.x > Screen.width)
            transform.position = new Vector3(bottomScreen.x, transform.position.y,
                transform.position.z);
        else if (screenPos.x < 0)
            transform.position = new Vector3(topScreen.x, transform.position.y,
                transform.position.z);
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
        // store the rigid body in an attribute for easier access.
        rbody = GetComponent<Rigidbody2D>();
    }

    /*----------------------------------------------------------------------------------------------
     * Name:    Update
     * Type:    Method
     * Purpose: Performs per-frame updates.
     * Input:   Nothing.
     * Output:  Nothing.
    ----------------------------------------------------------------------------------------------*/
    void Update()
    {
        // move the player in the 4 directions based on the keys we set up for it
        if (Input.GetKey(moveUp))
        {
            if (linearMovement) // simple constant velocity
                rbody.velocity = new Vector2(0f, speedY);
            else // if we were going to use a force instead
                rbody.AddForce(new Vector2(0, speedY));
        }
        else if (Input.GetKey(moveDown))
        {
            if (linearMovement)
                rbody.velocity = new Vector2(0f, -speedY);
            else
                rbody.AddForce(new Vector2(0f, -speedY));
        }
        else if (Input.GetKey(moveLeft))
        {
            if (linearMovement)
                rbody.velocity = new Vector2(-speedX, 0f);
            else
                rbody.AddForce(new Vector2(-speedX, 0f));
        }
        else if (Input.GetKey(moveRight))
        {
            if (linearMovement)
                rbody.velocity = new Vector2(speedX, 0f);
            else
                rbody.AddForce(new Vector2(speedX, 0f));
        }
        else
        { // no input, reset the speed
            rbody.velocity = new Vector2(0f, 0f);
        }
        AdjustPosition();
    }
}
