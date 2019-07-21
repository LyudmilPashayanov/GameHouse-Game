using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementScript : MonoBehaviour

{
    float maxX = 3.3f;
    float minX = -3.3f;
    float maxY = 4.2f;
    float minY = -4.2f;
    public float moveSpeed = 2f;
    private float tChange = 0f;
    private float randomX;
    private float randomY;
    public void Start()
    {
        var vertExtent = Camera.main.orthographicSize;
        var horzExtent = vertExtent * Screen.width / Screen.height;

        // Calculations assume map is position at the origin
        minX = horzExtent -7f;
        maxX = 7f - horzExtent;
        minY = vertExtent - 9;
        maxY = 9 - vertExtent;

    }
    public void Update()
    {

        // change to random direction at random intervals
        if (Time.time >= tChange)
        {
            randomX = Random.Range(-moveSpeed, moveSpeed); // with float parameters, a random float
            randomY = Random.Range(-moveSpeed, moveSpeed); //  between -2.0 and 2.0 is returned
            // set a random interval between 0.5 and 1.5
            tChange = Time.time + Random.Range(1f, 1.5f);
        }

        transform.Translate(new Vector3(randomX, randomY, 0) * 2f * Time.deltaTime);
        // if object reached any border, revert the appropriate direction
        if (transform.position.x >= maxX || transform.position.x <= minX)
        {
            randomX = -randomX;
        }
        if (transform.position.y >= maxY || transform.position.y <= minY)
        {
            randomY = -randomY;
        }
        //// make sure the position is inside the borders
        //transform.position.x = Mathf.Clamp(transform.position.x, minX, maxX);
        //transform.position.y = Mathf.Clamp(transform.position.y, minY, maxY);
    }
}
