using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OngProject.Infrastructure
{
    public class GenericResult<T>
    {
        public GenericResult()
        {
           
        }
        public GenericResult(T data)
        {
            data = data;
        }
        /// <summary>
        /// Allows you to know info about your request
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// Allows you to know status about your request
        /// </summary>
        public bool IsSuccess { get; set; }
        /// <summary>
        /// If exist list of errors
        /// </summary>
        // public IEnumerable<string> Errors { get; set; }
        public string Error { get; set; }
        /// <summary>
        /// requested object
        /// </summary>
        public T data { get; set; }
    }
}
