using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class AreaInfo
{
	public string name;
    public int foodCount;


	public AreaInfo(StateSaver.Area name, int foodcount){
		this.name = StateSaver.areaNames[(int)name];
		this.foodCount = foodcount;
	}

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

}