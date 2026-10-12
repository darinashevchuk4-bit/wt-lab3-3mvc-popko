namespace wt_lab3_3mvc_popko.Models;

/// <summary>
/// Model — музыкальный трек (вариант 13 «Музыкальный сервис»).
/// Содержит только данные предметной области, без вывода в консоль.
/// </summary>
public class Track
{
    public int Id { get; set; }

    public string Title { get; set; } = "";

    public string Artist { get; set; } = "";

    public string Album { get; set; } = "";

    public string Genre { get; set; } = "";

    public int Year { get; set; }

    // Длительность в секундах
    public int DurationSeconds { get; set; }

    // Количество прослушиваний
    public long PlayCount { get; set; }

    // Доступен ли трек для прослушивания в сервисе
    public bool IsAvailable { get; set; }

    // Вычисляемое свойство модели (данные, не форматирование)
    public TimeSpan Duration => TimeSpan.FromSeconds(DurationSeconds);
}
