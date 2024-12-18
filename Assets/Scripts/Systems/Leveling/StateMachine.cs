using System;
using System.Collections.Generic;

namespace Systems.Leveling
{
#warning МЫСЛЯ
    /*
     * а что если мы храним несколько графов, не перестраивая их
     * мы подменяем графы в зависимости от состояния
     * тогда надо решить проблему НЕ прерывания движения во время подмены графа
     * общие ID вершин для всех графов ???
     * 
     * КОГДА У НАС 1 граф, который мы перестраиваем
     * мы решаем проблему обрезания пути
     * 
     * ПРОБЛЕМА ОТНОСИТЕЛЬНОГО ДВИЖЕНИЯ ОСТАЕТСЯ В ОБОИХ ВАРИАНТАХ
     */


    public class StateMachine
    {
        private Dictionary<int, List<IStateable>> _states;

        public void ChangeState(int stateId)
        {
            var options = _states[stateId];

            for (int i = 0; i < options.Count; i++)
            {
                options[i].AcceptState(stateId);
            }
        }
    }
}