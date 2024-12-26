namespace Data.Model
{
    public class GenralResponse<T>
    {
        

            public bool Success { get; set; }
            public string Message { get; set; }
            public T Data { get; set; }
            public List<string> Errors { get; set; } = new List<string>();
        }
    
}
