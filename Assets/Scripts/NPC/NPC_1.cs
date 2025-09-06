using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NPC_1 : MonoBehaviour
{
	private GameController GC;

	public string nomeArquivoXml;

	public GameObject canvasNPC;
	public Button btnA;
	public TMP_Text textoBtnA, textoBtnB,
		caixaTexto;

	public int idFala;
	public int idDialogo;
	public List<string> fala0,
		fala1,
		fala2,
		fala3,
		fala4,
		fala5,

		respostaFala0;

	public List<string> LinhasDialogo;

	private bool dialogoOn;

	public GameObject painelResposta;
	private bool respondendoPergunta;
    
    void Start()
    {
		GC = FindObjectOfType(typeof(GameController)) as GameController;
		LinhasDialogo.Clear();
		LoadDialogoData();
	}

    void Update()
    {
		/*if (GC.currentState == GameState.DIALOGO && Input.GetButtonDown("Fire1"))
		{
			Interacao();
		}*/
	}

	public void Interacao()
	{
		if(GC.currentState == GameState.GAMEPLAY)
		{
			GC.ChangeState(GameState.DIALOGO);

			idFala = 0;

			if(idDialogo == 3 && GC.missao1 == true)
			{
				idDialogo = 4;
			}

			PrepararDialogo();
			Dialogo();
			canvasNPC.SetActive(true);
			dialogoOn = true;

		}
	}

	public void Falar()
	{
		if (dialogoOn == true && respondendoPergunta == false)
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
				textoBtnA.text = respostaFala0[0];
				textoBtnB.text = respostaFala0[1];
				painelResposta.SetActive(true);
				btnA.Select();
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

				case 4:
					idDialogo = 5;
					break;
			}

			canvasNPC.SetActive(false);
			dialogoOn = false;
			GC.ChangeState(GameState.FIMDIALOGO);
		}
	}

	void PrepararDialogo()
	{
		LinhasDialogo.Clear();

		switch (idDialogo)
		{
			case 0:

				foreach (string s in fala0)
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
			case 4:
				foreach (string s in fala4)
				{
					LinhasDialogo.Add(s);
				}
				break;
			case 5:
				foreach (string s in fala5)
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

	void LoadDialogoData()
	{
		TextAsset xmlData = (TextAsset)Resources.Load(GC.idiomaFolder[GC.idioma] + "/" + nomeArquivoXml);
		XmlDocument xmlDocument = new();

		xmlDocument.LoadXml(xmlData.text);

		foreach (XmlNode dialogo in xmlDocument["dialogos"].ChildNodes)
		{
			string dialogoName = dialogo.Attributes["name"].Value;

			foreach(XmlNode f in dialogo["falas"].ChildNodes)
			{
				switch(dialogoName)
				{
					case "fala0":
						fala0.Add(GC.TextoFormatado(f.InnerText));
						break;

					case "fala1":
						fala1.Add(f.InnerText);
						break;

					case "fala2":
						fala2.Add(f.InnerText);
						break;

					case "fala3":
						fala3.Add(f.InnerText);
						break;

					case "fala4":
						fala4.Add(f.InnerText);
						break;

					case "fala5":
						fala5.Add(f.InnerText);
						break;

					case "resposta0":
						respostaFala0.Add(f.InnerText);
						break;
				}
			}
		}
	}
}
