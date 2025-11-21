#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace TimeKit.Editor
{
    public class TimeKitDebugWindow : EditorWindow
    {
        private ScrollView _rightPane;
        private Label _timeLabel;
        private IVisualElementScheduledItem _rightPaneSchedule;
        private ClockType? _selectedClockType;
        
        [MenuItem("Window/TimeKit/Debug")]
        public static void OpenWindow()
        {
            GetWindow<TimeKitDebugWindow>("TimeKit Debug");
        }

        private void CreateGUI()
        {
            EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
            
            RebuildUI(EditorApplication.isPlaying);
        }

        private void OnDisable()
        {
            EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
        }

        private void OnPlayModeStateChanged(PlayModeStateChange state)
        {
            RebuildUI(EditorApplication.isPlaying);
        }

        private void RebuildUI(bool isPlaying)
        {
            var root = rootVisualElement;
            root.Clear();
            root.style.flexDirection = FlexDirection.Column;
            
            // 제목 라벨 
            var titleLabel = new Label("TimeKit Debug")
            {
                style =
                {
                    unityFontStyleAndWeight = FontStyle.Bold,
                    paddingBottom = 8f
                }
            };
            root.Add(titleLabel);

            // Play 모드 가드
            if (!isPlaying)
            {
                var helpBox = new HelpBox("Play Mode에서만 TimeKit 상태를 볼 수 있습니다.", HelpBoxMessageType.Info);
                root.Add(helpBox);
                return;
            }
            
            // enum 리스트 만들기
            var allClockTypes = Enum.GetValues(typeof(ClockType))
                .Cast<ClockType>()
                .ToList();
            
            // SplitView 생성
            var splitView = new TwoPaneSplitView(0, 250, TwoPaneSplitViewOrientation.Horizontal);
            root.Add(splitView);
            
            // 왼쪽 ListView
            var leftPane = LeftPane();
            splitView.Add(leftPane);
            
            // 오른쪽 Pane
            _rightPane = RightPane();
            splitView.Add(_rightPane);
        }

        private ListView LeftPane()
        {
            var allClockTypes = Enum.GetValues(typeof(ClockType))
                .Cast<ClockType>()
                .ToList();
            
            var leftPane = new ListView();
            
            leftPane.itemsSource = allClockTypes;
            leftPane.makeItem = () => new Label();
            leftPane.bindItem = (item, index) =>
            {
                (item as Label).text = allClockTypes[index].ToString();
            };
            leftPane.selectionChanged += OnClockSelectionChanged;

            return leftPane;
        }

        private ScrollView RightPane()
        {
            var rightPane = new ScrollView(ScrollViewMode.VerticalAndHorizontal);
            rightPane.style.paddingLeft = 8f;
            UpdateRightPane(rightPane);
            return rightPane;
        }

        private void UpdateRightPane(ScrollView rightPane)
        {
            rightPane.Clear();
            
            if (_selectedClockType == null)
            {
                var helpBox = new HelpBox("Select a Clock.", HelpBoxMessageType.Info);
                rightPane.Add(helpBox);
                return;
            }

            IClock clock = TimeManager.Clocks[_selectedClockType.Value];

            // 이름
            var clockStateLabel = new Label($"{_selectedClockType} (Resumed)");
            clockStateLabel.style.unityFontStyleAndWeight = FontStyle.Bold;
            rightPane.Add(clockStateLabel);

            var group1 = new GroupBox();
            // 시간
            var timeLabel = new Label($"Time: {clock.Time:F2}s");
            group1.Add(timeLabel);
            // delta time
            var deltaTimeLabel = new Label($"DeltaTime: {clock.DeltaTime:F3}");
            group1.Add(deltaTimeLabel);
            rightPane.Add(group1);
            
            var group2 = new GroupBox();
            // TimeScale
            var timeScaleHeader = new Label("TimeScale");
            timeScaleHeader.style.unityFontStyleAndWeight = FontStyle.Bold;
            group2.Add(timeScaleHeader);
            var timeScaleLabel = new Label(clock.TimeScale.ToString());
            group2.Add(timeScaleLabel);
            rightPane.Add(group2);
        }

        private void OnClockSelectionChanged(IEnumerable<object> selectedItems)
        {
            if (selectedItems.First() is not ClockType clock)
                return;

            // _rightPaneSchedule.Pause();
            _selectedClockType = clock;
            UpdateRightPane(_rightPane);
        }

        #region RightPane

        private void DrawRightPane()
        {
            _rightPane.Clear();
            
            if (_selectedClockType == null)
            {
                var helpBox = new HelpBox("Select a Clock.", HelpBoxMessageType.Info);
                _rightPane.Add(helpBox);
                return;
            }

            IClock clock = TimeManager.Clocks[(ClockType)_selectedClockType];
            DrawClockHeader(clock);

            _rightPaneSchedule = _rightPane.schedule
                .Execute(() => UpdateTimeLabel(clock.Time))
                .Every(1000);
        }

        private void DrawClockHeader(IClock clock)
        {
            var nameLabel = new Label($"{clock.Type}")
            {
                style = { unityFontStyleAndWeight = FontStyle.Bold }
            };
            _rightPane.Add(nameLabel);
            
            _timeLabel = new Label();
            UpdateTimeLabel(clock.Time);
            _rightPane.Add(_timeLabel);
        }
        private void UpdateTimeLabel(double time)
        {
            _timeLabel.text = time.ToString("F3");
        }

        #endregion


    }
}
#endif