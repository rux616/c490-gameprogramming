/*--------------------------------------------------------------------------------------------------
 * Author:      Dan Cassidy
 * Date:        2015-09-10
 * Assignment:  Homework 2
 * Source File: Constants.cs
 * Language:    C#
 * Course:      CSCI-C 490, Game Programming and Design, TuTh 17:30
 * Purpose:     Contains all the constants in the game for easy management.
 * Note:        The constants utilize Pascal casing instead of all caps as Microsoft themselves use
 *              this naming convention in the .NET Framework class library. (See the System.Int32
 *              class as an example, specifically the MaxValue and MinValue fields.)
--------------------------------------------------------------------------------------------------*/
public static class Constants {
    // The number of tiles on the game board.
    public const int NumTiles = 8;

    // The maximum number of tiles allowed to be open at once.
    public const int MaxTilesOpen = 3;

    // ID of an invalid tile, meaning one that has not been chosen.
    public const int InvalidTile = -1;

    // Number of frames before all tiles close.  (Actual length in seconds unknown.)
    public const int TimeBeforeClose = 100;

    // Lower and upper limits of the randomly generated numbers.
    public const int MyNumberLowerLimit = -9;
    public const int MyNumberUpperLimit = 9;
}
