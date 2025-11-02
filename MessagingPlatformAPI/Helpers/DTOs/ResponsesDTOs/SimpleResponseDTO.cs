namespace MessagingPlatformAPI.Helpers.DTOs.ResponsesDTOs
{
    public class SimpleResponseDTO
    {
        public bool IsSuccess { get; set; }
        public string? Message { get; set; }
    }
}
/*
public class SimpleResponseDTO<T>
    {
        public bool IsSuccess { get; set; }
        public string? Message { get; set; }
        public T? obj { get; set; }
    }

---> no error


public class SimpleResponseDTO
    {
        public bool IsSuccess { get; set; }
        public string? Message { get; set; }
        public T? obj { get; set; }
    }

---> error (T is the error)
*/ 
// ??????????????????

