namespace NewsManager.WebUI.Helpers
{
    using System.Collections.Specialized;
    using System.Web;
    using System.Web.Routing;

    public static class RouteValueDictionaryExtensions
    {
        public static RouteValueDictionary AddQueryStringParameters(this RouteValueDictionary dict)
        {
            var querystring = HttpContext.Current.Request.QueryString;

            foreach (var key in querystring.AllKeys) 
                AddToDict(dict, key, querystring);
                
            return dict;
        }

        private static void AddToDict(RouteValueDictionary dict, string key, NameValueCollection querystring)
        {
            if (dict != null && !dict.ContainsKey(key))
            {
                var value = querystring.GetValues(key);
                if(value != null)
                    dict.Add(key, value[0]);
            }

        }

//        public static RouteValueDictionary AddContextData(this RouteValueDictionary dict)
//        {
//            var querystring = HttpContext.Current.Request.QueryString;
//            AddToDict(dict, "category", querystring);
//            AddToDict(dict, "searchBy", querystring);
//            AddToDict(dict, "searchString", querystring);
//            
//            return dict;
//        }

        public static RouteValueDictionary ExceptFor(this RouteValueDictionary dict, params string[] keysToRemove)
        {
            foreach (var key in keysToRemove)
                if (dict.ContainsKey(key))
                    dict.Remove(key);

            return dict;
        }
    }
}