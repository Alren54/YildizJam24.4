using Alren;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
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
        private GameObject selectedBay;
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
                    PickVictim();
                }
                timer += Time.deltaTime;
                timerProgressBar.GetComponent<RectTransform>().localScale = new Vector3(timer / timerLimit, 1, 1);
            }

        }
        void PickVictim()
        {
            selectedBay = gameManager.GatherBay();
            StartCoroutine(StartDisasterTimer());
        }

        public IEnumerator StartDisasterTimer()
        {
            print("Felaketin kime yapilcagi belli");
            GameObject victimBay = selectedBay;
            victimBay.GetComponent<Renderer>().material.color = Color.black;

            yield return new WaitForSeconds(victimBay.GetComponent<tzdevil.Gameplay.Hexagon>().HexagonType switch
            {
                HexagonType.Sand => 1f,
                HexagonType.Stone => 1f,
                _ => throw new Exception("Kaç saniye beklicem bu cisim ne")
            });
            StartCoroutine(gameManager.StartDisaster(victimBay));
        }
    }
}

