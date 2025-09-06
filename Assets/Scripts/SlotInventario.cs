using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotInventario : MonoBehaviour
{
	public int idSlot;

	private GameController GameController;

	private PItemInfo pItemInfo;

	public GameObject objetoSlot;

    void Start()
    {
		GameController = FindObjectOfType(typeof(GameController)) as GameController;
		pItemInfo = FindObjectOfType(typeof(PItemInfo)) as PItemInfo;
	}

    void Update()
    {
        
    }

	public void UsarItem()
	{
		if(objetoSlot != null)
		{
			//objetoSlot.SendMessage("UsarItem", SendMessageOptions.DontRequireReceiver);
			pItemInfo.objetoSlot = objetoSlot;
			pItemInfo.idSlot = idSlot;

			pItemInfo.CarregarInfoItem();

			GameController.OpenItemInfo();
		}
		
	}
}
