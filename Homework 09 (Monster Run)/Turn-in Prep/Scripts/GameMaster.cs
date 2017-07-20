/*--------------------------------------------------------------------------------------------------
 * Author:      Dan Cassidy
 * Date:        2015-11-12
 * Assignment:  Homework 9 - Monster Run
 * Source File: GameMaster.cs
 * Language:    C#
 * Course:      CSCI-C 490, Game Programming and Design, TuTh 17:30
 * Purpose:     Controls the flow of the game via the GM object.
--------------------------------------------------------------------------------------------------*/
using UnityEngine;
using UnityEngine.UI;

public class GameMaster : MonoBehaviour
{
	enum State
	{
		idle, rollingToMove, moving, powerUp, monster, rollingToFight, fightMonster, gameOver
	};
	enum Tile
	{
		stone, pumpkin, candy, monster1, monster2, monster3
	};

	// GameObject references for the player, the die, and all the tiles.
	public GameObject player;
	public GameObject die;
	public GameObject[] tiles;

	public Material[] mats;

	// Buttons
	public Button button1;
	public Button button2;
	public Button button3;

	public Text ScoreText;
	public Text PowerText;
	public Text MonsterStrengthText;
	public Text GameOverText;

	public float playerMovementSpeed;

	Tile[] tileType;
	State state;

	int playerPosition = 0;
	int playerTargetPosition = 0;
	int storedPower = 0;
	int fightModifier = 0;
	int monsterStrength = 0;
	int playerScore = 0;

	/*----------------------------------------------------------------------------------------------
     * Name:    Action1
     * Type:    Method
     * Purpose: Handles the clicks from button1.
     * Input:   Nothing.
     * Output:  Nothing.
    ----------------------------------------------------------------------------------------------*/
	public void Action1()
	{
		// FSM implementation.
		switch (state)
		{
			// Handles rolling the die.
			case State.idle:
				die.GetComponent<DieControl>().RollDie();
				ChangeState(State.rollingToMove);
				break;

			// Handles ignoring a power up.
			case State.powerUp:
				ChangeState(State.idle);
				break;

			// Handles running from a monster.
			case State.monster:
				MovePlayer(Constants.RunFromFightPenalty);
				break;

			// Handles resetting the game.
			case State.gameOver:
				Reset();
				ChangeState(State.idle);
				break;
		}
	}

	/*----------------------------------------------------------------------------------------------
     * Name:    Action2
     * Type:    Method
     * Purpose: Handles the clicks from button2.
     * Input:   Nothing.
     * Output:  Nothing.
    ----------------------------------------------------------------------------------------------*/
	public void Action2()
	{
		// FSM implementation.
		switch (state)
		{
			// Handles picking up a power up.
			case State.powerUp:
				switch (tileType[playerPosition])
				{
					case Tile.pumpkin:
						UpdatePower(Constants.PowerupStrength1);
						break;

					case Tile.candy:
						UpdatePower(Constants.PowerupStrength2);
						break;
				}
				ChangeState(State.idle);
				break;

			// Handles fighting a monster without using a power up.
			case State.monster:
				fightModifier = 0;
				die.GetComponent<DieControl>().RollDie();
				ChangeState(State.rollingToFight);
				break;
		}
	}

	/*----------------------------------------------------------------------------------------------
     * Name:    Action3
     * Type:    Method
     * Purpose: Handles the clicks from button3.
     * Input:   Nothing.
     * Output:  Nothing.
    ----------------------------------------------------------------------------------------------*/
	public void Action3()
	{
		// FSM implementation.
		switch (state)
		{
			// Handles fighting a monster using a power up.
			case State.monster:
				fightModifier = storedPower;
				UpdatePower(0);
				die.GetComponent<DieControl>().RollDie();
				ChangeState(State.rollingToFight);
				break;
		}
	}

	/*----------------------------------------------------------------------------------------------
     * Name:    ChangeState
     * Type:    Method
     * Purpose: Handles changing states.
     * Input:   State stateToChangeTo, contains the new State that will be changed to.
     * Output:  Nothing.
    ----------------------------------------------------------------------------------------------*/
	void ChangeState(State stateToChangeTo)
	{
		// There's nothing to do except change the state, but this was made into a method in case
		// there were additional tasks that needed to be completed to facilitate changing states,
		// such as recording the previous state, etc.
		state = stateToChangeTo;
	}

	/*----------------------------------------------------------------------------------------------
     * Name:    LateUpdate
     * Type:    Method
     * Purpose: Handles the late updates of the object.
     * Input:   Nothing.
     * Output:  Nothing.
    ----------------------------------------------------------------------------------------------*/
	void LateUpdate()
	{
		// FSM implementation.
		switch (state)
		{
			// Checks the die velocity while rolling to move and if it is below a certain threshold,
			// move the player.
			case State.rollingToMove:
				if (die.GetComponent<Rigidbody>().velocity.magnitude < Constants.DieVelocityMagnitudeLowerLimit)
					MovePlayer(die.GetComponent<DieControl>().topNumber);
				break;

			// Checks the die velocity while rolling to fight a monster, and if it is below a
			// certain threshold, changes the state.
			case State.rollingToFight:
				if (die.GetComponent<Rigidbody>().velocity.magnitude < Constants.DieVelocityMagnitudeLowerLimit)
					ChangeState(State.fightMonster);
				break;

			// Handles the smooth movement of the player from tile A to tile B on the board.
			case State.moving:
				// Check if the player is at the tile's center position.
				if (player.transform.position.x == tiles[playerPosition].transform.position.x &&
					player.transform.position.z == tiles[playerPosition].transform.position.z)
				{
					// If the player has not reached the final destination, continue to move.
					if (playerPosition > playerTargetPosition)
						--playerPosition;
					else if (playerPosition < playerTargetPosition)
						++playerPosition;
					else
					// Player has reached its final destination.
					{
						if (playerPosition == tiles.Length - 1)
						{
							// If the player is on the final tile, the game is over.
							GameOverText.gameObject.SetActive(true);
							ChangeState(State.gameOver);
						}
						else
							// Otherwise, decide what to do based upon which tile type the player
							// has landed on.
							switch (tileType[playerPosition])
							{
								case Tile.stone:
									ChangeState(State.idle);
									break;

								case Tile.pumpkin:
								case Tile.candy:
									ChangeState(State.powerUp);
									break;

								case Tile.monster1:
								case Tile.monster2:
								case Tile.monster3:
									ChangeState(State.monster);
									break;
							}
					}
				}
				break;
		}
	}

	/*----------------------------------------------------------------------------------------------
     * Name:    MovePlayer
     * Type:    Method
     * Purpose: Handles setting a movement target position for the player.
     * Input:   int offset, contains the requested offset from the player's current position.
     * Output:  Nothing.
    ----------------------------------------------------------------------------------------------*/
	void MovePlayer(int offset)
	{
		// Check if the requested offset is possible.
		if (playerTargetPosition + offset < 0)
			// Offset would put player earlier than the starting tile, so set it to 0 instead.
			playerTargetPosition = 0;
		else if (playerTargetPosition + offset >= tiles.Length)
			// Offset would put player further than the ending tile, so set it to the max instead.
			playerTargetPosition = tiles.Length - 1;
		else
			// No issues with the requested offset.
			playerTargetPosition += offset;

		ChangeState(State.moving);
	}

	/*----------------------------------------------------------------------------------------------
     * Name:    PlacePlayer
     * Type:    Method
     * Purpose: Instantaneously places the player on a tile given by the index in the tiles array.
	 *			Only uses the x
	 *			and z coordinates of the tile.
     * Input:	int place, contains the index (in the tiles array) of the tile to move to.
     * Output:  Nothing.
    ----------------------------------------------------------------------------------------------*/
	void PlacePlayer(int place)
	{
		// Directly manipulate the player game object.
		player.transform.position = new Vector3(tiles[place].transform.position.x,
												player.transform.position.y,
												tiles[place].transform.position.z);
	}

	/*----------------------------------------------------------------------------------------------
     * Name:    Reset
     * Type:    Method
     * Purpose: Resets the game.
     * Input:   Nothing.
     * Output:  Nothing.
    ----------------------------------------------------------------------------------------------*/
	void Reset()
	{
		// Disable the game over text.
		GameOverText.gameObject.SetActive(false);

		// Reset the incremental movement variables.
		playerPosition = 0;
		playerTargetPosition = 0;

		// Move the player back to the beginning of the path.
		PlacePlayer(0);

		// Zero out the score.
		UpdateScore(-playerScore);

		// Zero out the stored power.
		UpdatePower(0);
	}

	/*----------------------------------------------------------------------------------------------
     * Name:    SetButtons
     * Type:    Method
     * Purpose: Sets up the UI buttons.
     * Input:   Nothing.
     * Output:  Nothing.
    ----------------------------------------------------------------------------------------------*/
	void SetButtons()
	{
		// FSM implementation.
		switch (state)
		{
			// Idle state, show only the roll button.
			case State.idle:
				button1.gameObject.SetActive(true);
				button2.gameObject.SetActive(false);
				button3.gameObject.SetActive(false);

				button1.GetComponentInChildren<Text>().text = Constants.ButtonTextRoll;
				break;

			// Show buttons relating to the powerUp state.
			case State.powerUp:
				button1.gameObject.SetActive(true);
				button2.gameObject.SetActive(true);
				button3.gameObject.SetActive(false);

				button1.GetComponentInChildren<Text>().text = Constants.ButtonTextPowerUpIgnore;
				button2.GetComponentInChildren<Text>().text = Constants.ButtonTextPowerUpTake;
				break;

			// Show buttons relating to the monster state.
			case State.monster:
				button1.gameObject.SetActive(true);
				button2.gameObject.SetActive(true);
				button3.gameObject.SetActive(storedPower > 0 ? true : false);

				button1.GetComponentInChildren<Text>().text = Constants.ButtonTextMonsterRun;
				button2.GetComponentInChildren<Text>().text = Constants.ButtonTextMonsterFight;
				if (button3.IsActive())
					button3.GetComponentInChildren<Text>().text = Constants.ButtonTextMonsterFightPowerUp;
				break;

			// Game is over, show the reset button.
			case State.gameOver:
				button1.gameObject.SetActive(true);
				button2.gameObject.SetActive(false);
				button3.gameObject.SetActive(false);

				button1.GetComponentInChildren<Text>().text = Constants.ButtonTextReset;
				break;
		}
	}

	/*----------------------------------------------------------------------------------------------
     * Name:    Start
     * Type:    Method
     * Purpose: Used for one-time initialization instructions.
     * Input:   Nothing.
     * Output:  Nothing.
    ----------------------------------------------------------------------------------------------*/
	void Start()
	{
		int i, j;
		tileType = new Tile[tiles.Length];

		for (i = 0; i < tiles.Length; i++)
		{
			// use the name of the tiles to populate the array
			tiles[i] = GameObject.Find("tile" + i);
			if (tiles[i])
			{
				// use the material's name to set the type
				Material mat = tiles[i].GetComponent<Renderer>().material;
				tileType[i] = Tile.stone;
				for (j = 0; j < mats.Length; j++)
					if (mat.name.IndexOf(mats[j].name) >= 0)
						tileType[i] = (Tile)j;
			}
		}

		// Utilize the Reset() method to handle setting defaults.
		Reset();
	}

	/*----------------------------------------------------------------------------------------------
     * Name:    Update
     * Type:    Method
     * Purpose: Called once per frame, and is used to implement button changes, moving, and
	 *			for resolving a fight.
     * Input:   Nothing.
     * Output:  Nothing.
    ----------------------------------------------------------------------------------------------*/
	void Update()
	{
		// Set buttons.
		SetButtons();

		// Update the monster strength if the player is on a monster tile.
		UpdateMonsterStrength();

		// FSM implementation.
		switch (state)
		{
			// Handle the actual incremental movement from a tile to one of its neighbors.
			case State.moving:
				// Determine the step size of the movement, based upon the time between updates and
				// the chosen speed.
				float step = playerMovementSpeed * Time.deltaTime;

				// Utilize the Vector3.MoveTowards() method to handle the movement.
				player.transform.position = Vector3.MoveTowards(
					player.transform.position,
					new Vector3(
						tiles[playerPosition].transform.position.x,
						player.transform.position.y,
						tiles[playerPosition].transform.position.z),
					step);
				break;

			// Resolve a fight between the player and a monster.
			case State.fightMonster:
				// Calculate the player's roll as the die roll plus the fight modifier.
				int playerRoll = die.GetComponent<DieControl>().topNumber + fightModifier;

				// Determine the outcome of the fight.
				if (playerRoll >= monsterStrength)
				{
					// Player beat the monster, so add the difference to the score.
					UpdateScore(playerRoll - monsterStrength);

					// Fight is resolved, change the state back to idle to wait for user input.
					ChangeState(State.idle);
				}
				else
					// Player lost to the monster, so send them back by the difference.
					MovePlayer(playerRoll - monsterStrength);
				break;
		}
	}

	/*----------------------------------------------------------------------------------------------
     * Name:    UpdateMonsterStrength
     * Type:    Method
     * Purpose: Shows or hides the monster strength as appropriate.
     * Input:   Nothing.
     * Output:  Nothing.
    ----------------------------------------------------------------------------------------------*/
	void UpdateMonsterStrength()
	{
		// FSM implementation.
		switch (state)
		{
			// Updates the monster strength when the in the appropriate state.
			case State.monster:
			case State.rollingToFight:
			case State.fightMonster:
				// Set the appropriate monster strength
				switch (tileType[playerPosition])
				{
					case Tile.monster1:
						monsterStrength = Constants.MonsterStrength1;
						break;

					case Tile.monster2:
						monsterStrength = Constants.MonsterStrength2;
						break;

					case Tile.monster3:
						monsterStrength = Constants.MonsterStrength3;
						break;
				}
				// Display the monster strength.
				MonsterStrengthText.gameObject.SetActive(true);
				MonsterStrengthText.text = Constants.MonsterStrengthText + monsterStrength.ToString();
				break;

			default:
				// Hide the monster strength when not applicable.
				MonsterStrengthText.gameObject.SetActive(false);
				break;
		}
	}

	/*----------------------------------------------------------------------------------------------
     * Name:    UpdatePower
     * Type:    Method
     * Purpose: Sets the player's stored power, and then updates the power display.
     * Input:   int newPowerValue, contains the new power value to store.
     * Output:  Nothing.
    ----------------------------------------------------------------------------------------------*/
	void UpdatePower(int newPowerValue)
	{
		// Set the stored power to the desired number.
		storedPower = newPowerValue;

		// Update the power in the UI.
		PowerText.text = Constants.PowerText + storedPower.ToString();
	}

	/*----------------------------------------------------------------------------------------------
     * Name:    UpdateScore
     * Type:    Method
     * Purpose: Adds a number to the score, and then updates the score display.
     * Input:   int offset, contains the number of points to add to the score.
     * Output:  Nothing.
    ----------------------------------------------------------------------------------------------*/
	void UpdateScore(int offset)
	{
		// Add the specified number to the score.
		playerScore += offset;

		// Update the score in the UI.
		ScoreText.text = Constants.ScoreText + playerScore.ToString();
	}
}
