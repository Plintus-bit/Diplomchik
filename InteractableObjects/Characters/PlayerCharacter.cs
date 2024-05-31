using Player;
using UnityEngine;

public class PlayerCharacter : Character
{
    [SerializeField] private PlayerAbilities _abilities;
    [SerializeField] private PlayerInput _input;
    [SerializeField] private Rigidbody _rb;

    public PlayerAbilities Abilities => _abilities;
    public PlayerInput PlayerInput => _input;
    public Rigidbody PlayerRB => _rb;
}