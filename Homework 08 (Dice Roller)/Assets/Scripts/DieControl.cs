using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DieControl : MonoBehaviour
{
    public Text topText;
    public int topNumber;

    Rigidbody rbody;
    int[] topAngleX = { 0, 0, 0, 270, 90, 0, 0 };
    int[] topAngleZ = { 0, 0, 270, 0, 0, 90, 180 };
    
    void Start()
    {
        rbody = GetComponent<Rigidbody>();
        //RollDie();
        Randomize();
    }

    void LateUpdate()
    {
        GetTop();
    }

    public void RollDie()
    {
        const float VMIN = -25.0f;
        const float VMAX = 25.0f;
        const float TMIN = -15.0f;
        const float TMAX = 15.0f;

        rbody.velocity = new Vector3(
            Random.Range(VMIN, VMAX),
            1.0f,
            Random.Range(VMIN, VMAX));
        rbody.AddTorque(new Vector3(
            Random.Range(TMIN, TMAX),
            Random.Range(TMIN, TMAX),
            Random.Range(TMIN, TMAX)));
    }

    public void GetTop()
    {
        float smallestAngle = float.MaxValue;
        float currentAngle = 0;

        for (int i = 1; i <= 6; i++)
        {
            currentAngle = Mathf.Abs(rbody.rotation.eulerAngles.x - topAngleX[i]) +
                Mathf.Abs(rbody.rotation.eulerAngles.z - topAngleZ[i]);

            if (currentAngle < smallestAngle)
            {
                topNumber = i;
                smallestAngle = currentAngle;
            }
        }

        topText.text = "Top: " + topNumber.ToString();
    }

    public void Randomize()
    {
        int randomTopNumber = Random.Range(1, 7);

        rbody.transform.Rotate(new Vector3(
            topAngleX[randomTopNumber],
            0.0f,
            topAngleZ[randomTopNumber]));
    }
}
