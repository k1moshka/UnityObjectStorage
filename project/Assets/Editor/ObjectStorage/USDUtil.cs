namespace UnityStaticData
{
    /// <summary>
    /// Хранит вспомогательные методы
    /// </summary>
    public static class USDUtil
    {
        /// <summary>
        /// Возвращает имя для поля класса которое предоставляет связанные индексы
        /// </summary>
        /// <param name="entityName">Имя сущности поля</param>
        /// <param name="isMany">Показывает список сущностей или одна сущность хранится в поле</param>
        /// <returns></returns>
        public static string GetNameForIndexes(string entityName, bool isMany)
        {
            return GetNameForPrivateField(entityName, isMany) + "Indexes";
        }
        /// <summary>
        /// Возвращает имя поля класса которое предоставляет сущности
        /// </summary>
        /// <param name="entityName">Имя сущности поля</param>
        /// <param name="isMany">Показывает список сущностей или одна сущность хранится в поле</param>
        /// <returns></returns>
        public static string GetNameForPrivateField(string entityName, bool isMany = false)
        {
            var result = "_" + entityName.ToLower();

            return isMany ? result + "s" : result;
        }
        /// <summary>
        /// Получение имени для линка загружаемого свойства 
        /// </summary>
        /// <param name="propertyName">Имя свойства для которого нужно получить имя поля линка</param>
        /// <returns></returns>
        public static string GetLinkName(string propertyName)
        {
            return "_" + propertyName.ToLower() + "Link";
        }
        /// <summary>
        /// Возвращет путь валидный для Resource.Load метода
        /// </summary>
        /// <param name="fullPathToResources">Полный путь до ресурса прим. Assets/Resource/Resource.txt</param>
        /// <returns></returns>
        public static string GetLocalizedPath(string fullPathToResources)
        {
            var resIndex = fullPathToResources.IndexOf("Resources/");
            if (resIndex == -1) return fullPathToResources;

            var extensionIndex = fullPathToResources.LastIndexOf('.');
            if (extensionIndex == -1)
                extensionIndex = fullPathToResources.Length;

            UnityEngine.Debug.Log(string.Format("{0}", new object[] {  fullPathToResources.Substring(
                resIndex + 10,
                extensionIndex - resIndex - 10) }));

            return fullPathToResources.Substring(
                resIndex + 10,
                extensionIndex - resIndex - 10);
        }
    }
}