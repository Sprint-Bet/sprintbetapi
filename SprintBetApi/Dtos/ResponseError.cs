namespace SprintBetApi.Dtos
{
    public class ResponseError
    {
        public ResponseError(string error)
        {
            Message = error;
        }

        public string Message { get; set; }
    }
}
