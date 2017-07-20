using UnityEngine;

public class ScrollControl : MonoBehaviour
{
    public float speed = -1f;
    float height;
    public Transform top, bottom;

    // Use this for initialization
    void Start()
    {
        // set the vertical speed of the background
        GetComponent<Rigidbody2D>().velocity = new Vector3(0f, speed, 0f);
        // find out how big our image is
        height = (top.position.y - bottom.position.y) / 2;
        //Debug.Log ("height: " + height);
    }

    // Update is called once per frame
    void Update()
    {
        if (speed > 0)
        {  // moving up
            // if we get past the height of one image, move both of them down to restart the cycle.
            if (transform.position.y >= height)
                transform.position = new Vector3(transform.position.x, -height, transform.position.z);
        }
        else // moving down
        {
            // if we get past minus the height of one image, move both of them up to restart the cycle.
            if (transform.position.y <= -height)
                transform.position = new Vector3(transform.position.x, height, transform.position.z);
        }
    }
}
