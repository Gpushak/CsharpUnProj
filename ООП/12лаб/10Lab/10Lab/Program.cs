using System;
using System.Collections.Generic;
using _10labLib; 

public class DoublyLinkedList<T>
{
    public class Node
    {
        public T Value;
        public Node Next;
        public Node Prev;

        public Node(T value)
        {
            Value = value;
        }
    }

    public Node Head { get; private set; }
    public Node Tail { get; private set; }

    public Node First { get { return Head; } }
    public Node Last { get { return Tail; } }

    public void AddLast(T value)
    {
        Node newNode = new Node(value);
        if (Head == null)
        {
            Head = newNode;
            Tail = newNode;
        }
        else
        {
            Tail.Next = newNode;
            newNode.Prev = Tail;
            Tail = newNode;
        }
    }

    public void InsertAfter(Node node, T value)
    {
        if (node == null) return;

        Node newNode = new Node(value)
        {
            Next = node.Next,
            Prev = node
        };
        if (node.Next != null)
        {
            node.Next.Prev = newNode;
        }
        else
        {
            Tail = newNode;
        }
        node.Next = newNode;
    }

    public void Clear()
    {
        Head = null;
        Tail = null;
    }
}

class Program
{
    static void Main(string[] args)
    {
        DoublyLinkedList<Production> list = new DoublyLinkedList<Production>();

        Production prod = new Production();
        prod.RandomInit();
        list.AddLast(prod);

        Factory factory = new Factory();
        factory.RandomInit();
        list.AddLast(factory);

        Gild gild = new Gild();
        gild.RandomInit();
        list.AddLast(gild);

        Workshop workshop = new Workshop();
        workshop.RandomInit();
        list.AddLast(workshop);

        Console.WriteLine("Исходный список:");
        PrintList(list);

        InsertOddPositionClones(list);

        Console.WriteLine("\nИзменённый список:");
        PrintList(list);

        list.Clear();
        list = null;
    }

    static void PrintList(DoublyLinkedList<Production> list)
    {
        int i = 1;
        for (var node = list.First; node != null; node = node.Next)
        {
            Console.WriteLine($"Элемент {i}:");
            node.Value.Show();
            Console.WriteLine();
            i++;
        }
    }

    static void InsertOddPositionClones(DoublyLinkedList<Production> list)
    {
        // Сначала собираем ссылки на узлы, находящиеся на нечётных позициях
        List<DoublyLinkedList<Production>.Node> nodesToClone = new List<DoublyLinkedList<Production>.Node>();
        int pos = 1;
        for (var node = list.First; node != null; node = node.Next)
        {
            if (pos % 2 == 1)
            {
                nodesToClone.Add(node);
            }
            pos++;
        }

        // Для каждого найденного узла создаём клон и вставляем его после оригинала.
        foreach (var node in nodesToClone)
        {
            Production cloned = (Production)node.Value.Clone();
            list.InsertAfter(node, cloned);
        }
    }
}
