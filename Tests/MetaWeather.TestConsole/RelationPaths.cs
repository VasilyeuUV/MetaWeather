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

        /// <summary>
        /// PI запрос информации по ID места и дате в формате YYYY/MM/DD
        /// </summary>
        public string InfoByIdDate { get; set; }
    }
}