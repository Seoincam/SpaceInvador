#if UNITY_EDITOR
using UnityEditor;
using UnityEngine.UIElements;

namespace TimeKit.Editor.Debugging.Windows
{
    public sealed partial class DebugWindow : EditorWindow
    {
        /*
         * 추후 Package로 만들면 아래 방법 사용!!
         * AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Packages/<name-of-the-package>/main_window.uxml");
         * https://docs.unity3d.com/kr/2023.2/Manual/UIE-manage-asset-reference.html
         */
        private const string UxmlPath = "Assets/TimeKit/Editor/Debugging/UI/TimeKitDebug.uxml";
        private VisualTreeAsset _uxml;

        private TabView _tabView;
        private Tab _summaryTab;
        private Tab _detailTab;
        private Tab _clockTypesTab;
        
        private enum TabId { Summary, Detail, ClockTypes }
        private TabId _activeTabId;
        
        private SummaryTabController _summaryTabController;
        private DetailTabController _detailTabController;
        
        private IVisualElementScheduledItem _updateSchedule;
        
        [MenuItem("Window/TimeKit/Debug")]
        public static void OpenWindow()
        {
            GetWindow<DebugWindow>("TimeKit Debug");
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

            _summaryTabController = new SummaryTabController(root);
            _summaryTabController.ClockRowChosen += OnClockRowChosen;
            _detailTabController = new DetailTabController(root);
            
            if (!isPlaying)
            {
                var help = new HelpBox("Play Mode에서만 TimeKit 상태를 볼 수 있습니다.", HelpBoxMessageType.Info);
                root.Add(help);
                return;
            }
            
            _summaryTabController?.ConfigMcList();
            _detailTabController?.Config();
        }

        private void HookTabEvents()
        {
            _tabView.activeTabChanged += (prev, cur) =>
            {
                if (cur == _summaryTab)
                    _activeTabId = TabId.Summary;
                else if (cur == _detailTab)
                    _activeTabId = TabId.Detail;
                else if (cur == _clockTypesTab)
                    _activeTabId = TabId.ClockTypes;
            };

            var initial = _tabView.activeTab;
            if (initial == _summaryTab)
                _activeTabId = TabId.Summary;
            else if (initial == _detailTab)
                _activeTabId = TabId.Detail;
            else if (initial == _clockTypesTab)
                _activeTabId = TabId.ClockTypes;
        }

        private void SetupUpdateLoop()
        {
            _updateSchedule?.Pause();

            _updateSchedule = rootVisualElement.schedule
                .Execute(UpdateActiveTab)
                .Every(100);
            void UpdateActiveTab()
            {
                if (!EditorApplication.isPlaying)
                    return;

                switch (_activeTabId)
                {
                    case TabId.Summary:
                        _summaryTabController?.Refresh();
                        break;
                    
                    case TabId.Detail:
                        _detailTabController?.Refresh();
                        break;
                    
                    case TabId.ClockTypes:
                        break;
                }
            }
        }
        
        private void OnClockRowChosen(ClockType type)
        {
            _detailTabController.OnClockRowChosen(type);
            _tabView.activeTab = _detailTab;
        }
    }
}
#endif