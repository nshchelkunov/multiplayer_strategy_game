using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseManager : MonoBehaviour {

	bool clickIndicator; // Нажатие на гекс игрока
	public Color playerСolor = Color.blue; // Цвет гекса игрока

	int[][] neighborsOfOdd = new int[][] // Список возможных соседей для нечетного гекса
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
	int[][] neighborsOfEven = new int[][] // Список потенциально возможных ходов для четного гекса
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


	void Update () {

		if (EventSystem.current.IsPointerOverGameObject()) // EventSystem.current Возвращает текущее EventSystem.
		{
			return;
		}
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition); // Создает луч в точку, где находится указатель мыши
		RaycastHit hitInfo; // Структура, для хранения инфо о поверхности
		
		if (Physics.Raycast (ray, out hitInfo)) // Если луч встречат коллайдер, то возвращает тру + структуру данных
		{
			GameObject ourHitObject = hitInfo.collider.transform.parent.gameObject; // Доступ к обьекту, на котором мышь

			if (ourHitObject.GetComponent<Hex>() != null)
			{
				MouseOver_Hex (ourHitObject); //Это гекс!
			}
		}
	}
	void MouseOver_Hex (GameObject ourHitObject)
	{
		if (Input.GetMouseButtonDown(0)) // Если нажата левая кнопка мыши
		{	
			MeshRenderer mr =  ourHitObject.GetComponentInChildren<MeshRenderer>(); // GetComponentInChildren(Type t) возвращяет компонент типа t
			
			if (mr.material.color == playerСolor) // Если это гекс игрока, то подсвечиваем возможные ходы
			{

				int x = ourHitObject.GetComponent<Hex>().x; // Сохраняем координаты гекса
				int y = ourHitObject.GetComponent<Hex>().y; 
				
				if (y % 2 == 1)
				{
					FreeHexDesignation (x, y, ref neighborsOfOdd);
				}
				else
				{
					FreeHexDesignation (x, y, ref neighborsOfEven);
				}
			}
		}
	}

	void FreeHexDesignation (int x, int y, ref int[][] listOfNeighbors)
	{
		
		// Проитерировать соответствующий список (массив масивов)
    	// Во время итерации проверять, если состояние (в словаре состояния) имеет значение 5, то изменить цвет (подсветить).
		foreach (int[] element in listOfNeighbors)
		{
			string nameHex = "Hex_" + (x + element[0]) + "_" + (y + element[1]);
			
			if (Map.stateOfPlay.ContainsKey(nameHex) && Map.stateOfPlay [nameHex] == 5)
			{
				GameObject hex = GameObject.Find(nameHex);
				hex.GetComponentInChildren<MeshRenderer>().material.color = Color.gray;

			}

			

			//Debug.Log ("Hex_" + (x + element[0]) + "_" + (y + element[1]));
			//GameObject check = GameObject.Find ("Hex_" + (x + element[0]) + "_" + (y + element[1]));
		}

	}
}


