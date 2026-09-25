using UnityEngine;

public class CreateMagazineBuilder : MonoBehaviour
{
    [SerializeField]
    private Transform _magazineButtonPerent;

    public void OpenMenu(ForgingMenuBuilder.MenuBuilder builder)
    {
        foreach(Transform child in _magazineButtonPerent)
        {
            child.GetComponent<SelectMagazineCaseIndexButton>().SetMagazine(builder.magazine);
        }
    }
}
