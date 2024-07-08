using Newtonsoft.Json;

namespace UI.Helperscustom
{
    public static class SaveJsonToSection
    {
        public static void SaveObject(HttpContext context, string key, object obj)
        {
            var value = JsonConvert.SerializeObject(obj);
            context.Session.SetString(key, value);
        }

        public static T GetObject<T>(HttpContext context, string key)
        {
            var data = context.Session.GetString(key);
            if(data != null)
            {
                return JsonConvert.DeserializeObject<T>(data)!;
            } else
            {
                return default!;
            }
        }
    }
}
