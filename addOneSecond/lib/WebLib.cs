using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;


class webLib {
    public static async Task<string> HttpPost(string url, string postDataStr, string encode = "utf-8", CookieCollection cc = null) {
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        request.CookieContainer = new CookieContainer();
        if (cc != null) {
            request.CookieContainer.Add(new Uri(url), cc);
        }
        //设置请求方式为POST
        request.Method = "POST";
        //在POST里一定要注意写入Content—Length，这里的长度是指POST上传的数据的长度，可以使用Encoding中的GetByteCount方法完成
        request.Headers["Content-Length"] = Encoding.UTF8.GetByteCount(postDataStr).ToString();
        //ContentType设置为Web表单模式
        request.ContentType = "application/x-www-form-urlencoded";

        //test request header
        //request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
        //request.Headers["Accept-Encoding"] = "gzip, deflate";
        //request.Headers["Accept-Language"] = "zh-CN,zh;q=0.8,en-US;q=0.5,en;q=0.3";
        //request.Headers["Connection"] = "keep-alive";
        //request.Headers["DNT"] = "1";
        //request.Headers["Host"] = "10.3.8.211";
        //request.Headers["Referrer"] = "http://10.3.8.211/";
        //request.Headers["User-Agent"] = "Mozilla/5.0 (Windows NT 10.0; WOW64; rv:47.0) Gecko/20100101 Firefox/47.0";

        //拿到request的输入流
        Stream myRequestStream = await request.GetRequestStreamAsync();

        //use this function to register the encoding machine or it will throw expect Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

        //I Don't know why it doesn't work, so I use direct function to write
        //StreamWriter myStreamWriter = new StreamWriter(myRequestStream, Encoding.GetEncoding(encode));
        //myStreamWriter.Write(postDataStr);

        //将传输的数据转化为ASCII码写入输入流
        byte[] bs = Encoding.ASCII.GetBytes(postDataStr);
        myRequestStream.Write(bs, 0, bs.Length);

        //异步得到Response并且将Response转换为String
        HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync();
        Stream myResponseStream = response.GetResponseStream();
        StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding(encode));
        string retString = myStreamReader.ReadToEnd();
        return retString;
    }
    public static async Task<string> HttpGet(string url, string getDataStr = null, string encode = "utf-8",CookieCollection cc = null) {
        //Get方式提交数据只需要在网址后面使用？即可，如果多组数据，需要在提交的时候使用&连接
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url + (getDataStr == null ? "" : "?") + getDataStr);
        //将Cookie写入
        request.CookieContainer = new CookieContainer();
        if (cc != null) {
            request.CookieContainer.Add(new Uri(url), cc);
        }
        //设置request的方式为GET
        request.Method = "GET";
        //设置HTTP头的内容类型,如果需要在Http头中加入其他内容，可以直接使用 request.Headers["头名称"]="头内容" 来添加
        request.ContentType = "text/html;charset=UTF-8";
        //通过异步方法拿到回应
        HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync();
        //写入流
        Stream myResponseStream = response.GetResponseStream();
        //注册编码转换器（这里同之前WPF开发中不同，需要事先注册编码转换器才能使用）
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        //进行内容编码转换
        StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding(encode));
        //将转换后的内容转化为字符串并返回
        string retString = myStreamReader.ReadToEnd();
        return retString;
    }

}
