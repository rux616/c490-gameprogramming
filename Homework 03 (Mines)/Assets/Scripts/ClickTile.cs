/*--------------------------------------------------------------------------------------------------
 * Author:      Dan Cassidy
 * Date:        2015-09-13
 * Assignment:  Homework 3
 * Source File: ClickTile.cs
 * Language:    C#
 * Course:      CSCI-C 490, Game Programming and Design, TuTh 17:30
 * Purpose:     Controls the individual tiles.
 *
 * Note:        The flag "exploded" was not used and has been removed.
--------------------------------------------------------------------------------------------------*/
using UnityEngine;
using System.Collections;

public class ClickTile : MonoBehaviour
{
    static public bool isGameOver = false;
    static public GameObject tableRef;

    public Sprite tileBack;
    public Sprite tileOpen;
    public Sprite tileFlagged;
    public Sprite tileMine;
    public Sprite tileExploded;

    public bool isFlagged = false;
    public bool isMine = false;
    public bool isOpen = false;

    private bool mouseIsOver = false;

    private int row;
    private int column;

    /*----------------------------------------------------------------------------------------------
     * Name:    OpenTile
     * Type:    Method
     * Purpose: Open the tile.
     * Input:   Nothing.
     * Output:  Nothing.
    ----------------------------------------------------------------------------------------------*/
    public void OpenTile()
    {
        // Regardless of what happens, the tile is now open.  Set it as such.
        isOpen = true;

        // Check if the tile is a mine.
        if (isMine)
        {
            // Boom.  Set the sprite to the exploded sprite.
            GetComponent<SpriteRenderer>().sprite = tileExploded;

            // Check if the game is already over.
            if (!isGameOver)
            {
                // Set the game over flag and let the table manager know that this tile has been
                // opened (blow up).
                isGameOver = true;
                tableRef.GetComponent<TableManager>().OpenMine();
            }
        }
        else
        {
            // No boom.  Set the sprite to the open sprite.
            GetComponent<SpriteRenderer>().sprite = tileOpen;

            // Let the table manager know that this tile has been opened.
            tableRef.GetComponent<TableManager>().OpenEmptyTile(row, column);
        }

    }

    /*----------------------------------------------------------------------------------------------
     * Name:    Reset
     * Type:    Method
     * Purpose: Reset the tile for a new game.
     * Input:   Nothing.
     * Output:  Nothing.
    ----------------------------------------------------------------------------------------------*/
    public void Reset()
    {
        // Reset all flags back to default.
        isGameOver = false;
        isMine = false;
        isOpen = false;
        isFlagged = false;

        // Reset the text to empty.
        SetText(string.Empty);

        // Reset the sprite.
        GetComponent<SpriteRenderer>().sprite = tileBack;
    }

    /*----------------------------------------------------------------------------------------------
     * Name:    SetId
     * Type:    Method
     * Purpose: Set the ID (row and column) of the tile.
     * Input:   int idRow, contains the row portion of the ID.
     * Input:   int idColumn, contains the column portion of the ID.
     * Output:  Nothing.
    ----------------------------------------------------------------------------------------------*/
    public void SetId(int idRow, int idColumn)
    {
        // Set the row and column.
        row = idRow;
        column = idColumn;
    }

    /*----------------------------------------------------------------------------------------------
     * Name:    SetText
     * Type:    Method
     * Purpose: Set the text on the tile.
     * Input:   string textToSet, contains the text that the text mesh will be set to.
     * Output:  Nothing.
    ----------------------------------------------------------------------------------------------*/
    public void SetText(string textToSet)
    {
        // Set the text mesh's text object to the method argument.
        GetComponentInChildren<TextMesh>().text = textToSet;
    }

    /*----------------------------------------------------------------------------------------------
     * Name:    OnMouseEnter
     * Type:    Method
     * Purpose: Called when the mouse enters the space above this tile.
     * Input:   Nothing.
     * Output:  Nothing.
    ----------------------------------------------------------------------------------------------*/
    void OnMouseEnter()
    {
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
    void OnMouseExit()
    {
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
    void Start()
    {
        // Clear the tile's text.
        SetText(string.Empty);
    }

    /*----------------------------------------------------------------------------------------------
     * Name:    Update
     * Type:    Method
     * Purpose: Called once per frame to see if the mouse has been clicked on this tile.
     * Input:   Nothing.
     * Output:  Nothing.
    ----------------------------------------------------------------------------------------------*/
    void Update()
    {
        // Check to see if the mouse is over this tile, the tile is closed, and the game isn't over.
        if (mouseIsOver && !isOpen && !isGameOver)
        {
            // Check to see if the left mouse button is clicked and the tile is not flagged.
            if (Input.GetMouseButtonDown(0) && !isFlagged)
            {
                // The tile is closed. Open it.
                OpenTile();
            }
            // Check to see if the right mouse button is clicked.
            else if (Input.GetMouseButtonDown(1))
            {
                // Check to see if the tile is flagged and there are flags left.
                if (!isFlagged && tableRef.GetComponent<TableManager>().minesLeft > 0)
                {
                    // Tile is not flagged; flag it, set the appropriate sprite, and decrement the
                    // number of mines left.
                    isFlagged = true;
                    GetComponent<SpriteRenderer>().sprite = tileFlagged;
                    tableRef.GetComponent<TableManager>().UpdateMineCount(-1);
                    tableRef.GetComponent<TableManager>().CheckWin();
                }
                else if (isFlagged)
                {
                    // Tile is flagged; un-flag it, set the appropriate sprite, and increment the
                    // number of mines left.
                    GetComponent<SpriteRenderer>().sprite = tileBack;
                    isFlagged = false;
                    tableRef.GetComponent<TableManager>().UpdateMineCount(1);
                }
            }
        }
    }
}
