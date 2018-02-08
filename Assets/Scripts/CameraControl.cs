using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraControl : MonoBehaviour {

	public GameObject player;
	[Header("Smallest Camera Size")]
	public float cameraMinWidth = 10;
	public float cameraMinHeight = 10;
    [Header("Largest Camera Size")]
    public float cameraMaxWidth = 20;
    public float cameraMaxHeight = 20;

	// Activate your position limitations for the Y axis by turning this on.

	[Header("Camera Movement Boundaries")]
	public bool limitCamXMovement = true;
	public bool limitCamYMovement = true;
    public Vector2 roomSizeMin;
    public Vector2 roomSizeMax;

    [Header("Camera Movement Speeds")]
    public float cameraMoveSpeed = 0.1f;
    public float cameraScaleSpeed = 0.02f;

    private Camera cam;
	private bool pause = false;
	private bool revealing = false;
	private bool waitToCenter = false;
	private List<Vector3> revPoints;

    void Start () {
        // Set the camera for calculation later
        cam = this.GetComponent<Camera>();
    }


	void LateUpdate()
	{
		//Updates the camera position based on player location
		CameraUpdate();
	}

	void CameraUpdate ()
	{
		if (pause) {
			pause = false;
		}
		else if (!revealing) {
			// Calculate the ideal camera size for this room
			float roomWidth = roomSizeMax.x - roomSizeMin.x;
			float roomHeight = roomSizeMax.y - roomSizeMin.y;
        	
			// Ensure camera doesn't get bigger or smaller than its min and max bounds
			if (roomWidth > cameraMaxWidth) {
				roomWidth = cameraMaxWidth;
			} else if (roomWidth < cameraMinWidth) {
				roomWidth = cameraMinWidth;
			}
			if (roomHeight > cameraMaxHeight) {
				roomHeight = cameraMaxHeight;
			} else if (roomHeight < cameraMinHeight) {
				roomHeight = cameraMinHeight;
			}

			// If aspect ratio on ideal room size doesn't match the current aspect ratio, change width or height to make it match
			float aspect = cam.aspect;
        
			// If width is too big, lower it to be in line with aspect ration
			if (roomWidth / roomHeight > aspect) {
				roomWidth = roomHeight * aspect;
			}

        	// If height is too big, lower it to be in line with aspect ratio
        	else if (roomWidth / roomHeight < aspect) {
				roomHeight = roomWidth / aspect;
			}

			// Set camera size gradually
        
			// If camera needs to be bigger, boost it up
			if (cam.orthographicSize < roomHeight / 2.0f) {
				cam.orthographicSize += cameraScaleSpeed;
			}
        	// If cam needs to be smaller, scale it down
        	else if (cam.orthographicSize > roomHeight / 2.0f) {
				cam.orthographicSize -= cameraScaleSpeed;
			}
			// If the camera is close enough to the right size, jump it there
			if (Mathf.Abs (cam.orthographicSize - (roomHeight / 2.0f)) < cameraScaleSpeed) {
				cam.orthographicSize = roomHeight / 2.0f;
			}  
        
			float cameraHeight = cam.orthographicSize * 2.0f;
			float cameraWidth = cameraHeight * cam.aspect;

			// Calculate the camera position in this room
			Vector3 playerPosition = player.transform.position;
			Vector3 cameraPosition = new Vector3 (playerPosition.x, playerPosition.y, transform.position.z);

			// Here we clamp the desired position into the area declared in the limit variables.
			if (limitCamXMovement) {
				cameraPosition.x = Mathf.Clamp (cameraPosition.x, roomSizeMin.x + cameraWidth / 2.0f, roomSizeMax.x - cameraWidth / 2.0f);
			}
			if (limitCamYMovement) {
				cameraPosition.y = Mathf.Clamp (cameraPosition.y, roomSizeMin.y + cameraHeight / 2.0f, roomSizeMax.y - cameraHeight / 2.0f);
			}

			// and now we're updating the camera position using what came of all the calculations above.
			if (waitToCenter && this.transform.position == cameraPosition) {
				waitToCenter = false;
				SideScrollingPlayer playerCont = player.GetComponent<SideScrollingPlayer> ();
				if (playerCont) {
					playerCont.RevealDone ();
				}
			}
			this.transform.position = Vector3.MoveTowards (transform.position, cameraPosition, cameraMoveSpeed);
		} else {
			if (revPoints.Count != 0) {
				if (this.transform.position.x == revPoints [0].x && this.transform.position.y == revPoints[0].y) {
					revPoints.Remove (revPoints [0]);
					pause = true;
				} else {
					this.transform.position = Vector3.MoveTowards (this.transform.position, new Vector3 (revPoints [0].x, revPoints [0].y, this.transform.position.z), cameraMoveSpeed);
				}
			} else {
				revealing = false;
				waitToCenter = true;
			}
		}
	}


	// Public functions start here. These are for other objects/scripts to communicate with the camera.

	// Use this to change/activate level limits.
	public void SetNewLimits (Vector2 min, Vector2 max)
	{
        roomSizeMin = min;
        roomSizeMax = max;

	    limitCamXMovement = true;
		limitCamYMovement = true;
	}

	public void PanToSniffables(List<Vector3> points){
		revealing = true;
		revPoints = points;
	}
		
    // Turn off horizontal camera limits
	public void DeactivateXLimits()
	{
		limitCamXMovement = false;
	}

    //Turn off vertical camera limits
	public void DeactivateYLimits()
	{
		limitCamYMovement = false;
	}
}