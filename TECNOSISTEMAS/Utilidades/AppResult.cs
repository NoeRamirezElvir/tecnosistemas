namespace TECNOSISTEMAS.Utilidades
{
    public class AppResult
    {
        public bool IsValid { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }

        public static AppResult Error(string message)
        {
            return new AppResult { Message = message, IsValid = false };
        }
        public static AppResult Success(string message, object Data)
        {
            return new AppResult { Message = message, Data = Data, IsValid = true };
        }

    }
}
