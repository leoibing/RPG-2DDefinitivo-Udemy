using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arma : MonoBehaviour
{
	private GameController GameController;

	public GameObject[] itemColetar;

	private bool coletado;

	void Start()
	{
		GameController = FindObjectOfType(typeof(GameController)) as GameController;
	}
	public void Coletar()
	{
		if(coletado == false)
		{
			coletado = true;
			GameController.ColetarArma(itemColetar[Random.Range(0, itemColetar.Length)]);
		}
		
		Destroy(gameObject);
	}
}
