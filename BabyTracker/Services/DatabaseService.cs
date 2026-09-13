using SQLite;
using BabyTracker.Models;

namespace BabyTracker.Services
{
    public class DatabaseService
    {
        private SQLiteConnection? _database;
        private readonly string _dbPath;

        public DatabaseService()
        {
            _dbPath = Path.Combine(FileSystem.AppDataDirectory, "babytracker.db3");
        }

        private SQLiteConnection GetConnection()
        {
            if (_database != null)
                return _database;

            _database = new SQLiteConnection(_dbPath);
            _database.CreateTable<BabyRecord>();
            return _database;
        }

        public void Initialize()
        {
            GetConnection();
        }

        public int SaveRecord(BabyRecord record)
        {
            var db = GetConnection();
            return db.Insert(record);
        }

        public List<BabyRecord> GetAllRecords()
        {
            var db = GetConnection();
            return db.Table<BabyRecord>()
                     .OrderByDescending(r => r.Timestamp)
                     .ToList();
        }

        public List<BabyRecord> GetRecordsByType(RecordType type)
        {
            var db = GetConnection();
            return db.Table<BabyRecord>()
                     .Where(r => r.Type == type)
                     .OrderByDescending(r => r.Timestamp)
                     .ToList();
        }

        public List<BabyRecord> GetTodayRecords()
        {
            var db = GetConnection();
            var today = DateTime.Today;
            return db.Table<BabyRecord>()
                     .Where(r => r.Timestamp >= today)
                     .OrderByDescending(r => r.Timestamp)
                     .ToList();
        }

        public void DeleteRecord(int id)
        {
            var db = GetConnection();
            var record = db.Table<BabyRecord>().FirstOrDefault(r => r.Id == id);
            if (record != null)
            {
                db.Delete(record);
            }
        }
    }
}
