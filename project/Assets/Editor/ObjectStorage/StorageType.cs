namespace UnityStaticData
{
    /// <summary>
    /// Тип хранения данных
    /// </summary>
    public enum StorageType
    {
        /// <summary>
        /// Компайл, означает, что данные будут компилироваться вместе с проектом в исходниках.
        /// </summary>
        Compile,
        /// <summary>
        /// Рантайм, означает, чт данные будут хранится ввиде ресурсов, динамичсеки подгружаемых
        /// </summary>
        Runtime,
    }
}