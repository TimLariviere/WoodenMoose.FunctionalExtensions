namespace WoodenMoose.FunctionalExtensions.Rop
{
    public class Response<T>
    {
        public ResponseType Type { get; set; }
        public T Data { get; set; }
        public string FailureReason { get; set; }
    }
}
