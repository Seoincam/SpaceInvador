using TimeKit.Editor.Debugging.Data;
using UnityEngine;
using UnityEngine.UIElements;

namespace TimeKit.Editor.Debugging.Windows
{
    public partial class DebugWindow
    {
        private sealed class DetailTabController
        {
            private readonly MultiColumnListView _mcList;
            private readonly EnumField _clockType;
            
            public DetailTabController(MultiColumnListView mcList, EnumField clockType)
            {
                _mcList = mcList;
                _clockType = clockType;
                _clockType.Init(ClockType.GamePlay);
            }

            public void ConfigMcList()
            {
                var linked = ClockDebug.InfoMap[(ClockType)_clockType.value].Linked;
            
                _mcList.showAlternatingRowBackgrounds = AlternatingRowBackground.All;
            
                // itemSource
                _mcList.itemsSource = linked;
            
                // makeCell
                var columnNames = new[] { "linked-type", "position" };
                foreach (var s in columnNames)
                {
                    _mcList.columns[s].makeCell = () => new Label
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
                _mcList.columns["linked-type"].bindCell = (e, index) =>
                {
                    (e as Label).text = linked[index].GetType().ToString();
                };
                _mcList.columns["position"].bindCell = (e, index) =>
                {
                    (e as Label).text = linked[index].Trace;
                };
            }

            public void Refresh()
            {
                // _mcList.RefreshItems();
                Debug.Log("DebugWindow.DetailTabController.Refresh");
            }
        }
    }
}