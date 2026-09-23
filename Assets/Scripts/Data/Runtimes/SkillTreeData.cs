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

    public void SetTree(TreeData data)
    {
        if(_treeDatas.ContainsKey(data.id))
        {
            _treeDatas[data.id] = data;
        }
        else
        {
            _treeDatas.Add(data.id, data);
        }
    }

    public void SetChart(ChartData data)
    {
        if(_chartDatas.ContainsKey(data.id))
        {
            _chartDatas[data.id] = data;
        }
        else
        {
            _chartDatas.Add(data.id, data);
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
        public Dictionary<int, CellData> cells;

        public TreeData(int dataId)
        {
            id = dataId;
            name = "";
            unlocked = new();
            cells = new();
        }

        public TreeData SetName(string newName)
        {
            name = newName;
            return this;
        }

        public TreeData AddUnlocked(int cellNumber)
        {
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

        public TreeData AddCell(int cellNumber, CellData data)
        {
            if(cells.ContainsKey(cellNumber))
            {
                Debug.LogError($"{name}の{cellNumber}番は登録済みです。");
                return this;
            }

            cells.Add(cellNumber, data);
            return this;
        }
    }

    [Serializable]
    public struct CellData
    {
        public CellType cellType;
        public List<int> connectedCells;

        public CellData(CellType type, List<int> connecteds = null)
        {
            connecteds ??= new();
            
            cellType = type;
            connectedCells = connecteds;
        }
    }

    [Serializable]
    public struct ChartData
    {
        public int id;
        public string name;
        public int perentTreeId;
        public List<IBranchData> selected;

        public ChartData(int dataId)
        {
            id = dataId;
            name = "";
            perentTreeId = default;
            selected = new();
        }

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
            selected.Add(cell);
            return this;
        }
    }

    [Serializable]
    public struct BranchCell : IBranchData
    {
        public int cellNumber;

        public BranchCell(int number)
        {
            cellNumber = number;
        }
    }

    [Serializable]
    public struct BranchTrigger : IBranchData
    {
        public int cellNumber;
        public List<IBranchData> connected;

        public BranchTrigger(int number)
        {
            cellNumber = number;
            connected = new();
        }

        public BranchTrigger AddCell(IBranchData cell)
        {
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

        public BranchSwitch(int number)
        {
            cellNumber = number;
            connectedTrue = new();
            connectedFalse = new();
        }

        public BranchSwitch AddCell(IBranchData cell, bool conditions)
        {
            if(conditions)
            {
                connectedTrue.Add(cell);
            }
            else
            {
                connectedFalse.Add(cell);
            }

            return this;
        }
    }

    public interface IBranchData{}
}
