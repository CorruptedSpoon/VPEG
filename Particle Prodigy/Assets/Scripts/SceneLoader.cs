﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void LoadSceneOnClick(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }
}
