namespace QAPI.Models.DTO
{
    public class ResponseModel<T>
    {
        public string Message { get; set; }

        public bool Success { get; set; }

        public T Data { get; set; }

        public ResponseModel(string message, bool success, T data = default)
        {
            Message = message;
            Success = success;
            Data = data;
        }
        public static ResponseModel<T> SuccessResponse(T data, string message = "Operación exitosa")
        {
            return new ResponseModel<T>(message, true, data);
        }

        public static ResponseModel<T> ErrorResponse(string message)
        {
            return new ResponseModel<T>(message, false);
        }
    }
}
