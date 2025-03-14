using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using _10labLib; // базовые классы: Production, Factory, Gild, Workshop

namespace LabAssignment
{
    // 1. Обобщённая коллекция, которая работает с элементами типа Production (и его наследниками).
    public class Collection<T> : IEnumerable<T> where T : Production, new()
    {
        protected List<T> items = new List<T>();

        // Свойство только для чтения, возвращающее текущее количество элементов.
        public int Length => items.Count;

        // Метод для заполнения коллекции (элементы создаются автоматически с помощью RandomInit)
        public void Fill(int count)
        {
            items.Clear();
            for (int i = 0; i < count; i++)
            {
                T item = new T();
                item.RandomInit();
                items.Add(item);
            }
        }

        // Добавление одного элемента
        public void Add(T item)
        {
            items.Add(item);
        }

        // Добавление нескольких элементов (массив объектов, если объект можно привести к типу T)
        public void AddRange(object[] array)
        {
            foreach (var obj in array)
            {
                if (obj is T tObj)
                    items.Add(tObj);
            }
        }

        // Удаление элемента по индексу – возвращает false, если индекс некорректный
        public bool RemoveAt(int index)
        {
            if (index < 0 || index >= items.Count)
                return false;
            items.RemoveAt(index);
            return true;
        }

        // Очистка коллекции
        public void Clear()
        {
            items.Clear();
        }

        // Сортировка элементов коллекции по указанному имени свойства с помощью Reflection
        public void SortByField(string fieldName)
        {
            PropertyInfo prop = typeof(T).GetProperty(fieldName);
            if (prop == null)
                throw new ArgumentException("Неверное имя поля");
            items = items.OrderBy(x => prop.GetValue(x)).ToList();
        }

        // Реализация итератора
        public IEnumerator<T> GetEnumerator()
        {
            return items.GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public override string ToString()
        {
            string result = "";
            foreach (var item in items)
            {
                result += item.ToString() + "\n";
            }
            return result;
        }
    }

    // 2. Класс MyCollection, производный от Collection<Production>
    public class MyCollection : Collection<Production>
    {
        // Можно добавить дополнительные методы, если это требуется задачей.
    }

    // 6. Делегат для событий
    public delegate void CollectionHandler(object source, CollectionHandlerEventArgs args);

    // 7. Класс аргументов событий для передачи информации об изменениях в коллекции.
    public class CollectionHandlerEventArgs : EventArgs
    {
        public string CollectionName { get; set; }
        public string ChangeType { get; set; }
        public object ChangedObject { get; set; }

        public CollectionHandlerEventArgs(string collectionName, string changeType, object changedObject)
        {
            CollectionName = collectionName;
            ChangeType = changeType;
            ChangedObject = changedObject;
        }

        public override string ToString()
        {
            return $"Коллекция: {CollectionName}, изменение: {ChangeType}, объект: {ChangedObject}";
        }
    }

    // 4 и 5. Класс MyNewCollection – наследник MyCollection, с дополнительными свойствами, индексатором и событиями.
    public class MyNewCollection : MyCollection
    {
        // Открытое автосвойство с названием коллекции.
        public string CollectionName { get; set; }

        // События, извещающие об изменениях.
        public event CollectionHandler CollectionCountChanged;
        public event CollectionHandler CollectionReferenceChanged;

        // Индексатор с методами get и set. При изменении элемента (set) генерируется событие.
        public Production this[int index]
        {
            get
            {
                if (index < 0 || index >= items.Count)
                    throw new IndexOutOfRangeException();
                return items[index];
            }
            set
            {
                if (index < 0 || index >= items.Count)
                    throw new IndexOutOfRangeException();
                items[index] = value;
                // Генерация события при изменении ссылки
                CollectionReferenceChanged?.Invoke(this, new CollectionHandlerEventArgs(CollectionName, "Заменён элемент", value));
            }
        }

        // Метод для добавления элементов по умолчанию. При каждом добавлении вызывается событие.
        public void AddDefaults(int count)
        {
            for (int i = 0; i < count; i++)
            {
                Production prod = new Production();
                prod.RandomInit();
                items.Add(prod);
                CollectionCountChanged?.Invoke(this, new CollectionHandlerEventArgs(CollectionName, "Добавлен элемент (default)", prod));
            }
        }

        // Метод добавления элементов из массива объектов.
        public void Add(object[] array)
        {
            foreach (var obj in array)
            {
                if (obj is Production prod)
                {
                    items.Add(prod);
                    CollectionCountChanged?.Invoke(this, new CollectionHandlerEventArgs(CollectionName, "Добавлен элемент", prod));
                }
            }
        }

        // Метод удаления элемента по индексу с возвратом false, если индекс некорректный.
        public bool Remove(int index)
        {
            if (index < 0 || index >= items.Count)
                return false;
            Production removed = items[index];
            bool result = RemoveAt(index);
            if (result)
                CollectionCountChanged?.Invoke(this, new CollectionHandlerEventArgs(CollectionName, "Удалён элемент", removed));
            return result;
        }
    }

    // 11. Класс JournalEntry для хранения записи об изменении коллекции.
    public class JournalEntry
    {
        public string CollectionName { get; set; }
        public string ChangeType { get; set; }
        public string ChangedData { get; set; }

        public JournalEntry(string collectionName, string changeType, string changedData)
        {
            CollectionName = collectionName;
            ChangeType = changeType;
            ChangedData = changedData;
        }

        public override string ToString()
        {
            return $"Коллекция: {CollectionName}, изменение: {ChangeType}, данные: {ChangedData}";
        }
    }

    // 11. Класс Journal, который хранит список записей изменений.
    public class Journal
    {
        public List<JournalEntry> Entries { get; private set; }

        public Journal()
        {
            Entries = new List<JournalEntry>();
        }

        // Обработчик событий, добавляющий новую запись в журнал.
        public void CollectionChangedHandler(object source, CollectionHandlerEventArgs args)
        {
            Entries.Add(new JournalEntry(args.CollectionName, args.ChangeType, args.ChangedObject.ToString()));
        }

        public override string ToString()
        {
            string result = "";
            foreach (var entry in Entries)
            {
                result += entry.ToString() + "\n";
            }
            return result;
        }
    }

    // 12–14. Демонстрационная программа
    public class Program
    {
        public static void Main(string[] args)
        {
            // Создание двух коллекций MyNewCollection.
            MyNewCollection collection1 = new MyNewCollection { CollectionName = "Collection1" };
            MyNewCollection collection2 = new MyNewCollection { CollectionName = "Collection2" };

            // Создание двух объектов Journal.
            Journal journal1 = new Journal();
            Journal journal2 = new Journal();

            // Подписываем journal1 на события CollectionCountChanged и CollectionReferenceChanged первой коллекции.
            collection1.CollectionCountChanged += journal1.CollectionChangedHandler;
            collection1.CollectionReferenceChanged += journal1.CollectionChangedHandler;

            // Подписываем journal2 на событие CollectionReferenceChanged обеих коллекций.
            collection1.CollectionReferenceChanged += journal2.CollectionChangedHandler;
            collection2.CollectionReferenceChanged += journal2.CollectionChangedHandler;

            // Внесение изменений в первую коллекцию.
            // Добавление элементов по умолчанию.
            collection1.AddDefaults(2);

            // Добавление элементов через метод Add(object[]).
            Production prod1 = new Factory("Factory1", 50, 5);
            Production prod2 = new Gild("Gild1", 30, "John");
            collection1.Add(new object[] { prod1, prod2 });

            // Удаление элемента по индексу.
            collection1.Remove(1); // удаляем элемент с индексом 1

            // Изменение элемента через индексатор (генерируется событие CollectionReferenceChanged).
            Production prod3 = new Workshop("Workshop1", 40, new List<string> { "Tool1", "Tool2" });
            collection1[0] = prod3;

            // Внесение изменений во вторую коллекцию.
            collection2.AddDefaults(3);
            // Изменение элемента во второй коллекции через индексатор.
            Production prod4 = new Factory("Factory2", 60, 3);
            collection2[1] = prod4;

            // Вывод записей обоих журналов.
            Console.WriteLine("Записи Journal1:");
            Console.WriteLine(journal1.ToString());

            Console.WriteLine("Записи Journal2:");
            Console.WriteLine(journal2.ToString());

            // Задержка консоли (для просмотра результата в консольном приложении)
            Console.ReadLine();
        }
    }
}
