/*--------------------------------------------------------------------------------------------------
 * Author:      Dan Cassidy
 * Date:        2015-09-13
 * Assignment:  Homework 3
 * Source File: TableManager.cs
 * Language:    C#
 * Course:      CSCI-C 490, Game Programming and Design, TuTh 17:30
 * Purpose:     Controls the table of tiles and the general game logic.
 *
 * Note:        The grid is drawn such that tile (0, 0) is at the top left corner.
--------------------------------------------------------------------------------------------------*/
using UnityEngine;
using System.Collections;

public class TableManager : MonoBehaviour
{
    private static int[] rowOffsetMapping = { -1, -1, -1, 0, 0, 1, 1, 1 };
    private static int[] columnOffSetMapping = { -1, 0, 1, -1, 1, -1, 0, 1 };
    private static Color[] dangerLevel =
    {
        new Color(),                        // white
        new Color(0.2f, 0.2f, 1.0f),        // blue
        new Color(0.2f, 0.8f, 0.2f),        // green
        new Color(1.0f, 1.0f, 0.2f),        // yellow
        new Color(0.8f, 0.4f, 0.0f),        // orange
        new Color(0.9f, 0.0f, 0.0f),        // red
        new Color(0.6f, 0.0f, 0.0f),        // dim red
        new Color(0.3f, 0.0f, 0.0f),        // dark red
        new Color(0.0f, 0.0f, 0.0f)         // black
    };

    public GameObject tileRef;

    public int minesLeft;

    private GameObject[,] tileArray;

    // Game settings.
    private int rows = 9;
    private int columns = 9;
    private int numberMines = 15;

    /*----------------------------------------------------------------------------------------------
     * Name:    CheckWin
     * Type:    Method
     * Purpose: Check the tiles to see if a winning situation has been reached.
     * Input:   Nothing.
     * Output:  Nothing.
    ----------------------------------------------------------------------------------------------*/
    public void CheckWin()
    {
        // Gateway check to see if any mines are left.
        if (minesLeft != 0)
        {
            // No win for you.
            return;
        }

        // Run through all the tiles.
        foreach (var tile in tileArray)
        {
            // Check if any mines are left, if the tile is not open, or if the tile is not flagged.
            if (!tile.GetComponent<ClickTile>().isOpen && !tile.GetComponent<ClickTile>().isFlagged)
            {
                // No win for you.
                return;
            }
        }

        // Game is over.
        ClickTile.isGameOver = true;

        // Run through all the tiles again.
        foreach (var tile in tileArray)
        {
            // Check if the tile is flagged.
            if (tile.GetComponent<ClickTile>().isFlagged)
            {
                // Set the sprite to the mine.
                tile.GetComponent<SpriteRenderer>().sprite =
                    tile.GetComponent<ClickTile>().tileMine;
            }
        }
    }

    /*----------------------------------------------------------------------------------------------
     * Name:    OpenEmptyTile
     * Type:    Method
     * Purpose: Opens a tile and handles the setting of text on the tile or cascade-opening a blank.
     * Input:   int row, contains the row of the tile to open.
     * Input:   int column, contains the column of the tile to open.
     * Output:  Nothing.
    ----------------------------------------------------------------------------------------------*/
    public void OpenEmptyTile(int row, int column)
    {
        // Get how many mines are nearby.
        int nearbyMines = CountNeighbors(row, column);

        // Check to see if there are any.
        if (nearbyMines > 0)
        {
            // Set the text for the number of mines and also change the color to indicate danger.
            tileArray[row, column].GetComponent<ClickTile>().SetText(nearbyMines.ToString());
            tileArray[row, column].GetComponentInChildren<TextMesh>().color =
                dangerLevel[nearbyMines];
        }
        else
        {
            // It's a blank!  Open all tiles in the region unless flagged (or mined, or open).
            OpenAllFree(row, column);
        }

        // The tile has been opened, now check if it triggers the winning conditions.
        CheckWin();
    }

    /*----------------------------------------------------------------------------------------------
     * Name:    OpenMine
     * Type:    Method
     * Purpose: Detonate all the mines.
     * Input:   Nothing.
     * Output:  Nothing.
    ----------------------------------------------------------------------------------------------*/
    public void OpenMine()
    {
        // Run through the list of tiles and detonate the mines.
        foreach (var tile in tileArray)
        {
            // Check if the tile is mined.
            if (tile.GetComponent<ClickTile>().isMine)
            {
                // Boom.  Open the tile.
                tile.GetComponent<ClickTile>().OpenTile();
            }
        }
    }

    /*----------------------------------------------------------------------------------------------
     * Name:    Reset
     * Type:    Method
     * Purpose: Reset the game board and tiles for a new game.
     * Input:   Nothing.
     * Output:  Nothing.
    ----------------------------------------------------------------------------------------------*/
    public void Reset()
    {
        // Run through all the tiles and reset them.
        foreach (var tile in tileArray)
        {
            tile.GetComponent<ClickTile>().Reset();
        }

        // Place new mines.
        PlaceMines();

        // Reset the mines counter.
        minesLeft = numberMines;
        UpdateMineCount();
    }

    /*----------------------------------------------------------------------------------------------
     * Name:    UpdateMineCount
     * Type:    Method
     * Purpose: Increment the number of mines left and refresh the display.
     * Input:   int increment = 0, contains the amount to adjust the mine count by.
     * Output:  Nothing.
    ----------------------------------------------------------------------------------------------*/
    public void UpdateMineCount(int increment = 0)
    {
        // Increment the number of mines left.
        minesLeft += increment;

        // Update the display.
        GetComponentInChildren<TextMesh>().text = "Mines Left: " + minesLeft;
    }

    /*----------------------------------------------------------------------------------------------
     * Name:    CountNeighbors
     * Type:    Method
     * Purpose: Counts the number of mines directly next to the given tile.
     * Input:   int row, contains the row of the tile to check.
     * Input:   int column, contains the column of the tile to check.
     * Output:  The number of mines next to this tile.
    ----------------------------------------------------------------------------------------------*/
    int CountNeighbors(int row, int column)
    {
        int count = 0;

        // Run through the possible offsets for this tile.
        for (int offsetIndex = 0; offsetIndex < rowOffsetMapping.Length; offsetIndex++)
        {
            // Set the offset coordinates for this iteration.
            int offsetRow = row + rowOffsetMapping[offsetIndex];
            int offsetColumn = column + columnOffSetMapping[offsetIndex];

            // Check to make sure that the tile coordinates are valid.
            if (offsetRow >= 0 && offsetRow < rows && offsetColumn >= 0 && offsetColumn < columns)
            {
                // Check if the specified tile is a mine.
                if (tileArray[offsetRow, offsetColumn].GetComponent<ClickTile>().isMine)
                {
                    // Increment the count of how many mines are around the specified tile.
                    count++;
                }
            }
        }

        // Return the count of nearby mines.
        return count;
    }

    /*----------------------------------------------------------------------------------------------
     * Name:    OpenAllFree
     * Type:    Method
     * Purpose: Open all tiles in a region, unless already open, flagged, or mined, using implicit
     *          recursion.
     * Input:   int row, contains the row of the tile to open.
     * Input:   int column, contains the column of the tile to open.
     * Output:  Nothing.
    ----------------------------------------------------------------------------------------------*/
    void OpenAllFree(int row, int column)
    {
        // Run through the possible offsets for this tile.
        for (int offsetIndex = 0; offsetIndex < rowOffsetMapping.Length; offsetIndex++)
        {
            // Set the offset coordinates for this iteration.
            int offsetRow = row + rowOffsetMapping[offsetIndex];
            int offsetColumn = column + columnOffSetMapping[offsetIndex];

            // Check to make sure that the tile coordinates are valid.
            if (offsetRow >= 0 && offsetRow < rows && offsetColumn >= 0 && offsetColumn < columns)
            {
                // Check if the specified tile is not open and not flagged.
                if (!tileArray[offsetRow, offsetColumn].GetComponent<ClickTile>().isOpen
                    && !tileArray[offsetRow, offsetColumn].GetComponent<ClickTile>().isFlagged)
                {
                    // Open the tile.
                    tileArray[offsetRow, offsetColumn].GetComponent<ClickTile>().OpenTile();
                }
            }
        }
    }

    /*----------------------------------------------------------------------------------------------
     * Name:    PlaceMines
     * Type:    Method
     * Purpose: Places the mines in the minefield
     * Input:   Nothing.
     * Output:  Nothing.
    ----------------------------------------------------------------------------------------------*/
    void PlaceMines()
    {
        // Initialize local variables.
        int itemsLeftToDistribute = numberMines;
        int poolToDistributeTo = rows * columns;

        // Run through the list of tiles and choose random tiles to mine.
        foreach (var tile in tileArray)
        {
            if (Random.Range(0, poolToDistributeTo--) < itemsLeftToDistribute)
            {
                tile.GetComponent<ClickTile>().isMine = true;
                if (--itemsLeftToDistribute == 0)
                {
                    break;
                }
            }
        }
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
        // Set the click tile reference to the table.
        ClickTile.tableRef = gameObject;

        // Initialize the tile array.
        tileArray = new GameObject[rows, columns];
        for (int rowIndex = 0; rowIndex < rows; rowIndex++)
        {
            for (int columnIndex = 0; columnIndex < columns; columnIndex++)
            {
                // Initialize the actual tile object and record it in the array.
                tileArray[rowIndex, columnIndex] = Instantiate(tileRef) as GameObject;

                // Tell that tile its ID.
                tileArray[rowIndex, columnIndex].GetComponent<ClickTile>().SetId(rowIndex,
                    columnIndex);

                // Move the tile so we get a nice grid.
                tileArray[rowIndex, columnIndex].transform.position = new Vector3(
                    (columnIndex - columns / 2) / 1.5f,
                    (rows / 2 - rowIndex) / 1.5f,
                    0f);
            }
        }

        // Place the mines.
        PlaceMines();

        // Initialize the mine counter.
        minesLeft = numberMines;
        UpdateMineCount();
    }
}
