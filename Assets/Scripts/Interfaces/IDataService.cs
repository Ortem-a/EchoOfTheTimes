using EchoOfTheTimes.Persistence;
using System.Collections.Generic;

namespace EchoOfTheTimes.Interfaces
{
    public interface IDataService
    {
        void Save(GameData data, bool overwrite = true);
        GameData Load(string name);
        void Delete(string name);
        void DeleteAll();
        IEnumerable<string> ListSaves();
    }
}