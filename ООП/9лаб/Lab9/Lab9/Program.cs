using System;

class Program
{
    static void Main()
    {
        bool exit = false;
        while (!exit)
        {
            UserInterface.ShowMainMenu();
            int choice = UserInterface.GetUserChoice();
            switch (choice)
            {
                case 1:
                    UserInterface.DemonstratePointClass();
                    break;
                case 2:
                    UserInterface.DemonstrateOverloadedOperators();
                    break;
                case 3:
                    UserInterface.DemonstratePointArray();
                    break;
                case 4:
                    Console.WriteLine("Выход из программы.");
                    exit = true;
                    break;
                default:
                    Console.WriteLine("Неверный выбор. Попробуйте снова.");
                    break;
            }
        }
    }

    public static Point FindNearestToOrigin(PointArray pointArray)
    {
        if (pointArray.Length == 0)
        {
            Console.WriteLine("Массив пустой или не инициализирован.");
            return null;
        }

        Point nearestPoint = pointArray[0];
        double minDistance = nearestPoint.DistanceToOrigin();

        for (int i = 1; i < pointArray.Length; i++)
        {
            double distance = pointArray[i].DistanceToOrigin();
            if (distance < minDistance)
            {
                minDistance = distance;
                nearestPoint = pointArray[i];
            }
        }
        return nearestPoint;
    }
}
