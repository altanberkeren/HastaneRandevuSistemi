using System;
using System.IO;
using Microsoft.Data.Sqlite;

namespace HastaRandevuUI.Models
{
    public class Database
    {
        private const string DbFile = "HastaRandevu.db";

        public static SqliteConnection GetConnection()
        {
            return new SqliteConnection($"Data Source={DbFile}");
        }

        public static void Initialize()
        {
            if (!File.Exists(DbFile))
            {
                File.Create(DbFile).Close(); // Dosyayı oluştur
            }

            using var connection = GetConnection();
            connection.Open();

            var command = connection.CreateCommand();

            // Branşlar
            command.CommandText = @"
                CREATE TABLE IF NOT EXISTS Branslar (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    BransAdi TEXT NOT NULL
                );";
            command.ExecuteNonQuery();

            // Doktorlar
            command.CommandText = @"
                CREATE TABLE IF NOT EXISTS Doktorlar (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    DoktorAdi TEXT NOT NULL,
                    DoktorSoyadi TEXT NOT NULL,
                    BransID INTEGER NOT NULL,
                    FOREIGN KEY(BransID) REFERENCES Branslar(Id)
                );";
            command.ExecuteNonQuery();

            // Randevular
            command.CommandText = @"
                CREATE TABLE IF NOT EXISTS Randevular (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    HastaAdi TEXT NOT NULL,
                    HastaSoyadi TEXT NOT NULL,
                    BransID INTEGER NOT NULL,
                    DoktorID INTEGER NOT NULL,
                    Tarih TEXT NOT NULL,
                    FOREIGN KEY(BransID) REFERENCES Branslar(Id),
                    FOREIGN KEY(DoktorID) REFERENCES Doktorlar(Id)
                );";
            command.ExecuteNonQuery();
        }

        public static void SeedSampleData()
        {
            using var connection = GetConnection();
            connection.Open();

            var checkCmd = connection.CreateCommand();
            checkCmd.CommandText = "SELECT COUNT(*) FROM Branslar;";
            long count = (long)(checkCmd.ExecuteScalar() ?? 0);
            if (count > 0) return;

            var cmd = connection.CreateCommand();

            // Örnek Branşlar
            cmd.CommandText = @"
                INSERT INTO Branslar (BransAdi) VALUES 
                ('Kardiyoloji'),
                ('Dahiliye'),
                ('Göz Hastalıkları');
            ";
            cmd.ExecuteNonQuery();

            // Örnek Doktorlar
            cmd.CommandText = @"
                INSERT INTO Doktorlar (DoktorAdi, DoktorSoyadi, BransID) VALUES
                ('Ahmet', 'Yılmaz', 1),
                ('Mehmet', 'Demir', 1),
                ('Ayşe', 'Kara', 2),
                ('Fatma', 'Aksoy', 3);
            ";
            cmd.ExecuteNonQuery();
        }
    }
}
