using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace פרויקט2
{
    internal class HtmlElement
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<string> Attributes { get; set; }
        public List<string> Classes { get; set; }
        public string inerHtml { get; set; }
        public HtmlElement Father { get; set; }
        public List<HtmlElement> Children { get; set; }
        public override string ToString()
        {
            string attributesCon = Attributes != null ? string.Join(", ", Attributes) : "null";
            string classesCon = Classes != null ? string.Join(", ", Classes) : "null";
            string parentCon = Father != null ? Father.Name : "null";
            //string childrenCon = Children != null ? string.Join(", ", Children) : "null";
            int childrenCount = Children.Count();

            return $"Id: {Id}\nName: {Name}\nAttributes: {attributesCon}\nClasses: {classesCon}\nInnerHtml: {inerHtml}\nParent: {parentCon}\nChildren: {childrenCount}";
        }

        public HtmlElement()
        {
            Attributes = new List<string>();
            Classes = new List<string>();
            Children = new List<HtmlElement>();
            inerHtml = "";
        }
        //מחזירה רשימה של העץ ומטה
        public static IEnumerable<HtmlElement> descendants(HtmlElement element)
        {
            //יצירת תור
            Queue<HtmlElement> queue = new Queue<HtmlElement>();
            if (element != null)
                queue.Enqueue(element);
            //כל עוד התור לא ריק 
            while (queue.Count > 0)
            {
                //תוסיף את האלמנט לרשימה
                yield return queue.Peek();
                //תוציא את האלמנט העליון מהרשימה
                element = queue.Dequeue();
                //תוסיף את הילדים לתור
                foreach (HtmlElement child in element.Children)
                    queue.Enqueue(child);
            }
        }
        //מחזירה רשימה של הבן וכל אבותיו עד השורש
        public IEnumerable<HtmlElement> ancestors(HtmlElement element)
        {
            while (element.Name != "html")
            {
                yield return element;
                element = element.Father;
            }
        }

        //מחזירה רשימה של אלמנטים המתאימים לסלקטור שהתקבל
        public static HashSet<HtmlElement> fits(Selector selector, HtmlElement htmlElement, HashSet<HtmlElement> htmlElements)
        {
            //אם הגעת לעלה - התגית לא מתאימה, תחזור אחורה
            if (htmlElement == null)
                return htmlElements;
            //מערך של כל הצאצאים של האלמנט 
            IEnumerable<HtmlElement> htmls = descendants(htmlElement);
            //אם האלמנט תואמ לסלקטור 
            if ((selector.tagName == null || htmlElement.Name == selector.tagName) && (selector.id == null || htmlElement.Id == selector.id))
            {
                int i = 0;
                //של האלמנט מכילים את של הסלקטורclassesאם ה
                for (; i < selector.classes.Count; i++)
                    if (!(htmlElement.Classes.Contains(selector.classes[i])))
                        break;
                if (i == selector.classes.Count)
                    //hash אם הגעת לסוף הסלקטור תוסיף את האלמנט ל 
                    if (selector.child.child == null)
                        htmlElements.Add(htmlElement);
                    else
                        //תשלח את כל הבנים של האלמנט עם הילד של הסלקטור
                        foreach (HtmlElement h in htmlElement.Children)
                            fits(selector.child, h, htmlElements);
            }
            //תעבור על כל הבנים של האלמנט ותשלח אותם עם הסלקטור
            foreach (HtmlElement h in htmlElement.Children)
                fits(selector, h, htmlElements);
            return htmlElements;
        }
    }

    internal class HtmlHelper
    {
        public string[] HtmlTags { get; set; }
        public string[] HtmlVoidTags { get; set; }

        //יצירת מופע יחיד של המחלקה
        private readonly static HtmlHelper _instance = new HtmlHelper();
        public static HtmlHelper Instance => _instance;

        //jsonשימוש בקבצי ה
        private HtmlHelper()
        {
            string json1 = File.ReadAllText("HtmlTags.json");
            HtmlTags = JsonSerializer.Deserialize<string[]>(json1);

            string json2 = File.ReadAllText("HtmlVoidTags.json");
            HtmlVoidTags = JsonSerializer.Deserialize<string[]>(json2);
        }
    }

    public class Selector
    {
        public string tagName { get; set; }
        public string id { get; set; }
        public List<string> classes { get; set; }
        public Selector father { get; set; }
        public Selector child { get; set; }
        public Selector()
        {
            classes = new List<string>();
        }

        //Selector המרת מחרוזת של שאילתה לאוביקט 
        public static Selector convertS(string q)
        {
            //json קבצי 
            HtmlHelper hh = HtmlHelper.Instance;
            string[] htmlTags = hh.HtmlTags;
            string[] htmlVoidTags = hh.HtmlVoidTags;

            Selector root = null;
            Selector selector = new Selector();
            //חלוקה לפי רווחים
            string[] n = q.Split(' ');
            //(מעבר על כל חלקי המחרוזת והצבתם בסלקטור(כל חלק בדרגה נוספת
            foreach (string n2 in n)
            {
                //הצבה במשתנה עזר כדי לא להרוס את המחרוזת
                string n1 = n2;
                string[] nq = n1.Split('#', '.');
                int i = 0;
                if (!(n1.StartsWith('#') || n1.StartsWith('.')) && htmlTags.Contains(nq[0]))
                {
                    selector.tagName = nq[0];
                    n1 = n1.Substring(nq[0].Length);
                }
                i = 1;
                for (; i < nq.Length; i++)
                {
                    if (n1.StartsWith('#'))
                        selector.id = nq[i];
                    else if (n1.StartsWith('.'))
                        selector.classes.Add(nq[i]);
                    if (n1.IndexOf(' ') == nq[i].Length + 1)
                        n1.Substring(nq[i].Length + 1);
                    else
                        n1.Substring(nq[i].Length);
                }
                //אם השורש ריק תציב בו
                if (root == null)
                {
                    root = selector;
                    selector = new Selector();
                    selector.father = root;
                    root.child = selector;
                }
                else
                {
                    selector.child = new Selector();
                    selector.child.father = selector;
                    selector = selector.child;
                }
            }
            return root;
        }

    }

}
