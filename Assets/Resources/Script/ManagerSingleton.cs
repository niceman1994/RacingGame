using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerSingleton<T> : MonoBehaviour where T : MonoBehaviour 
{
	private static object _Lock = new object(); // 동시 접근할 수도 있으니 예외처리를 위해 만듦
	private static T instance;

	public static T Instance 
	{
		get 
		{
			lock (_Lock) 
			{
				if (instance == null) 
				{
					GameObject obj;
					obj = GameObject.Find(typeof(T).Name); // 게임오브젝트를 이름으로 찾아서 넣는다.

					if (obj == null) 
					{
						obj = new GameObject(typeof(T).Name);
						instance = obj.AddComponent<T>(); // 없어서 찾은 오브젝트에 컴포넌트 추가
					}
					else 
					{
						instance = obj.GetComponent<T>(); // 오브젝트가 있을 경우 해당 오브젝트의 컴포넌트를 가져온다.
					}
				}
				return instance;
			}
		}
	}

	public void Awake() 
	{
		DontDestroyOnLoad(gameObject);
	}
}