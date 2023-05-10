namespace YCNRefine.Models
{
    public class ChatModel
    {
        public int? Id { get; set; }

        public int DatasetId { get; set; }

        public string? Name { get; set; }    

        public DatasetModel? Dataset { get; set; }   

        public IEnumerable<MessageModel>? Messages { get; set; }
    }
}
