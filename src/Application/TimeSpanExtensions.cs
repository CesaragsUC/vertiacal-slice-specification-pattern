namespace Application;

public static class TimeSpanExtensions
{
    /// <summary>
    /// Converte para segundos (padrão Prometheus)
    /// </summary>
    public static double ToSeconds(this TimeSpan timeSpan)
    {
        return timeSpan.TotalSeconds;
    }

    /// <summary>
    /// Formata de forma legível para logs
    /// </summary>
    public static string ToHumanReadable(this TimeSpan timeSpan)
    {
        if (timeSpan.TotalMilliseconds < 1)
            return $"{timeSpan.TotalMicroseconds:F2} μs";

        if (timeSpan.TotalSeconds < 1)
            return $"{timeSpan.TotalMilliseconds:F2} ms";

        if (timeSpan.TotalMinutes < 1)
            return $"{timeSpan.TotalSeconds:F2} s";

        if (timeSpan.TotalHours < 1)
            return $"{timeSpan.TotalMinutes:F2} min";

        return $"{timeSpan.TotalHours:F2} h";
    }

    /// <summary>
    /// Retorna unidade e valor apropriados
    /// </summary>
    public static (double value, string unit) ToAppropriateUnit(this TimeSpan timeSpan)
    {
        if (timeSpan.TotalMilliseconds < 1)
            return (timeSpan.TotalMicroseconds, "μs");

        if (timeSpan.TotalSeconds < 1)
            return (timeSpan.TotalMilliseconds, "ms");

        if (timeSpan.TotalMinutes < 1)
            return (timeSpan.TotalSeconds, "s");

        if (timeSpan.TotalHours < 1)
            return (timeSpan.TotalMinutes, "min");

        return (timeSpan.TotalHours, "h");
    }
}
