using BlazorApp.Address_Analysis.Interface;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;

namespace BlazorApp.Address_Analysis.Engine
{
    public class Recognize_Engine : IRecognizeEngine
    {
        private List<string> _str_AddressList;
        private List<string> _str_matched;
        private string str_RawData;
        
        public Recognize_Engine(List<string> in_AddressList, string in_Raw_string)
        {
            _str_AddressList = new List<string>();
            _str_matched = new List<string>();
            _str_AddressList = in_AddressList;
            str_RawData = in_Raw_string;
        }

        public string Set_Info()
        {
            return _str_matched.OrderByDescending(s => s.Length).First();
        }

        public bool With_Info()
        {
            //throw new System.NotImplementedException();
            bool result = false;
            foreach (string temp_Address in _str_AddressList)
            {
                findMatching(str_RawData, temp_Address);
            }

            return result;
        }

        private void findMatching(string inRaw, string inTarget)
        {
            string pattern = $"{inTarget}";
            Match match = Regex.Match(inRaw, pattern);
            if (match.Success)
            {
                //Group g = match.Groups[0];
                //_str_matched.Add(g.ToString());
                _str_matched.Add(inTarget);
            }
        }
    }
}
