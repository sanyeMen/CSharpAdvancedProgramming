using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace CSharpAdvancedProgramming.Reflect
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple=true, Inherited=true)]
    public class UserDefineAttribute : Attribute
    {

        public UserDefineAttribute(DateTime lastmodify, string msg)
        {
            this.lastModify = lastmodify;
            this.message = msg;
        }

        private DateTime lastModify;

        public DateTime LastModify
        {
            get { return lastModify; }
            set { this.lastModify = value; }
        }

        private string message;

        public string Message
        {
            get { return message; }
            set { this.message = value; }
        }


    }
}
