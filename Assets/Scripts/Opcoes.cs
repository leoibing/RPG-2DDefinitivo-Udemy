using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Opcoes : MonoBehaviour
{
	private AudioController audioController;

	public Slider volumeMusica,
		volumeFx;


	void Start()
    {
		audioController = FindObjectOfType(typeof(AudioController)) as AudioController;

		volumeMusica.value = audioController.volumeMaximoMusica;
		volumeFx.value = audioController.volumeMaximoFX;
	}

    public void AlterarValorMusica()
    {
		float tempVolumeMusica = volumeMusica.value;

		audioController.volumeMaximoMusica = tempVolumeMusica;
		audioController.sMusic.volume = tempVolumeMusica;

		PlayerPrefs.SetFloat("volumeMaximoMusica", tempVolumeMusica);
	}

	public void AlterarVolumeFx()
	{
		float tempVolumeFx = volumeFx.value;
		//audioController.sFx.volume = tempVolumeFx;
		audioController.volumeMaximoFX = tempVolumeFx;
		PlayerPrefs.SetFloat("volumeMaximoFx", tempVolumeFx);
	}
}
