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
	 
	public ArrayList neighbors = new ArrayList();

	string nameHex;

	public int[][] neighborsOfOdd = new int[][] // Список возможных соседей для нечетного гекса
	{
		new int[] {  0, 1 },
		new int[] {  1, 1 },
		new int[] {  1, 0 },
		new int[] {  1,-1 },
		new int[] {  0,-1 },
		new int[] { -1, 0 },

		new int[] { -1, 1 },
		new int[] { -1, 2 },
		new int[] {  0, 2 },
		new int[] {  1, 2 },
		new int[] {  2, 1 },//
		new int[] {  2, 0 },
		new int[] {  2,-1 },//
		new int[] {  1,-2 },
		new int[] {  0,-2 },
		new int[] { -1,-2 },
		new int[] { -1,-1 },
		new int[] { -2, 0 }
	};

	public int[][] neighborsOfEven = new int[][] // Список потенциально возможных ходов для четного гекса
	{
		new int[] { -1, 1 }, // Соседние клетки
		new int[] {  0, 1 },
		new int[] {  1, 0 },
		new int[] {  0,-1 },
		new int[] { -1,-1 },
		new int[] { -1, 0 },//
		
		new int[] { -1, 2 }, //       // Клетки через 1 гекс
		new int[] {  0, 2 },
		new int[] {  1, 2 },
		new int[] {  1, 1 },
		new int[] {  2, 0 },
		new int[] {  1,-1 },
		new int[] {  1,-2 },
		new int[] {  0,-2 },
		new int[] { -1,-2 },
		new int[] { -2,-1 },//
		new int[] { -2, 0 },//
		new int[] { -2, 1 } //
	};

	public void BacklightStrokes (int x, int y, ref int[][] listOfNeighbors) //Подсветка
	{
		
		// Проитерировать соответствующий список (массив масивов)
    	// Во время итерации проверять, если состояние (в словаре состояния) имеет значение 5, то изменить цвет (подсветить).
		neighbors.Clear(); //Очистить список подсвеченных соседей

		foreach (int[] element in listOfNeighbors)
		{
			nameHex = "Hex_" + (x + element[0]) + "_" + (y + element[1]);
			
			if (Map.stateOfPlay.ContainsKey(nameHex) && Map.stateOfPlay [nameHex] == 5)
			{
				GameObject hex = GameObject.Find(nameHex);
				hex.GetComponentInChildren<MeshRenderer>().material.color = Color.gray;
				neighbors.Add (nameHex);
				//Debug.Log(nameHex);
			}

			//Debug.Log ("Hex_" + (x + element[0]) + "_" + (y + element[1]));
			//GameObject check = GameObject.Find ("Hex_" + (x + element[0]) + "_" + (y + element[1]));
		}
	}

	public void ClearBacklight () //Очистка подсвеченных гексов
	{
		foreach (string element in neighbors)
		{
			GameObject hex = GameObject.Find(element);
			hex.GetComponentInChildren<MeshRenderer>().material.color = Color.white;
			Debug.Log (hex.GetComponentInChildren<MeshRenderer>().material.color);
		}
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

	void Start () 
	{
		
		TestGenerationOfGameState (width, height, ref stateOfPlay); // Инициализация тестового словаря состояния игры
		
		MapInitialization (width, height, ref stateOfPlay); // Генерация карты на основе ширины, высоты и словаря состояния игры
	
	}

	void Update ()
	{
		
	}
}



