using EchoOfTheTimes.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EchoOfTheTimes.ScriptableObjects
{
    [CreateAssetMenu(menuName = "ScriptableObjects/ColorStateSettings", order = 2)]
    public class ColorStateSettingsScriptableObject : ScriptableObject
    {
        public List<ColorState> ColorStates;

        public Color GetColor(int stateId)
        {
            var colorState = ColorStates.Find((x) => x.StateId == stateId);

            if (colorState != null)
            {
                return colorState.color;
            }

            return Color.magenta;
        }
    }
}