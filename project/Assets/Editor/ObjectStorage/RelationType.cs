namespace UnityStaticData
{
    /// <summary>
    /// Тип свзяи сущности
    /// </summary>
    public enum RelationType
    {
        /// <summary>
        /// Один ко многим
        /// </summary>
        OneToMany = 0,
        /// <summary>
        /// Один к одному
        /// </summary>
        OneToOne = 1,
        /// <summary>
        /// Много к одному
        /// </summary>
        ManyToOne = 2,
    }

    /// <summary>
    /// Описатель связи между сущностями
    /// </summary>
    [System.Serializable]
    public struct Relation
    {
        /// <summary>
        /// Тип связи
        /// </summary>
        public RelationType RelationType    { get; set; }
        /// <summary>
        /// Имя сущности с которой установлена связь
        /// </summary>
        public string       EntityName      { get; set; }
    }
}
