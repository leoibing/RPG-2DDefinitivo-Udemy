using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
	private GameController GameController;

	public int idItem;

    void Start()
    {
		GameController = FindObjectOfType(typeof(GameController)) as GameController;
	}

    void Update()
    {
        
    }

	public void UsarItem()
	{
		print(idItem);
		GameController.UsarItemArma(idItem);
	}
}
