using System;
using System.Collections.Generic;
using System.Text;

using Task3_WebApiCore.BooksDB.Entities;
using Task3_WebApiCore.BooksDB.Entities.DbEntity;
using Task3_WebApiCore.BooksDB.Entities.WebEntity;
using Task3_WebApiCore.BooksDB.Enumeration;

namespace Task3_WebApiCore.BooksDB.Abstraction
{
    public interface IBooksRepository
    {
        public String Print(Entitie entitie);
        public List<Object> GetAllEntities(Entitie entitie);
        public Object GetEntitie(Entitie entitie, Int32 entitie_id);
        public Int32 Add(Object obj);
        public Int32 Refresh(Object obj);
        public Boolean Remove(Object obj);
        public IEnumerable<KeyValuePair<String, String>> GetStatistics();
        public Int32 GetEntitieCount(Entitie entitie);
    }
}
