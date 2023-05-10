namespace YCNRefine.Models
{
    public class OriginalSourceModel
    {
        public int Id { get; set; }

        public string Name { get; set; }    

        public int DatasetId { get; set; }

        public DatasetModel Dataset { get; set; }
    }
}
