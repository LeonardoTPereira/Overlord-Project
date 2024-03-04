using System;
using Game.Audio;
using Game.LevelManager.DungeonLoader;
using UnityEngine.SceneManagement;
using Game.MenuManager;
using UnityEngine;
using UnityEngine.UIElements;

namespace PlatformGame.Dungeon.DungeonLoader
{
    public class PlatformDungeonSceneManager : DungeonSceneManager
    {
        public GameObject minimapCanvas;

        protected override bool VerifySceneName(Scene scene)
        {
            return scene.name is not ("Dungeon");
        }

        protected override void SetGameOverCurrentLevel()
        {
            base.SetGameOverCurrentLevel();
            FindObjectOfType<PauseMenu>().currentLevel = selectedLevels.GetCurrentLevel();
        }
        /*
        protected override void PlayBackgroundMusic()
        {
            
        }
        */
        protected override void LoadSecondaryScenes()
        {
            
        }
        /*
        protected override void PlayBgm(AudioManager.BgmTracks bgmTrack)
        {
            //No sound yet
        }
        */
        protected override void GameOver(object sender, EventArgs eventArgs)
        {
            gameOverScreen.SetActive(true);
        }
        
        protected override void LevelComplete(object sender, EventArgs eventArgs)
        {
            PlayBgm(AudioManager.BgmTracks.VictoryTheme);
            SetComplete();
            victoryScreen.SetActive(true);
            minimapCanvas.SetActive(false);
            //SceneManager.LoadScene("ExperimentLevelSelector");
        }
    }
}