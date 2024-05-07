using UnityEngine;
using DG.Tweening;
using Zenject;
using EchoOfTheTimes.Core;
using EchoOfTheTimes.Units;

public class RotateObject : MonoBehaviour
{
    public float intervalBetweenRotations = 1f; // �������� ���������� �������� � ��������
    public float rotationDuration = 1f; // ����������������� �������� � ��������

    private GraphVisibility _graph; // ��������� ��� ������ � ������
    private Player _player;
    private float currentRotation = 0f; // ������� ���� ��������

    // ������������� ����� Zenject
    [Inject]
    private void Construct(GraphVisibility graph, Player player)
    {
        _graph = graph;
        _player = player;
    }

    void Start()
    {
        // ���������, ��� ���� ��� ��������� ������������
        if (_graph == null)
        {
            Debug.LogError("GraphVisibility component is not injected properly!");
            return;
        }

        // ��������� ������������� ������� ��������
        InvokeRepeating("RotateObjectMethod", 0, intervalBetweenRotations);
    }

    private void RotateObjectMethod()
    {
        // �������� StopAndLink ��� ������ ��������
        _player.StopAndLink(null);

        // ��������� ������� ���� ��������
        float targetRotation = currentRotation + 90f;

        // ������� ������ �� ������� ���� ������ ��� Y � ������� DOTween
        transform.DORotate(new Vector3(0, targetRotation, 0), rotationDuration)
                 .SetEase(Ease.Linear)
                 .OnComplete(() => {
                     // �������� ForceUnlink ��� ���������� ��������
                     _player.ForceUnlink();
                     // �������� ����� GraphResetAndLoad ����� ���������� ��������
                     GraphResetAndLoad();
                 });

        // ��������� ������� ���� ��������
        currentRotation = targetRotation;
    }

    private void GraphResetAndLoad()
    {
        // ������ ������������ ����� ����� ��������
        _graph.ResetAndLoad();
    }
}
