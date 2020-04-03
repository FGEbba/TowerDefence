using System.Collections.Generic;
using UnityEngine;

namespace Tools
{
    public interface IPool<T>
    {
        T Rent(bool returnActive);
    }

    public class GameObjectPool : IPool<GameObject>
    {
        private uint m_InitSize;
        private uint m_ExpandBy;

        private GameObject m_Prefab;
        private Transform m_Parent;

        readonly Stack<GameObject> m_Objects = new Stack<GameObject>();


        public GameObjectPool(uint initSize, GameObject prefab, uint expandBy = 1, Transform parent = null)
        {
            m_InitSize = (uint) Mathf.Max(1, initSize);
            m_ExpandBy = expandBy;
            m_Prefab = prefab;
            m_Parent = parent;
            m_Prefab.SetActive(false);
            Expand((uint) Mathf.Max(1, initSize));
        }


        private void Expand(uint amount)
        {
            for(int i = 0; i < amount; i++)
            {
                GameObject instance = Object.Instantiate(m_Prefab, m_Parent);
                m_Objects.Push(instance);
            }
        }


        public GameObject Rent(bool returnActive)
        {
            if(m_Objects.Count == 0)
            {
                Expand(m_ExpandBy);
            }

            GameObject instance = m_Objects.Pop();
            instance.SetActive(returnActive);
            return instance;
        }

        //Destroy objects if the shit aint necessary anymore
        public void Clear()
        {


        }
    }
}