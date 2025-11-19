using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace LK.Runtime.Components
{
    public class MultiPage : MonoBehaviour
    {
        [SerializeField] private List<GameObject> pages;
        [SerializeField] private UnityEvent<int> onPageTurn;
    
    
        public List<GameObject> Pages => pages;
        public int CurrentPageIndex => _currentPageIndex;
        public int PagesCount => pages.Count;
        public UnityEvent<int> OnPageTurn => onPageTurn;
    
        private int _currentPageIndex;
    
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

        public void SetCurrentPage(int pageIndex)
        {
            var newCurrentPageIndex = Mathf.Clamp(pageIndex, 0, pages.Count - 1);
        
            if (newCurrentPageIndex != _currentPageIndex)
            {
                pages[_currentPageIndex].SetActive(false);
                _currentPageIndex = newCurrentPageIndex;
                pages[_currentPageIndex].SetActive(true);
                onPageTurn.Invoke(_currentPageIndex);
            }
        }

        private void Start()
        {
            _currentPageIndex = 0;
            pages[0].SetActive(true);
            for (var i = 1; i < pages.Count; i++)
            {
                pages[i].SetActive(false);
            }
            onPageTurn.Invoke(_currentPageIndex);
        }
    }
}
