using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseManager : MonoBehaviour 
{	
	public Color playerСolor = Color.blue; 

	void Update () 
	{	
		// EventSystem.current Возвращает текущее EventSystem.
		if (EventSystem.current.IsPointerOverGameObject()) 
		{
			return;
		}
		// Создает луч в точку, где находится указатель мыши
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition); 
		// Структура, для хранения инфо о поверхности
		RaycastHit hitInfo; 
		
		// Если луч встречат коллайдер, то возвращает "истина" + структуру данных
		if (Physics.Raycast (ray, out hitInfo)) 
		{
			// Доступ к обьекту, на котором мышь
			GameObject ourHitObject = hitInfo.collider.transform.parent.gameObject; 

			if (ourHitObject.GetComponent<Hex>() != null)
			{
				MouseOver_Hex (ourHitObject); //Это гекс!
			}
		}
	}
	
	void MouseOver_Hex (GameObject ourHitObject)
	{
		// Если нажата левая кнопка мыши
		if (Input.GetMouseButtonDown(0)) 
		{	
			GameObject mapObject = GameObject.Find("Map");
			// GetComponentInChildren(Type t) возвращяет компонент типа t
			MeshRenderer mr =  ourHitObject.GetComponentInChildren<MeshRenderer>(); 
			
			// Сохраняем координаты гекса
			int x = ourHitObject.GetComponent<Hex>().x; 
			int y = ourHitObject.GetComponent<Hex>().y;

			// Если это гекс игрока, то подсвечиваем возможные ходы
			if (mr.material.color == playerСolor) 
			{
				//Убрать подсветку
				mapObject.GetComponent<Map>().ClearBacklight (); 

				if (y % 2 == 1)
				{
					//Новая подсветка
					mapObject.GetComponent<Map>().BacklightStrokes (x, y, ref mapObject.GetComponent<Map>().neighborsOfOdd); 
				}
				else
				{
					 mapObject.GetComponent<Map>().BacklightStrokes (x, y, ref mapObject.GetComponent<Map>().neighborsOfEven);
				}
			}
			else
			{
				string name = "Hex_" + x  + "_" + y;
				// Если этот ход возможен (был подсвеченным)
				if (mapObject.GetComponent<Map>().neighbors.Contains(name)) 
				{
					// Отправить на сервер
					// Убрать подсветку
					mapObject.GetComponent<Map>().ClearBacklight(); 
				}
			}
		}
	}
}


