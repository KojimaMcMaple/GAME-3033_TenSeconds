using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///  The Source file name: BombController.cs
///  Author's name: Trung Le (Kyle Hunter)
///  Student Number: 101264698
///  Program description: Responsible for moving the individual bombs
///  Date last Modified: See GitHub
///  Revision History: See GitHub
/// </summary>
public class BombController : MonoBehaviour
{
    [SerializeField] private GlobalEnums.ObjType type_ = GlobalEnums.ObjType.ENEMY; //compares with hit obj, determines which pool to return to
    [SerializeField] private int damage_ = 10;
    [SerializeField] private float despawn_time_ = 0.1f;
    private Vector3 spawn_pos_;
    private BombManager bomb_manager_;
    private float timer_ = 0f;
    private bool has_hit_ = false;

    private void Awake()
    {
        bomb_manager_ = GameObject.FindObjectOfType<BombManager>();
    }

    private void OnEnable()
    {
        has_hit_ = false ;
        timer_ = 0f ;
    }

    private void FixedUpdate()
    {
        CheckBounds();
    }

    /// <summary>
    /// When bomb 
    /// </summary>
    private void CheckBounds()
    {
        timer_ += Time.deltaTime;
        if (timer_ > despawn_time_)
        {
            timer_ = 0f;
            bomb_manager_.ReturnBomb(this.gameObject, type_);
        }
    }

    /// <summary>
    /// Mutator for private variable
    /// </summary>
    /// <param name="value"></param>
    public void SetSpawnPos(Vector3 value)
    {
        spawn_pos_ = value;
    }


    /// <summary>
    /// When bomb collides with something, move bomb back to pool
    /// </summary>
    public void OnTriggerEnter(Collider other)
    {
        if (!has_hit_)
        {
            IDamageable<int> other_interface = other.gameObject.GetComponent<IDamageable<int>>();
            if (other_interface != null)
            {
                if (other_interface.obj_type == GlobalEnums.ObjType.PLAYER)
                {
                    other_interface.ApplyDamage(damage_);
                    has_hit_ = true;
                }
            }
        }
        bomb_manager_.ReturnBomb(this.gameObject, type_);
    }
}
