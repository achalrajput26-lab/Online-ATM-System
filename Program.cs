using System;
using System.Collections.Generic;
    public class Program
{
    public static List<Account> accounts = new List<Account>();
    public static void Main(string[] args)
    {
        SaveLoad.LoadData();
        
        bool end = false;
        while (!end)
        {
            Console.Clear();
            Console.WriteLine("=========== ATM-System ===========");
            Console.WriteLine();
            Console.WriteLine("1 - Log in");
            Console.WriteLine("2 - Create an account");
            Console.WriteLine("3 - Exit");
            Console.WriteLine();
            Console.WriteLine("==================================");

            int choice;
            Console.Write("Your choice :");
            while(!int.TryParse(Console.ReadLine(), out choice))
            {
                Console.WriteLine("Please enter a number !!");
                Console.Write("Your choice : ");
            }

            switch (choice)
            {
                case 1:
                    bool isSuccess = ATM.Login();
                    
                    if(isSuccess)
                    {
                        ATM.ShowMenu();
                    }
                break;
                
                case 2:
                    ATM.CreateAnAccount();
                break;

                case 3:
                    end = true;
                break;
                
                default:
                    Console.WriteLine("Please enter a number between 1 and 3 !!");
                break;
            }
        }
    }
}