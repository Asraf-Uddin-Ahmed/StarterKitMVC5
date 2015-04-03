using System;
namespace $safeprojectname$.Aggregates
{
    public interface IUser : IEntity
    {
        DateTime CreationTime { get; set; }
        string DecryptedPassword { get; }
        string EmailAddress { get; set; }
        string EncryptedPassword { get; set; }
        DateTime? LastLogin { get; set; }
        DateTime? LastWrongPasswordAttempt { get; set; }
        string Name { get; set; }
        $safeprojectname$.Enums.UserStatus Status { get; set; }
        $safeprojectname$.Enums.UserType TypeOfUser { get; set; }
        DateTime UpdateTime { get; set; }
        string UserName { get; set; }
        System.Collections.Generic.ICollection<UserVerification> UserVerifications { get; set; }
        int WrongPasswordAttempt { get; set; }
    }
}
