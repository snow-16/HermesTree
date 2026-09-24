using UnityEngine;

public abstract class SelectWindowBuilder<T> : MonoBehaviour
where T : CustomButton
{
    [SerializeField]
    protected Transform _buttonLine;

    protected GameObject _buttonBase;

    void OnEnable()
    {
        _buttonBase = _buttonLine.GetChild(0).gameObject;
        _buttonBase.SetActive(false);
        Build();
    }

    void OnDisable()
    {
        foreach(Transform child in _buttonLine)
        {
            if(child.gameObject != _buttonBase)
            {
                Destroy(child.gameObject);
            }
        }
    }

    protected abstract void Build();

    protected T CreateButton()
    {
        var button = Instantiate(_buttonBase);
        button.transform.SetParent(_buttonLine);
        button.transform.localScale = _buttonBase.transform.localScale;
        button.SetActive(true);
        return button.GetComponent<T>();
    }
}
