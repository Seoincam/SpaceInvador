#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using TimeKit.Editor.Debugging.Data;
using UnityEngine;
using UnityEngine.UIElements;

namespace TimeKit.Editor.Debugging.Windows
{
    public sealed partial class DebugWindow
    {
        private sealed class SummaryTabController
        {
            private readonly MultiColumnListView _mcList;

            public event Action<ClockType> ClockRowChosen;

            public SummaryTabController(VisualElement root)
            {
                _mcList = root.Q<MultiColumnListView>("clocks-mc-list");
            }

            public void ConfigMcList()
            {
                _mcList.showAlternatingRowBackgrounds = AlternatingRowBackground.All;
                _mcList.selectionType = SelectionType.Single;
                
                var infos = ClockDebugManager.Infos;
            
                // itemSource
                _mcList.itemsSource = infos;
            
                // makeCell
                var columnNames = new[] { "clock-type", "time", "delta-time", "time-scale", "linked" };
                foreach (var name in columnNames)
                {
                    _mcList.columns[name].makeCell = () =>
                    {
                        var label = new Label
                        {
                            enableRichText = true
                        };
                        label.style.unityTextAlign = TextAnchor.MiddleLeft;
                        label.style.paddingLeft = 8;
                        label.style.height = Length.Percent(100);
                        return label;
                    };
                }
            
                // bindCell — CLOCK TYPE
                _mcList.columns["clock-type"].bindCell = (e, index) =>
                {
                    var info = infos[index];
                    var label = (Label)e;
            
                    string color = info.IsStopped ? "#FF6666" : "#FFFFFF";
            
                    label.text = $"<b><color={color}>{info.Type}</color></b>";
                };
            
                // bindCell — TIME 
                _mcList.columns["time"].bindCell = (e, index) =>
                {
                    var info = infos[index];
                    var label = (Label)e;
            
                    string formatted = DebugWindowUtils.FormatClockTime(info.Time);
            
                    label.text = $"<b>{formatted}</b>";
                };
            
                // bindCell — DELTA TIME
                _mcList.columns["delta-time"].bindCell = (e, index) =>
                {
                    var dt = infos[index].DeltaTime;
                    var label = (Label)e;
            
                    label.text = $"Δ {dt:F3}";
                };
            
                // bindCell — TIME SCALE
                _mcList.columns["time-scale"].bindCell = (e, index) =>
                {
                    var scale = infos[index].TimeScale;
                    var label = (Label)e;
                    
                    label.text = $"<color=#C0FFB0>{scale:F1}</color>";
                };
            
                // bindCell — LINKED COUNT
                _mcList.columns["linked"].bindCell = (e, index) =>
                {
                    int count = infos[index].LinkedCount;
                    var label = (Label)e;
            
                    string color = count > 0 ? "#80D6FF" : "#777777";
                    string icon  = count > 0 ? "🔗" : "—";
            
                    label.text = $"<color={color}>{icon} {count}</color>";
                };
                
                // 더블클릭 시 detail view로 이동
                _mcList.itemsChosen += OnClockRowChosen;
            }

            public void Refresh()
            {
                _mcList?.RefreshItems();
            }

            private void OnClockRowChosen(IEnumerable<object> chosenItems)
            {
                foreach (var item in chosenItems)
                {
                    if (item is ClockDebugInfo info)
                        ClockRowChosen?.Invoke(info.Type);
                }
            }
        }
    }
}
#endif