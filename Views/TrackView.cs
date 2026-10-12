using wt_lab3_3mvc_popko.Models;

namespace wt_lab3_3mvc_popko.Views;

/// <summary>
/// View — отображение данных о треках в консоли.
/// View только выводит и форматирует переданные данные:
/// она не ищет треки и не изменяет коллекцию.
/// </summary>
public class TrackView
{
    // ---------- Служебные сообщения ----------

    public void ShowWelcome()
    {
        Console.WriteLine("==================================================");
        Console.WriteLine("        ♫  МУЗЫКАЛЬНЫЙ СЕРВИС «MUSICBOX»  ♫");
        Console.WriteLine("          MVC-приложение, вариант 13");
        Console.WriteLine("==================================================");
    }

    public void ShowMenu()
    {
        Console.WriteLine();
        Console.WriteLine("=== МЕНЮ ===");
        Console.WriteLine("1. Показать все треки");
        Console.WriteLine("2. Найти трек по ID");
        Console.WriteLine("3. Добавить трек");
        Console.WriteLine("4. Удалить трек");
        Console.WriteLine("5. Поиск по названию или исполнителю");
        Console.WriteLine("6. Показать треки по жанру");
        Console.WriteLine("7. Топ по популярности (сортировка)");
        Console.WriteLine("8. Статистика медиатеки");
        Console.WriteLine("0. Выход");
    }

    public void ShowPrompt(string text)
    {
        Console.Write(text);
    }

    public void ShowMessage(string message)
    {
        Console.WriteLine(message);
    }

    public void ShowSuccess(string message)
    {
        Console.WriteLine($"[OK] {message}");
    }

    public void ShowError(string message)
    {
        Console.WriteLine($"[Ошибка] {message}");
    }

    // ---------- Вариант 1: список треков в виде таблицы ----------

    public void ShowTracks(List<Track> tracks, string title = "СПИСОК ТРЕКОВ")
    {
        Console.WriteLine($"=== {title} ===");

        if (tracks.Count == 0)
        {
            Console.WriteLine("Треки не найдены.");
            return;
        }

        Console.WriteLine($"{"ID",-4}{"Исполнитель",-18}{"Название",-21}{"Жанр",-13}{"Время",6}  Доступен");
        Console.WriteLine(new string('-', 72));

        foreach (var track in tracks)
        {
            Console.WriteLine(
                $"{track.Id,-4}" +
                $"{Cut(track.Artist, 17),-18}" +
                $"{Cut(track.Title, 20),-21}" +
                $"{Cut(track.Genre, 12),-13}" +
                $"{FormatDuration(track.Duration),6}  " +
                $"{(track.IsAvailable ? "да" : "нет")}");
        }

        Console.WriteLine(new string('-', 72));
        Console.WriteLine($"Всего: {tracks.Count}");
    }

    // ---------- Вариант 2: подробная карточка одного трека ----------

    public void ShowTrack(Track track)
    {
        Console.WriteLine("=== ТРЕК ===");
        Console.WriteLine($"ID:             {track.Id}");
        Console.WriteLine($"Название:       {track.Title}");
        Console.WriteLine($"Исполнитель:    {track.Artist}");
        Console.WriteLine($"Альбом:         {track.Album}");
        Console.WriteLine($"Жанр:           {track.Genre}");
        Console.WriteLine($"Год выпуска:    {track.Year}");
        Console.WriteLine($"Длительность:   {FormatDuration(track.Duration)}");
        Console.WriteLine($"Прослушивания:  {track.PlayCount:N0}");
        Console.WriteLine($"Доступен:       {(track.IsAvailable ? "да" : "нет")}");
    }

    // ---------- Вариант 3: рейтинг (топ) треков ----------

    public void ShowTopTracks(List<Track> tracks)
    {
        Console.WriteLine("=== ТОП ТРЕКОВ ПО ПРОСЛУШИВАНИЯМ ===");
        int place = 1;
        long max = tracks.Count > 0 ? tracks[0].PlayCount : 1;

        foreach (var track in tracks)
        {
            int bar = max > 0 ? (int)(track.PlayCount * 20 / max) : 0;
            Console.WriteLine(
                $"{place++,2}. {Cut(track.Artist + " — " + track.Title, 36),-36} " +
                $"{new string('█', bar),-20} {track.PlayCount:N0}");
        }
    }

    // ---------- Вариант 4: статистика ----------

    public void ShowStatistics(int count, int availableCount, TimeSpan totalDuration,
                               long totalPlays, Track? longest, Dictionary<string, int> byGenre)
    {
        Console.WriteLine("=== СТАТИСТИКА МЕДИАТЕКИ ===");
        Console.WriteLine($"{"Всего треков:",-29}{count}");
        Console.WriteLine($"{"Доступно для прослушивания:",-29}{availableCount}");
        Console.WriteLine($"{"Общая длительность:",-29}{FormatDuration(totalDuration)}");
        Console.WriteLine($"{"Всего прослушиваний:",-29}{totalPlays:N0}");
        if (longest != null)
            Console.WriteLine($"{"Самый длинный трек:",-29}{longest.Title} ({FormatDuration(longest.Duration)})");

        Console.WriteLine();
        Console.WriteLine("Количество треков по жанрам:");
        foreach (var pair in byGenre)
            Console.WriteLine($"   {pair.Key,-14} {pair.Value}");
    }

    public void ShowGenres(List<string> genres)
    {
        Console.WriteLine($"Доступные жанры: {string.Join(", ", genres)}");
    }

    // ---------- Вспомогательные методы форматирования ----------

    private static string FormatDuration(TimeSpan duration)
    {
        return duration.TotalHours >= 1
            ? duration.ToString(@"h\:mm\:ss")
            : duration.ToString(@"m\:ss");
    }

    private static string Cut(string text, int max)
    {
        return text.Length <= max ? text : text[..(max - 1)] + "…";
    }
}
