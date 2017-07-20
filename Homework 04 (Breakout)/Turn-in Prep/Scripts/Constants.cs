/*--------------------------------------------------------------------------------------------------
 * Author:      Dan Cassidy
 * Date:        2015-09-24
 * Assignment:  Homework 4
 * Source File: Constants.cs
 * Language:    C#
 * Course:      CSCI-C 490, Game Programming and Design, TuTh 17:30
 * Purpose:     Holds myriad constants for the game for centralization and ease of access.
--------------------------------------------------------------------------------------------------*/
using UnityEngine;

public class Constants : MonoBehaviour
{
    // Brick strength details.
    public const int BrickStrengthDefault = BrickStrengthMin;
    public const int BrickStrengthMin = 1;
    public const int BrickStrengthMax = 4;

    // Points awarded for breaking bricks and balls left in the reserve.
    public const int PointsPerBrick = 1;
    public const int PointsPerBall = 5;

    // Different strings that are used.
    public const string GameOverLossText = "Game Over";
    public const string GameOverWinText = "Excellent";
    public const string ScoreText = "Score : ";

    // Names of certain GameObjects for use with GameObject.Find().
    public const string EndTextObjectName = "endText";
    public const string ScoreTextObjectName = "scoreText";
    public const string GameControllerObjectName = "Main Camera";
    public const string BallObjectName = "ball";
    public const string NewGameButtonObjectName = "newGameButton";
}
