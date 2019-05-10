using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MockEndpoint_Spike
{
    [DataContract(Name = "Response")]
    public class Response
    {
        [DataMember(Name = "Success")]
        public bool Success { get; set; }

        [DataMember(Name = "Messages")]
        public List<string> Messages { get; set; } = new List<string>();
    }
}
