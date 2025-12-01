#if UNITY_EDITOR
using TimeKit.Editor.Debugging.Data;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace TimeKit.Editor.Debugging.Windows
{
    public class TimeKitDebugWindow : EditorWindow
    {
        /*
         * 추후 Package로 만들면 아래 방법 사용!!
         * AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Packages/<name-of-the-package>/main_window.uxml");
         * https://docs.unity3d.com/kr/2023.2/Manual/UIE-manage-asset-reference.html
         */
        
        private const string UxmlPath = "Assets/TimeKit/Editor/Debug/UI/TimeKitDebug.uxml";
        private VisualTreeAsset _uxml;

        private TabView _tabView;
        private Tab _summaryTab;
        private Tab _detailTab;
        private Tab _clockTypesTab;
        
        private MultiColumnListView _summaryMcList;
        private MultiColumnListView _detailMcList;
        private EnumField _detailClockTypeEnumField;
        
        private enum TabId { Summary, ClockTypes }
        private TabId _activeTabId;

        private IVisualElementScheduledItem _updateSchedule;
        
        [MenuItem("Window/TimeKit/Debug")]
        public static void OpenWindow()
        {
            GetWindow<TimeKitDebugWindow>("TimeKit Debug");
        }

        private void CreateGUI()
        {
            var root = rootVisualElement;
            
            if (!_uxml)
                _uxml = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(UxmlPath);
            if (!_uxml)
            {
                var noUxmlWarning = new HelpBox($"UXML not found at: {UxmlPath}", HelpBoxMessageType.Error);
                root.Add(noUxmlWarning);
                return;
            }

            EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
            
            BuildUIFromUxml(EditorApplication.isPlaying);
            HookTabEvents();
            SetupUpdateLoop();

            
            return;
            void OnPlayModeStateChanged(PlayModeStateChange state)
            {
                BuildUIFromUxml(EditorApplication.isPlaying);
            }
        }
        
        private void BuildUIFromUxml(bool isPlaying)
        {
            var root = rootVisualElement;
            root.Clear();

            // 생성 및 Query
            var tree = _uxml.Instantiate();
            root.Add(tree);

            _tabView = root.Q<TabView>("timekit-tab-view");
            _summaryTab = root.Q<Tab>("summary-tab");
            _clockTypesTab = root.Q<Tab>("clock-types-tab");
            _detailTab = root.Q<Tab>("detail-tab");

            _summaryMcList = root.Q<MultiColumnListView>("clocks-mc-list");
            _detailMcList = root.Q<MultiColumnListView>("linked-detail-mc-list");
            _detailClockTypeEnumField = root.Q<EnumField>("clock-type");
            _detailClockTypeEnumField.Init(ClockType.GamePlay);
            
            if (!isPlaying)
            {
                var help = new HelpBox("Play Mode에서만 TimeKit 상태를 볼 수 있습니다.", HelpBoxMessageType.Info);
                root.Add(help);
                return;
            }

            ConfigSummaryMcList();
            ConfigDetailMcList();
        }

        private void ConfigSummaryMcList()
        {
            _summaryMcList.showAlternatingRowBackgrounds = AlternatingRowBackground.All;
            
            // itemSource
            _summaryMcList.itemsSource = ClockDebug.Infos;
            
            // makeCell
            var columnNames = new[] { "clock-type", "time", "delta-time", "time-scale", "linked" };
            foreach (var s in columnNames)
            {
                _summaryMcList.columns[s].makeCell = () => new Label
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
            _summaryMcList.columns["clock-type"].bindCell = (e, index) =>
            {
                var info = ClockDebug.Infos[index];
                var label = e as Label;
                
                label.text = info.Type.ToString();
                label.style.unityFontStyleAndWeight = FontStyle.Bold;
                label.style.color = info.IsStopped ? Color.red : Color.yellow;
            };
            _summaryMcList.columns["time"].bindCell = (e, index) =>
            {
                // TODO 1h 23m 32.23s 스타일로!
                (e as Label).text = ClockDebug.Infos[index].Time.ToString("F2");
            };
            _summaryMcList.columns["delta-time"].bindCell = (e, index) =>
            {
                (e as Label).text = ClockDebug.Infos[index].DeltaTime.ToString("F3");
            };
            _summaryMcList.columns["time-scale"].bindCell = (e, index) =>
            {
                (e as Label).text = ClockDebug.Infos[index].TimeScale.ToString("F1");
            };
            _summaryMcList.columns["linked"].bindCell = (e, index) =>
            {
                var info = ClockDebug.Infos[index];
                (e as Label).text = (info.Linked.Count).ToString();
            };
        }

        private void ConfigDetailMcList()
        {
            var linked = ClockDebug.InfoMap[(ClockType)_detailClockTypeEnumField.value].Linked;
            
            _detailMcList.showAlternatingRowBackgrounds = AlternatingRowBackground.All;
            
            // itemSource
            _detailMcList.itemsSource = linked;
            
            // makeCell
            var columnNames = new[] { "linked-type", "position" };
            foreach (var s in columnNames)
            {
                _detailMcList.columns[s].makeCell = () => new Label
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
            _detailMcList.columns["linked-type"].bindCell = (e, index) =>
            {
                (e as Label).text = linked[index].GetType().ToString();
            };
            _detailMcList.columns["position"].bindCell = (e, index) =>
            {
                (e as Label).text = linked[index].Trace;
            };
        }

        private void HookTabEvents()
        {
            _tabView.activeTabChanged += (prev, cur) =>
            {
                if (cur == _summaryTab)
                    _activeTabId = TabId.Summary;
                else if (cur == _clockTypesTab)
                    _activeTabId = TabId.ClockTypes;
            };

            var initial = _tabView.activeTab;
            if (initial == _summaryTab)
                _activeTabId = TabId.Summary;
            else if (initial == _clockTypesTab)
                _activeTabId = TabId.ClockTypes;
        }

        private void SetupUpdateLoop()
        {
            _updateSchedule?.Pause();

            _updateSchedule = rootVisualElement.schedule
                .Execute(UpdateActiveTab)
                .Every(100);


            return;
            void UpdateActiveTab()
            {
                if (!EditorApplication.isPlaying)
                    return;

                switch (_activeTabId)
                {
                    case TabId.Summary:
                        _summaryMcList?.RefreshItems();
                        break;
                    case TabId.ClockTypes:
                        break;
                }
            }
        }
    }
}
#endif