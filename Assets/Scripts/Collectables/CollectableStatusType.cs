namespace EchoOfTheTimes.Collectables
{
    public enum CollectableStatusType
    {
        /// <summary>
        /// Не собран, лежит на уровне
        /// </summary>
        NotCollected,
        /// <summary>
        /// Собран, не применен
        /// </summary>
        Unassigned,
        /// <summary>
        /// Собран, применен
        /// </summary>
        Assigned
    }
}