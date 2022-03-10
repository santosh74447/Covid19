using System;
using System.Collections.Generic;

namespace Covid19.Models
{
    public class CountryGraphModel
    {
        public Cases cases { get; set; }
        public Deaths deaths { get; set; }
    }     
    public class Cases
    {
        public List<DateTime> date { get; set; }
        public List<string> value { get; set; }
    }
    public class Deaths
    {
        public List<DateTime> date { get; set; }
        public List<string> value { get; set; }
    }
}
