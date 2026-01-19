using Newtonsoft.Json;

namespace Business.Utilities
{
   public class JsonUtil
    {
        public static string SerializeJson<T>(T t)
        {
            return JsonConvert.SerializeObject(t);
        }

        public static T DeserializeJson<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}
