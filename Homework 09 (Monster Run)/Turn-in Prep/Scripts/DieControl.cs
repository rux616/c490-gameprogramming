/*--------------------------------------------------------------------------------------------------
 * Author:      Dan Cassidy
 * Date:        2015-11-12
 * Assignment:  Homework 9 - Monster Run
 * Source File: DieControl.cs
 * Language:    C#
 * Course:      CSCI-C 490, Game Programming and Design, TuTh 17:30
 * Purpose:     Contains control code for the die.
--------------------------------------------------------------------------------------------------*/
using UnityEngine;
using UnityEngine.UI;

public class DieControl : MonoBehaviour
{
    public Text topText;
    public int topNumber;

    Rigidbody rbody;
    int[] topAngleX = { 0, 0, 0, 270, 90, 0, 0 };
    int[] topAngleZ = { 0, 0, 270, 0, 0, 90, 180 };

	/*----------------------------------------------------------------------------------------------
     * Name:    RollDie
     * Type:    Method
     * Purpose: Rolls the die.
     * Input:   Nothing.
     * Output:  Nothing.
    ----------------------------------------------------------------------------------------------*/
	public void RollDie()
	{
		// Establish minimum and maximum magnitudes for velocity and torque.
		const float VelocityMagnitudeMin = 5.0f;
		const float VelocityMagnitudeMax = 10.0f;
		const float TorqueMagnitudeMin = 25.0f;
		const float TorqueMagnitudeMax = 50.0f;

		// Helper array to decide whether a randomly generated velocity or torque will be positive
		// negative.
		int[] direction = { -1, 1 };

		// Set the velocity of the die.  Note that a little extra velocity on the Y-axis has been
		// added to ensure that the applied torque has a chance to do its job.
		rbody.velocity = new Vector3(
			direction[Random.Range(0, 2)] * Random.Range(VelocityMagnitudeMin, VelocityMagnitudeMax),
			5.0f,
			direction[Random.Range(0, 2)] * Random.Range(VelocityMagnitudeMin, VelocityMagnitudeMax));

		// Torque the die.
		rbody.AddTorque(new Vector3(
			direction[Random.Range(0, 2)] * Random.Range(TorqueMagnitudeMin, TorqueMagnitudeMax),
			direction[Random.Range(0, 2)] * Random.Range(TorqueMagnitudeMin, TorqueMagnitudeMax),
			direction[Random.Range(0, 2)] * Random.Range(TorqueMagnitudeMin, TorqueMagnitudeMax)));
	}

    /*----------------------------------------------------------------------------------------------
     * Name:    GetTop
     * Type:    Method
     * Purpose: Gets the top of the die, and sets the text accordingly.
     * Input:   Nothing.
     * Output:  Nothing.
    ----------------------------------------------------------------------------------------------*/
    void GetTop()
    {
		// Establish local variables.
        float angleX = 0;
        float angleZ = 0;
        float smallestAngle = float.MaxValue;
        float currentAngle = 0;

		// Check each of the 6 possible X and Z rotations in order to establish which number is
		// showing on top.
        for (int i = 1; i <= 6; i++)
        {
			// Get the X angle and check if its close to 0.  If it is, just set it to that.
            angleX = rbody.rotation.eulerAngles.x;
            if (angleX > 315)
                angleX = 0;

			// Get the Z angle and check if its close to 0.  If it is, just set it to that.
			angleZ = rbody.rotation.eulerAngles.z;
            if (angleZ > 315)
                angleZ = 0;

			// Calculate the difference between current angles and stored ideal angles.
            currentAngle = Mathf.Abs(angleX - topAngleX[i]) + Mathf.Abs(angleZ - topAngleZ[i]);

			// Check the calculated angle versus the smallest recorded.
            if (currentAngle < smallestAngle)
            {
				// If the calculated angle is smaller than the last recorded smallest angle, set the
				// top number appropriately and store the calculated angle.
                topNumber = i;
                smallestAngle = currentAngle;
            }
        }

		// Update the top text.
        topText.text = Constants.RollText + topNumber.ToString();
    }

	/*----------------------------------------------------------------------------------------------
     * Name:    LateUpdate
     * Type:    Method
     * Purpose: Handles updating things after the physics ticks and regular update scripts have
	 *			fired.
     * Input:   Nothing.
     * Output:  Nothing.
    ----------------------------------------------------------------------------------------------*/
	void LateUpdate()
	{
		// Get the top of the die.
		GetTop();
	}

	/*----------------------------------------------------------------------------------------------
     * Name:    Randomize
     * Type:    Method
     * Purpose: Randomize the top number of the die.
     * Input:   Nothing.
     * Output:  Nothing.
    ----------------------------------------------------------------------------------------------*/
	void Randomize()
    {
		// Generate the random number.
        int randomTopNumber = Random.Range(1, 7);

		// Apply the random number.
        rbody.transform.Rotate(new Vector3(
            topAngleX[randomTopNumber],
            0.0f,
            topAngleZ[randomTopNumber]));
    }

	/*----------------------------------------------------------------------------------------------
     * Name:    Start
     * Type:    Method
     * Purpose: Perform one-time initialization instructions.
     * Input:   Nothing.
     * Output:  Nothing.
    ----------------------------------------------------------------------------------------------*/
	void Start()
	{
		// Get the rigidbody of the die.
		rbody = GetComponent<Rigidbody>();

		// Randomize the initial number.
		Randomize();
	}
}
