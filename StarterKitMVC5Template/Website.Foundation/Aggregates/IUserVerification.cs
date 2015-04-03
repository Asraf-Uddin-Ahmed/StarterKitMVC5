using System;
namespace $safeprojectname$.Aggregates
{
    public interface IUserVerification : IEntity
    {
        DateTime CreationTime { get; set; }
        User User { get; set; }
        Guid UserID { get; set; }
        string VerificationCode { get; set; }
    }
}
