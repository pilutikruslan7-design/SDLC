using MorseDecoder.Model;

namespace MorseDecoder.View;

/// <summary>Интерфейс главного представления, с которым работает контроллер.</summary>
public interface IMainView
{
    /// <summary>Пользователь нажал «Ввести данные».</summary>
    event EventHandler? EnterDataRequested;

    /// <summary>Показывает окно ввода, заполненное <paramref name="initial"/>; null — ввод отменён.</summary>
    MorseInput? RequestInput(MorseInput initial);

    void ShowError(string message);
}
