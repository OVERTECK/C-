namespace Lib
{
    public class BaseAddress
    {
        public static string Current
        {
            get {
                return DeviceInfo.Current.Platform == DevicePlatform.Android
                    ? "http://10.0.2.2:8080" : "https://localhost:8000";
            }
        }
    }
}
