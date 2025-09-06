using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MudarCena : MonoBehaviour
{
	private Fade fade;

	public string cenaDestino;

	private GameController GameController;

	void Start()
    {
		fade = FindObjectOfType(typeof(Fade)) as Fade;
		GameController = FindObjectOfType(typeof(GameController)) as GameController;
	}

    void Update()
    {
        
    }

	public void Interacao()
	{
		StartCoroutine("MudancaCena");
	}

	IEnumerator MudancaCena()
	{
		fade.FadeIn();
		yield return new WaitWhile(() => fade.fume.color.a < 0.9f);

		if(cenaDestino == "Titulo") { Destroy(GameController.gameObject); }

		SceneManager.LoadScene(cenaDestino);
	}

}
