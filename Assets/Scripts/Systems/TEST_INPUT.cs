using Systems.Leveling;
using UnityEngine;

public class TEST_INPUT : MonoBehaviour
{
    public BridgeService BridgeService;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log("Disconnect");
            BridgeService.Disconnect();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Connect");
            BridgeService.Connect();
        }
    }
}
