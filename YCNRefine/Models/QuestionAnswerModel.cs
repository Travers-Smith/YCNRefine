namespace YCNRefine.Models
{
    public class QuestionAnswerModel
    {
        public int? Id { get; set; }

        public string Question { get; set; }    

        public string Answer { get; set; }

        public int OriginalSourceId { get; set; }

        public bool CorrectAnswer { get; set; }
    }
}
