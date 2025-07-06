using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Debug_SceneChange : MonoBehaviour
{
    public GameObject buttonPrefab;
    public Transform parentLayout;
    public string[] scenes;

    void Awake()
    {
        Debug.Log(scenes.Length);

        InitButtons();
    }

    private void InitButtons()
    {
        foreach (var scene in scenes)
        {
            GameObject button = Instantiate(buttonPrefab, parentLayout);
            button.GetComponentInChildren<TMP_Text>().text = scene;
            button.GetComponent<Button>().onClick.AddListener(()=>LoadScene(scene));
        }
    }

    public void LoadScene(string name) {
        SceneManager.LoadScene(name);
    }
}
