using Microsoft.AspNetCore.Identity;

namespace IdentityProject.CutomValidation
{
    public class CustomIdentityErrorDescriber : IdentityErrorDescriber
    {
        public override IdentityError InvalidUserName(string userName)
        {
            return new IdentityError()
            {
                Code = "InvalidUserName",
                Description = $" Bu {userName} geçersizdir"
            };
        }


        public override IdentityError DuplicateEmail(string email)
        {
            return new IdentityError()
            {
                Code = "DuplicateEmail",
                Description = $"Bu {email} zaten kullanılmaktadır."
            };
        }


        public override IdentityError DuplicateUserName(string userName)
        {
            return new IdentityError()
            {
                Code = "DuplicateUserName",
                Description = $"Bu {userName} adlı kullanıcı zaten kullanılmaktadır"
            };
        }

        public override IdentityError PasswordTooShort(int length)
        {
            return new IdentityError
            {
                Code = "PasswordTooShort",
                Description = $" Şifreniz en az {length} karakterli olmalıdır."
            };
        }


    }

}
