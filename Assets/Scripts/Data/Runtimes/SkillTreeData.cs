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
            Debug.Log($"{data.id}番のツリーを保存しました。");
        }
        else
        {
            _treeDatas.Add(data.id, data);
            Debug.Log($"{data.id}番のツリーを追加しました。");
        }

        _treeDatas = _treeDatas.OrderBy(pair => pair.Key).ToList().ToDictionary(pair => pair.Key, pair => pair.Value);
    }

    public void SetChart(ChartData data)
    {
        if(_chartDatas.ContainsKey(data.id))
        {
            _chartDatas[data.id] = data;
            Debug.Log($"{data.id}番のチャートを保存しました。");
        }
        else
        {
            _chartDatas.Add(data.id, data);
            Debug.Log($"{data.id}番のチャートを追加しました。");
        }

        _chartDatas = _chartDatas.OrderBy(pair => pair.Key).ToList().ToDictionary(pair => pair.Key, pair => pair.Value);
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
    public class TreeData
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

        public bool IsCellUnlocked(SimplePosition cellPosition)
        {
            return unlocked.Contains(cellPosition);
        }

        public TreeData Clone()
        {
            return new TreeData(id)
            {
                name = new(name),
                unlocked = new(unlocked),
                cells = new(cells.ToDictionary(cell => cell.Key, cell => cell.Value.Clone()))
            };
        }
    }

    [Serializable]
    public struct CellData
    {
        public CellType cellType;
        public SimplePosition connectFrom;
        public List<SimplePosition> connectedCells;

        public CellData(CellType type, SimplePosition from, List<SimplePosition> connecteds = null)
        {
            connecteds ??= new();
            
            cellType = type;
            connectFrom = from;
            connectedCells = connecteds;
        }

        public CellData Clone()
        {
            return new CellData(cellType, connectFrom, new(connectedCells));
        }
    }

    [Serializable]
    public class ChartData
    {
        public int id;
        public string name;
        public int perentTreeId;
        public List<IBranchData> selected;
        public List<SimplePosition> allSelecteds;
        public List<SelectableCells> canSelectings;

        public ChartData(int dataId)
        {
            id = dataId;
            name = "";
            perentTreeId = default;
            selected = new();
            allSelecteds = new();
            canSelectings = new(){new(0, 0)};
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

        public ChartData AddCell(SimplePosition cellPosition, CellData cell)
        {
            canSelectings.ForEach(selectables => 
            {
                if(selectables.selectables.Contains(cellPosition))
                {
                    selectables.AddConnection();
                }
            });
            canSelectings.RemoveAll(selectables => selectables.IsMaxSelected());

            IBranchData branch;
            var processor = DataManager.ReadData<CellDataBase>().CellList[cell.cellType].Processor;

            if(processor is TriggerProcessor)
            {
                branch = new BranchTrigger(cellPosition);
            }
            else if(processor is SwitchProcessor)
            {
                branch = new BranchSwitch(cellPosition);
            }
            else
            {
                branch = new BranchCell(cellPosition);
            }
            
            if(selected.Count > 0 && selected.Last().TryGetLastCell(cellPosition, branch) != null)
            {
                selected.Add(branch);
            }

            canSelectings.Add(new(cell.connectedCells, branch is SwitchProcessor));
            allSelecteds.Add(cellPosition);

            return this;
        }

        public bool CanSelecting(SimplePosition cellPosition)
        {
            return canSelectings.Any(selectables => selectables.selectables.Contains(cellPosition));
        }

        public ChartData Clone()
        {
            return new ChartData(id)
            {
                name = new(name),
                perentTreeId = perentTreeId,
                selected = selected.Select(branch => branch.Clone()).ToList(),
                allSelecteds = new(allSelecteds),
                canSelectings = canSelectings.Select(selectables => selectables.Clone()).ToList()
            };
        }
    }

    [Serializable]
    public struct SelectableCells
    {
        public List<SimplePosition> selectables;
        private bool isBranchSwitch;
        private int connectedCount;

        public SelectableCells(int x, int y)
        {
            selectables = new(){new(x, y)};
            isBranchSwitch = false;
            connectedCount = 0;
        }

        public SelectableCells(List<SimplePosition> connected, bool isSwitch)
        {
            selectables = connected;
            isBranchSwitch = isSwitch;
            connectedCount = 0;
        }

        public void AddConnection()
        {
            connectedCount++;
        }

        public bool IsMaxSelected()
        {
            return connectedCount == (isBranchSwitch ? 2 : 1);
        }

        public SelectableCells Clone()
        {
            return new(new(selectables), isBranchSwitch);
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

        public IBranchData Clone()
        {
            return this;
        }

        public BranchCell? TryGetLastCell(SimplePosition cellPosition, IBranchData branch)
        {
            return this;
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

        public IBranchData Clone()
        {
            return new BranchTrigger(cellPosition)
            {
                connected = connected.Select(branch => branch.Clone()).ToList()
            };
        }

        public BranchCell? TryGetLastCell(SimplePosition cellPosition, IBranchData branch)
        {
            var lastBranch = connected.Last().TryGetLastCell(cellPosition, branch);
            if(lastBranch.HasValue)
            {
                if(lastBranch.Value.cellPosition.Equals(cellPosition))
                {
                    connected.Add(branch);
                }
            }

            return null;
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

        public IBranchData Clone()
        {
            return new BranchSwitch(cellPosition)
            {
                connectedTrue = connectedTrue.Select(branch => branch.Clone()).ToList(),
                connectedFalse = connectedFalse.Select(branch => branch.Clone()).ToList()
            };
        }

        public BranchCell? TryGetLastCell(SimplePosition cellPosition, IBranchData branch)
        {
            TryInsert(connectedTrue, cellPosition, branch);
            TryInsert(connectedFalse, cellPosition, branch);

            return null;
        }

        private void TryInsert(List<IBranchData> connected, SimplePosition cellPosition, IBranchData branch)
        {
            var lastBranch = connected.Last().TryGetLastCell(cellPosition, branch);
            if(lastBranch.HasValue)
            {
                if(lastBranch.Value.cellPosition.Equals(cellPosition))
                {
                    connected.Add(branch);
                }
            }
        }
    }

    public interface IBranchData
    {
        IBranchData Clone();
        BranchCell? TryGetLastCell(SimplePosition cellPosition, IBranchData branch);
    }
}
