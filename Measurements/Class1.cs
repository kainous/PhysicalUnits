using System;
using Newtonsoft.Json.Linq;

namespace System.Measurements {
    public class Class1 {
        public void Test() {
            var sd = JObject.Parse("");
            var token = (JProperty)sd["Test"];
            //token.Value
        }
    }
}
