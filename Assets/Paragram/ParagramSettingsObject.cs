using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FL.Paragram
{
    [CreateAssetMenu(menuName = "FL/Paragram/Settings")]
    public class ParagramSettingsObject : ScriptableObject
    {
        public ParagramSettings Settings => _settings;

        [SerializeField] private ParagramSettings _settings = Default;
        private void Reset()
        {
            _settings = Default;
        }
        
        private static ParagramSettings Default => new ParagramSettings
        {
            CircleColor = new Color(1f, 1f, 0.2f), 
            ParalellogramColor = new Color(0.2f, 0.2f, 1f),
        };
    }
}
