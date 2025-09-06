using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
	private GameController GameController;

	public int valor = 1;

	void Start()
    {
		GameController = FindObjectOfType(typeof(GameController)) as GameController;
	}
    public void Coletar()
    {
		GameController.gold += valor;
		Destroy(gameObject);
	}
}
