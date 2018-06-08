using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour 
{

	public GameObject hexPrefab;
	public static Dictionary<string, int> stateOfPlay = new Dictionary<string, int>();
	
	int width = 16; // Size of map in terms of hex tiles
	int height = 16;

	float xOffset = 0.882f;
	float zOffset = 0.764f;
	 
	void Start () 
	{
		
		TestGenerationOfGameState (width, height, ref stateOfPlay); // Инициализация тестового словаря состояния игры
		
		MapInitialization (width, height, ref stateOfPlay); // Генерация карты на основе ширины, высоты и словаря состояния игры
	
	}

	void Update ()
	{
		
	}

	void MapInitialization (int width, int height, ref Dictionary<string, int> stateOfPlay)
	{
		
		for (int x = 0; x < width; x++)
		{
			for (int y = 0; y < height; y++)
			{
				if (stateOfPlay["Hex_" + x + "_" + y] == 0) //Если 0 то объект не нужно генерировать (пустое место).
				{
					continue;
				}

				float xPos = x * xOffset;
				if (y % 2 == 1)
				{
					xPos += xOffset / 2f;
				}
				GameObject hex_go = (GameObject)Instantiate (hexPrefab, new Vector3 (xPos, 0, y * zOffset), Quaternion.identity); // Создание обьекта - гекс

				hex_go.name = "Hex_" + x + "_" + y;
				hex_go.transform.SetParent(this.transform);
				hex_go.isStatic = true;
				hex_go.GetComponent<Hex>().x = x; // Сохраняем координаты гекса
				hex_go.GetComponent<Hex>().y = y; // в переменные x, y
				
				
				if (stateOfPlay["Hex_" + x + "_" + y] != 5 && stateOfPlay["Hex_" + x + "_" + y] == 1) //Если 1 то Объект, захвачен игроком А (клиент), гекс окрашивается в синий цвет.
				{
					MeshRenderer mr =  hex_go.GetComponentInChildren<MeshRenderer>();
					mr.material.color = Color.blue;
				}
				else if (stateOfPlay["Hex_" + x + "_" + y] == 2) //Если 2 то Объект, захвачен игроком B (противник), гекс окрашивается в красный цвет.
				{
					MeshRenderer mr =  hex_go.GetComponentInChildren<MeshRenderer>();
					mr.material.color = Color.red;
				}
			}
		}
	}
	void TestGenerationOfGameState (int width, int height, ref Dictionary<string, int> stateOfPlay)
	{
		for (int x = 0; x < width; x++)
		{
			for (int y = 0; y < height; y++)
			{
				stateOfPlay.Add ("Hex_" + x + "_" + y, 5);
			}
		}

		stateOfPlay["Hex_0_0"] = 1;
		stateOfPlay["Hex_3_4"] = 1;
		//stateOfPlay["Hex_3_3"] = 2;
		stateOfPlay["Hex_0_5"] = 2;
		stateOfPlay["Hex_3_7"] = 0;

	}
}



