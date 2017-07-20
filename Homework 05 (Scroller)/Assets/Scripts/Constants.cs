/*--------------------------------------------------------------------------------------------------
 * Author:      Dan Cassidy
 * Date:        2015-10-01
 * Assignment:  Homework 5
 * Source File: Constants.cs
 * Language:    C#
 * Course:      CSCI-C 490, Game Programming and Design, TuTh 17:30
 * Purpose:     Holds myriad constants for the game for centralization and ease of access.
--------------------------------------------------------------------------------------------------*/
using UnityEngine;

public class Constants : MonoBehaviour
{
    // Tags.
    public const string PlayerTag = "Player";
    public const string GroundTag = "Ground";

    // Score text.
    public const string ScoreText = "Score : ";

    // Various object names.
    public const string GMObjectName = "GM";
    public const string MoonObjectName = "moon";
    public const string NewGameButtonObjectName = "newGameButton";
    public const string ScoreTextObjectName = "scoreText";

    // Initial score.
    public const int InitialScore = 50;

    // Number of friends and number of foes to spawn.
    public const int NumFriends = 5;
    public const int NumFoes = 5;

    // X and Y coordinates for critter respawning.
    public const int CritterSpawnXMin = 10;
    public const int CritterSpawnXMax = 10;
    public const int CritterSpawnYMin = 10;
    public const int CritterSpawnYMax = 500;

    // Min and max for the scaling factor for critters.
    public const float ScaleMin = 0.75f;
    public const float ScaleMax = 1.25f;
}
