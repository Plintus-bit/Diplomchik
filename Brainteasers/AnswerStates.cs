using System;
using System.Collections.Generic;
using UnityEngine;

namespace Brainteasers
{
    [Serializable]
    public class AnswerStates<T>
    {
        [SerializeField] private List<bool> answerStates;
        [SerializeField]private List<T> trueValues;
        [SerializeField]private List<T> startValues;

        public List<T> StartValues => startValues;

        public void Init()
        {
            if (startValues.Count == 1)
            {
                for (int i = 1; i < trueValues.Count; i++)
                {
                    startValues.Add(startValues[0]);
                }
            }
            SetStartAnswerStates();
        }
        
        public bool CheckAnswer(int index, T data)
        {
            if (trueValues[index].Equals(data))
            {
                answerStates[index] = true;
                return true;
            }
            answerStates[index] = false;
            return false;
        }

        public void SetStartAnswerStates()
        {
            for (int i = 0; i < startValues.Count; i++)
            {
                CheckAnswer(i, startValues[i]);
            }
        }

        public bool CheckIsWin()
        {
            foreach (var answer in answerStates)
            {
                if (!answer) return false;
            }
            return true;
        }
    }
}