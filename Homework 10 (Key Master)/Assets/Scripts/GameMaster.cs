using UnityEngine;
using System.Collections.Generic;

public class GameMaster : MonoBehaviour
{
	// an enum to hold the type of objects we can have in the maze
	public enum CellType
	{
		space, wall, blueKey, blueDoor, redKey, redDoor,
		yellowKey, yellowDoor, greenKey, greenDoor, purpleKey, purpleDoor
	};

	// A Point struct to be stored in linked lists by Prim's algorithm.
	public struct Point
	{
		public int x, y;
		public Point(int vx, int vy) : this()
		{ this.x = vx; this.y = vy; }
	};

	public CellType[,] maze;
	public GameObject[,] bricks;
	public CellType[,] mazeRC;
	public GameObject[,] bricksRC;

	public int mazeWidth, mazeHeight;
	static int offsetX = 2, offsetY = 2;
	static float tileSize = 0.6f;
	int doorsLeft;
	//public static int level = 1;
	public static int level = 5;

	public GameObject brickRef;
	public GameObject[] placeables;

	// Create a maze and the 3D objects composing it
	void MakeMaze(int cols, int rows)
	{
		mazeWidth = cols;
		mazeHeight = rows;

		// set the offsets so that (0, 0) is in the center of the maze
		offsetX = mazeWidth / 2;
		offsetY = mazeHeight / 2;

		// allocate the array
		maze = new CellType[mazeWidth, mazeHeight];

		// create the CellType maze
		Prim();
		// create the maze of wall 3D objects
		BuildMaze();
	}

	// Initialize all the cells in the maze with the same value
	void InitMaze(CellType val)
	{
		for (int i = 0; i < mazeWidth; i++)
			for (int j = 0; j < mazeHeight; j++)
				maze[i, j] = val;
	}

	// returns the cells neighboring the given one in a list
	// considers a 4-cell neighborhood.
	// returns less than 4 nodes if close to a border
	LinkedList<Point> Neighbors(int cx, int cy)
	{
		LinkedList<Point> neighbs = new LinkedList<Point>();
		if (cx > 1)
		{
			neighbs.AddFirst(new Point(cx - 1, cy));
		}
		if (cx < mazeWidth - 2)
		{
			neighbs.AddLast(new Point(cx + 1, cy));
		}
		if (cy > 1)
		{
			neighbs.AddLast(new Point(cx, cy - 1));
		}
		if (cy < mazeHeight - 2)
		{
			neighbs.AddLast(new Point(cx, cy + 1));
		}
		return neighbs;
	}

	// converts a column number into a position on the stage
	static public float Col2X(int c)
	{
		return c * tileSize - offsetX * tileSize;
	}

	// converts a column number into a position on the stage
	static public float Row2Y(int r)
	{
		return r * tileSize - offsetY * tileSize;
	}

	// converts an x position to a column number
	static public int X2col(float xVal)
	{
		return offsetX + (int)(xVal / tileSize);
	}

	// converts a y position to a row number
	static public int Y2row(float yVal)
	{
		return offsetY + (int)(yVal / tileSize);
	}

	// builds the physical maze out of wall objects, 
	// assuming that we have generated it beforehand in the maze array.
	void BuildMaze()
	{
		int i, j;
		float brickWidth, brickY;
		brickY = brickRef.transform.position.y;
		brickWidth = brickRef.transform.localScale.x;
		tileSize = brickWidth + 0.001f; // a small gap between the bricks to be able to see them

		bricks = new GameObject[mazeWidth, mazeHeight];
		for (i = 0; i < mazeWidth; i++)
			for (j = 0; j < mazeHeight; j++)
			{
				if (maze[i, j] == CellType.wall) // place a brick where we have a wall in the maze
				{
					bricks[i, j] = Instantiate(brickRef) as GameObject;
					bricks[i, j].transform.position = new Vector3(Col2X(i), brickY, Row2Y(j));
				}
				else
				{
					bricks[i, j] = null;
				}
			}
	}

	// implementation of Prim's algorithm to generate a maze
	void Prim()
	{
		LinkedListNode<Point> node;
		int cx, cy, dx, dy, r;
		LinkedList<Point> neighbs;

		// start by filling up the maze with walls
		InitMaze(CellType.wall);

		// start from the center of the maze with a space
		// that's where the player should be
		int startx = mazeWidth / 2, starty = mazeHeight / 2;

		// the frontier contains all the neighbors of the starting position
		LinkedList<Point> frontier = Neighbors(startx, starty);
		maze[startx, starty] = CellType.space;

		while (frontier.Count > 0)               // while we still have nodes in the frontier
		{
			r = Random.Range(0, frontier.Count); // choose a random one
			node = NodeAtIndex(frontier, r);
			cx = node.Value.x;
			cy = node.Value.y;
			frontier.Remove(node);               // remove it from the frontier
			neighbs = Neighbors(cx, cy);
			if (CountSpaces(neighbs) == 1)       // make sure it doesn't close a cycle
			{
				maze[cx, cy] = CellType.space;   // make it a space

				for (node = neighbs.First; node != null; node = node.Next) // process its neighbors
				{
					dx = node.Value.x;
					dy = node.Value.y;
					// if the neighbor is not a space
					if (maze[dx, dy] != CellType.space)
					{
						LinkedList<Point> n = Neighbors(dx, dy);
						// if it has exactly one space among its neighbors and is not already in the frontier
						if (CountSpaces(n) == 1 && !ContainsNode(frontier, dx, dy))
						{
							//add this neighbor to the frontier;
							frontier.AddLast(new Point(dx, dy));
						}
					}
				}
			}
		}
	}

	// returns the node at a given index from the list
	LinkedListNode<Point> NodeAtIndex(LinkedList<Point> list, int index)
	{
		LinkedListNode<Point> n = list.First;
		int i = 0;
		while (n != null && i < index)
		{
			n = n.Next;
			++i;
		}
		return n;
	}

	// counts the spaces in the list
	int CountSpaces(LinkedList<Point> neighb)
	{
		int count = 0;
		for (LinkedListNode<Point> n = neighb.First; n != null; n = n.Next)
		{
			if (maze[n.Value.x, n.Value.y] == CellType.space)
			{
				count++;
			}
		}
		return count;
	}

	// checks if the list contains a node given with x and y
	bool ContainsNode(LinkedList<Point> list, int cx, int cy)
	{
		LinkedListNode<Point> p = list.First;
		while (p != null)
		{
			if (p.Value.x == cx && p.Value.y == cy)
			{
				return true;
			}
			p = p.Next;
		}
		return false;
	}

	/*----------------------------------------------------------------------------------------------
     * Name:    Start
     * Type:    Method
     * Purpose: Used for one-time initialization statements.
     * Input:   Nothing.
     * Output:  Nothing.
    ----------------------------------------------------------------------------------------------*/
	void Start()
	{
		// Deactivate all the placeable game objects.
		foreach (GameObject obj in placeables)
			obj.SetActive(false);

		// Set the level size and then use that number to create the maze.
		int levelSize = 5 + level * 3;
		MakeMaze(levelSize, levelSize);

		// Set the number of doors in the level.
		doorsLeft = level;

		// Populate the maze with the number of doors.  (Up to 5.)
		PopulateMaze(doorsLeft);
	}

	/*----------------------------------------------------------------------------------------------
     * Name:    PopulateMaze
     * Type:    Method
     * Purpose: Places the appropriate number of door/key pairs into the maze.
     * Input:   int numDoors, contains the number of door/key pairs to place into the maze.  Must be
	 *			between 1 (inclusive) and 5 (inclusive).
     * Output:  Nothing.
    ----------------------------------------------------------------------------------------------*/
	void PopulateMaze(int numDoors)
	{
		// Declare variables to keep track of the number of objects that need to be placed and that
		// have been placed.
		int numObjects = 2 * numDoors;
		int placedObjects = 0;

		// Declare various variables here so that they may be used inside the while loop without
		// being reinitialized.
		LinkedList<Point> path;
		Point origin = new Point(offsetY, offsetX);
		int lowerBoundary;
		Point point;
		LinkedListNode<Point> node;
		Point offsetPoint;
		float posOffX;
		float posOffY;

		// Loop while there are still objects to be placed.
		while (placedObjects < numObjects)
		{
			// Walk the maze to get a path to place an object and ensure the path is long enough.
			while ((path = WalkMaze(origin)).Count < numObjects - placedObjects)
			{
				// Nothing to do, everything is taken care of in the while statement.
			}

			// Create lower boundary for the random placement of the object.
			lowerBoundary = path.Count - path.Count / (numObjects - placedObjects);

			// Make sure the lower boundary doesn't exceed maximal value of 1 less than the number
			// of points in the path.
			if (lowerBoundary == path.Count)
				--lowerBoundary;

			// Get a random point on the path to place object.
			node = NodeAtIndex(path, Random.Range(lowerBoundary, path.Count));
			point = node.Value;

			// Check if the object to place is a door.
			if (placeables[placedObjects].CompareTag("door") == true)
			{
				// If the object is a door, check the previous point to make sure it is valid.
				if (node.Previous != null)
					// If there is a previous point, set it as the offset point.
					offsetPoint = node.Previous.Value;
				else
					// If there is no previous point in the linked list, then the player's starting
					// position is the offset point.
					offsetPoint = new Point(origin.x, origin.y);

				// Compute the X and Y positional offsets to get the door very close to the edge of
				// its occupied tile.
				posOffX = (Col2X(point.x) - Col2X(offsetPoint.x)) / 2.3f;
				posOffY = (Row2Y(point.y) - Row2Y(offsetPoint.y)) / 2.3f;

				// Check if the X positional offset is not 0.
				if (posOffX != 0)
					// If the X positional offset is not 0, then the door should be rotated 90
					// degrees around the Y axis so that it seals the entrance to its tile from the left or right.
					placeables[placedObjects].transform.Rotate(new Vector3(0.0f, 90.0f, 0.0f));
				else
					// Otherwise, rotate it back to 0, so the door seals the entrance to its tile
					// from the top or the bottom.
					placeables[placedObjects].transform.Rotate(new Vector3(0.0f, 0.0f, 0.0f));
			}
			else
			{
				// The object is not a door, so no offset or rotation is needed.
				posOffX = 0;
				posOffY = 0;
			}

			// Place the object and set it active.
			placeables[placedObjects].transform.position = new Vector3(Col2X(point.x) - posOffX, placeables[placedObjects].transform.position.y, Row2Y(point.y) - posOffY);
			placeables[placedObjects].SetActive(true);

			// Update the maze matrix with the type of placed object.
			maze[point.x, point.y] = NumberToCellType(placedObjects);

			// Increment the number of placed objects.
			++placedObjects;
		}
	}

	/*----------------------------------------------------------------------------------------------
     * Name:    WalkingNeighbors
     * Type:    Method
     * Purpose: Returns a linked list of neighboring points that are spaces (and therefore are able
	 *			to be walked to).
     * Input:   Point origin, contains the point that is checked for space-type neighbors.
     * Output:  LinkedList<Point>, the linked list of any points found.
    ----------------------------------------------------------------------------------------------*/
	LinkedList<Point> WalkingNeighbors(Point origin)
	{
		// Create a new linked list object to hold the neighbors.
		LinkedList<Point> neighbors = new LinkedList<Point>();

		// Examine each neighbor.
		foreach (Point neighbor in Neighbors(origin.x, origin.y))
			// Check each neighbor to see if it is a space.
			if (maze[neighbor.x, neighbor.y] == CellType.space)
				// If the neighbor is a space, add it to the neighbor list.
				neighbors.AddLast(neighbor);

		// Return the list of neighbors that are spaces.
		return neighbors;
	}

	/*----------------------------------------------------------------------------------------------
     * Name:    ValidWalkingNeighbors
     * Type:    Method
     * Purpose: Generates a linked list of valid neighboring points that can be walked to (i.e. they
	 *			are "space" type cells).  Note that valid in this case includes the removal of the
	 *			previous walked point.
     * Input:   Point origin, contains the point that is checked for valid space-type neighbors.
	 * Input:	Point previousPoint, contains the previously-walked point.
     * Output:  LinkedList<Point>, the linked list of any points found.
    ----------------------------------------------------------------------------------------------*/
	LinkedList<Point> ValidWalkingNeighbors(Point origin, Point previousPoint)
	{
		// Get all the walking neighbors for this cell (i.e. they are spaces).
		LinkedList<Point> walkingNeighbors = WalkingNeighbors(origin);

		// Remove the previously walked point as it is an invalid candidate.
		walkingNeighbors.Remove(previousPoint);

		// Return the list of neighbors that are spaces are not the origin point.
		return walkingNeighbors;
	}

	/*----------------------------------------------------------------------------------------------
     * Name:    Update
     * Type:    Method
     * Purpose: Handles any per-frame updates.
     * Input:   Nothing.
     * Output:  Nothing.
    ----------------------------------------------------------------------------------------------*/
	void Update()
	{
		// Check if there are any doors left to unlock.
		if (doorsLeft == 0)
		{
			// If not, then check the level.
			if (level < 5)
			{
				// If the level is less than 5, then increment the level and load the next level.
				++level;
				Application.LoadLevel("maze");
			}
			else
			{
				// The last level has been finished, so load the end scene.
				Application.LoadLevel("end");
			}
		}
	}

	/*----------------------------------------------------------------------------------------------
     * Name:    WalkMaze
     * Type:    Method
     * Purpose: Walks the maze, creating a contiguous path from the origin, but not including the
	 *			origin.
     * Input:   Point origin, contains the origin point from where the path starts.
     * Output:  LinkedList<Point>, the linked list containing the walked path.
    ----------------------------------------------------------------------------------------------*/
	LinkedList<Point> WalkMaze(Point origin)
	{
		// Create a linked list object to hold the walked path.
		LinkedList<Point> path = new LinkedList<Point>();

		// Add the origin point to the path.
		path.AddLast(origin);

		// Get the initial list of neighbors.
		LinkedList<Point> validWalkingNeighbors = WalkingNeighbors(path.First.Value);

		// Randomly decide on the initial direction.
		int nextPointIndex = Random.Range(0, validWalkingNeighbors.Count);

		// Add that initial point to the walked path.
		path.AddLast(NodeAtIndex(validWalkingNeighbors, nextPointIndex).Value);

		// Get the next valid walking neighbor and check to make sure there IS a next valid walking
		// neighbor.
		while ((validWalkingNeighbors = ValidWalkingNeighbors(path.Last.Value,
			path.Last.Previous.Value)).Count != 0)
		{
			// Randomly decide on a direction.
			nextPointIndex = Random.Range(0, validWalkingNeighbors.Count);

			// Add that point to the walked path.
			path.AddLast(NodeAtIndex(validWalkingNeighbors, nextPointIndex).Value);
		}

		// Remove the origin point.
		path.RemoveFirst();

		// Return the path.
		return path;
	}

	/*----------------------------------------------------------------------------------------------
     * Name:    NumberToCellType
     * Type:    Method
     * Purpose: Converts a number to a CellType.  This is basically a cheap way to get the
	 *			appropriate cell type for a specific colored door or key.
     * Input:   int number, contains the number that will be converted to the CellType.
     * Output:  CellType, the cell type that the number converted to.
    ----------------------------------------------------------------------------------------------*/
	CellType NumberToCellType(int number)
	{
		// blue, red, yellow, green, purple
		switch (number)
		{
			case 0:
				// Blue door.
				return CellType.blueDoor;

			case 1:
				// Blue key.
				return CellType.blueKey;

			case 2:
				// Red door.
				return CellType.redDoor;

			case 3:
				// Red key.
				return CellType.redKey;

			case 4:
				// Yellow door.
				return CellType.yellowDoor;

			case 5:
				// Yellow key.
				return CellType.yellowKey;

			case 6:
				// Green door.
				return CellType.greenDoor;

			case 7:
				// Green key.
				return CellType.greenKey;

			case 8:
				// Purple door.
				return CellType.purpleDoor;

			case 9:
				// Purple key.
				return CellType.purpleKey;

			default:
				// Default.
				return CellType.space;
		}
	}

	/*----------------------------------------------------------------------------------------------
     * Name:    DoorUnlocked
     * Type:    Method
     * Purpose: Handles the event of a door being unlocked by the player.
     * Input:   Nothing.
     * Output:  Nothing.
    ----------------------------------------------------------------------------------------------*/
	public void DoorUnlocked()
	{
		// A door has been unlocked, so decrement the number of doors left to be unlocked.
		--doorsLeft;
	}
}
