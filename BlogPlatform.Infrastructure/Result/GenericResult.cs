namespace BlogPlatform.Infrastructure.Result
{
    public class GenericResult<T> : BaseResult
    {
        public T Value { get; set; }
    }
}
