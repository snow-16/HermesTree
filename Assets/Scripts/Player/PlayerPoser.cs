using UnityEngine;
using UnityEngine.Events;

public class PlayerPoser : MonoBehaviour
{
    [SerializeField]
    private UnityEvent _onPose;

    void Start()
    {
        InputObserver.AddListener()
        .SetInput(InputObserver.InputMap.Player.Pose)
        .SetType(InputType.NowPressed)
        .SetAction(OnPose)
        .SetOutputProcessing(input => new())
        .SetListenerObject(gameObject)
        .Build();
    }

    private void OnPose(Vector2 input)
    {
        _onPose?.Invoke();
    }
}
