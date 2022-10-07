namespace MultiTenancy.Application.DTOs.Response
{
    public class RegisterUserResponse
    {
        public bool Success { get; private set; }
        public List<string> Errors { get; private set; }

        public RegisterUserResponse() => Errors = new List<string>();

        public RegisterUserResponse(bool success = true) : this() => Success = success;

        public void AddErrors(IEnumerable<string> errors) => Errors.AddRange(errors);
    }
}