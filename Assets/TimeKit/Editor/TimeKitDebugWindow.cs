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
        /*
         * 추후 Package로 만들면 아래 방법 사용!!
         * AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Packages/<name-of-the-package>/main_window.uxml");
         * https://docs.unity3d.com/kr/2023.2/Manual/UIE-manage-asset-reference.html
         */
        
        private const string UxmlPath = "Assets/TimeKit/Editor/TimeKitDebug.uxml";
        private VisualTreeAsset _uxml;

        private TabView _tabView;
        private Tab _summaryTab;
        private Tab _clockTypesTab;
        private MultiColumnListView _summaryMcList;
        
        private enum TabId { Summary, ClockTypes }
        private TabId _activeTabId;
        
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
            
            RebuildUI(Application.isPlaying);

            
            return;
            void OnPlayModeStateChanged(PlayModeStateChange state)
            {
                RebuildUI(EditorApplication.isPlaying);
            }
        }

        private void RebuildUI(bool isPlaying)
        {
            var root = rootVisualElement;
            root.Clear();
            
            if (!isPlaying)
            {
                var help = new HelpBox("Play Mode에서만 TimeKit 상태를 볼 수 있습니다.", HelpBoxMessageType.Info);
                root.Add(help);
                return;
            }

            root.Add( _uxml.Instantiate());
            var mcList = root.Q<MultiColumnListView>();
            mcList.showAlternatingRowBackgrounds = AlternatingRowBackground.All;
            
            /*
             * 데이터 처리.
             * TODO 추후 Core 로직으로 옮겨서 따로 처리하기!
             */
            var clockTypes = Enum.GetValues(typeof(ClockType))
                .Cast<ClockType>()
                .ToArray();
            var clocks = new IClock[clockTypes.Length];
            for (int i = 0; i < clockTypes.Length; i++)
                clocks[i] = TimeManager.Clocks[clockTypes[i]];
            
            // itemSource
            mcList.itemsSource = clocks;
            
            // makeCell
            var columnNames = new[] { "clock-type", "time", "delta-time", "time-scale" };
            foreach (var s in columnNames)
                mcList.columns[s].makeCell = () =>
                {
                    var label = new Label();
                    label.style.unityTextAlign = TextAnchor.MiddleLeft;
                    label.style.height = Length.Percent(100);
                    label.style.paddingLeft = 8f;
                    return label;
                };
            
            // bindCell
            mcList.columns["clock-type"].bindCell = (e, index) =>
            {
                (e as Label).text = clocks[index].Type.ToString();
                (e as Label).style.unityFontStyleAndWeight = FontStyle.Bold;
                (e as Label).style.color = Color.yellow;
            };
            mcList.columns["time"].bindCell = (e, index) =>
            {
                // 1h 23m 32.23s 스타일로!
                (e as Label).text = clocks[index].Time.ToString("F2");
            };
            mcList.columns["delta-time"].bindCell = (e, index) =>
            {
                (e as Label).text = clocks[index].DeltaTime.ToString("F3");
            };
            mcList.columns["time-scale"].bindCell = (e, index) =>
            {
                (e as Label).text = clocks[index].TimeScale.ToString("F1");
            };
        }
    }
}
#endif