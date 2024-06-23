using System.Collections.Generic;
using System.Text.RegularExpressions;
using פרויקט2;


//htmlשל דף ה string מחזיר 
//var html = await function.Load("https://moodle.malkabruk.co.il/mod/book/view.php?id=100&chapterid=121");                                                

//htmlקובץ ה
//string html = "<!-- דף סיום קניה -->\r\n<!DOCTYPE html>\r\n<html lang=\"en\">\r\n  <head>\r\n    <meta charset=\"UTF-8\">\r\n    <meta http-equiv=\"X-UA-Compatible\" content=\"IE=edge\">\r\n    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">\r\n    <link rel=\"stylesheet\" href=\"../css/homePage.css\">\r\n    <title>סיום קניה</title>\r\n  </head>\r\n  <body>\r\n                <a href=\"../html/homePage.html\"> חזרה לדף הבית </a>\r\n    <form action=\"../html/finish.html\" style=\"width: 50%; border: 3px double;align-self: center;margin-top: 150px;margin-bottom: 20px;align-content: center;\"dir=\"rtl\" >\r\n    <legend> תשלום</legend>\r\n    <label>כתובת מייל</label>\r\n    <br>\r\n    <input id=\"cMail\" required>\r\n    <br>\r\n    <br>\r\n    <label>מספר כרטיס</label>\r\n    <br>\r\n    <input id=\"cNumber\" autocomplete=\"cNumber\" inputmode=\"numeric\" pattern=\"[\\d ]{10,30}\" required>\r\n    <br>\r\n    <br>\r\n    <label>שם בעל הכרטיס</label>\r\n    <br>\r\n    <input id=\"cName\" autocomplete=\"cName\" pattern=\"[\\p{L} \\-\\.]+\" required>\r\n    <br>\r\n    <br>\r\n    <label>תוקף הכרטיס</label>\r\n    <br>\r\n    <input id=\"cExp\"             autocomplete=\"cExp\" placeholder=\"MMYY\" pattern=\"[\\d ]{4,4}\" required>\r\n    <br>\r\n    <br>\r\n    <label>הספרות שבגב הכרטיס</label>\r\n    <br>\r\n    <input id=\"cCsc\" autocomplete=\"cCsc\" inputmode=\"numeric\" placeholder=\"cvv\" pattern=\"[\\d ]{3,4}\" required>\r\n    <br>\r\n    <br>\r\n    <input type=\"submit\" id=\"pay\" value=\"אישור ותשלום\"> \r\n    </form>\r\n    <script src=\"../js/all.js\"></script>\r\n  </body>\r\n</html>\r\n";
string html = "<!-- דף סיום קניה -->\r\n<!DOCTYPE html>\r\n<html lang=\"en\">\r\n  <head>\r\n    <meta charset=\"UTF-8\">\r\n    <meta http-equiv=\"X-UA-Compatible\" content=\"IE=edge\">\r\n    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">\r\n    <link rel=\"stylesheet\" href=\"../css/homePage.css\">\r\n    <title>סיום קניה</title>\r\n  </head>\r\n  <body class=\"c\">\r\n    <a href=\"../html/homePage.html\"> חזרה לדף הבית </a>\r\n    <form action=\"../html/finish.html\" style=\"width: 50%; border: 3px double;align-self: center;margin-top: 150px;margin-bottom: 20px;align-content: center;\"dir=\"rtl\" >\r\n    <legend> תשלום</legend>\r\n    <label>כתובת מייל</label>\r\n    <br>\r\n    <input id=\"cMail\" required>\r\n    <br>\r\n    <br>\r\n    <label>מספר כרטיס</label>\r\n    <br>\r\n    <input id=\"cNumber\" autocomplete=\"cNumber\" inputmode=\"numeric\" pattern=\"[\\d ]{10,30}\" required>\r\n    <br>\r\n    <br>\r\n    <label>שם בעל הכרטיס</label>\r\n    <br>\r\n    <input id=\"cName\" autocomplete=\"cName\" pattern=\"[\\p{L} \\-\\.]+\" required>\r\n    <br>\r\n    <br>\r\n    <label>תוקף הכרטיס</label>\r\n    <br>\r\n    <input id=\"cExp\" class=\"c\" autocomplete=\"cExp\" placeholder=\"MMYY\" pattern=\"[\\d ]{4,4}\" required>\r\n    <br>\r\n    <br>\r\n    <label>הספרות שבגב הכרטיס</label>\r\n    <br>\r\n    <input id=\"cCsc\" autocomplete=\"cCsc\" inputmode=\"numeric\" placeholder=\"cvv\" pattern=\"[\\d ]{3,4}\" required>\r\n    <br>\r\n    <br>\r\n    <input type=\"submit\" id=\"pay\" value=\"אישור ותשלום\"> \r\n    </form>\r\n    <script src=\"../js/all.js\"></script>\r\n  </body>\r\n</html>\r\n";
//המרה למחרוזת 
string[] arr = function.convertArr(html);

//בנית העץ
HtmlElement h = function.tree(arr);

//קבלת מחרוזת מהמשתמש
string s = Console.ReadLine();

//המרת המחרוזת לסלקטור
Selector selector = Selector.convertS(s);
//חיפוש כל האלמנטים התואמים לסלקטור
HashSet<HtmlElement> list = HtmlElement.fits(selector, h, new HashSet<HtmlElement>());
Console.WriteLine(list.Count);
foreach (var item in list)
{
    Console.WriteLine(item.ToString() + "\n");
}

Console.WriteLine("finish");
