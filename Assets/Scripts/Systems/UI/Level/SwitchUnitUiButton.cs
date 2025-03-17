using Systems.Units;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Systems.UI.Level
{
    public class SwitchUnitUiButton : MonoBehaviour
    {
        private UnitManagementService _unitManagement;

        private Button _button;

        [Inject]
        private void Construct(UnitManagementService unitManagement)
        {
            _unitManagement = unitManagement;

            _button = GetComponent<Button>();
            _button.onClick.AddListener(SwitchButton_OnClick);
        }

        private void SwitchButton_OnClick()
        {
            _unitManagement.SwitchToNextUnit();
        }
    }
}