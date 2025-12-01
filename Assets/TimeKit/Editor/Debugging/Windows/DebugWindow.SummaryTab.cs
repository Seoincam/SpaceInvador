#if UNITY_EDITOR
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

            public SummaryTabController(VisualElement root)
            {
                _mcList = root.Q<MultiColumnListView>("clocks-mc-list");
            }

            public void ConfigMcList()
            {
                var infos = ClockDebugManager.Infos;
                
                _mcList.showAlternatingRowBackgrounds = AlternatingRowBackground.All;
                
                // itemSource
                _mcList.itemsSource = infos;
                
                // makeCell
                var columnNames = new[] { "clock-type", "time", "delta-time", "time-scale", "linked" };
                foreach (var name in columnNames)
                {
                    _mcList.columns[name].makeCell = () => new Label
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
                _mcList.columns["clock-type"].bindCell = (e, index) =>
                {
                    var info = infos[index];
                    var label = e as Label;
                
                    label.text = info.Type.ToString();
                    label.style.unityFontStyleAndWeight = FontStyle.Bold;
                    label.style.color = info.IsStopped ? Color.red : Color.yellow;
                };
                
                _mcList.columns["time"].bindCell = (e, index) =>
                {
                    // TODO 1h 23m 32.23s 스타일로!
                    (e as Label).text = infos[index].Time.ToString("F2");
                };
                
                _mcList.columns["delta-time"].bindCell = (e, index) =>
                {
                    (e as Label).text = infos[index].DeltaTime.ToString("F3");
                };
                
                _mcList.columns["time-scale"].bindCell = (e, index) =>
                {
                    (e as Label).text = infos[index].TimeScale.ToString("F1");
                };
                
                _mcList.columns["linked"].bindCell = (e, index) =>
                {
                    (e as Label).text = (infos[index].LinkedCount).ToString();
                };
            }

            public void Refresh()
            {
                _mcList?.RefreshItems();
            }
        }
    }
}
#endif