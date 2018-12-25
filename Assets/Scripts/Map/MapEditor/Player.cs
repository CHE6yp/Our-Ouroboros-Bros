using System.Collections;
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
                Chunk.playTesting = false;
                SceneManager.LoadScene(1);
            }

            if (Input.GetKeyDown(KeyCode.S))
                Chunk.instance.SaveTemplate();
            if (Input.GetKeyDown(KeyCode.N))
                Chunk.instance.NewTemplate();
            if (Input.GetKeyDown(KeyCode.P))
                PlayTest();


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
            if (Input.GetKeyDown(KeyCode.Alpha5))
                Chunk.ChangeBlockType(5);
            if (Input.GetKeyDown(KeyCode.Alpha6))
                Chunk.ChangeBlockType(6);
            if (Input.GetKeyDown(KeyCode.Alpha7))
                Chunk.ChangeBlockType(7);
            if (Input.GetKeyDown(KeyCode.Alpha8))
                Chunk.ChangeBlockType(8);
            if (Input.GetKeyDown(KeyCode.Alpha9))
                Chunk.ChangeBlockType(9);

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