namespace CrossCutting.Response
{
    public class ResponseDetails
    {
        public object Result { get; set; }
        public bool Errored { get; set; }
        public string ErrorMessage { get; set; }
    }
}