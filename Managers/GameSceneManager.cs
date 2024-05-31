using System;
using Enums;
using Interfaces;
using Player;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
    public class GameSceneManager : MonoBehaviour
    {
        public Canvas canvas;
        public BasicUIWindow pauseMenu;
        public ScreenCloser screenCloser;

        public void SwitchToScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }

        public void CloseScreen(Action action)
        {
            screenCloser.CloseScreenAnim(action);
        }

        public void OpenScreen(Action action)
        {
            screenCloser.OpenScreenAnim(action);
        }

        public void ExitGame()
        {
            screenCloser.ClearAction();
            Application.Quit();
        }

        public void Pause()
        {
            pauseMenu.Open();
        }
        
        public void Pause(PlayerInput input)
        {
            pauseMenu.Open();
            input.State = PlayerState.Pause;
        }
    }
}