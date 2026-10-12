using wt_lab3_3mvc_popko.Models;
using wt_lab3_3mvc_popko.Views;

namespace wt_lab3_3mvc_popko.Controllers;

/// <summary>
/// Controller — принимает команды пользователя, работает с коллекцией
/// объектов Model (Track) и передаёт результат во View (TrackView).
/// </summary>
public class TrackController
{
    private readonly List<Track> tracks;
    private readonly TrackView view;

    public TrackController()
    {
        // Начальные объекты модели
        tracks = new List<Track>
        {
            new Track { Id = 1, Title = "Bohemian Rhapsody", Artist = "Queen", Album = "A Night at the Opera",
                        Genre = "Rock", Year = 1975, DurationSeconds = 355, PlayCount = 2_500_000_000, IsAvailable = true },
            new Track { Id = 2, Title = "Blinding Lights", Artist = "The Weeknd", Album = "After Hours",
                        Genre = "Synth-pop", Year = 2019, DurationSeconds = 200, PlayCount = 4_700_000_000, IsAvailable = true },
            new Track { Id = 3, Title = "Believer", Artist = "Imagine Dragons", Album = "Evolve",
                        Genre = "Pop-rock", Year = 2017, DurationSeconds = 204, PlayCount = 2_900_000_000, IsAvailable = true },
            new Track { Id = 4, Title = "Bad Guy", Artist = "Billie Eilish", Album = "When We All Fall Asleep",
                        Genre = "Pop", Year = 2019, DurationSeconds = 194, PlayCount = 2_800_000_000, IsAvailable = false },
            new Track { Id = 5, Title = "Shape of You", Artist = "Ed Sheeran", Album = "÷ (Divide)",
                        Genre = "Pop", Year = 2017, DurationSeconds = 233, PlayCount = 4_000_000_000, IsAvailable = true },
            new Track { Id = 6, Title = "Viva La Vida", Artist = "Coldplay", Album = "Viva la Vida",
                        Genre = "Alternative", Year = 2008, DurationSeconds = 242, PlayCount = 2_000_000_000, IsAvailable = true },
            new Track { Id = 7, Title = "Get Lucky", Artist = "Daft Punk", Album = "Random Access Memories",
                        Genre = "Electronic", Year = 2013, DurationSeconds = 369, PlayCount = 1_500_000_000, IsAvailable = false },
        };

        view = new TrackView();
    }

    // ===================== Основные действия (CRUD) =====================

    // Index() → получить список объектов
    public void Index()
    {
        view.ShowTracks(tracks);
    }

    // Show() → получить один объект
    public void Show(int id)
    {
        var track = tracks.FirstOrDefault(t => t.Id == id);

        if (track == null)
        {
            view.ShowError($"Трек с ID = {id} не найден.");
            return;
        }

        view.ShowTrack(track);
    }

    // Add() → добавить объект (с проверкой данных)
    public void Add(Track track)
    {
        if (tracks.Any(t => t.Id == track.Id))
        {
            view.ShowError($"Трек с ID = {track.Id} уже существует.");
            return;
        }

        if (string.IsNullOrWhiteSpace(track.Title) || string.IsNullOrWhiteSpace(track.Artist))
        {
            view.ShowError("Название и исполнитель обязательны.");
            return;
        }

        tracks.Add(track);
        view.ShowSuccess($"Трек «{track.Artist} — {track.Title}» успешно добавлен (ID = {track.Id}).");
    }

    // Delete() → удалить объект
    public void Delete(int id)
    {
        var track = tracks.FirstOrDefault(t => t.Id == id);

        if (track == null)
        {
            view.ShowError($"Трек с ID = {id} не найден.");
            return;
        }

        tracks.Remove(track);
        view.ShowSuccess($"Трек «{track.Artist} — {track.Title}» успешно удалён.");
    }

    // ===================== Дополнительные операции =====================

    // Поиск по названию или исполнителю
    public void Search(string text)
    {
        var result = tracks
            .Where(t =>
                t.Title.Contains(text, StringComparison.OrdinalIgnoreCase) ||
                t.Artist.Contains(text, StringComparison.OrdinalIgnoreCase))
            .ToList();

        view.ShowTracks(result, $"РЕЗУЛЬТАТЫ ПОИСКА «{text}»");
    }

    // Фильтрация по жанру
    public void FilterByGenre(string genre)
    {
        var result = tracks
            .Where(t => t.Genre.Equals(genre, StringComparison.OrdinalIgnoreCase))
            .ToList();

        view.ShowTracks(result, $"ТРЕКИ ЖАНРА «{genre}»");
    }

    // Сортировка по популярности
    public void SortByPopularity()
    {
        var result = tracks
            .OrderByDescending(t => t.PlayCount)
            .ThenBy(t => t.Title)
            .ToList();

        view.ShowTopTracks(result);
    }

    // Подсчёт статистики
    public void Statistics()
    {
        var byGenre = tracks
            .GroupBy(t => t.Genre)
            .OrderByDescending(g => g.Count())
            .ThenBy(g => g.Key)
            .ToDictionary(g => g.Key, g => g.Count());

        view.ShowStatistics(
            count: tracks.Count,
            availableCount: tracks.Count(t => t.IsAvailable),
            totalDuration: TimeSpan.FromSeconds(tracks.Sum(t => t.DurationSeconds)),
            totalPlays: tracks.Sum(t => t.PlayCount),
            longest: tracks.MaxBy(t => t.DurationSeconds),
            byGenre: byGenre);
    }

    // ===================== Меню =====================

    public void Run()
    {
        view.ShowWelcome();

        while (true)
        {
            view.ShowMenu();
            view.ShowPrompt("Выберите действие: ");

            var command = Console.ReadLine();
            if (command == null) return; // конец ввода

            Console.WriteLine();

            switch (command.Trim())
            {
                case "1":
                    Index();
                    break;

                case "2":
                    ShowTrackByUserInput();
                    break;

                case "3":
                    AddTrackByUserInput();
                    break;

                case "4":
                    DeleteTrackByUserInput();
                    break;

                case "5":
                    SearchByUserInput();
                    break;

                case "6":
                    FilterByGenreUserInput();
                    break;

                case "7":
                    SortByPopularity();
                    break;

                case "8":
                    Statistics();
                    break;

                case "0":
                    view.ShowMessage("Работа приложения завершена. До свидания!");
                    return;

                default:
                    view.ShowError("Неизвестная команда. Введите число от 0 до 8.");
                    break;
            }
        }
    }

    // ===================== Обработка пользовательского ввода =====================

    private void ShowTrackByUserInput()
    {
        if (!TryReadId("Введите ID трека: ", out int id))
            return;

        Show(id);
    }

    private void AddTrackByUserInput()
    {
        view.ShowMessage("=== ДОБАВЛЕНИЕ ТРЕКА ===");

        string title = ReadRequired("Название: ");
        if (title == "") return;

        string artist = ReadRequired("Исполнитель: ");
        if (artist == "") return;

        view.ShowPrompt("Альбом (можно оставить пустым): ");
        string album = Console.ReadLine()?.Trim() ?? "";
        if (album == "") album = "Сингл";

        string genre = ReadRequired("Жанр: ");
        if (genre == "") return;

        view.ShowPrompt("Год выпуска: ");
        if (!int.TryParse(Console.ReadLine(), out int year) || year < 1900 || year > DateTime.Now.Year)
        {
            view.ShowError($"Некорректный год. Допустимо от 1900 до {DateTime.Now.Year}.");
            return;
        }

        view.ShowPrompt("Длительность (мм:сс): ");
        if (!TryParseDuration(Console.ReadLine(), out int seconds))
        {
            view.ShowError("Некорректная длительность. Пример: 3:45");
            return;
        }

        view.ShowPrompt("Доступен для прослушивания? (д/н): ");
        string available = (Console.ReadLine() ?? "").Trim().ToLower();
        if (available != "д" && available != "н")
        {
            view.ShowError("Ответ должен быть «д» или «н».");
            return;
        }

        int newId = tracks.Count == 0 ? 1 : tracks.Max(t => t.Id) + 1;

        Add(new Track
        {
            Id = newId,
            Title = title,
            Artist = artist,
            Album = album,
            Genre = genre,
            Year = year,
            DurationSeconds = seconds,
            PlayCount = 0,
            IsAvailable = available == "д"
        });
    }

    private void DeleteTrackByUserInput()
    {
        if (!TryReadId("Введите ID трека для удаления: ", out int id))
            return;

        Delete(id);
    }

    private void SearchByUserInput()
    {
        string text = ReadRequired("Введите часть названия или имени исполнителя: ");
        if (text == "") return;

        Search(text);
    }

    private void FilterByGenreUserInput()
    {
        view.ShowGenres(tracks.Select(t => t.Genre).Distinct().OrderBy(g => g).ToList());
        string genre = ReadRequired("Введите жанр: ");
        if (genre == "") return;

        FilterByGenre(genre);
    }

    // ---------- Вспомогательные методы ----------

    private bool TryReadId(string prompt, out int id)
    {
        view.ShowPrompt(prompt);

        if (!int.TryParse(Console.ReadLine(), out id) || id <= 0)
        {
            view.ShowError("Некорректный ID. Введите целое положительное число.");
            return false;
        }

        return true;
    }

    private string ReadRequired(string prompt)
    {
        view.ShowPrompt(prompt);
        string value = Console.ReadLine()?.Trim() ?? "";

        if (value == "")
            view.ShowError("Поле не может быть пустым.");

        return value;
    }

    private static bool TryParseDuration(string? input, out int seconds)
    {
        seconds = 0;
        if (string.IsNullOrWhiteSpace(input)) return false;

        string[] parts = input.Trim().Split(':');
        if (parts.Length != 2) return false;

        if (!int.TryParse(parts[0], out int min) || !int.TryParse(parts[1], out int sec))
            return false;
        if (min < 0 || sec < 0 || sec > 59) return false;

        seconds = min * 60 + sec;
        return seconds > 0;
    }
}
