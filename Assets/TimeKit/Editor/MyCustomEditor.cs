using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace TimeKit.Editor
{
    public class MyCustomEditor : EditorWindow
    {
        private VisualElement m_RightPane;
        
        [MenuItem("Window/My Custom Editor")]
        public static void ShowMyCustomEditor()
        {
            EditorWindow wnd = GetWindow<MyCustomEditor>();
            wnd.titleContent = new GUIContent("My Custom Editor Name");
            
            wnd.minSize = new Vector2(450, 200);
            wnd.maxSize = new Vector2(1920, 720);
        }

        public void CreateGUI()
        {
            rootVisualElement.Add(new Label("Root1"));
            
            // Get a list of all sprites in the project
            var allObjectGuids = AssetDatabase.FindAssets("t:Sprite");
            var allObjects = new List<Sprite>();
            foreach (var guid in allObjectGuids)
                allObjects.Add(AssetDatabase.LoadAssetAtPath<Sprite>(AssetDatabase.GUIDToAssetPath(guid)));

            // Create Split view
            var splitView = new TwoPaneSplitView(0, 250, TwoPaneSplitViewOrientation.Horizontal);
            rootVisualElement.Add(splitView);

            var leftPane = new ListView();
            splitView.Add(leftPane);
            m_RightPane = new ScrollView(ScrollViewMode.VerticalAndHorizontal);
            splitView.Add(m_RightPane);

            // Initialize
            leftPane.makeItem = () => new Label();
            leftPane.bindItem = (item, index) => { (item as Label).text = allObjects[index].name; };
            leftPane.itemsSource = allObjects;
            
            // React to the user's selection
            leftPane.selectionChanged += OnSpriteSelectionChange;
        }

        private void OnSpriteSelectionChange(IEnumerable<object> selectedItems)
        {
            m_RightPane.Clear();

            var selectedSprite = selectedItems.First() as Sprite;
            if (!selectedSprite)
                return;

            var spriteImage = new Image
            {
                scaleMode = ScaleMode.ScaleToFit,
                sprite = selectedSprite
            };
            
            m_RightPane.Add(spriteImage);
        }
    }
}