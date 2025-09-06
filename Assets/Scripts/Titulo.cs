using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Titulo : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        
    }

	public void SelecionarPersonagem(int idPersonagem)
	{
		PlayerPrefs.SetInt("idPersonagem", idPersonagem);
		SceneManager.LoadScene("Cena1");
	}
}
