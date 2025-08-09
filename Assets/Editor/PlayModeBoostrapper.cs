using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Editor
{
    [InitializeOnLoad]
    public static class PlayModeBoostrapper
    {
        private const string BOOTSTRAP_SCENE_PATH_PREF_KEY = "BootstrapScenePath";
        private static string _bootstrapScenePath;
        private static string DEFAULT_BOOSTRAP_SCENE = "Assets/FlappyBoiz/Scenes/Bootstrap.unity";

        static PlayModeBoostrapper()
        {
            // Load the path from EditorPrefs on startup
            _bootstrapScenePath = EditorPrefs.GetString(BOOTSTRAP_SCENE_PATH_PREF_KEY, DEFAULT_BOOSTRAP_SCENE);

            // Subscribe to play mode state changes
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
        }

        private static void OnPlayModeStateChanged(PlayModeStateChange state)
        {
            switch (state)
            {
                case PlayModeStateChange.ExitingEditMode:
                {
                    // This state is triggered just before entering Play Mode
                    // Store the current scene to return to it later
                    EditorPrefs.SetString("LastOpenedScenePath", SceneManager.GetActiveScene().path);

                    if (!string.IsNullOrEmpty(_bootstrapScenePath))
                    {
                        // Set the Editor's playModeStartScene
                        // This tells Unity to load this scene first when entering play mode.
                        EditorSceneManager.playModeStartScene =
                            AssetDatabase.LoadAssetAtPath<SceneAsset>(_bootstrapScenePath);
                        Debug.Log(
                            $"Preparing to start from Bootstrap scene: {EditorSceneManager.playModeStartScene.name}");
                    }
                    else
                    {
                        Debug.LogWarning(
                            "Bootstrap scene path is not set. Game will start from current scene or first scene in build settings.");
                    }

                    break;
                }
                case PlayModeStateChange.EnteredEditMode:
                {
                    // This state is triggered when exiting Play Mode and returning to Editor Mode
                    // Clear the playModeStartScene to avoid issues
                    EditorSceneManager.playModeStartScene = null;

                    // Optionally, reload the scene you were working on
                    string lastScenePath = EditorPrefs.GetString("LastOpenedScenePath", "");

                    if (!string.IsNullOrEmpty(lastScenePath) && lastScenePath != _bootstrapScenePath)
                    {
                        EditorSceneManager.OpenScene(lastScenePath);
                        Debug.Log($"Returned to last opened scene: {lastScenePath}");
                        EditorPrefs.DeleteKey("LastOpenedScenePath"); // Clean up
                    }

                    break;
                }
            }
        }
    }
}