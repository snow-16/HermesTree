using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class SkillTreeSwiper : MonoBehaviour
{
    private bool _isSwiping;

    private SkillTreeDataBase _skillTreeDataBase;

    void Start()
    {
        _skillTreeDataBase = DataManager.ReadData<SkillTreeDataBase>();
    }

    void Update()
    {
        if(Mouse.current.leftButton.wasPressedThisFrame && !EventSystem.current.IsPointerOverGameObject())
        {
            _isSwiping = true;
        }
        else if(_isSwiping && !Mouse.current.leftButton.isPressed)
        {
            _isSwiping = false;
        }

        if(_isSwiping)
        {
            transform.position += (Vector3)Pointer.current.delta.ReadValue() * _skillTreeDataBase.SwipeSpeed;
        }
    }
}
