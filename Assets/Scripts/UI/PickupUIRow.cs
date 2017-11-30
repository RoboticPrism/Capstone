using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickupUIRow : MonoBehaviour {

    public GameObject PickupItemUIPrefab;
    public List<Item> items = new List<Item>();

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void AddItem(PickupItem newItem)
    {
        items.Add(new Item(newItem.sprite));
        GameObject newUIObject = (GameObject)Instantiate(PickupItemUIPrefab, this.transform);
        newUIObject.GetComponentInChildren<Image>().sprite = newItem.sprite;
        RectTransform rect = newUIObject.GetComponent<RectTransform>();
        rect.offsetMin = new Vector2(items.Count - 1 * 0.05f, 0);
        rect.offsetMax = new Vector2((items.Count) * 0.05f, 1);
        rect.anchoredPosition = new Vector2(0, 0);
    }

    public int ItemCount()
    {
        return items.Count;
    }
}
