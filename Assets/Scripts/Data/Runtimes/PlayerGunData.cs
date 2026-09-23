using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerGunData : IData
{
    private bool _isAiming;
    public bool IsAiming => _isAiming;

    private Vector2 _targetPosition;
    public Vector2 TargetPosition => _targetPosition;
    public Vector2 WorldTargetPosition => Camera.main.ScreenToWorldPoint((Vector3)_targetPosition + new Vector3(0,0,-10));

    private MagazineData[] _magazineBases = new MagazineData[1];
    public MagazineData[] MagazineBases => _magazineBases;

    private MagazineData _magazine = new();
    public MagazineData Magazine => _magazine;
    public bool HasBallet => _magazine.bullets.Count > 0;
    public SkillTreeData.ChartData Bullet => _magazine.bullets[0];

    private int _selectedMagazineNumber;
    public int SelectedMagazineNumber => _selectedMagazineNumber;

    public void SetAiming(bool isAiming)
    {
        _isAiming = isAiming;
    }

    public void UpdatePosition(Vector2 pos)
    {
        _targetPosition = pos;
    }

    public void AddMagazine(int addend)
    {
        Array.Resize(ref _magazineBases, _magazineBases.Length + addend);
    }

    public void SetMagazine(int magazineNum, params SkillTreeData.ChartData[] bullet)
    {
        if(magazineNum > _magazineBases.Length - 1)
        {
            Debug.LogError($"マガジン番号{magazineNum}は不正です。");
            return;
        }

        var magazine = _magazineBases[magazineNum];
        magazine.bullets = bullet.ToList();
        _magazineBases[magazineNum] = magazine;
        ReloadMagazine();
    }

    public void UseMagazine()
    {
        _magazine.bullets.RemoveAt(0);
    }

    public void ReloadMagazine()
    {
        _magazine = _magazineBases[_selectedMagazineNumber].Clone();
        Debug.Log("リロード");
    }

    public void SwitchMagazine(bool isRight)
    {
        _selectedMagazineNumber = (int)Mathf.Repeat(_selectedMagazineNumber + (isRight ? 1 : -1), _magazineBases.Length);
    }

    public struct MagazineData
    {
        public List<SkillTreeData.ChartData> bullets;

        public MagazineData(int bulletCount)
        {
            bullets = new(new SkillTreeData.ChartData[bulletCount]);
        }

        public MagazineData Clone()
        {
            var clone = this;
            clone.bullets = new(bullets);
            return clone;
        }
    }
}
