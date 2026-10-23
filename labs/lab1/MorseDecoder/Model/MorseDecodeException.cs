namespace MorseDecoder.Model;

/// <summary>Ошибка разбора введённых сигналов (некорректные данные).</summary>
public sealed class MorseDecodeException : Exception
{
    public MorseDecodeException(string message) : base(message)
    {
    }
}
