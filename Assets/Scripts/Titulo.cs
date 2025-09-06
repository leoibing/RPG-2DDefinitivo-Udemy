using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

using System.IO;

public class Titulo : MonoBehaviour
{
	private AudioController audioController;

	public Button btnCarregarJogo,

		btnCarregarSlot1,
		btnCarregarSlot2,
		btnCarregarSlot3,

		btnNovoSlot1,
		btnNovoSlot2,
		btnNovoSlot3;

	public GameObject btnDelete1,
		btnDelete2,
		btnDelete3;


	void Start()
	{
		audioController = FindObjectOfType(typeof(AudioController)) as AudioController;

		VerificarSaveGame();

	}

	public void SelecionarPersonagem(int idPersonagem)
	{
		PlayerPrefs.SetInt("idPersonagem", idPersonagem);
		SceneManager.LoadScene("Load");
	}

	void VerificarSaveGame()
	{
		btnCarregarJogo.interactable = false;

		btnCarregarSlot1.interactable = false;
		btnCarregarSlot2.interactable = false;
		btnCarregarSlot3.interactable = false;

		btnNovoSlot1.interactable = true;
		btnNovoSlot2.interactable = true;
		btnNovoSlot3.interactable = true;

		btnDelete1.SetActive(false);
		btnDelete2.SetActive(false);
		btnDelete3.SetActive(false);

		if (File.Exists(Application.persistentDataPath + "/playerdata1.dat"))
		{
			btnCarregarSlot1.interactable = true;

			btnNovoSlot1.interactable = false;
			btnDelete1.SetActive(true);
		}

		if (File.Exists(Application.persistentDataPath + "/playerdata2.dat"))
		{
			btnCarregarSlot2.interactable = true;

			btnNovoSlot2.interactable = false;
			btnDelete2.SetActive(true);
		}

		if (File.Exists(Application.persistentDataPath + "/playerdata3.dat"))
		{
			btnCarregarSlot3.interactable = true;

			btnNovoSlot3.interactable = false;
			btnDelete3.SetActive(true);
		}

		if(btnCarregarSlot1.interactable == true || btnCarregarSlot2.interactable == true || btnCarregarSlot3.interactable == true)
		{
			btnCarregarJogo.interactable = true;
		}
	}

	public void NovoJogo(int slot)
	{
		switch (slot)
		{
			case 1:
				PlayerPrefs.SetString("slot", "playerdata1.dat");
				break;
			case 2:
				PlayerPrefs.SetString("slot", "playerdata2.dat");
				break;
			case 3:
				PlayerPrefs.SetString("slot", "playerdata3.dat");
				break;
		}
	}

	public void CarregarJogo(int slot)
	{
		switch (slot)
		{
			case 1:
				PlayerPrefs.SetString("slot", "playerdata1.dat");
				break;
			case 2:
				PlayerPrefs.SetString("slot", "playerdata2.dat");
				break;
			case 3:
				PlayerPrefs.SetString("slot", "playerdata3.dat");
				break;
		}

		SceneManager.LoadScene("Load");
	}

	public void DeleteSave(int slot)
	{
		switch(slot)
		{
			case 1:
				if(File.Exists(Application.persistentDataPath + "/playerdata1.dat"))
				{
					File.Delete(Application.persistentDataPath + "/playerdata1.dat");
				}
				break;
			case 2:
				if (File.Exists(Application.persistentDataPath + "/playerdata2.dat"))
				{
					File.Delete(Application.persistentDataPath + "/playerdata2.dat");
				}
				break;
			case 3:
				if (File.Exists(Application.persistentDataPath + "/playerdata3.dat"))
				{
					File.Delete(Application.persistentDataPath + "/playerdata3.dat");
				}
				break;
		}

		VerificarSaveGame();
	}

	public void  Click()
	{
		audioController.TocarFx(audioController.fxClick, 1);
	}
}
