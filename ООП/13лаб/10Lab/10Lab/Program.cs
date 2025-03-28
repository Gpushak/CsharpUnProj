using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using _10labLib; 

namespace LabAssignment
{
    // Перечисление для типа изменений в коллекции.
    public enum ChangeTypeEnum
    {
        Added,
        Removed,
        Replaced
    }

    // 1. Класс MyCollection<T> на базе Collection<T>
    // Ограничиваем тип T наследованием от Production.
    public class MyCollection<T> : Collection<T> where T : Production
    {
        public string CollectionName { get; set; }

        public MyCollection(string name)
        {
            CollectionName = name;
        }

        // Метод для добавления элементов по умолчанию.
        // Здесь добавляются, например, два объекта базового класса Production.
        public void AddDefaults()
        {
            // Можно использовать конструктор с параметрами или метод RandomInit(), если нужно.
            Add((T)Activator.CreateInstance(typeof(T), "Production_A", 10));
            Add((T)Activator.CreateInstance(typeof(T), "Production_B", 20));
        }
    }

    // 2. Класс MyNewCollection<T> с поддержкой событий
    public class MyNewCollection<T> : MyCollection<T> where T : Production
    {
        // События для изменений количества элементов и для замены ссылки
        public event EventHandler<CollectionHandlerEventArgs>? CollectionCountChanged;
        public event EventHandler<CollectionHandlerEventArgs>? CollectionReferenceChanged;

        public MyNewCollection(string name) : base(name)
        {
        }

        // Переопределяем метод Add для генерации события при добавлении элемента
        public new void Add(T item)
        {
            base.Add(item);
            OnCollectionCountChanged(ChangeTypeEnum.Added, item);
        }

        // Удаление элемента по индексу с генерацией события
        public bool Remove(int index)
        {
            if (index < 0 || index >= Count)
                return false;

            T item = this[index];
            base.RemoveAt(index);
            OnCollectionCountChanged(ChangeTypeEnum.Removed, item);
            return true;
        }

        // Переопределяем индексатор для генерации события при замене элемента
        public new T this[int index]
        {
            get => base[index];
            set
            {
                T oldItem = base[index];
                base[index] = value;
                OnCollectionReferenceChanged(ChangeTypeEnum.Replaced, oldItem, value);
            }
        }

        // Метод генерации события для добавления/удаления
        protected virtual void OnCollectionCountChanged(ChangeTypeEnum changeType, T item)
        {
            CollectionCountChanged?.Invoke(this, new CollectionHandlerEventArgs(CollectionName, changeType, item.ToString()));
        }

        // Метод генерации события для замены элемента
        protected virtual void OnCollectionReferenceChanged(ChangeTypeEnum changeType, T oldItem, T newItem)
        {
            CollectionReferenceChanged?.Invoke(this, new CollectionHandlerEventArgs(CollectionName, changeType, $"Old: {oldItem}, New: {newItem}"));
        }
    }

    // 3. Класс аргументов события
    public class CollectionHandlerEventArgs : EventArgs
    {
        public string CollectionName { get; }
        public ChangeTypeEnum ChangeType { get; }
        public string ObjectInfo { get; }

        public CollectionHandlerEventArgs(string collectionName, ChangeTypeEnum changeType, string objectInfo)
        {
            CollectionName = collectionName;
            ChangeType = changeType;
            ObjectInfo = objectInfo;
        }

        public override string ToString()
        {
            return $"Collection: {CollectionName}, Change: {ChangeType}, Object: {ObjectInfo}";
        }
    }

    // 4. Класс JournalEntry для записи информации об изменениях коллекции.
    public class JournalEntry
    {
        public string CollectionName { get; }
        public string ChangeType { get; }
        public string ObjectInfo { get; }

        public JournalEntry(string collectionName, string changeType, string objectInfo)
        {
            CollectionName = collectionName;
            ChangeType = changeType;
            ObjectInfo = objectInfo;
        }

        public override string ToString()
        {
            return $"Collection: {CollectionName}, Change: {ChangeType}, Object: {ObjectInfo}";
        }
    }

    // 5. Класс Journal для хранения событий – журнал изменений.
    public class Journal
    {
        private readonly List<JournalEntry> entries = new List<JournalEntry>();

        // Обработчик события для изменений количества элементов
        public void OnCollectionCountChanged(object? source, CollectionHandlerEventArgs args)
        {
            entries.Add(new JournalEntry(args.CollectionName, args.ChangeType.ToString(), args.ObjectInfo));
        }

        // Обработчик события для замены элементов
        public void OnCollectionReferenceChanged(object? source, CollectionHandlerEventArgs args)
        {
            entries.Add(new JournalEntry(args.CollectionName, args.ChangeType.ToString(), args.ObjectInfo));
        }

        public void PrintJournal()
        {
            foreach (var entry in entries)
            {
                Console.WriteLine(entry);
            }
        }
    }

    // 6. Демонстрационная программа
    public class Program
    {
        public static void Main(string[] args)
        {
            // Создаем две коллекции MyNewCollection с элементами типа Production
            // При желании можно использовать объекты Factory, Gild или Workshop.
            var collection1 = new MyNewCollection<Production>("Collection 1");
            var collection2 = new MyNewCollection<Production>("Collection 2");

            // Создаем журналы
            var journal1 = new Journal();
            // Подписываем journal1 на события первой коллекции
            collection1.CollectionCountChanged += journal1.OnCollectionCountChanged;
            collection1.CollectionReferenceChanged += journal1.OnCollectionReferenceChanged;

            var journal2 = new Journal();
            // Подписываем journal2 на событие замены элементов второй коллекции
            collection2.CollectionReferenceChanged += journal2.OnCollectionReferenceChanged;

            // Добавление элементов в коллекции
            Console.WriteLine("\nДобавляем элементы в коллекции...");
            collection1.AddDefaults();
            // Добавляем конкретные объекты – можно использовать производные классы
            Production prod1 = new Factory("Factory1", 50, 5);
            Production prod2 = new Gild("Gild1", 30, "John");
            collection2.Add(prod1);
            collection2.Add(prod2);

            // Удаление элемента из первой коллекции
            Console.WriteLine("\nУдаляем элемент из Collection 1...");
            collection1.Remove(0);

            // Присвоение новых значений (замена элемента) во второй коллекции
            Console.WriteLine("\nПрисваиваем новые значения в Collection 2...");
            Production prod3 = new Workshop("Workshop1", 40, new List<string> { "Tool1", "Tool2" });
            collection2[0] = prod3;

            // Вывод журналов
            Console.WriteLine("\nJournal 1:");
            journal1.PrintJournal();

            Console.WriteLine("\nJournal 2:");
            journal2.PrintJournal();

            // Для консольного приложения – ожидание ввода пользователя.
            Console.ReadLine();
        }
    }
}
