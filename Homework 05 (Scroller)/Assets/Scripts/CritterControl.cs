/*--------------------------------------------------------------------------------------------------
 * Author:      Dr. Dana Vrajitoru
 * Author:      Dan Cassidy
 * Date:        2015-10-01
 * Assignment:  Homework 5
 * Source File: CritterControl.cs
 * Language:    C#
 * Course:      CSCI-C 490, Game Programming and Design, TuTh 17:30
 * Purpose:     Controls the different aspects of critters, including respawn and collision
 *              behavior.
--------------------------------------------------------------------------------------------------*/
using UnityEngine;

public class CritterControl : MonoBehaviour
{
    public bool friend = false;
    public int scoreValue = 0;
    public bool respawn = true;

    static Camera mainCam = null;
    static GameMaster master = null;

    AudioSource sound;

    float baseScaleX = 1.0f;
    float baseScaleY = 1.0f;

    /*----------------------------------------------------------------------------------------------
     * Name:    Reset
     * Type:    Method
     * Purpose: Performs actions associated with resetting the game.
     * Input:   Nothing.
     * Output:  Nothing.
    ----------------------------------------------------------------------------------------------*/
    public void Reset()
    {
        // Respawn this object.
        Respawn();
    }

    /*----------------------------------------------------------------------------------------------
     * Name:    OnCollisionEnter2D
     * Type:    Method
     * Purpose: Handles collisions with other physics objects.
     * Input:   Collision2D colInfo, contains the collision information for this collision.
     * Output:  Nothing.
    ----------------------------------------------------------------------------------------------*/
    void OnCollisionEnter2D(Collision2D colInfo)
    {
        // Determine if this object has collided with the player or something else.
        if (colInfo.collider.tag == Constants.PlayerTag)
        {
            // Play contact sound.
            sound.Play();

            // Determine if this object should respawn or not.
            if (respawn)
                Respawn();
            else
                gameObject.SetActive(false);

            // Update the score.  Make sure this is the final thing we do in this branch of the
            // method as this has the potential to end the game, thereby deactivating all the
            // "critter" objects.
            if (friend)
                master.AddPoints(scoreValue);
            else
                master.AddPoints(-scoreValue);
        }
        else if (colInfo.collider.tag == Constants.GroundTag)
        {
            // Determine if this object should respawn or not.
            if (respawn)
                Respawn();
            else
                gameObject.SetActive(false);
        }
    }

    /*----------------------------------------------------------------------------------------------
     * Name:    Respawn
     * Type:    Method
     * Purpose: Respawn the critter at the top of the screen within some random range.
     * Input:   Nothing.
     * Output:  Nothing.
    ----------------------------------------------------------------------------------------------*/
    void Respawn()
    {
        // Randomly scale the object.
        float randScale = Random.Range(Constants.ScaleMin, Constants.ScaleMin + 1);
        transform.localScale = new Vector3(baseScaleX * randScale, baseScaleY * randScale,
            transform.localScale.z);

        // Reset x and y velocity to 0.
        GetComponent<Rigidbody2D>().velocity = new Vector2(0.0f, 0.0f);

        // Randomize the initial position based on the screen size above the top of the screen
        float x = Random.Range(Constants.CritterSpawnXMin, Screen.width - 9);
        float y = Screen.height + Random.Range(Constants.CritterSpawnYMin,
            Constants.CritterSpawnYMax + 1);

        // Then covert it to world coordinates and assign it to the critter.
        Vector3 pos = mainCam.ScreenToWorldPoint(new Vector3(x, y, 0f));
        pos.z = transform.position.z;
        transform.position = pos;
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
        // Find the camera from the object tagged as Player.
        if (!mainCam)
            mainCam = GameObject.FindWithTag(Constants.PlayerTag).GetComponent<PlayerControl>().
                mainCam;

        // Find the game master object.
        if (!master)
            master = GameObject.Find(Constants.GMObjectName).GetComponent<GameMaster>();

        // Store the audio source for easy access.
        sound = GetComponent<AudioSource>();

        // Set the base transform scale so we don't get really big or really small objects
        // eventually.
        baseScaleX = transform.localScale.x;
        baseScaleY = transform.localScale.y;

        // Spawn the object.
        Respawn();
    }
}
