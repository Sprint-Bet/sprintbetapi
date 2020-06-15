namespace SprintBetApi.Dtos
{
    public class ErrorMessage
    {
        public ErrorMessage(string error)
        {
            Message = error;
        }

        public string Message { get; set; }
    }
}
