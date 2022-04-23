using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///  The Source file name: BombFactory.cs
///  Author's name: Trung Le (Kyle Hunter)
///  Student Number: 101264698
///  Program description: Manages the type of object to spawn
///  Date last Modified: See GitHub
///  Revision History: See GitHub
/// </summary>
[System.Serializable]
public class BombFactory : MonoBehaviour
{
    [Header("Bomb Types")]
    public GameObject enemy_bomb;
    public GameObject player_bomb;

    /// <summary>
    /// Instantiates an object and returns a reference to it
    /// </summary>
    /// <returns></returns>
    public GameObject CreateBomb(GlobalEnums.ObjType type = GlobalEnums.ObjType.PLAYER)
    {
        GameObject temp = null;
        switch (type)
        {
            case GlobalEnums.ObjType.PLAYER:
                temp = Instantiate(player_bomb, this.transform);
                break;
            case GlobalEnums.ObjType.ENEMY:
                temp = Instantiate(enemy_bomb, this.transform);
                break;
            default:
                break;
        }
        temp.SetActive(false);
        return temp;
    }
}
