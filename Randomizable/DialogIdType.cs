using System;
using System.Collections.Generic;
using Data;
using Interfaces;
using Managers;
using UnityEngine;

namespace Randomizable
{
    [Serializable]
    public class DialogIdType : IChance
    {
        public static DialogLoader _dialogLoader;
        public int chance;
        public List<string> dialogIds;
        [HideInInspector]
        public Dialog dialog;
        public int Chance => chance;

        public Dialog GetDialog()
        {
            return dialog;
        }
    }
}