namespace AvansFysioWebservice.Model.Errors
{
    public class ErrorMessage405
    {
        public int Status { get; set; } = 405;
        public string Message { get; set; } = "Method not allowed.";
    }
}
