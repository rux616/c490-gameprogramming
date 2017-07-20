/*--------------------------------------------------------------------------------------------------
 * Author:      Dan Cassidy
 * Date:        2015-09-08
 * Assignment:  Homework 2
 * Source File: FrameManager.cs
 * Language:    C#
 * Course:      CSCI-C 490, Game Programming and Design, TuTh 17:30
 * Purpose:     Is the main script for the game.  Manages the tiles and checks for winning
 *              conditions.
--------------------------------------------------------------------------------------------------*/
using UnityEngine;
using System.Collections;

public class FrameManager : MonoBehaviour {
    static public GameObject[] tileArray = new GameObject[Constants.NumTiles];
    public GameObject winMessage;

    private bool gameStarted = false;

    private int countOpen = 0;
    private int sum = 0;
    private int[] chosenTile = new int[Constants.MaxTilesOpen];

    private int timeLeftToClose = Constants.TimeBeforeClose;

    /*----------------------------------------------------------------------------------------------
     * Name:    IsPlayable
     * Type:    Method
     * Purpose: Checks the tile ID passed as a parameter to see if it is a playable tile.
     * Input:   int id, contains the ID number of the tile to check for playability.
     * Output:  bool, representing whether the tile ID is playable (true) or not (false).
    ----------------------------------------------------------------------------------------------*/
    public bool IsPlayable(int id) {
        // Check to make sure there are available spots to open a tile.
        if (countOpen == Constants.MaxTilesOpen) {
            return false;
        }

        // Check to make sure the tile is not already open.
        foreach (int tile in chosenTile) {
            if (tile == id) {
                return false;
            }
        }

        // If the other checks passed and the method has reached this point, the tile is playable.
        return true;
    }

    /*----------------------------------------------------------------------------------------------
     * Name:    PlayTile
     * Type:    Method
     * Purpose: Play a tile. Records that a tile has been played, adds its number to the current
     *          sum, sets (or resets) the timer, and increments the number of open tiles.
     * Input:   int id, contains the ID of the tile to play.
     * Input:   int number, contains the number value of the tile to play.
     * Output:  Nothing.
    ----------------------------------------------------------------------------------------------*/
    public void PlayTile(int id, int number) {
        // Run through the list of chosen tiles to find the first available spot.
        for (int index = 0; index < chosenTile.Length; index++) {
            if (chosenTile[index] == Constants.InvalidTile) {
                // Assign that spot to the ID, add the number to the sum, start the timer, and 
                // increment the number of open tiles.
                chosenTile[index] = id;
                sum += number;
                countOpen++;
                timeLeftToClose = Constants.TimeBeforeClose;
                break;
            }
        }
    }

    /*----------------------------------------------------------------------------------------------
     * Name:    UnplayTile
     * Type:    Method
     * Purpose: "Unplays" the tile with the given number, subtracts that tiles numeric value from
     *          the current sum, and decrements the number of open tiles.  Does *not* reset the
     *          timer.
     * Input:   int id, contains the ID of the tile to unplay.
     * Input:   int number, contains the number value of the tile to unplay.
     * Output:  Nothing.
    ----------------------------------------------------------------------------------------------*/
    public void UnplayTile(int id, int number) {
        // Run through the list of chosen tiles to find the correct one.
        for (int index = 0; index < chosenTile.Length; index++) {
            if (chosenTile[index] == id) {
                // Invalidate the index, subtract the number from the sum, and decrement the number
                // of open tiles.
                chosenTile[index] = Constants.InvalidTile;
                sum -= number;
                countOpen--;
                break;
            }
        }
    }

    /*----------------------------------------------------------------------------------------------
     * Name:    CheckWin
     * Type:    Method
     * Purpose: Checks if a winning condition exists on the game board.
     * Input:   Nothing.
     * Output:  bool, representing whether the a winning condition exists (true) or not (false).
    ----------------------------------------------------------------------------------------------*/
    bool CheckWin() {
        // Check if the correct number of tiles are open and whether the sum is correct.
        if (countOpen == Constants.MaxTilesOpen && sum == 0) {
            return true;
        }
        else {
            return false;
        }
    }

    /*----------------------------------------------------------------------------------------------
     * Name:    CloseOpenTiles
     * Type:    Method
     * Purpose: Closes all the currently open tiles.
     * Input:   Nothing.
     * Output:  Nothing.
    ----------------------------------------------------------------------------------------------*/
    void CloseOpenTiles() {
        // Run through the open tiles.
        for (int index = 0; index < chosenTile.Length; index++) {
            // Check to see if the tile at a specific index is valid.
            if (chosenTile[index] != Constants.InvalidTile) {
                // If it is, close that tile and invalidate that index.
                tileArray[chosenTile[index]].GetComponent<ClickTile>().CloseTile();
                chosenTile[index] = Constants.InvalidTile;
            }
        }
    }

    /*----------------------------------------------------------------------------------------------
     * Name:    GameWon
     * Type:    Method
     * Purpose: Ends the game by deactivating all the tiles and the frame.  Also activates the
     *          winning text.
     * Input:   Nothing.
     * Output:  Nothing.
    ----------------------------------------------------------------------------------------------*/
    void GameWon() {
        // Deactivate all the tiles.
        foreach (var tile in tileArray) {
            tile.SetActive(false);
        }

        // Deactivate the frame.
        gameObject.SetActive(false);

        // Activate the win message.
        winMessage.SetActive(true);
    }

    /*----------------------------------------------------------------------------------------------
     * Name:    MakeSolution
     * Type:    Method
     * Purpose: A solution set may or may not already exist within the tiles, so this method creates
     *          one so there is a guaranteed solution to the puzzle.
     * Input:   Nothing.
     * Output:  Nothing.
    ----------------------------------------------------------------------------------------------*/
    void MakeSolution() {
        int tile1, tile2, tile3;

        // Choose random tiles that will contain a guaranteed solution and make sure none of them
        // match each other.
        tile1 = Random.Range(0, Constants.NumTiles);
        do {
            tile2 = Random.Range(0, Constants.NumTiles);
        } while (tile2 == tile1);
        do {
            tile3 = Random.Range(0, Constants.NumTiles);
        } while (tile3 == tile1 || tile2 == tile3);

        // Get the numbers the chosen tiles contain.
        int num1 = tileArray[tile1].GetComponent<ClickTile>().myNumber;
        int num2 = tileArray[tile2].GetComponent<ClickTile>().myNumber;

        // Make sure we don't end up with a number larger than the upper limit.
        if (num1 + num2 > Constants.MyNumberUpperLimit) {
            num1 -= num1 + num2 - Constants.MyNumberUpperLimit;
            tileArray[tile1].GetComponent<ClickTile>().myNumber = num1;
        }

        // Make sure we don't end up with a number smaller than the lower limit.
        if (num1 + num2 < Constants.MyNumberLowerLimit) {
            num1 -= num1 + num2 - Constants.MyNumberLowerLimit;
            tileArray[tile1].GetComponent<ClickTile>().myNumber = num1;
        }

        tileArray[tile3].GetComponent<ClickTile>().myNumber = -(num1 + num2);
    }

    /*----------------------------------------------------------------------------------------------
     * Name:    Start
     * Type:    Method
     * Purpose: Performs some basic initialization.
     * Input:   Nothing.
     * Output:  Nothing.
    ----------------------------------------------------------------------------------------------*/
    void Start() {
        // Set the ClickTile frame manager reference to this script's game object.
        ClickTile.frameRef = gameObject;

        // Initialize the chosenTile array in a specific-array-length-independent way.
        for (int index = 0; index < chosenTile.Length; index++) {
            chosenTile[index] = Constants.InvalidTile;
        }
    }

    /*----------------------------------------------------------------------------------------------
     * Name:    Update
     * Type:    Method
     * Purpose: Called once per frame.  For this game, can be considered the main game loop.
     * Input:   Nothing.
     * Output:  Nothing.
    ----------------------------------------------------------------------------------------------*/
    void Update() {
        // Check if the game has been started yet.
        if (!gameStarted) {
            // If the game has not been started, ensure a solution exists, then mark the game as
            // started.
            MakeSolution();
            gameStarted = true;
        }
        // Check if there are any open tiles.
        else if (countOpen != 0) {
            // If any tiles are open, check if there is any time left before the open tiles get
            // closed.
            if (timeLeftToClose != 0) {
                // Time is left, so decrement the timer.
                timeLeftToClose--;
            }
            else {
                // The timer has run out so check the game to see if it is in a winning condition.
                if (CheckWin()) {
                    // If the game is in a winning condition, make it known.
                    GameWon();
                }
                else {
                    // The game is not in a winning condition, so close any open tiles.
                    CloseOpenTiles();
                }
            }
        }
    }
}
