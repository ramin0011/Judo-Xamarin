using Android.Webkit;
using Java.Lang;
using Java.Interop;

namespace JudoDotNetXamarinAndroidSDK
{
   
    public class JsonParsingJavaScriptInterface : Java.Lang.Object
    {

        JsonListener _callBack;

        public JsonParsingJavaScriptInterface (JsonListener callback)
        {
            _callBack = callback;
        }

        [Export]
        [JavascriptInterface]
        public void parseJsonFromHtml (string content)
        {
            if (content != null && content.Length > 0) {
                try {
                    var startIndex = content.IndexOf ("{");
                    string json = content.Substring (startIndex, (content.LastIndexOf ("}") - startIndex + 1));
                    _callBack.onJsonReceived (json);
                    //jsonListener.onJsonReceived (json);
                } catch (Exception ignore) {
                }
            }
        }

        public interface JsonListener
        {
            void onJsonReceived (string json);
        }
    }
}

