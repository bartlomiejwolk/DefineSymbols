using UnityEngine;
using UnityEditor;

namespace DefineSymbolsModule {

    public class DefineSymbolsEditorWindow : EditorWindow {

        #region FIELDS

        private BuildTargetGroup buildTargetGroup;
        private string symbols;

        #endregion

        #region METHODS

        [MenuItem("Tools/Define Symbols")]
        private static void Init() {
            // Get existing open window or if none, make a new one:
            DefineSymbolsEditorWindow window =
                (DefineSymbolsEditorWindow)
                    GetWindow(typeof (DefineSymbolsEditorWindow));
            window.Show();

            DontDestroyOnLoad(window);
        }

        #endregion

        #region UNITY MESSAGES

        private void OnGUI() {
            DrawBuildTargetGroupDropdown();
            DrawSymbolsTextField();

            EditorGUILayout.Space();

            EditorGUILayout.HelpBox(
                "These buttons will take effect only when the `Symbols` text field is not focues.",
                MessageType.Info);

            EditorGUILayout.BeginHorizontal();

            DrawRefreshButton();
            DrawClearButton();
            DrawApplyButton();

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space();

            EditorGUILayout.LabelField(
                "Predefined symbols",
                EditorStyles.boldLabel);

            DrawPredefinedSymbolButtons();
        }
        #endregion

        #region DRAW METHODS
        private void DrawClearButton() {
            var clear = GUILayout.Button(new GUIContent(
                "Clear",
                "Clear the `Symbols` text field."));

            if (clear) {
                symbols = "";
            }
        }


        private void DrawPredefinedSymbolButtons() {
            EditorGUILayout.BeginHorizontal();

            var server = GUILayout.Button("SERVER");
            var filelogger = GUILayout.Button("FILELOGGER");

            if (server) {
                symbols += string.IsNullOrEmpty(symbols) ? "" : ";";
                symbols += "SERVER";
            }

            if (filelogger) {
                symbols += string.IsNullOrEmpty(symbols) ? "" : ";";
                symbols += "FILELOGGER";
            }

            EditorGUILayout.EndHorizontal();
        }


        private void DrawApplyButton() {
            var applySettings = GUILayout.Button(new GUIContent(
                "Apply",
                "Recompile source with new compiler directives."));

            if (applySettings) {
                PlayerSettings.SetScriptingDefineSymbolsForGroup(
                    buildTargetGroup, symbols);

                Debug.Log("Symbols applied!");
            }
        }

        private void DrawRefreshButton() {
            var refresh = GUILayout.Button(new GUIContent(
                "Refresh",
                "Update `Symbols` field with currently active compiler directives."));

            if (refresh) {
                symbols =
                    PlayerSettings.GetScriptingDefineSymbolsForGroup(
                        buildTargetGroup);

                Debug.Log("Symbols refreshed!");
                FileLogger.Logger.LogString(symbols);
            }
        }

        private void DrawSymbolsTextField() {
            EditorGUIUtility.labelWidth = 60;
            symbols = EditorGUILayout.TextField(
                new GUIContent(
                    "Symbols",
                    "Compiler directives that will be compiled into the source. Use semicolon as delimiter (without spaces)."),
                symbols);
            EditorGUIUtility.labelWidth = 0;
        }

        private void DrawBuildTargetGroupDropdown() {
            buildTargetGroup =
                (BuildTargetGroup) EditorGUILayout.EnumPopup(
                new GUIContent(
                    "Build Target Group",
                    ""),
                buildTargetGroup);
        }

        private string DrawSymbol(Rect position, string value) {
            if (value == null) {
                value = "";
            }

            return EditorGUI.TextField(position, value);
        }

        #endregion
    }

}