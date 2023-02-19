using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rules : MonoBehaviour
{
	public float rMax = 10;
	public float rMin = 1;
	public float rMinRepulsion = 1;
	public float speed = 5;
	public const int typeNum = 3;
	public float[,] attractionGrid = new float[typeNum, typeNum];

	// Start is called before the first frame update
	void Start()
    {
		GenerateAttractionGrid();

	}

    // Update is called once per frame
    void Update()
    {
        
    }

	void GenerateAttractionGrid() {
		for (int i = 0; i < attractionGrid.GetLength(0); i++)
		{
			for (int j = 0; j < attractionGrid.GetLength(1); j++)
			{
				attractionGrid[i, j] = 0;
			}
		}
	}
}
