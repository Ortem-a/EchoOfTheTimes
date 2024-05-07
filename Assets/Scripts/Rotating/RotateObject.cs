using UnityEngine;
using DG.Tweening;
using Zenject;
using EchoOfTheTimes.Core;
using EchoOfTheTimes.Units;

public class RotateObject : MonoBehaviour
{
    public float intervalBetweenRotations = 1f; // Интервал повторения вращения в секундах
    public float rotationDuration = 1f; // Продолжительность вращения в секундах

    private GraphVisibility _graph; // Компонент для работы с графом
    private Player _player;
    private float currentRotation = 0f; // Текущий угол поворота

    // Инициализация через Zenject
    [Inject]
    private void Construct(GraphVisibility graph, Player player)
    {
        _graph = graph;
        _player = player;
    }

    void Start()
    {
        // Проверяем, что граф был правильно инъектирован
        if (_graph == null)
        {
            Debug.LogError("GraphVisibility component is not injected properly!");
            return;
        }

        // Запускаем повторяющийся процесс вращения
        InvokeRepeating("RotateObjectMethod", 0, intervalBetweenRotations);
    }

    private void RotateObjectMethod()
    {
        // Вызываем StopAndLink при начале вращения
        _player.StopAndLink(null);

        // Вычисляем целевой угол поворота
        float targetRotation = currentRotation + 90f;

        // Вращаем объект на целевой угол вокруг оси Y с помощью DOTween
        transform.DORotate(new Vector3(0, targetRotation, 0), rotationDuration)
                 .SetEase(Ease.Linear)
                 .OnComplete(() => {
                     // Вызываем ForceUnlink при завершении вращения
                     _player.ForceUnlink();
                     // Вызываем метод GraphResetAndLoad после завершения вращения
                     GraphResetAndLoad();
                 });

        // Обновляем текущий угол поворота
        currentRotation = targetRotation;
    }

    private void GraphResetAndLoad()
    {
        // Логика перестроения графа после вращения
        _graph.ResetAndLoad();
    }
}
