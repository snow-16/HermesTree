using Unity.Cinemachine;
using UnityEngine;

public class HorizontalScroll : CinemachineExtension
{
    [SerializeField]
    private float _height;

    protected override void PostPipelineStageCallback(CinemachineVirtualCameraBase vcam, CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
    {
        if(enabled && stage == CinemachineCore.Stage.Finalize)
        {
            var pos = state.RawPosition;
            pos.y = _height;
            state.RawPosition = pos;
        }
    }
}
