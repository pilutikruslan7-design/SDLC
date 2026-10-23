using MorseDecoder.Controller;
using MorseDecoder.Model;
using MorseDecoder.View;

namespace MorseDecoder;

internal static class Program
{
    [STAThread]
    private static void Main()
    {
        ApplicationConfiguration.Initialize();

        // Сборка триады MVC:
        // модель ничего не знает о представлении и контроллере (только об интерфейсе наблюдателя),
        // представление подписывается на модель, контроллер связывает действия пользователя с моделью.
        var model = new MorseModel();
        var view = new MainForm(model);
        _ = new MorseController(model, view);

        Application.Run(view);
    }
}
