using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
namespace Helper
{
    /// <summary>
    /// SAMPLE:
    /// JArray jAr = new JArray();
    /// Helper.JsonArrayToggler tog = new Helper.JsonArrayToggler(jAr, "", "style=\"display:none\"", "disabled=\"disabled\"");
    /// bool b = tog["defaultElement=control-inventory-names-add, isAllowed=false"].GetValue<bool>("isHideifDisabled");
    /// b = tog["defaultElement=control-inventory-names-add"].GetValue<bool>("isAllowed");
    /// string ss = tog["defaultElement=control-inventory-names-add, isAllowed=false"]["isHideifDisabled"];
    /// 
    /// 
    /// </summary>

    public class JsonArrayToggler
    {
        private JArray _testArray;
        private Dictionary<string, string> _TestExpression;
        ToggleExpressionResult TExpression { get; set; }
        
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


        public JsonArrayToggler(JArray jArray, string Default, string OKresult, string NotOKresult)
        {
            TExpression = new ToggleExpressionResult();
            TExpression.Default = Default;
            TExpression.OK = OKresult;
            TExpression.NotOK = NotOKresult;
            _testArray = jArray;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="findMatchExpression">var1=val1, var2=val2</param>
        /// <returns></returns>
        public virtual ToggleExpressionResult this[string findMatchExpression]
        {
            get
            {
                TExpression.TargetElement = null;
                SetMatchExpression(findMatchExpression);
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
                        TExpression.TargetElement = j;
                        break;
                    }
                }

                return TExpression;
            }
        }
    }
    public class ToggleExpressionResult
    {
        public string Default { get; set; }
        public string OK { get; set; }
        public string NotOK { get; set; }
        public JObject TargetElement { get; set; }

        public override string ToString()
        {
            if (TargetElement == null)
                return NotOK;
            return OK;
        }

        public string this[string target, string targetval]
        {
            get {
                if (TargetElement == null)
                    return Default;

               if( TargetElement.GetValue(target, StringComparison.OrdinalIgnoreCase)?.Value<string>().ToLower() == targetval.ToLower())
                    return OK;
                return NotOK;
            }
        }
        public string this[string boolTarget]
        {
            get
            {
                return this[boolTarget, "true"];
            }

        }

        public T GetValue<T>(string objectName)
        {
            // if (TargetElement == null)
            // return null;
            if (TargetElement == null)
                return default(T);

            return TargetElement.GetValue(objectName, StringComparison.OrdinalIgnoreCase).Value<T>();
        }
    }


}
