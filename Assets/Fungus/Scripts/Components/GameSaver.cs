﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

namespace Fungus
{
    public class GameSaver : MonoBehaviour 
    {
        [SerializeField] protected string startScene = "";

        [SerializeField] protected List<Flowchart> flowcharts = new List<Flowchart>();

        #region Public methods

        public List<Flowchart> Flowcharts { get { return flowcharts; } }

        public virtual void Load(int slot)
        {
            var saveManager = FungusManager.Instance.SaveManager;
            saveManager.Load(slot, startScene);
        }

        public virtual void Save()
        {
            var saveManager = FungusManager.Instance.SaveManager;
            saveManager.Save();
        }

        public virtual void Delete(int slot)
        {
            var saveManager = FungusManager.Instance.SaveManager;
            saveManager.Delete(slot);
        }

        public virtual void LoadScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }

        #endregion
    }
}