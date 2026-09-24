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
        public List<SimplePosition> unlocked;
        public Dictionary<SimplePosition, CellData> cells;

        public TreeData(int dataId)
        {
            id = dataId;
            name = "";
            unlocked = new();
            cells = new();
        }

        public TreeData SetId(int newId)
        {
            id = newId;
            return this;
        }

        public TreeData SetName(string newName)
        {
            name = newName;
            return this;
        }

        public TreeData AddUnlocked(SimplePosition cellPosition)
        {
            if(unlocked.Contains(cellPosition))
            {
                Debug.LogError($"{name}の{cellPosition}番は解放済みです。");
                return this;
            }
            else if(!cells.ContainsKey(cellPosition))
            {
                Debug.LogError($"{name}の{cellPosition}番は未登録です。");
                return this;
            }

            unlocked.Add(cellPosition);
            return this;
        }

        public TreeData AddCell(SimplePosition cellPosition, CellData data)
        {
            if(cells.ContainsKey(cellPosition))
            {
                Debug.LogError($"{name}の{cellPosition}番は登録済みです。");
                return this;
            }

            cells.Add(cellPosition, data);
            return this;
        }
    }

    [Serializable]
    public struct CellData
    {
        public CellType cellType;
        public SimplePosition connectFrom;
        public List<SimplePosition> connectedCells;

        public CellData(CellType type, List<SimplePosition> connecteds = null)
        {
            connecteds ??= new();
            
            cellType = type;
            connectFrom = new();
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
        public SimplePosition cellPosition;

        public BranchCell(SimplePosition position)
        {
            cellPosition = position;
        }
    }

    [Serializable]
    public struct BranchTrigger : IBranchData
    {
        public SimplePosition cellPosition;
        public List<IBranchData> connected;

        public BranchTrigger(SimplePosition position)
        {
            cellPosition = position;
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
        public SimplePosition cellPosition;
        public List<IBranchData> connectedTrue;
        public List<IBranchData> connectedFalse;

        public BranchSwitch(SimplePosition position)
        {
            cellPosition = position;
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
