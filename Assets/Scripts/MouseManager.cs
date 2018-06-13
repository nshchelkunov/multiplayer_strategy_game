using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseManager : MonoBehaviour 
{

	//string pressedFirst, pressedSecond; // Нажатие на гекс игрока
	public Color playerСolor = Color.blue; // Цвет гекса игрока


	void Update () 
	{
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
			GameObject mapObject = GameObject.Find("Map");
			
			MeshRenderer mr =  ourHitObject.GetComponentInChildren<MeshRenderer>(); // GetComponentInChildren(Type t) возвращяет компонент типа t
			
			int x = ourHitObject.GetComponent<Hex>().x; // Сохраняем координаты гекса
			int y = ourHitObject.GetComponent<Hex>().y;

			if (mr.material.color == playerСolor) // Если это гекс игрока, то подсвечиваем возможные ходы
			{
				mapObject.GetComponent<Map>().ClearBacklight (); //Убрать подсветку

				if (y % 2 == 1)
				{
					mapObject.GetComponent<Map>().BacklightStrokes (x, y, ref mapObject.GetComponent<Map>().neighborsOfOdd); //Новая подсветка
				}
				else
				{
					 mapObject.GetComponent<Map>().BacklightStrokes (x, y, ref mapObject.GetComponent<Map>().neighborsOfEven);
				}
			}
			else
			{
				string name = "Hex_" + x  + "_" + y;
				if (mapObject.GetComponent<Map>().neighbors.Contains(name)) // Если этот ход возможен (был подсвеченным)
				{
					//Debug.Log ("Если этот ход возможен (был подсвеченным)" + name);
					// Отправить на сервер
					mapObject.GetComponent<Map>().ClearBacklight(); //Убрать подсветку
				}
			}
		}
	}
}


