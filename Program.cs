using System.Text;
using wt_lab3_3mvc_popko.Controllers;
using wt_lab3_3mvc_popko.Models;

// Лабораторная работа WT-3 «Реализация MVC-архитектуры в консольном приложении C#»
// Вариант 13 — Музыкальный сервис (Track). Выполнила: Попко Д.А., гр. 1д2401ГУП.

Console.OutputEncoding = Encoding.UTF8;
Console.InputEncoding = Encoding.UTF8;

var controller = new TrackController();

// Режим демонстрации (раздел 9 задания): dotnet run -- demo
if (args.Length > 0 && args[0] == "demo")
{
    controller.Index();
    Console.WriteLine();

    controller.Show(2);
    Console.WriteLine();

    controller.Add(
        new Track
        {
            Id = 8,
            Title = "Numb",
            Artist = "Linkin Park",
            Album = "Meteora",
            Genre = "Rock",
            Year = 2003,
            DurationSeconds = 185,
            PlayCount = 2_000_000_000,
            IsAvailable = true
        }
    );
    Console.WriteLine();

    controller.Index();
    Console.WriteLine();

    controller.Delete(2);
    Console.WriteLine();

    controller.Index();
    return;
}

// Основной режим — пользовательское меню (раздел 11 задания)
controller.Run();
