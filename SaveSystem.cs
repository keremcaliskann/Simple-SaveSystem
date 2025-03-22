using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Simple
{
    public class SaveSystem
    {
        private static SaveData _saveData = new SaveData();
        public static SaveData SaveData
        {
            get
            {
                if (!_loaded)
                    Load();
                return _saveData;
            }
        }

        public static Action OnSave;

        public static string SavePath => Application.persistentDataPath + "/save.data";
        private static bool _loaded;

        public static void Load()
        {
            OnSave = null;

            if (File.Exists(SavePath))
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                FileStream fileStream = new FileStream(SavePath, FileMode.Open);

                _saveData = binaryFormatter.Deserialize(fileStream) as SaveData;
                fileStream.Close();
            }
            else
            {
                _saveData = new SaveData();
            }
            _loaded = true;

            Debug.Log("Loaded");
        }
        public static void Save()
        {
            OnSave?.Invoke();

            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream fileStream = new FileStream(SavePath, FileMode.Create);

            binaryFormatter.Serialize(fileStream, SaveData != null ? SaveData : new SaveData());
            fileStream.Close();

            Debug.Log("Saved");
        }
        public static void Delete()
        {
            File.Delete(SavePath);
            Load();
            Save();

            Debug.Log("Save Deleted");
        }
    }
}