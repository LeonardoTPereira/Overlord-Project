using UnityEditor;
public class SolutionFileFixer : AssetPostprocessor
{
    private static string OnGeneratedCSProject(string path, string content)
    {
        return content.Replace("<ReferenceOutputAssembly>false</ReferenceOutputAssembly>", "<ReferenceOutputAssembly>true</ReferenceOutputAssembly>");
    }
}