using MorseDecoder.Model;
using MorseDecoder.View;

namespace MorseDecoder.Controller;

/// <summary>
/// Контроллер: принимает действия пользователя от представления и вызывает методы модели.
/// Представление контроллер не обновляет — это делает активная модель через уведомления.
/// </summary>
public sealed class MorseController
{
    private readonly MorseModel _model;
    private readonly IMainView _view;

    public MorseController(MorseModel model, IMainView view)
    {
        _model = model;
        _view = view;
        _view.EnterDataRequested += OnEnterDataRequested;
    }

    private void OnEnterDataRequested(object? sender, EventArgs e)
    {
        // В окне ввода восстанавливаются последние введённые данные.
        MorseInput input = _model.LastInput;

        while (true)
        {
            MorseInput? entered = _view.RequestInput(input);
            if (entered is null)
                return;

            try
            {
                _model.Decode(entered);
                return;
            }
            catch (MorseDecodeException ex)
            {
                _view.ShowError(ex.Message);
                // Повторно открываем окно с ошибочным вводом, чтобы его можно было исправить.
                input = entered;
            }
        }
    }
}
