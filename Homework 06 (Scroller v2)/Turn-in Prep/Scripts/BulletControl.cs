/*--------------------------------------------------------------------------------------------------
 * Author:      Dan Cassidy
 * Date:        2015-10-07
 * Assignment:  Homework 6
 * Source File: BulletControl.cs
 * Language:    C#
 * Course:      CSCI-C 490, Game Programming and Design, TuTh 17:30
 * Purpose:     Controls the bullets fired by the player.
--------------------------------------------------------------------------------------------------*/
using UnityEngine;

public class BulletControl : MonoBehaviour
{
    static Camera mainCam = null;

    /*----------------------------------------------------------------------------------------------
     * Name:    OnCollisionEnter2D
     * Type:    Method
     * Purpose: Handles collisions with other physics objects.
     * Input:   Collision2D colInfo, contains the collision information for this collision.
     * Output:  Nothing.
    ----------------------------------------------------------------------------------------------*/
    void OnCollisionEnter2D(Collision2D colInfo)
    {
        // Determine what this object has collided with via tag.
        switch (colInfo.collider.tag)
        {
            case Constants.CritterTag:
                // If the bullet has collided with a non-friend critter, destroy itself.
                if (!colInfo.gameObject.GetComponent<CritterControl>().friend)
                    Destroy(gameObject);
                break;

            case Constants.GroundTag:
                // If the bullet has collided with a ground collider, destroy itself.
                Destroy(gameObject);
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
        if (!mainCam)
            mainCam = GameObject.Find(Constants.MainCameraObjectName).GetComponent<Camera>();
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
        // Kill the bullet just after it goes off the top of the screen.
        if (mainCam.WorldToScreenPoint(transform.position).y > Screen.height + 20)
            Destroy(gameObject);
    }

}
