using System;

class SetCalculator
{
    // Словарь для хранения множеств по их именам
    static Dictionary<string, HashSet<int>> sets = new Dictionary<string, HashSet<int>>();

    static void Main(string[] args)
    {
        bool exit = false;

        while (!exit)
        {
            Console.WriteLine("\nМеню:");
            Console.WriteLine("1. Создать множество");
            Console.WriteLine("2. Удалить множество");
            Console.WriteLine("3. Составить выражение");
            Console.WriteLine("4. Вывести множество");
            Console.WriteLine("5. Выход");
            Console.Write("Выберите команду: ");

            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    CreateSet();
                    break;
                case "2":
                    DeleteSet();
                    break;
                case "3":
                    EvaluateExpression();
                    break;
                case "4":
                    PrintSet();
                    break;
                case "5":
                    exit = true;
                    break;
                default:
                    Console.WriteLine("Неверная команда.");
                    break;
            }
        }
    }

    // Создание множества
    static void CreateSet()
    {
        Console.Write("Введите имя множества: ");
        string setName = Console.ReadLine();

        if (sets.ContainsKey(setName))
        {
            Console.WriteLine($"Множество с именем '{setName}' уже существует.");
            return;
        }

        Console.Write("Введите элементы множества через запятую: ");
        string elementsInput = Console.ReadLine();
        try
        {
            HashSet<int> set = new HashSet<int>(elementsInput.Split(',').Select(int.Parse));
            sets[setName] = set;
            Console.WriteLine($"Множество '{setName}' создано.");
        }
        catch
        {
            Console.WriteLine("Ошибка ввода. Убедитесь, что вы ввели целые числа.");
        }
    }

    // Удаление множества
    static void DeleteSet()
    {
        Console.Write("Введите имя множества для удаления: ");
        string setName = Console.ReadLine();

        if (sets.Remove(setName))
        {
            Console.WriteLine($"Множество '{setName}' удалено.");
        }
        else
        {
            Console.WriteLine($"Множество с именем '{setName}' не найдено.");
        }
    }

    // Составление выражения
    static void EvaluateExpression()
    {
        Console.WriteLine("Введите выражение (пример: (A + B) & C ; где объедиение '+', пересечение '&', разность '-', сим. разность '^'): ");
        string expression = Console.ReadLine();

        try
        {
            // Заменяем символы операций для удобства
            expression = expression.Replace("∪", "+")  // Объединение
                                   .Replace("∩", "&")  // Пересечение
                                   .Replace("∆", "^")  // Симметрическая разность
                                   .Replace("/", "-"); // Разность

            HashSet<int> result = ParseExpression(expression);
            if (result != null)
            {
                Console.WriteLine($"Результат: {{ {string.Join(", ", result)} }}");
            }
            else
            {
                Console.WriteLine("Ошибка в выражении.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }
    }

    // Вывод множества
    static void PrintSet()
    {
        Console.Write("Введите имя множества для вывода: ");
        string setName = Console.ReadLine();

        if (sets.ContainsKey(setName))
        {
            HashSet<int> set = sets[setName];
            Console.WriteLine($"Множество '{setName}': {{ {string.Join(", ", set)} }}");
        }
        else
        {
            Console.WriteLine($"Множество с именем '{setName}' не найдено.");
        }
    }

    // Парсинг выражения и выполнение операций
    static HashSet<int> ParseExpression(string expression)
    {
        Stack<HashSet<int>> stack = new Stack<HashSet<int>>();
        Stack<char> operators = new Stack<char>();

        for (int i = 0; i < expression.Length; i++)
        {
            char c = expression[i];

            if (char.IsWhiteSpace(c)) continue;

            if (c == '(')
            {
                operators.Push(c);
            }
            else if (c == ')')
            {
                while (operators.Count > 0 && operators.Peek() != '(')
                {
                    ApplyOperator(stack, operators.Pop());
                }
                operators.Pop(); // Удаляем '(' из стека
            }
            else if (IsOperator(c))
            {
                while (operators.Count > 0 && Priority(operators.Peek()) >= Priority(c))
                {
                    ApplyOperator(stack, operators.Pop());
                }
                operators.Push(c);
            }
            else if (char.IsLetter(c))
            {
                string setName = c.ToString();
                if (sets.ContainsKey(setName))
                {
                    stack.Push(new HashSet<int>(sets[setName]));
                }
                else
                {
                    Console.WriteLine($"Множество '{setName}' не найдено.");
                    return null;
                }
            }
        }

        while (operators.Count > 0)
        {
            ApplyOperator(stack, operators.Pop());
        }

        return stack.Count > 0 ? stack.Pop() : null;
    }

    // Проверка на оператор
    static bool IsOperator(char c)
    {
        return c == '+' || c == '&' || c == '-' || c == '^';
    }

    // Приоритет операций
    static int Priority(char op)
    {
        if (op == '+') return 1;  
        if (op == '&') return 2;  
        if (op == '^') return 3;  
        if (op == '-') return 3;  
        return 0;
    }

    // Применение оператора к верхним элементам стека
    static void ApplyOperator(Stack<HashSet<int>> stack, char op)
    {
        var set2 = stack.Pop();
        var set1 = stack.Pop();
        HashSet<int> result;

        switch (op)
        {
            case '+':
                result = Union(set1, set2);
                break;
            case '&':
                result = Intersection(set1, set2);
                break;
            case '-':
                result = Difference(set1, set2);
                break;
            case '^':
                result = SymmetricDifference(set1, set2);
                break;
            default:
                throw new ArgumentException("Неизвестный оператор");
        }

        stack.Push(result);
    }

    // Реализация операций 
    static HashSet<int> Union(HashSet<int> set1, HashSet<int> set2)
    {
        HashSet<int> result = new HashSet<int>(set1);
        foreach (var item in set2)
        {
            result.Add(item);
        }
        return result;
    }

    static HashSet<int> Intersection(HashSet<int> set1, HashSet<int> set2)
    {
        HashSet<int> result = new HashSet<int>();
        foreach (var item in set1)
        {
            if (set2.Contains(item))
            {
                result.Add(item);
            }
        }
        return result;
    }

    static HashSet<int> Difference(HashSet<int> set1, HashSet<int> set2)
    {
        HashSet<int> result = new HashSet<int>(set1);
        foreach (var item in set2)
        {
            result.Remove(item);
        }
        return result;
    }

    static HashSet<int> SymmetricDifference(HashSet<int> set1, HashSet<int> set2)
    {
        HashSet<int> result = new HashSet<int>();
        foreach (var item in set1)
        {
            if (!set2.Contains(item))
            {
                result.Add(item);
            }
        }
        foreach (var item in set2)
        {
            if (!set1.Contains(item))
            {
                result.Add(item);
            }
        }
        return result;
    }
}

