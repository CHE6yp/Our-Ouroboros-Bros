﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MapEditor
{
    public class Player : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.JoystickButton6))
            {
                SceneManager.LoadScene(0);
            }

            if (Input.GetKeyDown(KeyCode.S))
                Chunk.instance.SaveTemplate();
            if (Input.GetKeyDown(KeyCode.R))
                Chunk.instance.ReverseMap();
            if (Input.GetKeyDown(KeyCode.N))
                Chunk.instance.NewTemplate();

            if (Input.GetAxis("Mouse ScrollWheel") > 0)
                Chunk.instance.PreviousTemplate();
            if (Input.GetAxis("Mouse ScrollWheel") < 0)
                Chunk.instance.NextTemplate();


            if (Input.GetKeyDown(KeyCode.Alpha1))
                Chunk.ChangeBlockType(1);
            if (Input.GetKeyDown(KeyCode.Alpha2))
                Chunk.ChangeBlockType(2);
            if (Input.GetKeyDown(KeyCode.Alpha3))
                Chunk.ChangeBlockType(3);
            if (Input.GetKeyDown(KeyCode.Alpha4))
                Chunk.ChangeBlockType(4);

            if (Input.GetKeyDown(KeyCode.H))
                UIManager.instance.ToggleHelp();
        }
    }
}