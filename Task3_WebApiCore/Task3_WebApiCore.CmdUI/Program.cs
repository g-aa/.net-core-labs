using System;
using System.Collections.Generic;

using Task3_WebApiCore.BooksDB.Entities.WebEntity;

namespace Task3_WebApiCore.CmdUI
{
    public class Program
    {
        private static string url = @"http://localhost:57980/";

        private static string sDefMessage = "Указанно не верное значение - '{0}'!";

        private static string[] sMainMenu = {
            "\nОсновное меню:",
            "0 - Выход",
            "1 - Очистка консоли",
            "2 - Распечатать",
            "3 - Удалить",
            "4 - Добавить",
            "5 - Статичтика",
            "6 - Обновить"
        };

        private static string[] sPrintMenu = {
            "\nМеню печати:",
            "0 - Возврат в предыдущее меню",
            "1 - Очистка консоли",
            "2 - Распечатать содержимое БД",
            "3 - Распечатать перечень авторов",
            "4 - Распечатать перечень книг",
            "5 - Распечатать перечень издательств",
            "6 - Распечатать перечень адресов"
        };

        private static string[] sRemoveMenu = {
            "\nМеню удалений:",
            "0 - Возврат в предыдущее меню",
            "1 - Очистка консоли",
            "2 - Удалить автора",
            "3 - Удалить книгу",
            "4 - Удалить издательство",
            "5 - Удалить адрес издательства"
        };

        private static string[] sAddMenu = {
            "\nМеню добавить объект:",
            "0 - Возврат в предыдущее меню",
            "1 - Очистка консоли",
            "2 - Добавить автора",
            "3 - Добавить книгу",
            "4 - Добавить издательство",
            "5 - Добавить адрес издательства"
        };

        private static string[] sUpdeteMenu = {
            "\nМеню Обновить объект:",
            "0 - Возврат в предыдущее меню",
            "1 - Очистка консоли",
            "2 - Обновить автора",
            "3 - Обновить книгу",
            "4 - Обновить издательство",
            "5 - Обновить адрес издательства"
        };

        static void Main(string[] args)
        {
            Console.WriteLine("Run...");
            Program.RunClient();
            Console.WriteLine("Stopped !");
        }

        static void RunClient()
        {
            BooksClient client = new BooksClient(url);
            while (true)
            {
                Array.ForEach(sMainMenu, (string s) => { Console.WriteLine(s); });
                char ch = char.ToLower(Console.ReadKey(true).KeyChar);
                switch (ch)
                {
                    case '0':
                        return;
                    case '1':
                        Console.Clear();
                        break;
                    case '2':
                        Program.PrintFunction(client);
                        break;
                    case '3':
                        Program.RemoveFunction(client);
                        break;
                    case '4':
                        Program.AddFunction(client);
                        break;
                    case '5':
                        IEnumerable<KeyValuePair<string, string>> statistics = client.GetStatisticsAsync().Result;
                        Console.WriteLine("\n");
                        foreach (var item in statistics)
                        {
                            Console.WriteLine(string.Format("{0,-50} {1}", item.Key, item.Value));
                        }
                        break;
                    case '6':
                        Program.UpdeteFunction(client);
                        break;
                    default:
                        Console.WriteLine(sDefMessage, ch);
                        break;
                }
                Console.WriteLine();
            }
        }

        static void PrintFunction(BooksClient client)
        {
            while (true)
            {
                Array.ForEach(sPrintMenu, (string s) => { Console.WriteLine(s); });
                char ch = char.ToLower(Console.ReadKey(true).KeyChar);
                switch (ch)
                {
                    case '0':
                        return;
                    case '1':
                        Console.Clear();
                        break;
                    case '2':
                        var allDb = client.GetBookDbAsync().Result;
                        Console.WriteLine("\n" + allDb);
                        break;
                    case '3':
                        var allAuthors = client.GetAuthorsAsync().Result;
                        Console.WriteLine("\n");
                        foreach (var item in allAuthors)
                        {
                            Console.WriteLine(item.ToString());
                        }
                        break;
                    case '4':
                        var allBooks = client.GetBooksAsync().Result;
                        Console.WriteLine("\n");
                        foreach (var item in allBooks)
                        {
                            Console.WriteLine(item.ToString());
                        }
                        break;
                    case '5':
                        var allPublishers = client.GetPublishersAsync().Result;
                        Console.WriteLine("\n");
                        foreach (var item in allPublishers)
                        {
                            Console.WriteLine(item.ToString());
                        }
                        break;
                    case '6':
                        var allAddresses = client.GetAddressesAsync().Result;
                        Console.WriteLine("\n");
                        foreach (var item in allAddresses)
                        {
                            Console.WriteLine(item.ToString());
                        }
                        break;
                    default:
                        Console.WriteLine(sDefMessage, ch);
                        break;
                }
                Console.WriteLine();
            }
        }

        static void RemoveFunction(BooksClient client)
        {
            while (true)
            {
                Array.ForEach(sRemoveMenu, (string s) => { Console.WriteLine(s); });
                char ch = char.ToLower(Console.ReadKey(true).KeyChar);
                switch (ch)
                {
                    case '0':
                        return;
                    case '1':
                        Console.Clear();
                        break;
                    case '2':
                        Program.RemoveAuthor(client);
                        break;
                    case '3':
                        Program.RemoveBook(client);
                        break;
                    case '4':
                        Program.RemovePublisher(client);
                        break;
                    case '5':
                        Program.RemoveAddress(client);
                        break;
                    default:
                        Console.WriteLine(sDefMessage, ch);
                        break;
                }
            }
        }

        static void RemoveAuthor(BooksClient client)
        {
            Console.WriteLine("\nВведите id автора для удаления,\nпример - 5:");
            string sLine = Console.ReadLine();
            if (uint.TryParse(sLine, out uint id))
            {
                var author = client.GetAuthorAsync(id).Result;
                if (author != null)
                {
                    Console.WriteLine(author.ToString());
                    Console.WriteLine("Вы хотите удалить данный объект y/n ?");
                    char ch = char.ToLower(Console.ReadKey(true).KeyChar);
                    switch (ch)
                    {
                        case 'y':
                            if (client.DeleteAuthorAsync(id).Result)
                            {
                                Console.WriteLine("Операция удаления прошла успешно!");
                            }
                            else
                            {
                                Console.WriteLine("Операция удаления прошла неуспешно!");
                            }
                            break;
                        default:
                            Console.WriteLine("Операция отменена!");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine(string.Format("Автор с id = {0} не числится в БД!", id));
                }
            }
            else
            {
                Console.WriteLine(string.Format("Введен неверный параметр {0}!", sLine));
            }
            return;
        }

        static void RemoveBook(BooksClient client)
        {
            Console.WriteLine("\nВведите id книги для удаления,\nпример - 2:");
            string sLine = Console.ReadLine();
            if (uint.TryParse(sLine, out uint id))
            {
                var ilement = client.GetBookAsync(id).Result;
                if (ilement != null)
                {
                    Console.WriteLine(ilement.ToString());
                    Console.WriteLine("Вы хотите удалить данный объект y/n ?");
                    char ch = char.ToLower(Console.ReadKey(true).KeyChar);
                    switch (ch)
                    {
                        case 'y':
                            if (client.DeleteBookAsync(id).Result)
                            {
                                Console.WriteLine("Операция удаления прошла успешно!");
                            }
                            else
                            {
                                Console.WriteLine("Операция удаления прошла неуспешно!");
                            }
                            break;
                        default:
                            Console.WriteLine("Операция отменена!");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine(string.Format("Книга с id = {0} не числится в БД!", id));
                }
            }
            else
            {
                Console.WriteLine(string.Format("Введен неверный параметр {0}!", sLine));
            }
            return;
        }

        static void RemovePublisher(BooksClient client)
        {
            Console.WriteLine("\nВведите id издателя для удаления,\nпример - 2:");
            string sLine = Console.ReadLine();
            if (uint.TryParse(sLine, out uint id))
            {
                var ilement = client.GetPublisherAsync(id).Result;
                if (ilement != null)
                {
                    Console.WriteLine(ilement.ToString());
                    Console.WriteLine("Вы хотите удалить данный объект y/n ?");
                    char ch = char.ToLower(Console.ReadKey(true).KeyChar);
                    switch (ch)
                    {
                        case 'y':
                            if (client.DeletePublisherAsync(id).Result)
                            {
                                Console.WriteLine("Операция удаления прошла успешно!");
                            }
                            else
                            {
                                Console.WriteLine("Операция удаления прошла неуспешно!");
                            }
                            break;
                        default:
                            Console.WriteLine("Операция отменена!");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine(string.Format("Издатель с id = {0} не числится в БД!", id));
                }
            }
            else
            {
                Console.WriteLine(string.Format("Введен неверный параметр {0}!", sLine));
            }
            return;
        }

        static void RemoveAddress(BooksClient client)
        {
            Console.WriteLine("\nВведите id адреса для удаления,\nпример - 2:");
            string sLine = Console.ReadLine();
            if (uint.TryParse(sLine, out uint id))
            {
                var ilement = client.GetAddressAsync(id).Result;
                if (ilement != null)
                {
                    Console.WriteLine(ilement.ToString());
                    Console.WriteLine("Вы хотите удалить данный объект y/n ?");
                    char ch = char.ToLower(Console.ReadKey(true).KeyChar);
                    switch (ch)
                    {
                        case 'y':
                            if (client.DeleteAddressAsync(id).Result)
                            {
                                Console.WriteLine("Операция удаления прошла успешно!");
                            }
                            else
                            {
                                Console.WriteLine("Операция удаления прошла неуспешно!");
                            }
                            break;
                        default:
                            Console.WriteLine("Операция отменена!");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine(string.Format("Адрес с id = {0} не числится в БД!", id));
                }
            }
            else
            {
                Console.WriteLine(string.Format("Введен неверный параметр {0}!", sLine));
            }
            return;
        }

        static void UpdeteFunction(BooksClient client)
        {
            while (true)
            {
                Array.ForEach(sUpdeteMenu, (string s) => { Console.WriteLine(s); });
                char ch = char.ToLower(Console.ReadKey(true).KeyChar);
                switch (ch)
                {
                    case '0':
                        return;
                    case '1':
                        Console.Clear();
                        break;
                    case '2':
                        Program.UpdateAuthor(client);
                        break;
                    case '3':
                        Program.UpdateBook(client);
                        break;
                    case '4':
                        Program.UpdatePublisher(client);
                        break;
                    case '5':
                        Program.UpdateAddress(client);
                        break;
                    default:
                        Console.WriteLine(sDefMessage, ch);
                        break;
                }
            }
        }

        static Dictionary<string, string> GetParams(string inputString, string[] paramsString)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();

            foreach (string s in inputString.Split('-'))
            {
                foreach (string p in paramsString)
                {
                    if (s.ToLower().Contains(p.ToLower()) && !result.ContainsKey(p.ToLower()))
                    {
                        result.Add(p.ToLower(), s.ToLower().Replace(p.ToLower() + ":", "").Trim());
                        break;
                    }
                }
            }
            return result;
        }

        static void UpdateBook(BooksClient client)
        {
            Console.WriteLine("\nВведите id книги для выполнения операции обновления,\nпример - 5:");
            string sLine = Console.ReadLine();
            if (uint.TryParse(sLine, out uint id))
            {
                WebBook book = client.GetBookAsync(id).Result;
                if (book != null)
                {
                    Console.WriteLine(book.GetFullString());
                    Console.WriteLine("Вы хотите обновить данный объект y/n ?");
                    char ch = char.ToLower(Console.ReadKey(true).KeyChar);
                    switch (ch)
                    {
                        case 'y':
                            Console.WriteLine("Введите редактируемуе параметры книги: ");
                            var inputParams = Program.GetParams(Console.ReadLine(), WebBook.GetStringHeaders());
                            if (inputParams.Count != 0)
                            {
                                foreach (var param in inputParams)
                                {
                                    if ("title".Equals(param.Key))
                                    {
                                        book.Title = param.Value;
                                    }
                                    else if ("year".Equals(param.Key))
                                    {
                                        if (int.TryParse(param.Value, out int year))
                                        {
                                            book.Year = year;
                                        }
                                    }
                                    else if ("authorsid".Equals(param.Key))
                                    {
                                        List<int> aIds = new List<int>();
                                        foreach (var sId in param.Value.Split(','))
                                        {
                                            if (int.TryParse(sId, out int bId))
                                            {
                                                aIds.Add(bId);
                                            }
                                        }
                                        book.AuthorsId = aIds;
                                    }
                                    else if ("publisherid".Equals(param.Key))
                                    {
                                        if (int.TryParse(param.Value, out int publisherid))
                                        {
                                            book.PublisherId = publisherid;
                                        }
                                    }
                                }
                                if (client.PutBookAsync(book).Result != 0)
                                {
                                    Console.WriteLine("Операция обновления прошла успешно!");
                                }
                                else
                                {
                                    Console.WriteLine("Операция обновления прошла неуспешно!");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Переданы неверные параметры для книги!");
                            }

                            break;
                        default:
                            Console.WriteLine("Операция отменена!");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine(string.Format("Книга с id = {0} не числится в БД!", id));
                }
            }
            else
            {
                Console.WriteLine(string.Format("Введен неверный параметр {0}!", sLine));
            }
            return;
        }

        static void UpdateAuthor(BooksClient client)
        {
            Console.WriteLine("\nВведите id автора для выполнения операции обновления,\nпример - 5:");
            string sLine = Console.ReadLine();
            if (uint.TryParse(sLine, out uint id))
            {
                var author = client.GetAuthorAsync(id).Result;
                if (author != null)
                {
                    Console.WriteLine(author.GetFullString());
                    Console.WriteLine("Вы хотите обновить данный объект y/n ?");
                    char ch = char.ToLower(Console.ReadKey(true).KeyChar);
                    switch (ch)
                    {
                        case 'y':
                            Console.WriteLine("Введите редактируемуе параметры автора: ");
                            var inputParams = Program.GetParams(Console.ReadLine(), WebAuthor.GetStringHeaders());
                            if (inputParams.Count != 0)
                            {
                                foreach (var param in inputParams)
                                {
                                    if ("firstname".Equals(param.Key))
                                    {
                                        author.FirstName = param.Value;
                                    }
                                    else if ("lastname".Equals(param.Key))
                                    {
                                        author.LastName = param.Value;
                                    }
                                    else if ("bookid".Equals(param.Key))
                                    {
                                        List<int> bIds = new List<int>();
                                        foreach (var sId in param.Value.Split(','))
                                        {
                                            if (int.TryParse(sId, out int bId))
                                            {
                                                bIds.Add(bId);
                                            }
                                        }
                                        author.BooksId = bIds;
                                    }
                                }
                                if (client.PutAuthorAsync(author).Result != 0)
                                {
                                    Console.WriteLine("Операция обновления прошла успешно!");
                                }
                                else
                                {
                                    Console.WriteLine("Операция обновления прошла неуспешно!");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Переданы неверные параметры для автора!");
                            }

                            break;
                        default:
                            Console.WriteLine("Операция отменена!");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine(string.Format("Автор с id = {0} не числится в БД!", id));
                }
            }
            else
            {
                Console.WriteLine(string.Format("Введен неверный параметр {0}!", sLine));
            }
            return;
        }

        static void UpdatePublisher(BooksClient client)
        {
            Console.WriteLine("\nВведите id издателя для выполнения операции обновления,\nпример - 5:");
            string sLine = Console.ReadLine();
            if (uint.TryParse(sLine, out uint id))
            {
                WebPublisher publisher = client.GetPublisherAsync(id).Result;
                if (publisher != null)
                {
                    Console.WriteLine(publisher.GetFullString());
                    Console.WriteLine("Вы хотите обновить данный объект y/n ?");
                    char ch = char.ToLower(Console.ReadKey(true).KeyChar);
                    switch (ch)
                    {
                        case 'y':
                            Console.WriteLine("Введите редактируемуе параметры издателя: ");
                            var inputParams = Program.GetParams(Console.ReadLine(), WebPublisher.GetStringHeaders());
                            if (inputParams.Count != 0)
                            {
                                foreach (var param in inputParams)
                                {
                                    if ("title".Equals(param.Key))
                                    {
                                        publisher.Title = param.Value;
                                    }
                                    else if ("booksid".Equals(param.Key))
                                    {
                                        List<int> bIds = new List<int>();
                                        foreach (var sId in param.Value.Split(','))
                                        {
                                            if (int.TryParse(sId, out int bId))
                                            {
                                                bIds.Add(bId);
                                            }
                                        }
                                        publisher.BooksId = bIds;
                                    }
                                    else if ("addressesid".Equals(param.Key))
                                    {
                                        List<int> aIds = new List<int>();
                                        foreach (var sId in param.Value.Split(','))
                                        {
                                            if (int.TryParse(sId, out int bId))
                                            {
                                                aIds.Add(bId);
                                            }
                                        }
                                        publisher.AddressesId = aIds;
                                    }
                                }
                                if (client.PutPublisherAsync(publisher).Result != 0)
                                {
                                    Console.WriteLine("Операция обновления прошла успешно!");
                                }
                                else
                                {
                                    Console.WriteLine("Операция обновления прошла неуспешно!");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Переданы неверные параметры для издателя!");
                            }
                            break;
                        default:
                            Console.WriteLine("Операция отменена!");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine(string.Format("Издатель с id = {0} не числится в БД!", id));
                }
            }
            else
            {
                Console.WriteLine(string.Format("Введен неверный параметр {0}!", sLine));
            }
            return;
        }

        static void UpdateAddress(BooksClient client)
        {
            Console.WriteLine("\nВведите id адреса для выполнения операции обновления,\nпример - 5:");
            string sLine = Console.ReadLine();
            if (uint.TryParse(sLine, out uint id))
            {
                WebAddress address = client.GetAddressAsync(id).Result;
                if (address != null)
                {
                    Console.WriteLine(address.GetFullString());
                    Console.WriteLine("Вы хотите обновить данный объект y/n ?");
                    char ch = char.ToLower(Console.ReadKey(true).KeyChar);
                    switch (ch)
                    {
                        case 'y':
                            Console.WriteLine("Введите редактируемуе параметры адреса: ");
                            var inputParams = Program.GetParams(Console.ReadLine(), WebAddress.GetStringHeaders());
                            if (inputParams.Count != 0)
                            {
                                foreach (var param in inputParams)
                                {
                                    if ("country".Equals(param.Key))
                                    {
                                        address.Country = param.Value;
                                    }
                                    else if ("city".Equals(param.Key))
                                    {
                                        address.City = param.Value;
                                    }
                                    else if ("street".Equals(param.Key))
                                    {
                                        address.Street = param.Value;
                                    }
                                    else if ("publisherid".Equals(param.Key))
                                    {
                                        if (int.TryParse(param.Value, out int pId))
                                        {
                                            address.PublisherId = pId;
                                        }
                                    }
                                }
                                if (client.PutAddressAsync(address).Result != 0)
                                {
                                    Console.WriteLine("Операция обновления прошла успешно!");
                                }
                                else
                                {
                                    Console.WriteLine("Операция обновления прошла неуспешно!");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Переданы неверные параметры для адреса!");
                            }
                            break;
                        default:
                            Console.WriteLine("Операция отменена!");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine(string.Format("Адрес с id = {0} не числится в БД!", id));
                }
            }
            else
            {
                Console.WriteLine(string.Format("Введен неверный параметр {0}!", sLine));
            }
            return;
        }

        static void AddFunction(BooksClient client)
        {
            while (true)
            {
                Array.ForEach(sAddMenu, (string s) => { Console.WriteLine(s); });
                char ch = char.ToLower(Console.ReadKey(true).KeyChar);
                switch (ch)
                {
                    case '0':
                        return;
                    case '1':
                        Console.Clear();
                        break;
                    case '2':
                        Program.AddAuthor(client);
                        break;
                    case '3':
                        Program.AddBook(client);
                        break;
                    case '4':
                        Program.AddPublisher(client);
                        break;
                    case '5':
                        Program.AddAddress(client);
                        break;
                    default:
                        Console.WriteLine(sDefMessage, ch);
                        break;
                }
            }
        }

        static void AddBook(BooksClient client)
        {
            WebBook book = new WebBook();

            Console.WriteLine("Введите параметры для новой книги:");
            Dictionary<string, string> newParametrs = Program.GetParams(Console.ReadLine(), WebBook.GetStringHeaders());
            if (newParametrs.Count != 0)
            {
                foreach (KeyValuePair<string, string> parametr in newParametrs)
                {
                    if ("title".Equals(parametr.Key))
                    {
                        book.Title = parametr.Value;
                    }
                    else if ("year".Equals(parametr.Key))
                    {
                        if (int.TryParse(parametr.Value, out int year))
                        {
                            book.Year = year;
                        }
                    }
                    else if ("authorsid".Equals(parametr.Key))
                    {
                        List<int> aIds = new List<int>();
                        foreach (var sId in parametr.Value.Split(','))
                        {
                            if (int.TryParse(sId, out int bId))
                            {
                                aIds.Add(bId);
                            }
                        }
                        book.AuthorsId = aIds;
                    }
                    else if ("publisherid".Equals(parametr.Key))
                    {
                        if (int.TryParse(parametr.Value, out int publisherid))
                        {
                            book.PublisherId = publisherid;
                        }
                    }
                }
                Console.WriteLine(string.Format("Были введены следующие параметры: {0}", book.GetFullString()));

                uint book_id = client.PostBookAsync(book).Result;
                if (book_id != 0)
                {
                    Console.WriteLine(string.Format("Операция записи прошла успешно, id = {0}!", book_id));
                }
                else
                {
                    Console.WriteLine("Операция записи прошла неуспешно!");
                }
            }
            else
            {
                Console.WriteLine("Переданы неверные параметры для книги!");
            }
            return;
        }

        static void AddAuthor(BooksClient client)
        {
            WebAuthor author = new WebAuthor();

            Console.WriteLine("Введите параметры для нового автора:");
            Dictionary<string, string> newParametrs = Program.GetParams(Console.ReadLine(), WebAuthor.GetStringHeaders());
            if (newParametrs.Count != 0)
            {
                foreach (KeyValuePair<string, string> parametr in newParametrs)
                {
                    if ("firstname".Equals(parametr.Key))
                    {
                        author.FirstName = parametr.Value;
                    }
                    else if ("lastname".Equals(parametr.Key))
                    {
                        author.LastName = parametr.Value;
                    }
                    else if ("bookid".Equals(parametr.Key))
                    {
                        List<int> bIds = new List<int>();
                        foreach (var sId in parametr.Value.Split(','))
                        {
                            if (int.TryParse(sId, out int bId))
                            {
                                bIds.Add(bId);
                            }
                        }
                        author.BooksId = bIds;
                    }
                }
                Console.WriteLine(string.Format("Были введены следующие параметры: {0}", author.GetFullString()));

                uint author_id = client.PostAuthorAsync(author).Result;
                if (author_id != 0)
                {
                    Console.WriteLine(string.Format("Операция записи прошла успешно, id = {0}!", author_id));
                }
                else
                {
                    Console.WriteLine("Операция записи прошла неуспешно!");
                }
            }
            else
            {
                Console.WriteLine("Переданы неверные параметры для автора!");
            }
            return;
        }

        static void AddPublisher(BooksClient client)
        {
            WebPublisher publisher = new WebPublisher();

            Console.WriteLine("Введите параметры для нового издателя:");
            Dictionary<string, string> newParametrs = Program.GetParams(Console.ReadLine(), WebPublisher.GetStringHeaders());
            if (newParametrs.Count != 0)
            {
                foreach (KeyValuePair<string, string> parametr in newParametrs)
                {
                    if ("title".Equals(parametr.Key))
                    {
                        publisher.Title = parametr.Value;
                    }
                    else if ("booksid".Equals(parametr.Key))
                    {
                        List<int> bIds = new List<int>();
                        foreach (var sId in parametr.Value.Split(','))
                        {
                            if (int.TryParse(sId, out int bId))
                            {
                                bIds.Add(bId);
                            }
                        }
                        publisher.BooksId = bIds;
                    }
                    else if ("addressesid".Equals(parametr.Key))
                    {
                        List<int> aIds = new List<int>();
                        foreach (var sId in parametr.Value.Split(','))
                        {
                            if (int.TryParse(sId, out int bId))
                            {
                                aIds.Add(bId);
                            }
                        }
                        publisher.AddressesId = aIds;
                    }
                }
                Console.WriteLine(string.Format("Были введены следующие параметры: {0}", publisher.GetFullString()));

                uint publisher_id = client.PostPublisherAsync(publisher).Result;
                if (publisher_id != 0)
                {
                    Console.WriteLine(string.Format("Операция записи прошла успешно, id = {0}!", publisher_id));
                }
                else
                {
                    Console.WriteLine("Операция записи прошла неуспешно!");
                }
            }
            else
            {
                Console.WriteLine("Переданы неверные параметры для издателя!");
            }
            return;
        }

        static void AddAddress(BooksClient client)
        {
            WebAddress address = new WebAddress();

            Console.WriteLine("Введите параметры для нового адреса:");
            Dictionary<string, string> newParametrs = Program.GetParams(Console.ReadLine(), WebAddress.GetStringHeaders());
            if (newParametrs.Count != 0)
            {
                foreach (KeyValuePair<string, string> parametr in newParametrs)
                {
                    if ("country".Equals(parametr.Key))
                    {
                        address.Country = parametr.Value;
                    }
                    else if ("city".Equals(parametr.Key))
                    {
                        address.City = parametr.Value;
                    }
                    else if ("street".Equals(parametr.Key))
                    {
                        address.Street = parametr.Value;
                    }
                    else if ("publisherid".Equals(parametr.Key))
                    {
                        if (int.TryParse(parametr.Value, out int pId))
                        {
                            address.PublisherId = pId;
                        }
                    }
                }
                Console.WriteLine(string.Format("Были введены следующие параметры: {0}", address.GetFullString()));

                uint address_id = client.PostAddressAsync(address).Result;
                if (address_id != 0)
                {
                    Console.WriteLine(string.Format("Операция записи прошла успешно, id = {0}!", address_id));
                }
                else
                {
                    Console.WriteLine("Операция записи прошла неуспешно!");
                }
            }
            else
            {
                Console.WriteLine("Переданы неверные параметры для адреса!");
            }
            return;
        }
    }
}
