using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

using Task3_WebApiCore.BooksDB.Entities.WebEntity;

namespace Task3_WebApiCore.CmdUI
{
    public class BooksClient
    {
        private string m_baseUrl;
        
        private HttpClient m_client;

        public BooksClient(string baseUrl)
        {
            if (baseUrl != null)
            {
                m_baseUrl = baseUrl;
                m_client = new HttpClient();
                m_client.DefaultRequestHeaders.Accept.Clear();
                m_client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                return;   
            }
            throw new ArgumentNullException("входной параметр baseUrl равен null!");
        }

        public async Task<IEnumerable<KeyValuePair<string, string>>> GetStatisticsAsync()
        {
            IEnumerable<KeyValuePair<string, string>> result = null;
            HttpResponseMessage message = await m_client.GetAsync(m_baseUrl + "api/Statistics");

            if (message.IsSuccessStatusCode)
            {
                result = await message.Content.ReadAsAsync<IEnumerable<KeyValuePair<string, string>>>();
            }
            return result;
        }

        public async Task<string> GetBookDbAsync()
        {
            string result = null;
            HttpResponseMessage message = await m_client.GetAsync(m_baseUrl + "api/BookDb");

            if (message.IsSuccessStatusCode)
            {
                result = await message.Content.ReadAsAsync<string>();
            }
            return result;
        }

        public async Task<IEnumerable<WebBook>> GetBooksAsync()
        {
            IEnumerable<WebBook> books = null;
            HttpResponseMessage message = await m_client.GetAsync(m_baseUrl + "api/Book");

            if (message.IsSuccessStatusCode)
            {
                books = await message.Content.ReadAsAsync<IEnumerable<WebBook>>();
            }
            return books;
        }

        public async Task<IEnumerable<WebAuthor>> GetAuthorsAsync()
        {
            IEnumerable<WebAuthor> author = null;
            HttpResponseMessage message = await m_client.GetAsync(m_baseUrl + "api/Author");

            if (message.IsSuccessStatusCode)
            {
                author = await message.Content.ReadAsAsync<IEnumerable<WebAuthor>>();
            }
            return author;
        }

        public async Task<IEnumerable<WebPublisher>> GetPublishersAsync()
        {
            IEnumerable<WebPublisher> publisher = null;
            HttpResponseMessage message = await m_client.GetAsync(m_baseUrl + "api/Publisher");

            if (message.IsSuccessStatusCode)
            {
                publisher = await message.Content.ReadAsAsync<IEnumerable<WebPublisher>>();
            }
            return publisher;
        }

        public async Task<IEnumerable<WebAddress>> GetAddressesAsync()
        {
            IEnumerable<WebAddress> address = null;
            HttpResponseMessage message = await m_client.GetAsync(m_baseUrl + "api/Address");

            if (message.IsSuccessStatusCode)
            {
                address = await message.Content.ReadAsAsync<IEnumerable<WebAddress>>();
            }
            return address;
        }

        public async Task<WebBook> GetBookAsync(uint book_id)
        {
            WebBook book = null;
            HttpResponseMessage message = await m_client.GetAsync(m_baseUrl + "api/Book/" + book_id);

            if (message.IsSuccessStatusCode)
            {
                book = await message.Content.ReadAsAsync<WebBook>();
            }
            return book;
        }

        public async Task<WebAuthor> GetAuthorAsync(uint author_id)
        {
            WebAuthor author = null;
            HttpResponseMessage message = await m_client.GetAsync(m_baseUrl + "api/Author/" + author_id);

            if (message.IsSuccessStatusCode)
            {
                author = await message.Content.ReadAsAsync<WebAuthor>();
            }
            return author;
        }

        public async Task<WebPublisher> GetPublisherAsync(uint publisher_id)
        {
            WebPublisher publisher = null;
            HttpResponseMessage message = await m_client.GetAsync(m_baseUrl + "api/Publisher/" + publisher_id);

            if (message.IsSuccessStatusCode)
            {
                publisher = await message.Content.ReadAsAsync<WebPublisher>();
            }
            return publisher;
        }

        public async Task<WebAddress> GetAddressAsync(uint address_id)
        {
            WebAddress address = null;
            HttpResponseMessage message = await m_client.GetAsync(m_baseUrl + "api/Address/" + address_id);

            if (message.IsSuccessStatusCode)
            {
                address = await message.Content.ReadAsAsync<WebAddress>();
            }
            return address;
        }

        public async Task<bool> DeleteBookAsync(uint book_id)
        {
            HttpResponseMessage message = await m_client.DeleteAsync(m_baseUrl + "api/Book/" + book_id);
            return await message.Content.ReadAsAsync<bool>();
        }

        public async Task<bool> DeleteAuthorAsync(uint author_id)
        {
            HttpResponseMessage message = await m_client.DeleteAsync(m_baseUrl + "api/Author/" + author_id);
            return await message.Content.ReadAsAsync<bool>();
        }

        public async Task<bool> DeletePublisherAsync(uint publisher_id)
        {
            HttpResponseMessage message = await m_client.DeleteAsync(m_baseUrl + "api/Publisher/" + publisher_id);
            return await message.Content.ReadAsAsync<bool>();
        }

        public async Task<bool> DeleteAddressAsync(uint address_id)
        {
            HttpResponseMessage message = await m_client.DeleteAsync(m_baseUrl + "api/Address/" + address_id);
            return await message.Content.ReadAsAsync<bool>();
        }

        public async Task<uint> PutBookAsync(WebBook book)
        {
            if (book.BookId != 0)
            {
                HttpResponseMessage message = await m_client.PutAsJsonAsync(m_baseUrl + "api/Book/" + book.BookId, book);
                if (message.IsSuccessStatusCode)
                {
                    return await message.Content.ReadAsAsync<uint>();
                }
            }
            return 0;
        }

        public async Task<uint> PutAuthorAsync(WebAuthor author)
        {
            if (author.AuthorId != 0)
            {
                HttpResponseMessage message = await m_client.PutAsJsonAsync(m_baseUrl + "api/Author/" + author.AuthorId, author);
                if (message.IsSuccessStatusCode)
                {
                    return await message.Content.ReadAsAsync<uint>();
                }
            }
            return 0;
        }

        public async Task<uint> PutPublisherAsync(WebPublisher publisher)
        {
            if (publisher.PublisherId != 0)
            {
                HttpResponseMessage message = await m_client.PutAsJsonAsync(m_baseUrl + "api/Publisher/" + publisher.PublisherId, publisher);
                if (message.IsSuccessStatusCode)
                {
                    return await message.Content.ReadAsAsync<uint>();
                }
            }
            return 0;
        }

        public async Task<uint> PutAddressAsync(WebAddress address)
        {
            if (address.AddressId != 0)
            {
                HttpResponseMessage message = await m_client.PutAsJsonAsync(m_baseUrl + "api/Address/" + address.AddressId, address);
                if (message.IsSuccessStatusCode)
                {
                    return await message.Content.ReadAsAsync<uint>();
                }
            }
            return 0;
        }

        public async Task<uint> PostBookAsync(WebBook book)
        {
            HttpResponseMessage message = await m_client.PostAsJsonAsync(m_baseUrl + "api/Book", book);
            if (message.IsSuccessStatusCode)
            {
                return await message.Content.ReadAsAsync<uint>();
            }
            return 0;
        }

        public async Task<uint> PostAuthorAsync(WebAuthor author)
        {
            HttpResponseMessage message = await m_client.PostAsJsonAsync(m_baseUrl + "api/Author", author);
            if (message.IsSuccessStatusCode)
            {
                return await message.Content.ReadAsAsync<uint>();
            }
            return 0;
        }

        public async Task<uint> PostPublisherAsync(WebPublisher publisher)
        {
            HttpResponseMessage message = await m_client.PostAsJsonAsync(m_baseUrl + "api/Publisher", publisher);
            if (message.IsSuccessStatusCode)
            {
                return await message.Content.ReadAsAsync<uint>();
            }
            return 0;
        }

        public async Task<uint> PostAddressAsync(WebAddress address)
        {
            HttpResponseMessage message = await m_client.PostAsJsonAsync(m_baseUrl + "api/Address", address);
            if (message.IsSuccessStatusCode)
            {
                return await message.Content.ReadAsAsync<uint>();
            }
            return 0;
        }

    }
}
