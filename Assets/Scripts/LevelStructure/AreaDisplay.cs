using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AreaDisplay : MonoBehaviour {

	public Sprite foodSprite;
	public Text foodText;
	public Image foodImage;
	private int foodCount = 0;

	// Use this for initialization
	void Start () {
		foodText.text = "x" + foodCount;
		foodImage.sprite = foodSprite;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public void SetFoodCount(int newFoodCount)
	{
		foodText.text = "x" + newFoodCount;
	}
}
