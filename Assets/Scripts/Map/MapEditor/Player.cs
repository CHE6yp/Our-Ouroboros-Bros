using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MapEditor
{
    public class Player : MonoBehaviour
    {

        int currentBlockType = 1;
        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.JoystickButton6))
            {
                Chunk.playTesting = false;
                SceneManager.LoadScene(1);
            }

            if (Input.GetKeyDown(KeyCode.S))
                Chunk.instance.SaveTemplate();
            if (Input.GetKeyDown(KeyCode.N))
                Chunk.instance.NewTemplate();
            if (Input.GetKeyDown(KeyCode.D))
                Chunk.instance.DeleteTemplate();
            if (Input.GetKeyDown(KeyCode.P))
                PlayTest();


            if (Input.GetAxis("Mouse ScrollWheel") > 0)
                if (Input.GetKey(KeyCode.LeftShift))
                    Chunk.instance.PreviousTemplate();
                else
                    Chunk.instance.PreviousBlockType();

            if (Input.GetAxis("Mouse ScrollWheel") < 0)
                if (Input.GetKey(KeyCode.LeftShift))
                    Chunk.instance.NextTemplate();
                else
                    Chunk.instance.NextBlockType(); 


            if (Input.GetKeyDown(KeyCode.Alpha1))
                Chunk.instance.ChangeBlockType(1);
            if (Input.GetKeyDown(KeyCode.Alpha2))
                Chunk.instance.ChangeBlockType(2);
            if (Input.GetKeyDown(KeyCode.Alpha3))
                Chunk.instance.ChangeBlockType(3);
            if (Input.GetKeyDown(KeyCode.Alpha4))
                Chunk.instance.ChangeBlockType(4);
            if (Input.GetKeyDown(KeyCode.Alpha5))
                Chunk.instance.ChangeBlockType(5);
            if (Input.GetKeyDown(KeyCode.Alpha6))
                Chunk.instance.ChangeBlockType(6);
            if (Input.GetKeyDown(KeyCode.Alpha7))
                Chunk.instance.ChangeBlockType(7);
            if (Input.GetKeyDown(KeyCode.Alpha8))
                Chunk.instance.ChangeBlockType(8);
            if (Input.GetKeyDown(KeyCode.Alpha9))
                Chunk.instance.ChangeBlockType(9);

            if (Input.GetKeyDown(KeyCode.H))
                UIManager.instance.ToggleHelp();
        }

        public void PlayTest()
        {
            Chunk.playTesting = true;
            Chunk.playTestTemplate = Chunk.instance.GetTemplate();
            SceneManager.LoadScene(1);
        }
    }
}