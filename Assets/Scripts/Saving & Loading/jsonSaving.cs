using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class jsonSaving : MonoBehaviour
{
    public bool isMusicMuted;
    public bool isSFXMuted;
    [Range(0f, 1f)] public float musicLevel;
    [Range(0f, 1f)] public float sfxLevel;
    settings_data playerData;
    string saveFilePath;

    void Start()
    {
        playerData = new settings_data();
        playerData.isMusicMuted = isMusicMuted;
        playerData.isSFXMuted = isSFXMuted;
        playerData.musicLevel = musicLevel;
        playerData.sfxLevel = sfxLevel;

        saveFilePath = Application.persistentDataPath + "/settingsData.json";
    }

    private void OnValidate()
    {
        if (playerData == null)
            playerData = new settings_data();

        playerData.isMusicMuted = isMusicMuted;
        playerData.isSFXMuted = isSFXMuted;
        playerData.musicLevel = musicLevel;
        playerData.sfxLevel = sfxLevel;
    }

    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.S))
        //    SaveGame();

        //if (Input.GetKeyDown(KeyCode.L))
        //    LoadGame();

        //if (Input.GetKeyDown(KeyCode.D))
        //    DeleteSaveFile();
    }

    public void SaveGame()
    {
        string savePlayerData = JsonUtility.ToJson(playerData);
        File.WriteAllText(saveFilePath, savePlayerData);

        UtilityScript.Log("Save file created at: " + saveFilePath);
    }

    public void LoadGame()
    {
        if (File.Exists(saveFilePath))
        {
            string loadPlayerData = File.ReadAllText(saveFilePath);
            playerData = JsonUtility.FromJson<settings_data>(loadPlayerData);

            //UtilityScript.Log("Load game complete! \nPlayer health: " + playerData.health + ", Player gold: " + playerData.gold + ", Player Position: (x = " + playerData.position.x + ", y = " + playerData.position.y + ", z = " + playerData.position.z + ")");
        }
        else
            UtilityScript.Log("There is no save files to load!");

    }

    public void DeleteSaveFile()
    {
        if (File.Exists(saveFilePath))
        {
            File.Delete(saveFilePath);

            UtilityScript.Log("Save file deleted!");
        }
        else
            UtilityScript.Log("There is nothing to delete!");
    }
}
