using UnityEngine;

namespace EchoOfTheTimes.UI.MainMenu
{
    public class ColumnService : MonoBehaviour
    {
        private Column[] _columns;
        private Column _activeColumn;

        private void Awake()
        {
            _columns = GetComponentsInChildren<Column>();

            for (int i = 0; i < _columns.Length; i++)
            {
                _columns[i].Id = i;
            }

            _activeColumn = _columns[0];
        }

        private void Start()
        {
            _activeColumn.SetEnable(true);
            _activeColumn.Raise();
        }

        public void HandleTouch(int index)
        {
            for (int i = 0; i < _columns.Length; i++)
            {
                _columns[i].SetEnable(false);
            }

            _activeColumn.Fall(() => {
                for (int i = 0; i < _columns.Length; i++)
                {
                    _columns[i].SetEnable(true);
                }
            });

            _activeColumn = _columns[index];
        }
    }
}