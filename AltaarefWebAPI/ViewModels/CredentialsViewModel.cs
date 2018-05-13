using AltaarefWebAPI.Validations;
using FluentValidation.Attributes;

namespace AltaarefWebAPI.ViewModels
{
    [Validator(typeof(CredentialsViewModelValidator))]
    public class CredentialsViewModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
