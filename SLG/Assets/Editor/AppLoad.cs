using UnityEditor;

namespace SLG
{
    [InitializeOnLoad]
    public class AppLoad
    {
        static AppLoad()
        {
            int Cnt = EditorApplication.update.GetInvocationList().Length;
            for (int i = 0; i < Cnt; ++i)
            {
                EditorApplication.update -= Update;
            }
            EditorApplication.update += Update;
        }

        static void Update()
        {
            LaunchInspector.Update();

        }
    }
}