using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///  The Source file name: BombManager.cs
///  Author's name: Trung Le (Kyle Hunter)
///  Student Number: 101264698
///  Program description: Manages the queue
///  Date last Modified: See GitHub
///  Revision History: See GitHub
/// </summary>
[System.Serializable]
public class BombManager : MonoBehaviour
{
    public Queue<GameObject> player_bomb_pool;
    public Queue<GameObject> enemy_bomb_pool;
    public int player_bomb_num;
    public int enemy_bomb_num;
    //public GameObject bomb_obj;

    private BombFactory factory_;

    private VfxManager vfx_manager_;

    private void Awake()
    {
        player_bomb_pool = new Queue<GameObject>();
        enemy_bomb_pool = new Queue<GameObject>();
        factory_ = GetComponent<BombFactory>();
        BuildBombPool(); //pre-build a certain num of bombs to improve performance

        vfx_manager_ = FindObjectOfType<VfxManager>();
    }

    /// <summary>
    /// Builds a pool of bombs in bomb_num amount
    /// </summary>
    private void BuildBombPool()
    {
        for (int i = 0; i < player_bomb_num; i++)
        {
            PreAddBomb(GlobalEnums.ObjType.PLAYER);
        }
        for (int i = 0; i < enemy_bomb_num; i++)
        {
            PreAddBomb(GlobalEnums.ObjType.ENEMY);
        }
    }

    /// <summary>
    /// Based on AddBomb() without num++, otherwise would cause infinite loop
    /// </summary>
    private void PreAddBomb(GlobalEnums.ObjType type = GlobalEnums.ObjType.PLAYER)
    {
        //var temp = Instantiate(bomb_obj, this.transform);
        var temp = factory_.CreateBomb(type);

        switch (type)
        {
            case GlobalEnums.ObjType.PLAYER:
                player_bomb_pool.Enqueue(temp);
                break;
            case GlobalEnums.ObjType.ENEMY:
                enemy_bomb_pool.Enqueue(temp);
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Uses the factory to spawn one object, add it to the queue, and increase the pool size 
    /// </summary>
    private void AddBomb(GlobalEnums.ObjType type = GlobalEnums.ObjType.PLAYER)
    {
        //var temp = Instantiate(bomb_obj, this.transform);
        var temp = factory_.CreateBomb(type);

        switch (type)
        {
            case GlobalEnums.ObjType.PLAYER:
                //temp.SetActive(false);
                player_bomb_pool.Enqueue(temp);
                player_bomb_num++;
                break;
            case GlobalEnums.ObjType.ENEMY:
                //temp.SetActive(false);
                enemy_bomb_pool.Enqueue(temp);
                enemy_bomb_num++;
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// // Removes a bomb from the pool and return a ref to it
    /// </summary>
    /// <param name="position"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    public GameObject GetBomb(Vector3 position,
                                GlobalEnums.ObjType type = GlobalEnums.ObjType.ENEMY)
    {
        GameObject temp = null;
        switch (type)
        {
            case GlobalEnums.ObjType.PLAYER:
                if (player_bomb_pool.Count < 1) //add one bomb if pool empty
                {
                    AddBomb(GlobalEnums.ObjType.PLAYER);
                }
                temp = player_bomb_pool.Dequeue();
                break;
            case GlobalEnums.ObjType.ENEMY:
                if (enemy_bomb_pool.Count < 1) //add one bomb if pool empty
                {
                    AddBomb(GlobalEnums.ObjType.ENEMY);
                }
                temp = enemy_bomb_pool.Dequeue();
                break;
            default:
                break;
        }
        temp.transform.position = position;
        //temp.transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, forward_direction, 2f * 3.14f, 0.0f), Vector3.up);
        temp.GetComponent<BombController>().SetSpawnPos(position);
        temp.SetActive(true);
        vfx_manager_.GetVfx(GlobalEnums.VfxType.BOMB, position, Vector3.forward);
        return temp;
    }

    /// <summary>
    /// Returns a bomb back into the pool
    /// </summary>
    /// <param name="returned_bomb"></param>
    /// <param name="type"></param>
    public void ReturnBomb(GameObject returned_bomb, GlobalEnums.ObjType type = GlobalEnums.ObjType.PLAYER)
    {
        returned_bomb.SetActive(false);

        switch (type)
        {
            case GlobalEnums.ObjType.PLAYER:
                player_bomb_pool.Enqueue(returned_bomb);
                break;
            case GlobalEnums.ObjType.ENEMY:
                enemy_bomb_pool.Enqueue(returned_bomb);
                break;
            default:
                break;
        }
    }
}
