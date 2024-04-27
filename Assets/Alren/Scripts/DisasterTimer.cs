using Alren;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

namespace Alren
{
    public class DisasterTimer : MonoBehaviour
    {
        [HideInInspector] public bool timerStarted;
        [SerializeField] private GameObject timerProgressBar;
        [SerializeField] private float timerLimit = 20;
        [SerializeField] private GameManager gameManager;
        private float timer;
        // Start is called before the first frame update
        private void Awake()
        {
            timerStarted = false;
        }
        void Start()
        {
            timer = 0;
        }

        // Update is called once per frame
        void Update()
        {
            if (timerStarted)
            {
                if (timer == 0)
                {
                    timerProgressBar.GetComponent<RectTransform>().localScale = Vector3.zero;
                }
                else if ((int)timer == (int)timerLimit)
                {
                    timer = 0;
                    gameManager.StartDisaster();
                }
                timer += Time.deltaTime;
                timerProgressBar.GetComponent<RectTransform>().localScale = new Vector3(timer / timerLimit, 1, 1);
            }

        }
    }
}

