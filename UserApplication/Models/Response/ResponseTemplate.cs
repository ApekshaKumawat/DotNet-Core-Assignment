namespace UserApplication.Models.Response
{
    public class ResponseTemplate<T>
    {
  
            public T? Data { get; set; }
            public string? Message { get; set; }
            public List<Error>? Errors { get; set; }
        }

        public class Error
        {
            public string? Message { get; set; }
            public string? Code { get; set; }
        }
}

