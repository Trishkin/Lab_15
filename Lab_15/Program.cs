using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading;

namespace Lab_15
{
    class Program
    {
        private static readonly Barrier _barrier = new Barrier(2);
        static void Main(string[] args)
        {
            foreach (Process i in Process.GetProcesses())
            {
                Console.WriteLine($"Имя процесса : {i.ProcessName} | ID процесса : {i.Id}");
                Console.WriteLine();
            }


            AppDomain domain = AppDomain.CurrentDomain;
            Console.WriteLine($"Имя домена : {domain.FriendlyName}");
            Console.WriteLine($"Базовый каталог : {domain.BaseDirectory}");
            Console.WriteLine($"Детали конфигурации : {domain.SetupInformation}");
            Assembly[] assemblies = domain.GetAssemblies();
            foreach (Assembly i in assemblies)
            {
                Console.WriteLine(i.GetName());
            }




            AppDomain newD = AppDomain.CreateDomain("MyDomen");
            newD.ExecuteAssembly(@"D:\OOP\Lab_14\Lab_14\bin\Debug\netcoreapp3.1\Lab_14.exe");
            AppDomain.Unload(newD);





            Console.WriteLine("___________________");
            Console.WriteLine("Введите число n :");
            int n = Int32.Parse(Console.ReadLine());
            Thread thread1 = new Thread(() => Number(n));
            thread1.Start();
            Thread t = Thread.CurrentThread;
            t.Name = "Поток №1";
            Console.WriteLine($"Имя потока : {t.Name}");
            Console.WriteLine($"Приотирет потока : {t.Priority}");
            Console.WriteLine($"ID потока : {Thread.CurrentThread.ManagedThreadId}");
            Thread.Sleep(2000);
            Console.WriteLine($"Текущее состояние потока : {Thread.CurrentThread.ThreadState}");
            Console.WriteLine($"Текущее состояние потока 1 : {thread1.ThreadState}");
            
            
            Thread thread2 = new Thread(() => EvenNumber(n));
            thread2.Start();


            Thread thread3 = new Thread(() => UnevenNumber(n));
            thread3.Start();
            TimerCallback tm = new TimerCallback(EndThis);
            Timer timer = new Timer(tm, 0, 0, 2000);
            Console.ReadKey();
        }
        public static void EndThis(object o)
        {
            Console.WriteLine("EndThis");
        }
        public static void Number(int n)
        {
            Thread.Sleep(1000);
            for (int i = 1; i <= n; i++)
            {
                Console.WriteLine(i);
            }
        }
        public static void EvenNumber(int n)
        {
            for (int i = 0; i <= n; i++)
            {
                if (i % 2 == 0)
                {
                    Console.WriteLine(i);
                }
                Thread.Sleep(200);
                _barrier.SignalAndWait();
            }
        }
        public static void UnevenNumber(int n)
        {
            for (int i = 0; i <= n; i++)
            {
                if (i % 2 == 1)
                {
                    Console.WriteLine(i);
                }
                Thread.Sleep(100);
                _barrier.SignalAndWait();
            }
        }
    }
}
