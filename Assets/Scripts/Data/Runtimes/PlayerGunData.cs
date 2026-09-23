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

    private Magazine[] _magazineBases = new Magazine[1];
    public Magazine[] MagazineBases => _magazineBases;

    private Magazine[] _magazines = new Magazine[1];
    public Magazine[] Magazines => _magazines;
    public Magazine SelectedMagazine => _magazines[_selectedMagazineNumber];
    public bool HasBallet => SelectedMagazine.bullets.Count > 0;

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
        Array.Resize(ref _magazineBases, _magazines.Length + addend);
        Array.Resize(ref _magazines, _magazineBases.Length);
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
        var magazine = _magazines[_selectedMagazineNumber];
        magazine.bullets.RemoveAt(0);
        _magazines[_selectedMagazineNumber] = magazine;
    }

    public void ReloadMagazine()
    {
        _magazines[_selectedMagazineNumber] = _magazineBases[_selectedMagazineNumber].Clone();
        Debug.Log("リロード");
    }

    public void SwitchMagazine(bool isRight)
    {
        _selectedMagazineNumber = (int)Mathf.Repeat(_selectedMagazineNumber + (isRight ? 1 : -1), _magazines.Length);
    }

    public struct Magazine
    {
        public List<SkillTreeData.ChartData> bullets;

        public Magazine(int bulletCount)
        {
            bullets = new(new SkillTreeData.ChartData[bulletCount]);
        }

        public Magazine Clone()
        {
            var clone = this;
            clone.bullets = new(bullets);
            return clone;
        }
    }
}
