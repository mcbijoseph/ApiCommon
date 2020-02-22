using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
namespace Helper
{
    public class JsonArrayToggler
    {
        private JArray _testArray;
        private Dictionary<string, string> _TestExpression;
        string OkResult;
        string NotOkResult;

        private void SetMatchExpression(string TestExpression)
        {
            TestExpression = TestExpression.ToLower();
            _TestExpression = new Dictionary<string, string>();
            foreach (string ex in TestExpression.Split(','))
            {
                string[] s = ex.Split('=');

                if (s.Length > 1)
                {
                    _TestExpression.Add(s[0].Trim(), s[1].Trim());
                }
            }
        }


        public JsonArrayToggler(JArray jArray, string TestExpression, string OKresult, string NotOKresult)
        {
            OkResult = OKresult;
            NotOkResult = NotOKresult;
            _testArray = jArray;
            SetMatchExpression(TestExpression);
        }

        public virtual string this[string findName]
        {
            get
            {
                bool isValid = false;
                foreach (JObject j in _testArray)
                {
                    //Validation
                    int validCount = _TestExpression.Keys.Count;
                    foreach (string s in _TestExpression.Keys)
                    {
                        //None Case Sensitive 
                        string jValue = j.GetValue(s, StringComparison.OrdinalIgnoreCase)?.Value<string>().ToLower();
                        if (jValue == _TestExpression[s])
                        {
                            validCount--;
                        }
                    }
                    if (validCount == 0)
                    {
                        isValid = true;
                        break;
                    }
                }
                if (isValid)
                    return OkResult;                
                return NotOkResult;
            }
        }

    }
}
