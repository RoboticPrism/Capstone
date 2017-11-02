using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupUIBar : MonoBehaviour {

    public List<PickupUIRow> rowItems;
    public int maxRowCount = 20;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void AddItem(PickupItem newItem)
    {
        foreach(PickupUIRow row in rowItems)
        {
            if (row.ItemCount() < 20)
            {
                row.AddItem(newItem);
                break;
            }
        }
    }
}
