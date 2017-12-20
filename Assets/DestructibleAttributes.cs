using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleAttributes : MonoBehaviour {

    public int MaterialType; // 0 = wood, 1 = stone, 2 = metal
    public int AmountToDrop;
    public int NeededHits;
    public int HitCounter;
    public GameObject PlayerDestroying;

    public void HitCountIncreaseAndCheck()
    {
        HitCounter = HitCounter + 1;
        if (HitCounter >= NeededHits)
        {
            switch(MaterialType)
            {
                case 0:
                    PlayerDestroying.GetComponent<PlayerStats>().WoodInInventory = PlayerDestroying.GetComponent<PlayerStats>().WoodInInventory + AmountToDrop;
                    Destroy(gameObject);
                    break;
                case 1:
                    PlayerDestroying.GetComponent<PlayerStats>().StoneInInventory = PlayerDestroying.GetComponent<PlayerStats>().StoneInInventory + AmountToDrop;
                    Destroy(gameObject);
                    break;
                case 2:
                    PlayerDestroying.GetComponent<PlayerStats>().MetalInInventory = PlayerDestroying.GetComponent<PlayerStats>().MetalInInventory + AmountToDrop;
                    Destroy(gameObject);
                    break;
            }
        }
    }
}
