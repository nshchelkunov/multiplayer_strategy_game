using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour 
{

	// Префаб элемента карты - гекса
	public GameObject hexPrefab;
	// Словарь состояния игры  Имя гекса : Состояние
	public static Dictionary<string, int> stateOfPlay = new Dictionary<string, int>();
	
	// Ширина, высота карты
	int width = 16; 
	int height = 16;

	// Константы смещения гексов
	const float xOffset = 0.882f;
	const float zOffset = 0.764f;
	 
	public ArrayList neighbors = new ArrayList();

	string nameHex;

	// Список возможных соседей для нечетного гекса
	public int[][] neighborsOfOdd = new int[][] 
	{
		// Соседние клетки
		new int[] {  0, 1 },
		new int[] {  1, 1 },
		new int[] {  1, 0 },
		new int[] {  1,-1 },
		new int[] {  0,-1 },
		new int[] { -1, 0 },
		// Клетки через 1 гекс
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

	// Список потенциально возможных ходов для четного гекса
	public int[][] neighborsOfEven = new int[][] 
	{
		new int[] { -1, 1 }, 
		new int[] {  0, 1 },
		new int[] {  1, 0 },
		new int[] {  0,-1 },
		new int[] { -1,-1 },
		new int[] { -1, 0 },
		
		new int[] { -1, 2 },      
		new int[] {  0, 2 },
		new int[] {  1, 2 },
		new int[] {  1, 1 },
		new int[] {  2, 0 },
		new int[] {  1,-1 },
		new int[] {  1,-2 },
		new int[] {  0,-2 },
		new int[] { -1,-2 },
		new int[] { -2,-1 },
		new int[] { -2, 0 },
		new int[] { -2, 1 } 
	};

	// Подсветка хода
	public void BacklightStrokes (int x, int y, ref int[][] listOfNeighbors) 
	{
		neighbors.Clear(); 

		foreach (int[] element in listOfNeighbors)
		{
			nameHex = "Hex_" + (x + element[0]) + "_" + (y + element[1]);
			
			if (Map.stateOfPlay.ContainsKey(nameHex) && Map.stateOfPlay [nameHex] == 5)
			{
				GameObject hex = GameObject.Find(nameHex);
				hex.GetComponentInChildren<MeshRenderer>().material.color = Color.gray;
				neighbors.Add (nameHex);
			}
		}
	}

	// Убрать подсветку
	public void ClearBacklight () 
	{
		foreach (string element in neighbors)
		{
			GameObject hex = GameObject.Find(element);
			hex.GetComponentInChildren<MeshRenderer>().material.color = Color.white;
			Debug.Log (hex.GetComponentInChildren<MeshRenderer>().material.color);
		}
	}	

	// Генерация карты из префабов на основе ширины, высоты и словаря состояния игры 
	void MapInitialization (int width, int height, ref Dictionary<string, int> stateOfPlay)
	{
		
		for (int x = 0; x < width; x++)
		{
			for (int y = 0; y < height; y++)
			{
				// Если 0, то объект не нужно генерировать (пустое место на карте).
				if (stateOfPlay["Hex_" + x + "_" + y] == 0) 
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
				
				// Если 1 то Объект, захвачен игроком А (клиент), гекс окрашивается в синий цвет.
				if (stateOfPlay["Hex_" + x + "_" + y] != 5 && stateOfPlay["Hex_" + x + "_" + y] == 1) 
				{
					MeshRenderer mr =  hex_go.GetComponentInChildren<MeshRenderer>();
					mr.material.color = Color.blue;
				}
				// Если 2 то Объект, захвачен игроком B (противник), гекс окрашивается в красный цвет.
				else if (stateOfPlay["Hex_" + x + "_" + y] == 2) 
				{
					MeshRenderer mr =  hex_go.GetComponentInChildren<MeshRenderer>();
					mr.material.color = Color.red;
				}
			}
		}
	}
	// Функция для генерации тестового словаря состояния игры 
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
		stateOfPlay["Hex_0_5"] = 2;
		stateOfPlay["Hex_3_7"] = 0;
	}

	void Start () 
	{
		
		TestGenerationOfGameState (width, height, ref stateOfPlay); 
		
		MapInitialization (width, height, ref stateOfPlay); 
	
	}

	void Update ()
	{
		
	}
}



