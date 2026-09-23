using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SkillTreeData : IData
{
    private Dictionary<int, TreeData> _treeDatas = new();
    public Dictionary<int, TreeData> TreeDatas => _treeDatas;

    private Dictionary<int, ChartData> _chartDatas = new();
    public Dictionary<int, ChartData> ChartDatas => _chartDatas;

    public void SetTree(int id, TreeData data)
    {
        if(_treeDatas.ContainsKey(id))
        {
            _treeDatas[id] = data;
        }
        else
        {
            _treeDatas.Add(id, data);
        }
    }

    public void SetChart(int id, ChartData data)
    {
        if(_chartDatas.ContainsKey(id))
        {
            _chartDatas[id] = data;
        }
        else
        {
            _chartDatas.Add(id, data);
        }
    }

    public void RemoveTree(int id)
    {
        _treeDatas.Remove(id);
        _chartDatas.Where(pair => pair.Value.perentTreeId == id).ToList().ForEach(pair =>
        {
            _chartDatas.Remove(pair.Key);
        });
    }

    public void RemoveChart(int id)
    {
        _chartDatas.Remove(id);
    }

    [Serializable]
    public struct TreeData
    {
        public int id;
        public string name;
        public List<int> unlocked;
        public Dictionary<int, CellSettingData> cells;

        public TreeData SetName(string newName)
        {
            name = newName;
            return this;
        }

        public TreeData AddUnlocked(int cellNumber)
        {
            unlocked ??= new();

            if(unlocked.Contains(cellNumber))
            {
                Debug.LogError($"{name}の{cellNumber}番は解放済みです。");
                return this;
            }
            else if(!cells.ContainsKey(cellNumber))
            {
                Debug.LogError($"{name}の{cellNumber}番は未登録です。");
                return this;
            }

            unlocked.Add(cellNumber);
            return this;
        }

        public TreeData AddCell(int cellNumber, CellSettingData cellData)
        {
            cells ??= new();

            if(cells.ContainsKey(cellNumber))
            {
                Debug.LogError($"{name}の{cellNumber}番は登録済みです。");
                return this;
            }

            cells.Add(cellNumber, cellData);
            return this;
        }
    }

    [Serializable]
    public struct ChartData
    {
        public int id;
        public string name;
        public int perentTreeId;
        public List<IBranchData> selected;

        public ChartData SetName(string newName)
        {
            name = newName;
            return this;
        }

        public ChartData SetPerentTree(TreeData perent)
        {
            perentTreeId = perent.id;
            return this;
        }

        public ChartData AddCell(IBranchData cell)
        {
            selected ??= new();
            selected.Add(cell);
            return this;
        }
    }

    [Serializable]
    public struct BranchCell : IBranchData
    {
        public int cellNumber;
    }

    [Serializable]
    public struct BranchTrigger : IBranchData
    {
        public int cellNumber;
        public List<IBranchData> connected;

        public BranchTrigger AddCell(IBranchData cell)
        {
            connected ??= new();
            connected.Add(cell);
            return this;
        }
    }

    [Serializable]
    public struct BranchSwitch : IBranchData
    {
        public int cellNumber;
        public List<IBranchData> connectedTrue;
        public List<IBranchData> connectedFalse;

        public BranchSwitch AddCell(IBranchData cell, bool conditions)
        {
            if(conditions)
            {
                connectedTrue ??= new();
                connectedTrue.Add(cell);
            }
            else
            {
                connectedFalse ??= new();
                connectedFalse.Add(cell);
            }

            return this;
        }
    }

    public interface IBranchData{}
}
