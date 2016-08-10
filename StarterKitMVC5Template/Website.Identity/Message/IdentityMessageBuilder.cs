using Ratul.Utility.Email;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Website.Foundation.Core;
using Website.Foundation.Core.Aggregates;
using Website.Foundation.Core.Repositories;
using Website.Foundation.Core.Services.Email;
using Website.Foundation.Persistence.Services.Email;
using Website.Foundation.Persistence.Template.Email;
using $safeprojectname$.Aggregates;

namespace $safeprojectname$.Message
{
    public class IdentityMessageBuilder : MessageBuilder, IIdentityMessageBuilder
    {
        private ApplicationUser _user;
        private string _subject;
        private string _body;

        private ApplicationUser _User
        {
            get
            {
                if (_user == null)
                {
                    throw new NullReferenceException("user value is not provided");
                }
                return _user;
            }
            set
            {
                _user = value;
            }
        }
        private string _Subject
        {
            get
            {
                if (string.IsNullOrEmpty(_subject))
                {
                    throw new NullReferenceException("subject value is not provided");
                }
                return _subject;
            }
            set
            {
                _subject = value;
            }
        }
        private string _Body
        {
            get
            {
                if (string.IsNullOrEmpty(_body))
                {
                    throw new NullReferenceException("body value is not provided");
                }
                return _body;
            }
            set
            {
                _body = value;
            }
        }


        public IdentityMessageBuilder(IUnitOfWork unitOfWork)
            : base(unitOfWork)
        { }

        public void Build(ApplicationUser user, string subject, string body)
        {
            _User = user;
            _Subject = subject;
            _Body = body;
        }



        protected override string GetSubject()
        {
            return _Subject;
        }

        protected override string GetBody()
        {
            return _Body;
        }

        protected override NameWithEmail GetFrom()
        {
            return base.GetSystemNameWithEmail();
        }

        protected override List<NameWithEmail> GetToList()
        {
            return new List<NameWithEmail>() 
            { 
                new NameWithEmail(_User.UserName, _User.Email) 
            };
        }

        protected override List<NameWithEmail> GetReplyToList()
        {
            return new List<NameWithEmail>() { base.GetSystemNameWithEmail() };
        }

    }
}
