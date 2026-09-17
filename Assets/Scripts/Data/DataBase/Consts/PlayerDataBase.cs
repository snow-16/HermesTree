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
}
