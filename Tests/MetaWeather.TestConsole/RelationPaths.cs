namespace MetaWeather.TestConsole
{
    internal class RelationPaths
    {
        /// <summary>
        /// API запрос по имени
        /// </summary>
        public string LocationByName { get; set; }

        /// <summary>
        /// API запрос по координатам
        /// </summary>
        public string LocationByCoord { get; set; }

        /// <summary>
        /// API запрос информации по ID места 
        /// </summary>
        public string InfoById { get; set; }
    }
}