namespace YCNRefine.Models
{
    public class GenerativeSampleModel
    {
        public int Id { get; set; }

        public string Input { get; set; } = null!;

        public string Output { get; set; } = null!;

        public string? Context { get; set; }

        public Guid UserIdentifier { get; set; }

        public int DatasetId { get; set; }

        public bool IsDeleted { get; set; }

        public string Name { get; set; }

        public DatasetModel? Dataset { get; set; }
    }
}
