using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MapEditor
{
    public class Player : MonoBehaviour
    {
        bool obstacles = false;
        int currentBlockType = 1;
        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.JoystickButton6))
            {
                SceneManager.LoadScene(1);
            }


            if (Input.GetAxis("Mouse ScrollWheel") > 0 && !Input.GetKey(KeyCode.LeftShift))
                BlockPlacer.instance.PreviousBlockType();
            if (Input.GetAxis("Mouse ScrollWheel") < 0 && !Input.GetKey(KeyCode.LeftShift))
                BlockPlacer.instance.NextBlockType();

            if (Input.GetKeyDown(KeyCode.H))
                UIManager.instance.ToggleHelp();

            if (Input.GetKeyDown(KeyCode.Tab))
                EditObstacles();

            ///
            if (!obstacles)
            {
                if (Input.GetKeyDown(KeyCode.S))
                    Chunk.instance.SaveTemplate();
                if (Input.GetKeyDown(KeyCode.N))
                    Chunk.instance.NewTemplate();
                if (Input.GetKeyDown(KeyCode.D))
                    Chunk.instance.DeleteTemplate();

                if (Input.GetAxis("Mouse ScrollWheel") > 0 && Input.GetKey(KeyCode.LeftShift))
                    Chunk.instance.PreviousTemplate();
                if (Input.GetAxis("Mouse ScrollWheel") < 0 && Input.GetKey(KeyCode.LeftShift))
                    Chunk.instance.NextTemplate();
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.S))
                    Obstacle.instance.SaveTemplate();
                if (Input.GetKeyDown(KeyCode.N))
                    Obstacle.instance.NewTemplate();
                if (Input.GetKeyDown(KeyCode.D))
                    Obstacle.instance.DeleteTemplate();

                if (Input.GetAxis("Mouse ScrollWheel") > 0 && Input.GetKey(KeyCode.LeftShift))
                    Obstacle.instance.PreviousTemplate();
                if (Input.GetAxis("Mouse ScrollWheel") < 0 && Input.GetKey(KeyCode.LeftShift))
                    Obstacle.instance.NextTemplate();
            }
        }

        void EditObstacles()
        {
            obstacles = !obstacles;
            //camera, ui
            UIManager.instance.SwichToObstacles(obstacles);
            if (obstacles)
                Camera.main.transform.localPosition += new Vector3(0, -10, 0);
            else
                Camera.main.transform.localPosition += new Vector3(0, 10, 0);
        }

    }
}