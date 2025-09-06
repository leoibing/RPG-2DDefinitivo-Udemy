using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NPC1 : MonoBehaviour
{
	public GameObject canvasNPC;
	public TMP_Text caixaTexto;

	public int idFala;
	public int idDialogo;
	public string[] fala,
		fala1,
		fala2,
		fala3;

	public List<string> LinhasDialogo;

	private bool dialogoOn;

	public GameObject painelResposta;
	private bool respondendoPergunta;
    
    void Start()
    {
        
    }

    void Update()
    {
        
    }

	public void Interacao()
	{
		if(dialogoOn == false)
		{
			idFala = 0;

			//LinhasDialogo.Clear();

			PrepararDialogo();

			Dialogo(); //caixaTexto.text = fala[idFala];
			canvasNPC.SetActive(true);
			dialogoOn = true;
		}
		else if (dialogoOn == true && respondendoPergunta == false)
		{
			idFala += 1;
			Dialogo();
		}
	}

	public void Dialogo()
	{
		if(idFala < LinhasDialogo.Count)
		{
			caixaTexto.text = LinhasDialogo[idFala];

			if(idDialogo == 0 && idFala == 2)
			{
				painelResposta.SetActive(true);
				respondendoPergunta = true;
			}
		}
		else
		{
			switch(idDialogo)
			{
				case 0:
					//idDialogo = 1;
					break;

				case 1:
					idDialogo = 3;
					break;

				case 2:
					idDialogo = 0;
					break;
			}

			canvasNPC.SetActive(false);
			dialogoOn = false;
		}
	}

	void PrepararDialogo()
	{
		LinhasDialogo.Clear();

		switch (idDialogo)
		{
			case 0:

				foreach (string s in fala)
				{
					LinhasDialogo.Add(s);
				}

				break;
			case 1:
				foreach (string s in fala1)
				{
					LinhasDialogo.Add(s);
				}
				break;
			case 2:
				foreach (string s in fala2)
				{
					LinhasDialogo.Add(s);
				}
				break;
			case 3:
				foreach (string s in fala3)
				{
					LinhasDialogo.Add(s);
				}
				break;
		}
	}

	public void BtnRespostaA()
	{
		idDialogo = 1;
		PrepararDialogo();
		idFala = 0;
		respondendoPergunta = false;
		painelResposta.SetActive(false);
		Dialogo();
	}

	public void BtnRespostaB()
	{
		idDialogo = 2;
		PrepararDialogo();
		idFala = 0;
		respondendoPergunta = false;
		painelResposta.SetActive(false);
		Dialogo();
	}
}
