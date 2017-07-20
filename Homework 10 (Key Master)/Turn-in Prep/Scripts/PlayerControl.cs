using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour {

    public KeyCode moveUp, moveDown, moveLeft, moveRight, dropKey;

    public float speedX = 0, speedZ = 0;
    public bool linearMovement = true;

    public Camera mainCam;

    private Rigidbody rbody;

	GameObject gm;
	Vector3 originalKeyPosition;
	Quaternion originalKeyRotation;
	Vector3 originalKeyScale;
	Transform originalKeyParent;
	GameObject heldKeyObject;
	int heldKey = -1;

    // Use this for initialization
    void Start () {
        // store the rigid body in an attribute for easier access.
        rbody = GetComponent<Rigidbody>();
		gm = GameObject.Find("GM");
    }
	
	// Update is called once per frame
	void Update () {

        // move the player in the 4 directions based on the keys we set up for it
        if (Input.GetKey(moveUp))
        {
            if (linearMovement) // simple constant velocity
                rbody.velocity = new Vector3(0f, 0f, speedZ);
            else if (rbody.velocity.z == 0)// if we were going to use a force instead
                rbody.velocity = new Vector3(0f, 0f, speedZ / 2); // start from 0 with some speed
            else
                rbody.AddForce(new Vector3(0, 0f, speedZ)); // then accelerate
        }
        else if (Input.GetKey(moveDown))
        {
            if (linearMovement)
                rbody.velocity = new Vector3(0f, 0f, -speedZ);
            else if (rbody.velocity.z == 0)
                rbody.velocity = new Vector3(0f, 0f, -speedZ / 2);
            else
                rbody.AddForce(new Vector3(0f, 0f, -speedZ));
        }
        else if (Input.GetKey(moveLeft))
        {
            if (linearMovement)
                rbody.velocity = new Vector3(-speedX, 0f, 0f);
            else if (rbody.velocity.x == 0)
                rbody.velocity = new Vector3(-speedX / 2, 0f, 0f);
            else
                rbody.AddForce(new Vector3(-speedX, 0f, 0f));
        }
        else if (Input.GetKey(moveRight))
        {
            if (linearMovement)
                rbody.velocity = new Vector3(speedX, 0f, 0f);
            else if (rbody.velocity.x == 0)
                rbody.velocity = new Vector3(speedX / 2, 0f, 0f);
            else
                rbody.AddForce(new Vector3(speedX, 0f, 0f));
        }
		else if (Input.GetKey(dropKey))
		{
			if (heldKey == -1)
				return;

			ReturnKey();
		}
        else
        { // no input, reset the speed
            rbody.velocity = new Vector3(0f, 0f, 0f);
        }
    }

	void OnCollisionEnter (Collision col)
	{
		if (col.gameObject.CompareTag("key") == true)
		{
			if (heldKey != -1)
				return;

			heldKeyObject = col.gameObject;
			originalKeyPosition = heldKeyObject.transform.position;
			originalKeyRotation = heldKeyObject.transform.rotation;
			originalKeyScale = heldKeyObject.transform.localScale;
			originalKeyParent = heldKeyObject.transform.parent;

            heldKeyObject.transform.parent = transform;
			heldKeyObject.transform.localPosition = new Vector3(0.0f, 2.5f, 0.0f);
			heldKeyObject.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);

			heldKey = int.Parse(heldKeyObject.name);
		}
		else if (col.gameObject.CompareTag("door") == true)
		{
			if (heldKey == -1)
				return;

			if (heldKey == int.Parse(col.gameObject.name))
			{
				heldKeyObject.SetActive(false);
				col.gameObject.SetActive(false);
				gm.GetComponent<GameMaster>().DoorUnlocked();
				ReturnKey();
			}
		}
	}

	void ReturnKey()
	{
		heldKeyObject.transform.parent = null;
		heldKeyObject.transform.position = originalKeyPosition;
		heldKeyObject.transform.rotation = originalKeyRotation;
		heldKeyObject.transform.localScale = originalKeyScale;
		heldKeyObject.transform.parent = originalKeyParent;

		heldKey = -1;
		heldKeyObject = null;
	}
}
