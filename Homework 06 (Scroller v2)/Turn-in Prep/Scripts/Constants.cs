/*--------------------------------------------------------------------------------------------------
 * Author:      Dan Cassidy
 * Date:        2015-10-07
 * Assignment:  Homework 6
 * Source File: Constants.cs
 * Language:    C#
 * Course:      CSCI-C 490, Game Programming and Design, TuTh 17:30
 * Purpose:     Holds myriad constants for the game for centralization and ease of access.
--------------------------------------------------------------------------------------------------*/
public class Constants
{
    // Tags.
    public const string PlayerTag = "Player";
    public const string GroundTag = "Ground";
    public const string BulletTag = "Bullet";
    public const string CritterTag = "Critter";

    // Audio clip names.
    public const string FireClipName = "fire";

    // Different strings.
    public const string ScoreText = "Score : ";
    public const string LeftText = " Left";
    public const string InfoTextL1 = 
        "Get to 0 points by\n" + 
        "collecting falling\n" +
        "objects.\n" + 
        "\n" + 
        "Helicopters: -5\n" + 
        "Parachutes: +7";
    public const string InfoTextL2 = 
        "Collect 7 parachutes\n" + 
        "while avoiding or\n" + 
        "shooting the rocks.";
    public const string LevelCompleteTextL1 =
        "Level 1 Complete\n" + 
        "Press [enter] to\n" +
        "continue";
    public const string LevelCompleteTextL2 = "Level 2 Complete";
    public const string GameOverText = "Game Over";

    // Various object names.
    public const string GMObjectName = "GM";
    public const string MoonObjectName = "moon";
    public const string NewGameButtonObjectName = "newGameButton";
    public const string ScoreTextObjectName = "scoreText";
    public const string InfoTextObjectName = "infoText";
    public const string FriendsLeftTextObjectName = "friendsLeftText";
    public const string PlayerObjectName = "orbital";
    public const string MainCameraObjectName = "Main Camera";

    // Initial score.
    public const int InitialScoreL1 = 50;
    public const int InitialScoreL2 = 0;
    public const int InitialFriendsLeftL1 = 0;
    public const int InitialFriendsLeftL2 = 7;

    // Number of friends and number of foes to spawn.
    public const int NumFriendsL1 = 5;
    public const int NumFoesL1 = 5;
    public const int NumFriendsL2 = 5;
    public const int NumFoesL2 = 10;

    // X and Y coordinates for critter respawning.
    public const int CritterSpawnXMin = 10;
    public const int CritterSpawnXMax = 10;
    public const int CritterSpawnYMin = 10;
    public const int CritterSpawnYMax = 500;

    // Min and max for the scaling factor for critters.
    public const float ScaleMin = 0.75f;
    public const float ScaleMax = 1.25f;

    // Level names.
    public const string LevelNameL1 = "Level1";
    public const string LevelNameL2 = "Level2";

    // Default health.
    public const int DefaultPlayerHealth = 1;
    public const int DefaultCritterHealth = 0;

    // Bullet position offsets and speeds.
    public const float BulletPositionOffsetHorizontal = -0.03f;
    public const float BulletPositionOffsetVertical = 0.6f;
    public const float BulletSpeedHorizontal = 0f;
    public const float BulletSpeedVertical = 10f;
}
