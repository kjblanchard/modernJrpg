using UnityEditor;
class EditorScripts
{
     static void PerformBuild ()
     {
         string[] scenes = { "Assets/Scenes/DebugRoom.unity", "Assets/Scenes/battle1.unity" };
        BuildPipeline.BuildPlayer(scenes, "WebGL-Dist", BuildTarget.WebGL, BuildOptions.None);
     }
}