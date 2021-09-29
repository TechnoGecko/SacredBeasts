
using UnityEngine;
using Animancer;

namespace Characters.States
{
  public class IdleState : CharacterState
  {
    [SerializeField] private ClipTransition _Animation;

#if UNITY_EDITOR
    /// <inheritdoc/>
    protected override void OnValidate()
    {
      base.OnValidate();
      _Animation?.Clip.EditModeSampleAnimation(Character);
    }
#endif


    public override void OnEnterState()
    {
    base.OnEnterState();
    Character.Animancer.Play(_Animation);
    }

  }
}