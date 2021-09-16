using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class BackgroundMovement : MonoBehaviour
{
    public GameObject camera;
    public GameObject child1;
    public GameObject child2;
    private float width;
    public int distance = 20;

    private float cameraPreviousPositionX;
    private float cameraCurrentPositionX;
    public float parallaxSpeed = 0.2f;

    void Start()
    {
        var renderer = child1.gameObject.GetComponent<Renderer>();
        width = renderer.bounds.size.x;
        child1.transform.position = gameObject.transform.position;
        child2.transform.position = new Vector3(gameObject.transform.position.x + width, gameObject.transform.position.y, distance);
        cameraCurrentPositionX = camera.transform.position.x;
        cameraPreviousPositionX = cameraCurrentPositionX;
    }

    //Update is called once per frame
    void Update()
    {
        cameraCurrentPositionX = camera.transform.position.x;
        float difference = cameraCurrentPositionX - cameraPreviousPositionX;

        if (transform.position.x  > camera.transform.position.x)
        {
            transform.position = new Vector3(transform.position.x - width, transform.position.y, distance);
        }
        else if (transform.position.x + width <= camera.transform.position.x)
        {
            transform.position = new Vector3(transform.position.x + width, transform.position.y, distance);
        }

        transform.position = new Vector3(transform.position.x - difference * parallaxSpeed, transform.position.y, distance);

        cameraPreviousPositionX = cameraCurrentPositionX;
    }
}
