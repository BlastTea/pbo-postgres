using ConsoleTables;
using Model;
using Npgsql;
using Service;
using System.Data;

internal class Program
{
    private static void Main(string[] args)
    {
        bool isStillContinue = true;
        DbHelper dbHelper = new DbHelper("localhost", 5432, "postgres", "HELLOWORLD123", "toko", "public");
        do
        {
            Console.Clear();
            List<Barang> barang = ModelConverter.FromDataTable<Barang>(dbHelper.Read("barang", "id_barang"))!;
            List<int?> ids = new List<int?>();
            ConsoleTable table = new ConsoleTable("Id Barang", "Nama Barang", "Harga Barang");
            foreach (Barang e in barang)
            {
                ids.Add(e.IdBarang);
                table.AddRow(e.IdBarang, e.Nama, e.Harga);
            }
            table.Write(format: Format.Alternative);
            Console.WriteLine("1. Tambah Barang");
            Console.WriteLine("2. Edit Barang");
            Console.WriteLine("3. Hapus Barang");
            Console.WriteLine("0. Exit");
            Console.Write("Pilihan Anda : ");
            string? choice = Console.ReadLine();
            if (choice == null)
            {
                continue;
            }

            switch (choice)
            {
                case "1":
                    {
                        // Tambah Barang
                        string? name = "";
                        int? price = 0;
                        do
                        {
                            Console.Clear();
                            Console.WriteLine("----Tambah Barang---");
                            if (name == "")
                            {
                                Console.Write("Nama  : ");
                                name = Console.ReadLine()?.Trim();
                                if (name == "")
                                {
                                    Console.Write("Nama masih kosong!!!");
                                    Console.ReadLine();
                                    continue;
                                }
                            }
                            else
                            {
                                Console.Write($"Nama  : {name}");
                            }
                            Console.Write("Harga : ");
                            string? tempPrice = Console.ReadLine();
                            try
                            {
                                price = int.Parse(tempPrice);
                            }
                            catch
                            {
                                Console.Write("Harga harus angka!!!");
                                Console.ReadLine();
                                continue;
                            }
                            if (price < 1)
                            {
                                Console.Write("Harga harus lebih dari 0!!!");
                                Console.ReadLine();
                                continue;
                            }
                            break;
                        } while (true);
                        Barang newItem = new Barang(null, name, price);
                        bool isCreated = dbHelper.Create("barang", newItem.ToJson());
                        if (isCreated)
                        {
                            Console.Write("Berhasil di tambahkan!!!");
                            Console.ReadLine();
                            continue;
                        }
                        Console.Write("Gagal di tambahkan!!!");
                        Console.ReadLine();
                        break;
                    }
                case "2":
                    {
                        // Edit Barang
                        int id = 0;
                        string? name = "";
                        int? price = 0;
                        do
                        {
                            Console.Clear();
                            Console.WriteLine("-----Edit Barang----");
                            table.Write(format: Format.Alternative);
                            if (!ids.Contains(id))
                            {
                                Console.Write("Pilih Id : ");
                                string? tempId = Console.ReadLine();
                                try
                                {
                                    id = int.Parse(tempId);
                                }
                                catch
                                {
                                    Console.Write("Id harus angka!!!");
                                    Console.ReadLine();
                                    continue;
                                }
                                if (!ids.Contains(id))
                                {
                                    Console.Write("Id tidak ditemukan!!!");
                                    Console.ReadLine();
                                    continue;
                                }
                            }
                            else
                            {
                                Console.WriteLine($"Pilih Id : {id}");

                            }

                            if (name == "")
                            {
                                Console.Write("Nama  : ");
                                name = Console.ReadLine()?.Trim();
                                if (name == "")
                                {
                                    Console.Write("Nama masih kosong!!!");
                                    Console.ReadLine();
                                    continue;
                                }
                            }
                            else
                            {
                                Console.Write($"Nama  : {name}");
                            }
                            Console.Write("Harga : ");
                            string? tempPrice = Console.ReadLine();
                            try
                            {
                                price = int.Parse(tempPrice);
                            }
                            catch
                            {
                                Console.Write("Harga harus angka!!!");
                                Console.ReadLine();
                                continue;
                            }
                            if (price < 1)
                            {
                                Console.Write("Harga harus lebih dari 0!!!");
                                Console.ReadLine();
                                continue;
                            }
                            break;
                        } while (true);
                        Barang item = new Barang(id, name, price);
                        bool isUpdated = dbHelper.Update("barang", item.ToJson(), "id_barang", id);
                        if (isUpdated)
                        {
                            Console.Write("Berhasil diedit!!!");
                            Console.ReadLine();
                            continue;
                        }
                        Console.Write("Gagal diedit!!!");
                        Console.ReadLine();
                        break;
                    }
                case "3":
                    {
                        // Hapus Barang
                        int id = 0;
                        bool isCancelDelete = true;
                        Barang selectedItem;
                        do
                        {
                            Console.Clear();
                            Console.WriteLine("----Hapus Barang----");
                            table.Write(format: Format.Alternative);
                            if (!ids.Contains(id))
                            {

                                Console.Write("Pilih Id : ");
                                string? tempId = Console.ReadLine();
                                try
                                {
                                    id = int.Parse(tempId);
                                }
                                catch
                                {
                                    Console.Write("Id harus angka!!!");
                                    Console.ReadLine();
                                    continue;
                                }
                                if (!ids.Contains(id))
                                {
                                    Console.Write("Id tidak ditemukan!!!");
                                    Console.ReadLine();
                                    continue;
                                }
                            }
                            else
                            {
                                Console.WriteLine($"Pilih Id : {id}");
                            }
                            selectedItem = barang.Where((e) => e.IdBarang == id).Single();
                            Console.Write($"Apakah anda yakin ingin menghapus {selectedItem.Nama}? (y/n) : ");
                            string? deleteChoice = Console.ReadLine();
                            if (!(deleteChoice == "y" || deleteChoice == "n"))
                            {
                                Console.Write("Jawaban harus y/n!!!");
                                Console.ReadLine();
                                continue;
                            }
                            isCancelDelete = deleteChoice == "n";
                            break;
                        } while (true);
                        if (isCancelDelete)
                        {
                            continue;
                        }
                        bool isDeleted = dbHelper.Delete("barang", "id_barang", id);
                        if (isDeleted)
                        {
                            Console.Write("Berhasil dihapus!!!");
                            Console.ReadLine();
                            continue;
                        }
                        Console.Write("Gagal dihapus!!!");
                        Console.ReadLine();
                        break;
                    }
                case "0":
                    isStillContinue = false;
                    break;
                default:
                    Console.Write("Pilihan tidak ada!!!");
                    Console.ReadLine();
                    continue;
            }
        } while (isStillContinue);
    }
}

namespace Model
{
    abstract class Model { }

    class ModelConverter
    {
        public static List<T>? FromDataTable<T>(DataTable table) where T : Model, new()
        {
            if (typeof(T) == typeof(Barang))
            {
                List<Barang> dataList = new List<Barang>();
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    DataRow data = table.Rows[i];
                    dataList.Add(new Barang((int)data["id_barang"], (String)data["nama_barang"], (int)data["harga_barang"]));
                }
                return dataList.Cast<T>().ToList();
            }
            return null;
        }
    }

    class Barang : Model
    {
        public int? IdBarang;
        public string? Nama;
        public int? Harga;

        public Barang() { }

        public Barang(int? IdBarang, string? Nama, int? Harga)
        {
            this.IdBarang = IdBarang;
            this.Nama = Nama;
            this.Harga = Harga;
        }

        public override string ToString()
        {
            return $"Barang({IdBarang}, {Nama}, {Harga})";
        }

        public Dictionary<string, dynamic> ToJson() => new Dictionary<string, dynamic> {
                {"nama_barang", Nama},
                {"harga_barang", Harga}
            };
    }
}

namespace Service
{

    class DbHelper
    {
        private string Host;
        private int Port;
        private string Username;
        private string Password;
        private string Database;
        private string Schema;
        private NpgsqlConnection Connection;

        public DbHelper(string Host, int Port, string Username, string Password, string Database, string Schema)
        {
            this.Host = Host;
            this.Port = Port;
            this.Username = Username;
            this.Password = Password;
            this.Database = Database;
            this.Schema = Schema;
            Connection = new NpgsqlConnection()
            {
                ConnectionString = $"Host={Host};Port={Port};Username={Username};Password={Password};Database={Database};"
            };
        }

        public int ExecuteNonQuery(string sql)
        {
            int affectedRows;
            try
            {
                Connection.Open();
                affectedRows = new NpgsqlCommand()
                {
                    Connection = Connection,
                    CommandText = sql
                }.ExecuteNonQuery();
                Connection.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                throw;
            }
            return affectedRows;
        }

        public DataTable ExecuteQuery(string sql)
        {
            DataTable table = new DataTable();
            try
            {
                Connection.Open();
                new NpgsqlDataAdapter(sql, Connection).Fill(table);
                Connection.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                throw;
            }
            return table;
        }

        public bool Create(string table, Dictionary<string, dynamic> data)
        {
            string column = "";
            string value = "";

            foreach (string key in data.Keys)
            {
                column += key;
                if (data[key] is int)
                {
                    value += data[key];
                }
                else
                {
                    value += $"'{data[key]}'";
                }

                if (key != data.Keys.Last())
                {
                    column += ", ";
                    value += ", ";
                }
            }

            int affectedRows;
            try
            {
                affectedRows = ExecuteNonQuery($"INSERT INTO {Schema}.{table} ({column}) VALUES ({value})");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                return false;
            }
            if (affectedRows > 0)
            {
                return true;
            }
            return false;
        }

        public DataTable Read(string table, string? orderBy) => ExecuteQuery($"SELECT * FROM {Schema}.{table} {(orderBy != null ? $"ORDER BY {orderBy}" : "")}");

        public bool Update(string table, Dictionary<string, dynamic> data, string whereColumnId, int whereId)
        {
            string set = "";
            foreach (string key in data.Keys)
            {
                if (data[key] is int)
                {
                    set += $"{key} = {data[key]}";
                }
                else
                {
                    set += $"{key} = '{data[key]}'";
                }
                if (key != data.Keys.Last())
                {
                    set += ", ";
                }
            }

            int affectedRows;
            try
            {
                affectedRows = ExecuteNonQuery($"UPDATE {Schema}.{table} SET {set} WHERE {whereColumnId} = {whereId}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                return false;
            }
            if (affectedRows > 0)
            {
                return true;
            }
            return false;
        }

        public bool Delete(string table, string whereColumnId, int whereId)
        {
            int affectedRows;
            try
            {
                affectedRows = ExecuteNonQuery($"DELETE FROM {Schema}.{table} WHERE {whereColumnId} = {whereId}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                return false;
            }
            if (affectedRows > 0)
            {
                return true;
            }
            return false;
        }
    }
}