using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapController : MonoBehaviour
{
    [SerializeField] private int anim_idx_idle_ = 1;
    [SerializeField] private int anim_idx_atk_ = 0;
    [SerializeField] private float fire_duration_ = 5f;
    private float curr_fire_duration_ = 0f;
    [SerializeField] private float fire_cooldown_ = 1.5f;
    private float curr_fire_cooldown_ = 0f;
    [SerializeField] private bool is_continuous_fire_ = false;
    [SerializeField] private int damage_ = 10;

    private GlobalEnums.EnemyState state_ = GlobalEnums.EnemyState.ATTACK;
    private Animation anim_;
    private List<AnimationState> anim_states_;
    private bool has_anim_ = false;

    void Awake()
    {
        anim_ = GetComponentInParent(typeof(Animation)) as Animation;
        anim_states_ = new List<AnimationState>();
        if (anim_ != null)
        {
            foreach (AnimationState s in anim_)
            {
                anim_states_.Add(s);
            }
            has_anim_ = true;
        }
        else
        {
            has_anim_ = false;
        }

        curr_fire_duration_ = fire_duration_;
        curr_fire_cooldown_ = fire_cooldown_;

        if (anim_idx_idle_ < 0)
        {
            is_continuous_fire_ = true; //forces is_continuous_fire_ when no idle anim
        }

        if (is_continuous_fire_)
        {
            if (has_anim_)
            {
                anim_.Play(anim_states_[anim_idx_atk_].name);
            }
        }
    }

    void Update()
    {
        if (is_continuous_fire_)
        {
            return;
        }

        switch (state_) //state machine
        {
            case GlobalEnums.EnemyState.IDLE:
                if (has_anim_)
                {
                    anim_.Play(anim_states_[anim_idx_idle_].name);
                }
                curr_fire_cooldown_ -= Time.deltaTime;
                if (curr_fire_cooldown_ < 0)
                {
                    curr_fire_duration_ = fire_duration_;
                    state_ = GlobalEnums.EnemyState.ATTACK;
                }
                break;
            case GlobalEnums.EnemyState.ATTACK:
                if (has_anim_)
                {
                    anim_.Play(anim_states_[anim_idx_atk_].name);
                }
                curr_fire_duration_ -= Time.deltaTime;
                if (curr_fire_duration_ < 0)
                {
                    curr_fire_cooldown_ = fire_cooldown_;
                    state_ = GlobalEnums.EnemyState.IDLE;
                }
                break;
            default:
                break;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log(">>> Trap OnCollisionEnter");
        if (state_ == GlobalEnums.EnemyState.ATTACK)
        {
            IDamageable<int> other_interface = collision.gameObject.GetComponent<IDamageable<int>>();
            if (other_interface != null)
            {
                other_interface.ApplyDamage(damage_);
                if (!is_continuous_fire_)
                {
                    curr_fire_cooldown_ = fire_cooldown_;
                    state_ = GlobalEnums.EnemyState.IDLE;
                    //collision.collider.attachedRigidbody.AddForce(-collision.impulse);
                }
            }
        }
    }
}
