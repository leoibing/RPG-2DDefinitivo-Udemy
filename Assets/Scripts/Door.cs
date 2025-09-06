using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
	private Fade fade;
	private Player Player;

	//public Transform tPlayer;
	public Transform destino;

	public bool escuro;
	public Material Luz2D, padrao2D;

    void Start()
    {
		fade = FindObjectOfType(typeof(Fade)) as Fade;
		Player = FindObjectOfType(typeof(Player)) as Player;
	}

    void Update()
    {
        
    }

	void Interacao()
	{
		StartCoroutine("AcionarPorta");
	}

	IEnumerator AcionarPorta()
	{
		fade.FadeIn();
		yield return new WaitWhile(() => fade.fume.color.a < 0.9f);
		Player.gameObject.SetActive(false);

		switch(escuro)
		{
			case true:
				//tPlayer.gameObject.GetComponent<SpriteRenderer>().material = Luz2D;
				Player.ChangeMaterial(Luz2D);
				break;

			case false:
				//tPlayer.gameObject.GetComponent<SpriteRenderer>().material = padrao2D;
				Player.ChangeMaterial(padrao2D);
				break;
		}

		Player.transform.position = destino.position;
		//yield return new WaitForSeconds(0.5f);
		Player.gameObject.SetActive(true);
		fade.FadeOut();
	}

}
