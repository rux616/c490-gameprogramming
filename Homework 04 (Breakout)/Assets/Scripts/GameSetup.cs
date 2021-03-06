using UnityEngine;
using System.Collections;

public class GameSetup : MonoBehaviour
{

    public Camera mainCam;

    public GameObject topWall;
    public GameObject bottomWall;
    public GameObject leftWall;
    public GameObject rightWall;

    BoxCollider2D topWallCol;
    BoxCollider2D bottomWallCol;
    BoxCollider2D leftWallCol;
    BoxCollider2D rightWallCol;

    public Transform[] players;

    float spriteSize = 0;

    // Use this for initialization
    void Start()
    {
        // do we have a sprite?
        if (topWall.GetComponent<SpriteRenderer>())
        {
            // get its width and store it 
            spriteSize = topWall.GetComponent<SpriteRenderer>().sprite.bounds.size.x;
        }
        else
        {
            // we don't have a sprite, so we get references to the colliders directly.
            topWallCol = topWall.GetComponent<BoxCollider2D>();
            bottomWallCol = bottomWall.GetComponent<BoxCollider2D>();
            leftWallCol = leftWall.GetComponent<BoxCollider2D>();
            rightWallCol = rightWall.GetComponent<BoxCollider2D>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (spriteSize > 0)
        {
            // we have a sprite, we use it to size and place the walls

            // Assuming that the sprite is square, find its size on screen
            float spriteScale = mainCam.WorldToScreenPoint(new Vector3(spriteSize, 0f, 0f)).x - mainCam.WorldToScreenPoint(new Vector3(0, 0f, 0f)).x;

            // move each wall to its edge location.
            topWall.transform.localScale = new Vector3(Screen.width / spriteScale, 0.5f, 1f);
            topWall.transform.position = new Vector3(0f, mainCam.ScreenToWorldPoint(new Vector3(0f, Screen.height, 0f)).y, 0f);

            bottomWall.transform.localScale = new Vector3(Screen.width / spriteScale, 0.5f, 1f);
            bottomWall.transform.position = new Vector3(0f, mainCam.ScreenToWorldPoint(new Vector3(0f, 0f, 0f)).y, 0f);

            leftWall.transform.localScale = new Vector3(0.5f, Screen.height / spriteScale, 1f);
            leftWall.transform.position = new Vector3(mainCam.ScreenToWorldPoint(new Vector3(0f, 0f, 0f)).x, 0f, 0f);

            rightWall.transform.localScale = new Vector3(0.5f, Screen.height / spriteScale, 1f);
            rightWall.transform.position = new Vector3(mainCam.ScreenToWorldPoint(new Vector3(Screen.width, 0f, 0f)).x, 0f, 0f);
        }
        else
        {
            // no sprite, size and place the colliders themselves
            topWallCol.size = new Vector2(mainCam.ScreenToWorldPoint(new Vector3(Screen.width * 2f, 0f, 0f)).x, 1f);
            topWallCol.offset = new Vector2(0f, mainCam.ScreenToWorldPoint(new Vector3(0f, Screen.height, 0f)).y + 0.5f);

            bottomWallCol.size = new Vector2(mainCam.ScreenToWorldPoint(new Vector3(Screen.width * 2f, 0f, 0f)).x, 1f);
            bottomWallCol.offset = new Vector2(0f, mainCam.ScreenToWorldPoint(new Vector3(0f, 0f, 0f)).y - 0.5f);

            leftWallCol.size = new Vector2(1f, mainCam.ScreenToWorldPoint(new Vector3(0f, Screen.height * 2f, 0f)).y);
            leftWallCol.offset = new Vector2(mainCam.ScreenToWorldPoint(new Vector3(0f, 0f, 0f)).x - 0.5f, 0f);

            rightWallCol.size = new Vector2(1f, mainCam.ScreenToWorldPoint(new Vector3(0f, Screen.height * 2f, 0f)).y);
            rightWallCol.offset = new Vector2(mainCam.ScreenToWorldPoint(new Vector3(Screen.width, 0f, 0f)).x + 0.5f, 0f);
        }

        // now place the players based on screen size
        if (players.Length == 1)
        {
            // one player, it must be horizontal
            players[0].position = new Vector3(players[0].position.x, mainCam.ScreenToWorldPoint(new Vector3(0f, 20f, 0f)).y, 0f);
        }
        else if (players.Length > 1)
        {
            // two players, they must be vertical
            players[0].position = new Vector3(mainCam.ScreenToWorldPoint(new Vector3(20f, 0f, 0f)).x, players[0].position.y, 0f);
            players[1].position = new Vector3(mainCam.ScreenToWorldPoint(new Vector3(Screen.width - 20f, 0f, 0f)).x, players[1].position.y, 0f);
        }
    }
}
