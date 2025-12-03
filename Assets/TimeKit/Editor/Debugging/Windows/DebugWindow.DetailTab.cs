#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using TimeKit.Editor.Debugging.Data;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace TimeKit.Editor.Debugging.Windows
{
    public sealed partial class DebugWindow
    {
        private sealed class DetailTabController
        {
            private ClockType _currentClockType;
            
            // pane1
            private readonly EnumField _clockTypeEnum;
            
            // pane2
            private readonly MultiColumnListView _clockMcList;
            
            // pane3
            private readonly Button _refreshButton;
            private readonly Label _clockTypeLabel;
            private readonly MultiColumnListView _linkedMcList;
            
            // buffer
            private readonly ClockDebugInfo[] _clockInfoSource = new ClockDebugInfo[1];
            private readonly List<ClockLinkDebugInfo> _linkedInfoSource = new();

            public DetailTabController(VisualElement root)
            {
                _clockTypeEnum = root.Q<EnumField>("clock-type");
                _clockMcList = root.Q<MultiColumnListView>("clock-detail-mc-list");
                _linkedMcList = root.Q<MultiColumnListView>("linked-detail-mc-list");
                _refreshButton = root.Q<Button>("refresh-button");
                _clockTypeLabel = root.Q<Label>("clock-type-label");
            }

            public void Config()
            {
                ConfigEnumField();
                ConfigClockMcList();
                ConfigRefreshButton();
                ConfigLinkedMcList();

                if (EditorApplication.isPlaying)
                {
                    foreach (ClockType clockType in Enum.GetValues(typeof(ClockType)))
                    {
                        var clock = TimeManager.GetRealClock(clockType);
                        clock.Linked.Changed -= OnLinkedGroupChanged;
                        clock.Linked.Changed += OnLinkedGroupChanged;
                    }
                }
            }

            private void ConfigEnumField()
            {
                _currentClockType = ClockType.GamePlay;
                _clockTypeEnum.Init(_currentClockType);
                UpdateClockInfo(_currentClockType);
                UpdateLinkedInfo();
                
                _clockTypeEnum.RegisterValueChangedCallback(evt =>
                {
                    _currentClockType = (ClockType)evt.newValue;
                    UpdateClockInfo(_currentClockType);
                    UpdateLinkedInfo();
                });
            }

            private void ConfigClockMcList()
            {
                // items
                _clockMcList.itemsSource = _clockInfoSource;

                // makeCell
                var columnNames = new[] { "time", "delta-time", "time-scale", "state" };
                foreach (var name in columnNames)
                {
                    _clockMcList.columns[name].makeCell = () =>
                    {
                        var label = new Label
                        {
                            enableRichText = true
                        };
                        label.style.unityTextAlign = TextAnchor.MiddleLeft;
                        label.style.paddingLeft = 6;
                        label.style.height = Length.Percent(100);
                        return label;
                    };
                }

                // bindCell — TIME
                _clockMcList.columns["time"].bindCell = (e, index) =>
                {
                    var info = _clockInfoSource[0];
                    var label = (Label)e;

                    label.text = $"<color=#FFD800><b>{DebugWindowUtils.FormatClockTime(info.Time)}</b></color>";
                };

                // bindCell — DELTA TIME
                _clockMcList.columns["delta-time"].bindCell = (e, index) =>
                {
                    var info = _clockInfoSource[0];
                    var label = (Label)e;

                    label.text = $"Δ {info.DeltaTime:F3}";
                };

                // bindCell — TIME SCALE
                _clockMcList.columns["time-scale"].bindCell = (e, index) =>
                {
                    var info = _clockInfoSource[0];
                    var label = (Label)e;

                    var scale = info.TimeScale;
                    string color = Mathf.Approximately(scale, 1f) ? "#C0FFB0" : "#FFD080";

                    label.text = $"<color={color}>{scale:F1}</color>";
                };

                // bindCell — STATE
                _clockMcList.columns["state"].bindCell = (e, index) =>
                {
                    var info = _clockInfoSource[0];
                    var label = (Label)e;

                    label.text = info.IsPaused ? "<b><color=#FF7474>Paused</color></b>" : ""; 
                };
            }
            
            private void ConfigRefreshButton()
            {
                _refreshButton.clicked += UpdateLinkedInfo;
            }

            private void ConfigLinkedMcList()
            {
                _linkedMcList.showAlternatingRowBackgrounds = AlternatingRowBackground.All;
                _linkedMcList.selectionType = SelectionType.Single;
                
                // itemSource
                _linkedMcList.itemsSource = _linkedInfoSource;
                
                // makeCell
                var columnNames = new[] { "target-type", "game-object", "scene-name", "hierarchy-path" };
                foreach (var name in columnNames)
                {
                    _linkedMcList.columns[name].makeCell = () => new Label()
                    {
                        style =
                        {
                            unityTextAlign = TextAnchor.MiddleLeft,
                            height = Length.Percent(100),
                            paddingLeft = 8f
                        }
                    };
                }
                
                // bindCell
                _linkedMcList.columns["target-type"].bindCell = (e, index) =>
                {
                    var info = _linkedInfoSource[index];
                    var label = (Label)e;

                    var color = info.TargetGameObject ? "#FFD800" : "#80D6FF";
                    var icon  = info.TargetGameObject ? "📦" : "⚙️";

                    label.text = $"<b><color={color}>{icon} {info.TargetType}</color></b>";
                };

                _linkedMcList.columns["game-object"].bindCell = (e, index) =>
                {
                    var info = _linkedInfoSource[index];
                    var label = (Label)e;

                    if (info.TargetGameObjectName == string.Empty)
                    {
                        label.text = "<color=#999999><i>— no object —</i></color>";
                    }
                    else
                    {
                        label.text = $"<color=#C0FFB0>{info.TargetGameObjectName}</color>";
                    }
                };

                _linkedMcList.columns["scene-name"].bindCell = (e, index) =>
                {
                    var scene = _linkedInfoSource[index].SceneName;
                    var label = (Label)e;

                    if (scene == string.Empty)
                    {
                        label.text = "<color=#777777>—</color>";
                    }
                    else
                    {
                        label.text = $"<color=#87CEFA>{scene}</color>";
                    }
                };
                
                _linkedMcList.columns["hierarchy-path"].bindCell = (e, index) =>
                {
                    var path = _linkedInfoSource[index].HierarchyPath;
                    var label = (Label)e;

                    if (path == string.Empty)
                    {
                        label.text = "<color=#777777><i>— no hierarchy —</i></color>";
                    }
                    else
                    {
                        label.text = $"<color=#AAAAAA></color> <color=#CCCCCC>{path}</color>";
                    }
                };
                
                // 더블클릭 시 Hierarchy 선택 + Ping
                _linkedMcList.itemsChosen += OnLinkedRowChosen;
            }

            private void OnLinkedRowChosen(IEnumerable<object> chosenItems)
            {
                foreach (var item in chosenItems)
                {
                    if (item is ClockLinkDebugInfo info && info.TargetGameObject)
                    {
                        Selection.activeGameObject = info.TargetGameObject;
                        EditorGUIUtility.PingObject(info.TargetGameObject);
                    }
                }
            }

            private void UpdateClockInfo(ClockType clockType)
            {
                var info = ClockDebugManager.InfoMap[clockType];
                _clockInfoSource[0] = info;
            }

            private void UpdateLinkedInfo()
            {
                _linkedInfoSource.Clear();
                _linkedInfoSource.AddRange(_clockInfoSource[0].LinkInfos);
                _linkedMcList.RefreshItems();
                
                _clockTypeLabel.text = "Clock Type: " + _clockInfoSource[0].Type;
            }

            public void Refresh()
            {
                _clockMcList.RefreshItems();
            }

            public void OnClockRowChosen(ClockType type)
            {
                _clockTypeEnum.value = type;
                UpdateClockInfo(type);
                UpdateLinkedInfo();
            }

            private void OnLinkedGroupChanged(ClockType clockType)
            {
                if (_currentClockType != clockType)
                    return;
                UpdateLinkedInfo();
            }
        }
    }
}
#endif