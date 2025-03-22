using System.IO;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
namespace Simple
{
    public class SaveSystemWindow : EditorWindow
    {
        [MenuItem("Simple/SaveSystem/Backup", priority = 1)]
        public static void BackupSaveWindow()
        {
            string sourcePath = Application.persistentDataPath + "/save.data";

            // Open a Save File Dialog for the user to choose the destination path
            string destinationPath = EditorUtility.SaveFilePanel("Save As", "", "save.data", "data");

            if (!string.IsNullOrEmpty(destinationPath))
            {
                CopySaveData(sourcePath, destinationPath);
            }
            else
            {
                Debug.Log("Save operation canceled.");
            }
        }

        private static void CopySaveData(string sourcePath, string destinationPath)
        {
            if (File.Exists(sourcePath))
            {
                File.Copy(sourcePath, destinationPath, overwrite: true);
                Debug.Log($"Save data copied to: {destinationPath}");
            }
            else
            {
                Debug.LogError($"Source file not found: {sourcePath}");
            }
        }

        [MenuItem("Simple/SaveSystem/Delete", priority = 2)]
        public static void ShowDeleteWindow()
        {
            File.Delete(SaveSystem.SavePath);
        }
    }
}
#endif