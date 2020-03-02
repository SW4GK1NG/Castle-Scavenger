using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour
{
    private static GameData instance = null;
	
	public static GameData Instance
	{
		get {
			if (instance == null)
			{
				instance = new GameData();
			}
			return instance;
		}
	}

    int DeathCounter = 0;
    public int death {
        get {
            return this.DeathCounter;
        }

        set{
            DeathCounter = value;
        }
    }
}
