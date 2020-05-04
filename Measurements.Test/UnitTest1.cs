using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;

namespace Measurements.Test {
    [TestClass]
    public class UnitTest1 {
        [TestMethod]
        public void TestMethod1() {
            var jobj = JObject.Parse(File.ReadAllText(@"..\..\..\..\Measurements\Configuration.json"));
            var dim = jobj["Dimensions"];
            foreach (JProperty sdf3 in dim) {
                var sdf = sdf3.Value<string>("Name");
                //var sdfosidn = sdf3.Key;
            }
        }
    }
}
