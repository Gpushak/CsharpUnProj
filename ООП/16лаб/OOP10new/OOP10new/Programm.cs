using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Serialization;
using _10labLib;
#pragma warning disable SYSLIB0011  // Для использования BinaryFormatter

namespace Lab16App
{
    // Собственная коллекция из лаб.12
    public class DoublyLinkedList<T>
    {
        public class Node
        {
            public T Value;
            public Node Next, Prev;
            public Node(T v) { Value = v; }
        }
        public Node Head, Tail;
        public void AddLast(T v)
        {
            var n = new Node(v);
            if (Head == null) Head = Tail = n;
            else { Tail.Next = n; n.Prev = Tail; Tail = n; }
        }
        public bool Remove(Predicate<T> match)
        {
            for (var n = Head; n != null; n = n.Next)
            {
                if (match(n.Value))
                {
                    if (n.Prev != null) n.Prev.Next = n.Next;
                    else Head = n.Next;
                    if (n.Next != null) n.Next.Prev = n.Prev;
                    else Tail = n.Prev;
                    return true;
                }
            }
            return false;
        }
        public IEnumerable<T> Enumerate()
        {
            for (var n = Head; n != null; n = n.Next)
                yield return n.Value;
        }
    }

    // Менеджер коллекции
    public class CollectionManager
    {
        private DoublyLinkedList<Production> data = new DoublyLinkedList<Production>();

        public void Add(Production p) => data.AddLast(p);
        public void Delete(string key) => data.Remove(x => x.Name.Equals(key, StringComparison.OrdinalIgnoreCase));
        public Production Find(string key) => data.Enumerate().FirstOrDefault(x => x.Name.Equals(key, StringComparison.OrdinalIgnoreCase));
        public void Update(string key, Action<Production> update)
        {
            var p = Find(key);
            if (p != null) update(p);
        }
        public IEnumerable<Production> GetAll() => data.Enumerate();
        public IEnumerable<Production> SortBy(Func<Production, object> keySel, bool desc = false)
            => desc ? data.Enumerate().OrderByDescending(keySel) : data.Enumerate().OrderBy(keySel);
        public IEnumerable<Production> FilterByType(Type t)
            => data.Enumerate().Where(x => x.GetType() == t);
        public IEnumerable<Production> FilterByField(Func<Production, bool> pred)
            => data.Enumerate().Where(pred);

        // Сериализация
        public void SaveBinary(string path)
        {
            using var fs = new FileStream(path, FileMode.Create);
            new BinaryFormatter().Serialize(fs, data.Enumerate().ToList());
        }
        public void SaveJson(string path)
        {
            var opts = new JsonSerializerOptions { WriteIndented = true };
            File.WriteAllText(path, JsonSerializer.Serialize(data.Enumerate().ToList(), opts));
        }
        public void SaveXml(string path)
        {
            var ser = new XmlSerializer(typeof(List<Production>), new Type[] { typeof(Factory), typeof(Gild), typeof(Workshop) });
            using var fs = new FileStream(path, FileMode.Create);
            ser.Serialize(fs, data.Enumerate().ToList());
        }
        public void SaveText(string path)
        {
            File.WriteAllLines(path, data.Enumerate().Select(x => $"{x.Name},{x.EmployeeCount}"));
        }
        public void LoadBinary(string path)
        {
            using var fs = new FileStream(path, FileMode.Open);
            var list = (List<Production>)new BinaryFormatter().Deserialize(fs);
            Reload(list);
        }
        public void LoadJson(string path)
        {
            var list = JsonSerializer.Deserialize<List<Production>>(File.ReadAllText(path));
            Reload(list);
        }
        public void LoadXml(string path)
        {
            var ser = new XmlSerializer(typeof(List<Production>), new Type[] { typeof(Factory), typeof(Gild), typeof(Workshop) });
            using var fs = new FileStream(path, FileMode.Open);
            var list = (List<Production>)ser.Deserialize(fs);
            Reload(list);
        }
        public void LoadText(string path)
        {
            var lines = File.ReadAllLines(path);
            data = new DoublyLinkedList<Production>();
            foreach (var ln in lines)
            {
                var parts = ln.Split(',');
                if (parts.Length >= 2 && int.TryParse(parts[1], out int cnt))
                {
                    var p = new Production { Name = parts[0], EmployeeCount = cnt };
                    data.AddLast(p);
                }
            }
        }
        private void Reload(List<Production> list)
        {
            data = new DoublyLinkedList<Production>();
            foreach (var p in list) data.AddLast(p);
        }

        // Измерение времени записи
        public void MeasureSave(int count)
        {
            var list = new List<Production>();
            for (int i = 0; i < count; i++) { var p = new Production(); p.RandomInit(); list.Add(p); }
            Measure("Binary sync", () => SaveListBinary(list, "tmp.bin"));
            Measure("Binary async", () => SaveListBinaryAsync(list, "tmp.bin").Wait());
            Measure("JSON sync", () => SaveListJson(list, "tmp.json"));
            Measure("JSON async", () => SaveListJsonAsync(list, "tmp.json").Wait());
            Measure("XML sync", () => SaveListXml(list, "tmp.xml"));
            Measure("Text sync", () => SaveListText(list, "tmp.txt"));
        }
        void Measure(string name, Action action)
        {
            var sw = Stopwatch.StartNew(); action(); sw.Stop(); Console.WriteLine($"{name}: {sw.ElapsedMilliseconds} ms");
        }
        void SaveListBinary(List<Production> list, string path)
        {
            using var fs = new FileStream(path, FileMode.Create);
            new BinaryFormatter().Serialize(fs, list);
        }
        async Task SaveListBinaryAsync(List<Production> list, string path)
        {
            using var ms = new MemoryStream();
            new BinaryFormatter().Serialize(ms, list);
            await File.WriteAllBytesAsync(path, ms.ToArray());
        }
        void SaveListJson(List<Production> list, string path)
            => File.WriteAllText(path, JsonSerializer.Serialize(list));
        async Task SaveListJsonAsync(List<Production> list, string path)
            => await File.WriteAllTextAsync(path, JsonSerializer.Serialize(list));
        void SaveListXml(List<Production> list, string path)
        {
            var ser = new XmlSerializer(typeof(List<Production>), new Type[] { typeof(Factory), typeof(Gild), typeof(Workshop) });
            using var fs = new FileStream(path, FileMode.Create);
            ser.Serialize(fs, list);
        }
        void SaveListText(List<Production> list, string path)
            => File.WriteAllLines(path, list.Select(x => $"{x.Name},{x.EmployeeCount}"));
    }

    class Program
    {
        static void Main()
        {
            var mgr = new CollectionManager();
            while (true)
            {
                Console.WriteLine("\n--- Меню приложения коллекций ---");
                Console.WriteLine("1. Добавить элемент");
                Console.WriteLine("2. Показать все элементы");
                Console.WriteLine("3. Удалить по ключу");
                Console.WriteLine("4. Корректировать по ключу");
                Console.WriteLine("5. Поиск по ключу");
                Console.WriteLine("6. Сортировать");
                Console.WriteLine("7. Фильтровать");
                Console.WriteLine("8. Сохранить коллекцию (.bin/.json/.xml)");
                Console.WriteLine("9. Загрузить коллекцию (.bin/.json/.xml/.txt)");
                Console.WriteLine("10. Сохранить в текстовый файл (.txt)");
                Console.WriteLine("11. Измерить время записи (1000 элементов)");
                Console.WriteLine("0. Выход");
                Console.Write("Выбор команды: ");
                if (!int.TryParse(Console.ReadLine(), out int choice)) continue;
                switch (choice)
                {
                    case 1:
                        Console.Write("Тип (1-Factory,2-Gild,3-Workshop): ");
                        var t = Console.ReadLine();
                        Production p = t == "1" ? new Factory() : t == "2" ? new Gild() : new Workshop();
                        p.RandomInit(); mgr.Add(p);
                        break;
                    case 2:
                        var all = mgr.GetAll().ToList();
                        if (!all.Any()) Console.WriteLine("Коллекция пуста.");
                        else all.ForEach(x => x.Show());
                        break;
                    case 3:
                        Console.Write("Имя для удаления: "); mgr.Delete(Console.ReadLine());
                        break;
                    case 4:
                        Console.Write("Имя для корректировки: "); var keyUp = Console.ReadLine();
                        mgr.Update(keyUp, x => { Console.Write("Новое имя: "); x.Name = Console.ReadLine(); });
                        break;
                    case 5:
                        Console.Write("Имя для поиска: "); var found = mgr.Find(Console.ReadLine());
                        if (found == null) Console.WriteLine("Не найдено."); else found.Show();
                        break;
                    case 6:
                        Console.Write("Сортировать по (1-Name,2-EmployeeCount): "); var sc = Console.ReadLine();
                        var sorted = mgr.SortBy(x => sc == "1" ? x.Name : (object)x.EmployeeCount);
                        sorted.ToList().ForEach(x => x.Show());
                        break;
                    case 7:
                        Console.Write("Фильтровать по типу (1-Factory,2-Gild,3-Workshop): "); var ft = Console.ReadLine();
                        Type ty = ft == "1" ? typeof(Factory) : ft == "2" ? typeof(Gild) : typeof(Workshop);
                        mgr.FilterByType(ty).ToList().ForEach(x => x.Show());
                        break;
                    case 8:
                        Console.Write("Путь для сохранения (.bin/.json/.xml): "); var sp = Console.ReadLine();
                        SaveByExt(mgr, sp);
                        break;
                    case 9:
                        Console.Write("Путь для загрузки (.bin/.json/.xml/.txt): "); var lp = Console.ReadLine();
                        LoadByExt(mgr, lp);
                        break;
                    case 10:
                        Console.Write("Путь для текстового файла (.txt): "); var tp = Console.ReadLine();
                        mgr.SaveText(tp);
                        break;
                    case 11:
                        mgr.MeasureSave(1000);
                        break;
                    case 0:
                        return;
                }
            }
        }

        static void SaveByExt(CollectionManager mgr, string path)
        {
            try
            {
                switch (Path.GetExtension(path).ToLower())
                {
                    case ".bin": mgr.SaveBinary(path); break;
                    case ".json": mgr.SaveJson(path); break;
                    case ".xml": mgr.SaveXml(path); break;
                    default: Console.WriteLine("Неподдерживаемый формат."); break;
                }
            }
            catch { Console.WriteLine("Ошибка при сохранении."); }
        }

        static void LoadByExt(CollectionManager mgr, string path)
        {
            try
            {
                switch (Path.GetExtension(path).ToLower())
                {
                    case ".bin": mgr.LoadBinary(path); break;
                    case ".json": mgr.LoadJson(path); break;
                    case ".xml": mgr.LoadXml(path); break;
                    case ".txt": mgr.LoadText(path); break;
                    default: Console.WriteLine("Неподдерживаемый формат."); break;
                }
            }
            catch { Console.WriteLine("Ошибка при загрузке."); }
        }
    }
}
#pragma warning restore SYSLIB0011