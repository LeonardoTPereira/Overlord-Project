using System.IO;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Scripting.Python;
#endif
using UnityEngine;
using Python.Runtime;

namespace Game.DataCollection
{
    public class PyPlayerModel : MonoBehaviour
    {
        const string kStateName = "com.unity.scripting.python.samples.pyside";

        /// <summary>
        /// Hack to get the current file's directory
        /// </summary>
        /// <param name="fileName">Leave it blank to the current file's directory</param>
        /// <returns></returns>
        private static string __DIR__([System.Runtime.CompilerServices.CallerFilePath] string fileName = "")
        {
            return Path.GetDirectoryName(fileName);
        }

        private void Start() {
            CreateOrReinitialize();
        }

       static void CreateOrReinitialize()
       {
            string dir = __DIR__();
            PythonRunner.EnsureInitialized();
            PythonRunner.RunFile(dir+"/PyPlayerModel.py");
        }
    }
}
