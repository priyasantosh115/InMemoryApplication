namespace Domain.Entities
{
    /// <summary>
    /// Settings for Memory cache
    /// </summary>
    public class MemoryCacheSettings
    {
        /// <summary>
        /// Maximum number of objects allowed in queue
        /// </summary>
        public int MemoryCacheObjectThreshold { get; set; }
    }
}
