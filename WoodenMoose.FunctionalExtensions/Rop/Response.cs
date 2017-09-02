namespace WoodenMoose.FunctionalExtensions.Rop
{
    public static class Response
    {
        public static Response<T> Success<T>(T data)
        {
            return Response<T>.Success(data);
        }

        public static Response<T> Failure<T>(string reason)
        {
            return Response<T>.Failure(reason);
        }
    }

    public class Response<T>
    {
        public static Response<T> Success(T data)
        {
            return new Response<T>
            {
                Type = ResponseType.Success,
                Data = data
            };
        }

        public static Response<T> Failure(string reason)
        {
            return new Response<T>
            {
                Type = ResponseType.Failure,
                FailureReason = reason
            };
        }

        private Response() { }

        public ResponseType Type { get; private set; }
        public T Data { get; private set; }
        public string FailureReason { get; private set; }
    }
}
