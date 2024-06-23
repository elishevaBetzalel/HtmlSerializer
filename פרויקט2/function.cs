using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


namespace פרויקט2
{
    internal class function
    {
        //קריאת הקובץ ע"פ הכתובת שמתקבלת
        public static async Task<string> Load(string url)
        {
            HttpClient client = new HttpClient();
            var response = await client.GetAsync(url);
            var html = await response.Content.ReadAsStringAsync();
            return html;
        }

        //המרת קובץ למערך ללא השורות המיותרות וכדו
        public static string[] convertArr(string url)
        {
            var cleanHtml = new Regex("\\s").Replace(url, " ");
            var htmlLines = new Regex("<(.*?)>").Split(cleanHtml).Where(s => s.Length > 0 && !string.IsNullOrWhiteSpace(s));
            return htmlLines.ToArray();
        }

        //HtmlElement המרת המחרוזת לאוביקט מסוג 
        public static HtmlElement convertE(string s)
        {
            HtmlElement h = new HtmlElement();
            //אורך המילה הראשונה
            int len = s.IndexOf(" ");
            if (len < 0)
                len = s.Length;
            h.Name = s.Substring(0, len);
            s = s.Substring(len);
            //חלוקה לאלמנטים
            var attributes = new Regex("([^\\s]*?)=\"(.*?)\"").Matches(s);
            //מעבר על כל האלמנטים
            foreach (var item in attributes)
            {
                string a = item.ToString();
                //מחיקת הרווחים בתחילת המחרוזת
                while (a.StartsWith(' '))
                    a = a.Substring(1);
                //id אם יש 
                if (a.StartsWith("id"))
                {
                    //idמחיקת תחילת המחרוזת עד שם ה
                    int x = a.IndexOf("\"") + 1;
                    a = a.Substring(x);
                    if (x >= 0)
                        h.Id = a.Substring(0, a.IndexOf("\""));
                }
                //class אם יש 
                else if (a.StartsWith("class"))
                {
                    //classes מחיקת תחילת המחרוזת עד תחילת ה
                    int x = a.IndexOf("\"") + 1;
                    a = a.Substring(x);
                    if (a.IndexOf(' ') > -1)
                    {
                        //classesוהוספתם למערך ה classes חלוקת ה
                        string[] c = a.Substring(x).Split(' ');
                        foreach (var c2 in c)
                            h.Classes.Add(c2);
                    }
                    else
                        h.Classes.Add(a.Substring(0, a.IndexOf("\"")));
                }
                //Attributes תוסיף אותו ל (Id ולא class אם זה משהו אחר(לא 
                else
                    h.Attributes.Add(a);
            }
            return h;
        }
        //Serialize - בנית העץ        
        public static HtmlElement tree(string[] arr)
        {
            //josnשמירת קבצי ה
            HtmlHelper hh = HtmlHelper.Instance;
            string[] htmlTags = hh.HtmlTags;
            string[] htmlVoidTags = hh.HtmlVoidTags;

            //משתני עזר
            HtmlElement root = new HtmlElement(), child = null;
            //מעבר על כל השורות בקובץ
            for (int i = 0; i < arr.Length; i++)
            {
                //אורך המילה הראשונה במחרוזת ושמירתה
                int len = arr[i].IndexOf(' ');
                if (len == -1)
                    len = arr[i].Length;
                string word = arr[i].Substring(0, len);

                //אם הגעת לסוף הדף
                if (arr[i].StartsWith("/html"))
                    return root;
                //אם יש הערה תמשיך לשורה הבאה
                if (arr[i].StartsWith("//")) ;
                else if (arr[i].StartsWith("/*"))
                    while (arr[i++].IndexOf("*/") == -1) ;
                //אם יש תגית סוגרת תחזור לאבא
                else if (arr[i].StartsWith("/"))
                    child = child.Father;
                //אם זה תגית
                else if (htmlTags.Contains(word))
                {
                    HtmlElement h = function.convertE(arr[i]);
                    //אם זה תגית שאין לה סגירה או שהיא נסגרת בסוף השורה הנוכחית
                    if (htmlVoidTags.Contains(word))
                    {
                        //חיבור אב ובן
                        h.Father = child;
                        child.Children.Add(h);
                    }
                    else
                    {
                        //חיבור אב ובן
                        if (child != null)
                            child.Children.Add(h);
                        //אם מדובר בראשון
                        else
                            root = h;
                        h.Father = child;
                        child = h;
                    }
                }
                //אם זה סתם שורה 
                else
                    root.inerHtml += arr[i];
            }
            return root;
        }
    }
}


