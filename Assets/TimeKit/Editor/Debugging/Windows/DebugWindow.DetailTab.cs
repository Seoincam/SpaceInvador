#if UNITY_EDITOR
using System.Collections.Generic;
using TimeKit.Editor.Debugging.Data;
using UnityEngine;
using UnityEngine.UIElements;

namespace TimeKit.Editor.Debugging.Windows
{
    public sealed partial class DebugWindow
    {
        private sealed class DetailTabController
        {
            // pane1
            private readonly EnumField _clockTypeEnum;
            
            // pane2
            private readonly MultiColumnListView _clockMcList;
            
            // pane3
            private readonly Button _refreshButton;
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
            }

            public void Config()
            {
                ConfigEnumField();
                ConfigClockMcList();
                ConfigRefreshButton();
                ConfigLinkedMcList();
            }

            private void ConfigEnumField()
            {
                var initialType = ClockType.GamePlay;
                _clockTypeEnum.Init(initialType);
                UpdateClockInfo(initialType);
                UpdateLinkedInfo();
                
                _clockTypeEnum.RegisterValueChangedCallback(evt =>
                {
                    Debug.Log("Changed clock type: " + evt.newValue);
                    UpdateClockInfo((ClockType)evt.newValue);
                    UpdateLinkedInfo();
                });
            }

            private void ConfigClockMcList()
            {
                // itemSource
                _clockMcList.itemsSource = _clockInfoSource;
                
                // makeCell
                var columnNames = new[] { "time", "delta-time", "time-scale", "state" };
                foreach (var name in columnNames)
                {
                    _clockMcList.columns[name].makeCell = () => new Label
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
                _clockMcList.columns["time"].bindCell = (e, index) =>
                {
                    // TODO 1h 23m 32.23s 스타일로!
                    (e as Label).text = _clockInfoSource[0].Time.ToString("F2");
                };
                
                _clockMcList.columns["delta-time"].bindCell = (e, index) =>
                {
                    (e as Label).text = _clockInfoSource[0].DeltaTime.ToString("F3");
                };
                
                _clockMcList.columns["time-scale"].bindCell = (e, index) =>
                {
                    (e as Label).text = _clockInfoSource[0].TimeScale.ToString("F1");
                };
                
                _clockMcList.columns["state"].bindCell = (e, index) =>
                {
                    (e as Label).text = _clockInfoSource[0].IsPaused ? "Paused" : "";
                };
            }
            
            private void ConfigRefreshButton()
            {
                _refreshButton.clicked += UpdateLinkedInfo;
            }

            private void ConfigLinkedMcList()
            {
                _linkedMcList.showAlternatingRowBackgrounds = AlternatingRowBackground.All;
                
                // itemSource
                _linkedMcList.itemsSource = _linkedInfoSource;
                
                // makeCell
                var columnNames = new[] { "target-type", "game-object", "scene-name", "hierarchy-path" };
                foreach (var name in columnNames)
                {
                    _linkedMcList.columns[name].makeCell = () => new Label();
                }
                
                // bindCell
                _linkedMcList.columns["target-type"].bindCell = (e, index) =>
                {
                    (e as Label).style.unityFontStyleAndWeight = FontStyle.Bold;
                    (e as Label).text = _linkedInfoSource[index].TargetType;
                };

                _linkedMcList.columns["game-object"].bindCell = (e, index) =>
                {
                    (e as Label).text = _linkedInfoSource[index].TargetGameObjectName;
                };

                _linkedMcList.columns["scene-name"].bindCell = (e, index) =>
                {
                    (e as Label).text = _linkedInfoSource[index].SceneName;
                };

                _linkedMcList.columns["hierarchy-path"].bindCell = (e, index) =>
                {
                    (e as Label).text = _linkedInfoSource[index].HierarchyPath;
                };
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
            }

            public void Refresh()
            {
                _clockMcList.RefreshItems();
            }
        }
    }
}
#endif