using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MyBoilerPlate.Web.Models
{
    [DataContract]
    public class ApiResponseModel
    {
        [DataMember(Name = "message", EmitDefaultValue = false)]
        public string Message { get; set; }

        [DataMember(Name = "code", EmitDefaultValue = false)]
        public string MessageCode { get; set; }

        public ApiResponseModel() { }
        public ApiResponseModel(string _message) { Message = _message; }
    }

    [DataContract]
    public class ApiResponseModel<T> : ApiResponseModel
    {
        [DataMember(Name = "data", EmitDefaultValue = false)]
        public T Data { get; set; }

        public ApiResponseModel() { }
        public ApiResponseModel(string _message) : base(_message) { }
        public ApiResponseModel(string _message, T _data) : this(_message) { Data = _data; }
    }

    [DataContract]
    public class ApiErrorResponseModel : ApiResponseModel
    {
        [DataMember(Name = "errors", EmitDefaultValue = true)]
        public Dictionary<string, IEnumerable<string>> Errors { get; set; }

        public ApiErrorResponseModel() { }
        public ApiErrorResponseModel(string _message) : base(_message) { }
        public ApiErrorResponseModel(string _message, Dictionary<string, IEnumerable<string>> _errors) : base(_message) { }
    }
}
