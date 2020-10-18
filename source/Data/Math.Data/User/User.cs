using System;
using System.Collections.Generic;
using System.Text;

namespace SoonLearning.Assessment.Data
{
    public class User : BaseObject
    {
        public string LoginId
        {
            get;
            set;
        }

        public string Password
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public string Gender
        {
            get;
            set;
        }

        public DateTime? Birthday
        {
            get;
            set;
        }

        public string Class
        {
            get;
            set;
        }

        public string Grade
        {
            get;
            set;
        }

        public string UserType
        {
            get;
            set;
        }

        public DateTime CreateDate
        {
            get;
            set;
        }

        public DateTime? ExpireDate
        {
            get;
            set;
        }

        public DateTime? LastLoginTime
        {
            get;
            set;
        }
    }
}
