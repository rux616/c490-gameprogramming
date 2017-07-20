/*--------------------------------------------------------------------------------------------------
 * Author:      Dan Cassidy
 * Date:        2015-09-10
 * Assignment:  Homework 2
 * Source File: ClickTile.cs
 * Language:    C#
 * Course:      CSCI-C 490, Game Programming and Design, TuTh 17:30
 * Purpose:     Controls the individual tiles.
--------------------------------------------------------------------------------------------------*/
using UnityEngine;
using System.Collections;

public class ClickTile : MonoBehaviour {
    static public GameObject frameRef;

    public Sprite tileGlyph;
    public Sprite tileOpen;

    public int myID = 0;
    public int myNumber = 0;

    private bool mouseIsOver = false;
    private bool isOpen = false;

    /*----------------------------------------------------------------------------------------------
     * Name:    OpenTile
     * Type:    Method
     * Purpose: Open the tile.
     * Input:   Nothing.
     * Output:  Nothing.
    ----------------------------------------------------------------------------------------------*/
    public void OpenTile() {
        // Change the tile's sprite to that of the open tile.
        GetComponent<SpriteRenderer>().sprite = tileOpen;

        // Change the tile's text to its number.
        GetComponentInChildren<TextMesh>().text = myNumber.ToString();

        // Tell the frame manager that this tile has been played.
        frameRef.GetComponent<FrameManager>().PlayTile(myID, myNumber);

        // Set this tile's open flag.
        isOpen = true;
    }

    /*----------------------------------------------------------------------------------------------
     * Name:    CloseTile
     * Type:    Method
     * Purpose: Close the tile.
     * Input:   Nothing.
     * Output:  Nothing.
    ----------------------------------------------------------------------------------------------*/
    public void CloseTile() {
        // Change the tile's sprite to that of its assigned glyph.
        GetComponent<SpriteRenderer>().sprite = tileGlyph;

        // Change the tile's text back to blank.
        GetComponentInChildren<TextMesh>().text = string.Empty;

        // Tell the frame manager that this tile has been unplayed.
        frameRef.GetComponent<FrameManager>().UnplayTile(myID, myNumber);

        // Unset this tile's open flag.
        isOpen = false;
    }

    /*----------------------------------------------------------------------------------------------
     * Name:    OnMouseEnter
     * Type:    Method
     * Purpose: Called when the mouse enters the space above this tile.
     * Input:   Nothing.
     * Output:  Nothing.
    ----------------------------------------------------------------------------------------------*/
    void OnMouseEnter() {
        // The mouse is entering the space above this tile.
        mouseIsOver = true;
    }

    /*----------------------------------------------------------------------------------------------
     * Name:    OnMouseExit
     * Type:    Method
     * Purpose: Called when the mouse exits the space above this tile.
     * Input:   Nothing.
     * Output:  Nothing.
    ----------------------------------------------------------------------------------------------*/
    void OnMouseExit() {
        // The mouse is exiting the space above this tile.
        mouseIsOver = false;
    }

    /*----------------------------------------------------------------------------------------------
     * Name:    Start
     * Type:    Method
     * Purpose: Performs some basic initialization.
     * Input:   Nothing.
     * Output:  Nothing.
    ----------------------------------------------------------------------------------------------*/
    void Start() {
        // Set this tile's number to a random value between the upper and lower limits, inclusive.
        myNumber = Random.Range(Constants.MyNumberLowerLimit, Constants.MyNumberUpperLimit + 1);

        // Add this tile's game object to the frame manager's internal list.
        FrameManager.tileArray[myID] = gameObject;
    }

    /*----------------------------------------------------------------------------------------------
     * Name:    Update
     * Type:    Method
     * Purpose: Called once per frame to see if the mouse has been left clicked on this tile.
     * Input:   Nothing.
     * Output:  Nothing.
    ----------------------------------------------------------------------------------------------*/
    void Update() {
        // Check to see if the mouse is over this tile and the left mouse button has been clicked.
        if (mouseIsOver && Input.GetMouseButtonDown(0)) {
            // If it is, then check to see whether the tile is open.
            if (isOpen) {
                // The tile is open.  Close it.
                CloseTile();
            }
            // If the tile is not open, check to see whether it is playable.
            else if (frameRef.GetComponent<FrameManager>().IsPlayable(myID)) {
                // The tile is playable, so proceed to open it.
                OpenTile();
            }
        }
    }
}
