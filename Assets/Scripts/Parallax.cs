using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
	public Transform background;
	public float parallaxScale,
		velocidade;

	public Transform cam;
	private Vector3 previewCamPosition;

    void Start()
    {
		cam = Camera.main.transform;
		previewCamPosition = cam.position;
    }

    void LateUpdate()
    {
		float parallaxX = (previewCamPosition.x - cam.position.x) * parallaxScale;
		float bgTargetX = background.position.x + parallaxX;

		Vector3 bgPos = new Vector3(bgTargetX, background.position.y, background.position.x);
		background.position = Vector3.Lerp(background.position, bgPos, velocidade * Time.deltaTime);

		previewCamPosition = cam.position;
    }
}
