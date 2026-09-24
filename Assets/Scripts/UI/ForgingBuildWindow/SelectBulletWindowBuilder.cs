using System.Linq;

public class SelectBulletWindowBuilder : SelectWindowBuilder<SelectBulletButton>
{
    private BulletDataBase _bulletDataBase;

    protected override void Build()
    {
        _bulletDataBase = DataManager.ReadData<BulletDataBase>();

        _bulletDataBase.BulletList.Select(bullet => bullet.Key).ToList().ForEach(bullet =>
        {
            CreateButton().SetType(bullet);
        });
    }
}
