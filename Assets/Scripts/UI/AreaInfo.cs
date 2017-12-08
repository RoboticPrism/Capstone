using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AreaInfo : MonoBehaviour
{

    public int foodCount;
    public Sprite foodSprite;
    public Text foodText;
    public Image foodImage;

    // Use this for initialization
    void Start()
    {
        foodText.text = "x" + foodCount;
        foodImage.sprite = foodSprite;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetFoodCount(int newFoodCount)
    {
        foodText.text = "x" + newFoodCount;
    }
}