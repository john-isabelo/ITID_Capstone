using Newtonsoft.Json;

namespace WeeklyTask.Infrastructure
{
    public static class SessionExtensions
    {
        // Define an extension method for the ISession interface that takes a key and an object value as parameters
        public static void SetJson(this ISession session, string key, object value)
        {
            // Serialize the object value into a JSON string using Newtonsoft.Json library
            var json = JsonConvert.SerializeObject(value);
            // Store the JSON string in the session using the key
            session.SetString(key, json);
        }

        // Define an extension method for the ISession interface that takes a key and returns a generic type T as the result
        public static T GetJson<T>(this ISession session, string key)
        {
            // Get the JSON string stored with the key from the session
            var json = session.GetString(key);
            // If there is no data stored with the key, return the default value of type T
            if (json == null)
            {
                return default(T);
            }
            // Otherwise, deserialize the JSON string into an object of type T using Newtonsoft.Json library and return it
            else
            {
                return JsonConvert.DeserializeObject<T>(json);
            }
        }
    }
}
