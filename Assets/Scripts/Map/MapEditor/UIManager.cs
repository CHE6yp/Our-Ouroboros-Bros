using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MapEditor
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager instance;
        public GameObject helpPanel;
        
        void Awake()
        {
            instance = this;
        }

        void Update()
        {

        }

        public void ToggleHelp()
        {
            helpPanel.SetActive(!helpPanel.activeSelf);
        }
    }
}
