using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSlide : MonoBehaviour
{
    [SerializeField] private List<GameObject> menus;
    [SerializeField] private List<bool> isMoving = new();
    [SerializeField] private List<bool> isOpen = new();

    private void Start()
    {
        for (int i = 0; i < menus.Count; i++)
        {
            isMoving.Add(false);
            isOpen.Add(false);
        }
    }
    public void SlideTheWindow(int element)
    {
        print(element);
        switch (element)
        {
            case 0:
                if (!isMoving[element] && !isOpen[element])
                {
                    menus[0].transform.DOMoveX(transform.position.x + 70, .25f);
                    isOpen[element] = true;
                    StartCoroutine(EnsureSliding(element));
                    print("0 gel");
                }
                else if (!isMoving[element] && isOpen[element])
                {
                    menus[0].transform.DOMoveX(transform.position.x - 70, .25f);
                    isOpen[element] = false;
                    StartCoroutine(EnsureSliding(element));
                    print("0 git");
                }
                break;
            case 1:
                if (!isMoving[element] && !isOpen[element])
                {
                    menus[1].transform.DOMoveY(transform.position.y + 120, .25f);
                    isOpen[element] = true;
                    StartCoroutine(EnsureSliding(element));
                    print("1 gel");
                }
                else if (!isMoving[element] && isOpen[element])
                {
                    menus[1].transform.DOMoveY(transform.position.y - 120, .25f);
                    isOpen[element] = false;
                    StartCoroutine(EnsureSliding(element));
                    print("1 git");
                }
                break;
            case 2:
                if (!isMoving[element] && !isOpen[element])
                {
                    menus[2].transform.DOMoveX(transform.position.x + 1630, .25f);
                    isOpen[element] = true;
                    StartCoroutine(EnsureSliding(element));
                    print("2 gel");
                }
                else if (!isMoving[element] && isOpen[element])
                {
                    menus[2].transform.DOMoveX(transform.position.x + 2180, .25f);
                    isOpen[element] = false;
                    StartCoroutine(EnsureSliding(element));
                    print("2 git");
                }
                break;
        }
    }

    private IEnumerator EnsureSliding(int element)
    {
        isMoving[element] = true;
        yield return new WaitForSeconds(1);
        isMoving[element] = false;
    }
}
