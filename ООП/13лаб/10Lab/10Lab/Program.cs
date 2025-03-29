using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using _10labLib; 

namespace LabAssignment
{
    public enum ChangeTypeEnum
    {
        Added,
        Removed,
        Replaced
    }

    public class MyCollection<T> : Collection<T> where T : Production
    {
        public string CollectionName { get; set; }

        public MyCollection(string name)
        {
            CollectionName = name;
        }

        public void AddDefaults()
        {
            Add((T)Activator.CreateInstance(typeof(T), "Production_A", 10));
            Add((T)Activator.CreateInstance(typeof(T), "Production_B", 20));
        }
    }

    public class MyNewCollection<T> : MyCollection<T> where T : Production
    {
        // События
        public event EventHandler<CollectionHandlerEventArgs>? CollectionCountChanged;
        public event EventHandler<CollectionHandlerEventArgs>? CollectionReferenceChanged;

        public MyNewCollection(string name) : base(name)
        {
        }

        public new void Add(T item)
        {
            base.Add(item);
            OnCollectionCountChanged(ChangeTypeEnum.Added, item);
        }

        public bool Remove(int index)
        {
            if (index < 0 || index >= Count)
                return false;

            T item = this[index];
            base.RemoveAt(index);
            OnCollectionCountChanged(ChangeTypeEnum.Removed, item);
            return true;
        }

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

        protected virtual void OnCollectionCountChanged(ChangeTypeEnum changeType, T item)
        {
            CollectionCountChanged?.Invoke(this, new CollectionHandlerEventArgs(CollectionName, changeType, item.ToString()));
        }

        protected virtual void OnCollectionReferenceChanged(ChangeTypeEnum changeType, T oldItem, T newItem)
        {
            CollectionReferenceChanged?.Invoke(this, new CollectionHandlerEventArgs(CollectionName, changeType, $"Old: {oldItem}, New: {newItem}"));
        }
    }

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

    public class Journal
    {
        private readonly List<JournalEntry> entries = new List<JournalEntry>();

        public void OnCollectionCountChanged(object? source, CollectionHandlerEventArgs args)
        {
            entries.Add(new JournalEntry(args.CollectionName, args.ChangeType.ToString(), args.ObjectInfo));
        }

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

    public class Program
    {
        public static void Main(string[] args)
        {

            var collection1 = new MyNewCollection<Production>("Collection 1");
            var collection2 = new MyNewCollection<Production>("Collection 2");

            var journal1 = new Journal();

            collection1.CollectionCountChanged += journal1.OnCollectionCountChanged;
            collection1.CollectionReferenceChanged += journal1.OnCollectionReferenceChanged;

            var journal2 = new Journal();

            collection2.CollectionReferenceChanged += journal2.OnCollectionReferenceChanged;

            Console.WriteLine("\nДобавляем элементы в коллекции...");
            collection1.AddDefaults();

            Production prod1 = new Factory("Factory1", 50, 5);
            Production prod2 = new Gild("Gild1", 30, "John");
            collection2.Add(prod1);
            collection2.Add(prod2);

            Console.WriteLine("\nУдаляем элемент из Collection 1...");
            collection1.Remove(0);

            Console.WriteLine("\nПрисваиваем новые значения в Collection 2...");
            Production prod3 = new Workshop("Workshop1", 40, new List<string> { "Tool1", "Tool2" });
            collection2[0] = prod3;

            Console.WriteLine("\nJournal 1:");
            journal1.PrintJournal();

            Console.WriteLine("\nJournal 2:");
            journal2.PrintJournal();

            Console.ReadLine();
        }
    }
}
