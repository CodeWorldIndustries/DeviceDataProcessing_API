namespace CrossCutting.Response
{
    public sealed class Response<TReturn> : Response
    {
        public TReturn Data { get; set; }
    }
}
