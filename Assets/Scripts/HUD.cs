using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUD : MonoBehaviour
{
	private GameController GC;
	private Player Player;
	public Image[]
		hpBar,
		mpBar;
	public Sprite
		half, full,
		mHalf, mFull;
	public GameObject
		painelMana,
		painelFlechas,
		boxHp,
		boxMp;
	public Image icoFlecha;
	public TMP_Text
		qtdFlechas,
		qtdHpBoxTxt,
		qtdMpBoxTxt;
	public RectTransform boxA, boxB;
	public Vector2 posA, posB;

	void Start()
    {
		GC = FindObjectOfType(typeof(GameController)) as GameController;
		Player = FindObjectOfType(typeof(Player)) as Player;

		painelMana.SetActive(false);
		painelFlechas.SetActive(false);
		boxMp.SetActive(false);
		boxHp.SetActive(false);

		if (GC.idClasse[GC.idPersonagem] == 2)
		{
			painelMana.SetActive(true);
			
		}
		else if (GC.idClasse[GC.idPersonagem] == 1)
		{
			icoFlecha.sprite = GC.icoFlecha[GC.idFlechaEquipada];
			painelFlechas.SetActive(true);
		}

		posA = boxA.anchoredPosition;
		posB = boxB.anchoredPosition;
	}

    void Update()
    {
		ControleBarraVida();

		PosicaoCaixaPocoes();

		if (painelMana.activeSelf == true)
		{
			ControleBarraMana();
		}
		else if (painelFlechas.activeSelf == true)
		{
			if (Input.GetButtonDown("bntL"))
			{
				if (GC.idFlechaEquipada == 0)
				{
					GC.idFlechaEquipada = GC.icoFlecha.Length - 1;
				}
				else
				{
					GC.idFlechaEquipada -= 1;
				}
			}
			else if (Input.GetButtonDown("bntR"))
			{
				if (GC.idFlechaEquipada == GC.icoFlecha.Length - 1)
				{
					GC.idFlechaEquipada = 0;
				}
				else
				{
					GC.idFlechaEquipada += 1;
				}
			}

			icoFlecha.sprite = GC.icoFlecha[GC.idFlechaEquipada];
			qtdFlechas.text = "x " + GC.qtdFlechas[GC.idFlechaEquipada].ToString();
		}

		qtdHpBoxTxt.text = GC.qtdPocoes[0].ToString();
		qtdMpBoxTxt.text = GC.qtdPocoes[1].ToString();
	}

	void PosicaoCaixaPocoes()
	{
		if(GC.qtdPocoes[0] > 0)
		{
			boxHp.GetComponent<RectTransform>().anchoredPosition = posA;
			boxMp.GetComponent<RectTransform>().anchoredPosition = posB;
		}
		else
		{
			boxHp.GetComponent<RectTransform>().anchoredPosition = posB;
			boxMp.GetComponent<RectTransform>().anchoredPosition = posA;
		}
	}

	void ControleBarraVida()
	{
		float percVida = (float)GC.vidaAtual / (float)GC.vidaMax;

		if (Input.GetButtonDown("itemA") && percVida < 1)
		{
			GC.UsarPocao(0); // Cura
		}

		foreach (Image img in hpBar)
		{
			img.enabled = true;
			img.sprite = full;
		}

		if (percVida >= 1)
		{
			
		}
		else if (percVida >= 0.9f)
		{
			hpBar[4].sprite = half;
		}
		else if (percVida >= 0.8f)
		{
			hpBar[4].enabled = false;
		}

		else if (percVida >= 0.7f)
		{
			hpBar[4].enabled = false;
			hpBar[3].sprite = half;
		}
		else if (percVida >= 0.6f)
		{
			hpBar[4].enabled = false;
			hpBar[3].enabled = false;
		}

		else if (percVida >= 0.5f)
		{
			hpBar[4].enabled = false;
			hpBar[3].enabled = false;
			hpBar[2].sprite = half;
		}
		else if (percVida >= 0.4f)
		{
			hpBar[4].enabled = false;
			hpBar[3].enabled = false;
			hpBar[2].enabled = false;
		}

		else if (percVida >= 0.3f)
		{
			hpBar[4].enabled = false;
			hpBar[3].enabled = false;
			hpBar[2].enabled = false;
			hpBar[1].sprite = half;
		}
		else if (percVida >= 0.2f)
		{
			hpBar[4].enabled = false;
			hpBar[3].enabled = false;
			hpBar[2].enabled = false;
			hpBar[1].enabled = false;
		}

		else if (percVida > 0f)
		{
			hpBar[4].enabled = false;
			hpBar[3].enabled = false;
			hpBar[2].enabled = false;
			hpBar[1].enabled = false;
			hpBar[0].sprite = half;
		}
		else if (percVida <= 0f)
		{
			hpBar[4].enabled = false;
			hpBar[3].enabled = false;
			hpBar[2].enabled = false;
			hpBar[1].enabled = false;
			hpBar[0].enabled = false;
		}

		if(GC.qtdPocoes[0] > 0) { boxHp.SetActive(true); }
		else { boxHp.SetActive(false); }
	}

	void ControleBarraMana()
	{
		float percMana = (float)GC.manaAtual / (float)GC.manaMax;

		if (Input.GetButtonDown("itemB") && percMana < 1)
		{
			GC.UsarPocao(1); // Mana
		}

		foreach (Image img in mpBar)
		{
			img.enabled = true;
			img.sprite = mFull;
		}

		if (percMana >= 1)
		{

		}
		else if (percMana >= 0.9f)
		{
			mpBar[4].sprite = mHalf;
		}
		else if (percMana >= 0.8f)
		{
			mpBar[4].enabled = false;
		}

		else if (percMana >= 0.7f)
		{
			mpBar[4].enabled = false;
			mpBar[3].sprite = mHalf;
		}
		else if (percMana >= 0.6f)
		{
			mpBar[4].enabled = false;
			mpBar[3].enabled = false;
		}

		else if (percMana >= 0.5f)
		{
			mpBar[4].enabled = false;
			mpBar[3].enabled = false;
			mpBar[2].sprite = mHalf;
		}
		else if (percMana >= 0.4f)
		{
			mpBar[4].enabled = false;
			mpBar[3].enabled = false;
			mpBar[2].enabled = false;
		}

		else if (percMana >= 0.3f)
		{
			mpBar[4].enabled = false;
			mpBar[3].enabled = false;
			mpBar[2].enabled = false;
			mpBar[1].sprite = mHalf;
		}
		else if (percMana >= 0.2f)
		{
			mpBar[4].enabled = false;
			mpBar[3].enabled = false;
			mpBar[2].enabled = false;
			mpBar[1].enabled = false;
		}

		else if (percMana > 0f)
		{
			mpBar[4].enabled = false;
			mpBar[3].enabled = false;
			mpBar[2].enabled = false;
			mpBar[1].enabled = false;
			mpBar[0].sprite = mHalf;
		}
		else if (percMana <= 0f)
		{
			mpBar[4].enabled = false;
			mpBar[3].enabled = false;
			mpBar[2].enabled = false;
			mpBar[1].enabled = false;
			mpBar[0].enabled = false;
		}

		if (GC.qtdPocoes[1] > 0) { boxMp.SetActive(true); }
		else { boxMp.SetActive(false); }
	}
}
