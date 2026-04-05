using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace LK.Runtime.Components
{
    public class MultiPage : MonoBehaviour
    {
        [SerializeField] private List<GameObject> pages;
        [SerializeField] private UnityEvent<int> onPageTurn;
    
    
        public IReadOnlyCollection<GameObject> Pages => pages.AsReadOnly();
        public int CurrentPageIndex => _currentPageIndex;
        
        public UnityEvent<int> OnPageTurn => onPageTurn;
    
        private int _currentPageIndex = -1;
        private bool isInitialized = false;
    
        public void NextPage()
        {
            SetCurrentPage(CurrentPageIndex + 1);
        }

        public void PreviousPage()
        {
            SetCurrentPage(_currentPageIndex - 1);
        }

        public void FirstPage()
        {
            SetCurrentPage(0);
        }

        public void TrailerPage()
        {
            SetCurrentPage(pages.Count - 1);
        }

        public void AddPage(GameObject page)
        {
            pages.Add(page);
            if (pages.Count == 1)
            {
                Init();
            }
            else
            {
                ValidateState();
            }
        }
        
        public void RemovePage(GameObject page)
        {
            RemovePageAt(pages.IndexOf(page)); 
        }

        public void RemovePageAt(int index)
        {
            pages.RemoveAt(index);
            if (index == _currentPageIndex)
            {
                Init();
            }
        }

        public void ClearPages()
        {
            pages.Clear();
            isInitialized = false;
        }
        
        public void SetCurrentPage(int pageIndex)
        {
            if (!isInitialized) throw new InvalidOperationException("The MultiPage has not been initialized.");
            
            var newCurrentPageIndex = Mathf.Clamp(pageIndex, 0, pages.Count - 1);
        
            if (newCurrentPageIndex != _currentPageIndex)
            {
                pages[_currentPageIndex].SetActive(false);
                _currentPageIndex = newCurrentPageIndex;
                pages[_currentPageIndex].SetActive(true);
                onPageTurn.Invoke(_currentPageIndex);
            }
        }

        private void Awake()
        {
            if(pages.Count <= 0) return;
            
            Init();
        }

        private void Init()
        {
            _currentPageIndex = 0;
            pages[0].SetActive(true);
            ValidateState();
            onPageTurn.Invoke(_currentPageIndex);
            isInitialized = true;
        }

        private void ValidateState()
        {
            for (var i = 0; i < pages.Count; i++)
            {
                if (i == _currentPageIndex && !pages[i].activeSelf)
                {
                    pages[i].SetActive(true);
                }

                if (i != _currentPageIndex && pages[i].activeSelf)
                {
                    pages[i].SetActive(false);
                }
            }
        }
    }
}
