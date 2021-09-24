using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Animancer;
using Animancer.FSM;
using Characters;


public class Character : MonoBehaviour
{
    [SerializeField]
    private CharacterAnimancerComponent _Animancer;
    public CharacterAnimancerComponent Animancer => _Animancer;

    [SerializeField]
    private CharacterBody2D _Body;
    public CharacterBody2D Body => _Body;

    [SerializeField]
    private Health _Health;
    public Health Health => _Health;

    [SerializeField]
    private CharacterState _Idle;
    public CharacterState Idle => _Idle;
    
#if UNITY_EDITOR
    /// <summary>[Editor-Only] Ensures that all fields have valid values and finds missing components nearby.</summary>
    protected virtual void OnValidate()
    {
        gameObject.GetComponentInParentOrChildren(ref _Animancer);
        gameObject.GetComponentInParentOrChildren(ref _Body);
        gameObject.GetComponentInParentOrChildren(ref _Health);
        gameObject.GetComponentInParentOrChildren(ref _Idle);
    }
#endif
}
