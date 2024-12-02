using System;
using System.Collections.Generic;
using System.Text;

namespace EmployeePortal.Application.Exceptions
{
    public class BaseException : Exception
    {
        private int _code;
        private string _description;

        public int Code
        {
            get => _code;
        }
        public string Description
        {
            get => _description;
        }

        public BaseException(string message, string description, int code) : base(message)
        {
            _code = code;
            _description = description;
        }
    }
}
