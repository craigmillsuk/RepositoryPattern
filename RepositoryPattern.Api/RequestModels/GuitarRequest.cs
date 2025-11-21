namespace RepositoryPattern.Api.RequestModels
{
    public class GuitarRequest
    {
        /// <summary>
        /// Id of the guitar
        /// </summary>
        public Guid Id { get; init; }

        /// <summary>
        /// Make of the guitar
        /// </summary>
        public string Make { get; init; } = string.Empty;

        /// <summary>
        /// Model of the guitar
        /// </summary>
        public string Model { get; init; } = string.Empty;

        /// <summary>
        /// Number of frets of the guitar
        /// </summary>
        public int NumberOfFrets { get; init; }

        /// <summary>
        /// String gauge of the guitar
        /// </summary>
        public string StringGauge { get; init; } = string.Empty;

        /// <summary>
        /// Pice of the guitar
        /// </summary>
        public double Price { get; init; }
    }
}
