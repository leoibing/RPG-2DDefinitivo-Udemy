using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImgFlecha : MonoBehaviour
{
	private GameController GC;
	private SpriteRenderer sRender;

	void Start()
    {
		GC = FindObjectOfType(typeof(GameController)) as GameController;
		sRender = GetComponent<SpriteRenderer>();
	}

    void Update()
    {
		sRender.sprite = GC.imgFlecha[GC.idFlechaEquipada];
    }
}
