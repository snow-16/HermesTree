using UnityEngine;

[CreateAssetMenu(fileName = "PlayerDataBase", menuName = "Scriptable Objects/PlayerDataBase")]
public class PlayerDataBase : DataBase
{
    [SerializeField]
    private float _maxSpeed;
    public float MaxSpeed => _maxSpeed;

    [SerializeField]
    private float _basicAirResistance;
    public float BasicAirResistance => _basicAirResistance;

    [SerializeField]
    private float _controlAirResistance;
    public float ControlAirResistance => _controlAirResistance;

    [SerializeField]
    private float _frictionDamping;
    public float FrictionDamping => _frictionDamping;

    [SerializeField]
    private float _airDamping;
    public float AirDamping => _airDamping;

    [SerializeField]
    private float _jumpPower;
    public float JumpPower => _jumpPower;

    [SerializeField]
    private float _maxJumpRise;
    public float MaxJumpRise => _maxJumpRise;

    [SerializeField]
    private float _perseverePower;
    public float PerseverePower => _perseverePower;

    [SerializeField]
    private int _maxPersevere;
    public int MaxPersevere => _maxPersevere;

    [SerializeField]
    private float _hoveringBorder;
    public float HorveringBorder => _hoveringBorder;

    [SerializeField]
    private float _gravityOnRise;
    public float GravityOnRise => _gravityOnRise;

    [SerializeField]
    private float _gravityOnHover;
    public float GravityOnHover => _gravityOnHover;

    [SerializeField]
    private float _gravityOnFall;
    public float GravityOnFall => _gravityOnFall;

    [SerializeField]
    private float _coyoteTime;
    public float CoyoteTime => _coyoteTime;

    [SerializeField]
    private float _sightRange;
    public float SightRange => _sightRange;
}
