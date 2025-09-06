using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioController : MonoBehaviour
{
	public AudioSource sMusic,
		sFx;

	//[Header("Musicas")]

	public AudioClip musicaTitulo,
		musicaFase1,

	//[Header("FX")]

	fxClick,
		
		fxSword,
		fxAxe,
		fxBow,
		fxStaff;

	//
	public float volumeMaximoMusica;
	public float volumeMaximoFX;

	//
	private AudioClip novaMusica;
	private string novaCena;
	private bool trocarCena;

	void Start()
	{
		DontDestroyOnLoad(gameObject);

		if(PlayerPrefs.GetInt("valoresIniciais") == 0)
		{
			PlayerPrefs.SetInt("valoresIniciais", 1);
			PlayerPrefs.SetFloat("volumeMaximoMusica", 1);
			PlayerPrefs.SetFloat("volumeMaximoFx", 1);
		}

		//
		volumeMaximoMusica = 1;
		volumeMaximoFX = 1;
		//sMusic.clip = musicaTitulo;
		//sMusic.volume = volumeMaximoMusica;
		//sMusic.Play();
		TrocarMusica(musicaTitulo, "Titulo", true);
	}

	public void TrocarMusica(AudioClip clip, string nomeCena, bool mudarCena)
	{
		novaMusica = clip;
		novaCena = nomeCena;
		trocarCena = mudarCena;

		StartCoroutine("ChangeMusic");
	}

	IEnumerator ChangeMusic()
	{
		for(float volume = volumeMaximoMusica; volume >= 0; volume -= 0.1f)
		{
			yield return new WaitForSecondsRealtime(0.1f);
			sMusic.volume = volume;
		}
		sMusic.volume = 0;

		sMusic.clip = novaMusica;
		sMusic.Play();

		for (float volume = 0; volume < volumeMaximoMusica; volume += 0.1f)
		{
			yield return new WaitForSecondsRealtime(0.1f);
			sMusic.volume = volume;
		}
		sMusic.volume = volumeMaximoMusica;

		if(trocarCena == true)
		{
			SceneManager.LoadScene(novaCena);
		}
	}

	public void TocarFx(AudioClip fx, float volume)
	{
		float tempVolume = volume;
		if(volume > volumeMaximoFX)
		{
			tempVolume = volumeMaximoFX;
		}
		sFx.volume = tempVolume;
		sFx.PlayOneShot(fx);
	}
}
