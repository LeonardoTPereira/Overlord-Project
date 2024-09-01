using System;
using Game.Audio;
using Game.LevelManager.DungeonLoader;
using UnityEngine.SceneManagement;

namespace PlatformGame.Dungeon.DungeonLoader
{
    public class PlatformDungeonSceneManager : DungeonSceneManager
    {
        protected override bool VerifySceneName(Scene scene)
        {
            return scene.name is not ("Dungeon");
        }
        
        protected override void PlayBackgroundMusic()
        {
            
        }

        protected override void SetGameOverCurrentLevel()
        {
            
        }

        protected override void LoadSecondaryScenes()
        {
            
        }
        
        protected override void PlayBgm(AudioManager.BgmTracks bgmTrack)
        {
            //No sound yet
        }

        protected override void GameOver(object sender, EventArgs eventArgs)
        {
            //Not implemented yet
        }
        
        protected override void LevelComplete(object sender, EventArgs eventArgs)
        {
            SetComplete();
            SceneManager.LoadScene("ExperimentLevelSelector");
        }
    }
}