using TimeKit.Editor.Debugging.Data;
using UnityEngine;
using UnityEngine.UIElements;

#if UNITY_EDITOR
namespace TimeKit.Editor.Debugging.Windows
{
    public partial class DebugWindow
    {
        private sealed class SummaryTabController
        {
            private readonly MultiColumnListView _mcList;
            
            public SummaryTabController(MultiColumnListView mcList)
            {
                _mcList = mcList;
            }

            public void ConfigMcList()
            {
                _mcList.showAlternatingRowBackgrounds = AlternatingRowBackground.All;
                
                // itemSource
                _mcList.itemsSource = ClockDebug.Infos;
                
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
                    var info = ClockDebug.Infos[index];
                    var label = e as Label;
                
                    label.text = info.Type.ToString();
                    label.style.unityFontStyleAndWeight = FontStyle.Bold;
                    label.style.color = info.IsStopped ? Color.red : Color.yellow;
                };
                _mcList.columns["time"].bindCell = (e, index) =>
                {
                    // TODO 1h 23m 32.23s 스타일로!
                    (e as Label).text = ClockDebug.Infos[index].Time.ToString("F2");
                };
                _mcList.columns["delta-time"].bindCell = (e, index) =>
                {
                    (e as Label).text = ClockDebug.Infos[index].DeltaTime.ToString("F3");
                };
                _mcList.columns["time-scale"].bindCell = (e, index) =>
                {
                    (e as Label).text = ClockDebug.Infos[index].TimeScale.ToString("F1");
                };
                _mcList.columns["linked"].bindCell = (e, index) =>
                {
                    var info = ClockDebug.Infos[index];
                    (e as Label).text = (info.Linked.Count).ToString();
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