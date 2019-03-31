using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallaxing : MonoBehaviour {

    public Transform[] backgrounds;  //Array of back/foregrounds
    private float[] parallaxScales;  //Proportion of the camera's movemtn to move
    public float smoothing = 1f;     //How smooth the parallax is going to be

    private Transform cam;           // reference to the main cameras transform
    private Vector3 previousCamPos;  // position of the camera en the previous frame

    //Great for references. Called before Start()
    private void Awake()
    {
        cam = Camera.main.transform;
    }

    void Start () {
        previousCamPos = cam.position;

        parallaxScales = new float[backgrounds.Length];
        for (int i = 0; i < backgrounds.Length; i++)
        {
            parallaxScales[i] = backgrounds[i].position.z * -1;
        }
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        for (int i = 0; i < backgrounds.Length; i++)
        {
            float parallax = (previousCamPos.x - cam.position.x) * parallaxScales[i];

            float backgroundTargetPosx = backgrounds[i].position.x + parallax;

            Vector3 backgroundTargetPos = new Vector3(backgroundTargetPosx, backgrounds[i].position.y, backgrounds[i].position.z);

            backgrounds[i].position = Vector3.Lerp(backgrounds[i].position, backgroundTargetPos, smoothing * Time.fixedDeltaTime);

                }
        previousCamPos = cam.position;
        }
}

