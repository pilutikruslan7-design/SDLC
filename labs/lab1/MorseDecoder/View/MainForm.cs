using MorseDecoder.Model;

namespace MorseDecoder.View;

/// <summary>
/// Главное окно (представление). Наблюдает за активной моделью: получив уведомление
/// <see cref="ModelChanged"/>, само читает из модели данные и перерисовывает метки.
/// </summary>
public sealed class MainForm : Form, IMainView, IMorseModelObserver
{
    private const string NoDataText = "— данные ещё не вводились —";

    private readonly MorseModel _model;

    private readonly TextBox _signalsBox;
    private readonly Label _alphabetLabel;
    private readonly TextBox _resultBox;
    private readonly Label _statsLabel;

    public event EventHandler? EnterDataRequested;

    public MainForm(MorseModel model)
    {
        _model = model;

        Text = "Декодер азбуки Морзе";
        Font = new Font("Segoe UI", 10F);
        AutoScaleMode = AutoScaleMode.Font;
        StartPosition = FormStartPosition.CenterScreen;
        ClientSize = new Size(620, 470);
        MinimumSize = new Size(480, 420);

        var layout = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            Padding = new Padding(14),
            ColumnCount = 1,
            RowCount = 8
        };
        layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));       // заголовок
        layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));       // «Введённые сигналы»
        layout.RowStyles.Add(new RowStyle(SizeType.Percent, 40F));   // сигналы
        layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));       // алфавит
        layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));       // «Результат»
        layout.RowStyles.Add(new RowStyle(SizeType.Percent, 60F));   // результат
        layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));       // статистика
        layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));       // кнопка

        var titleLabel = new Label
        {
            Text = "Декодирование текста из азбуки Морзе",
            Font = new Font("Segoe UI", 14F, FontStyle.Bold),
            AutoSize = true,
            Margin = new Padding(3, 0, 3, 10)
        };

        _signalsBox = CreateReadOnlyBox(new Font("Consolas", 12F));
        _alphabetLabel = new Label { AutoSize = true, Margin = new Padding(3, 6, 3, 10) };
        _resultBox = CreateReadOnlyBox(new Font("Segoe UI", 14F, FontStyle.Bold));
        _statsLabel = new Label { AutoSize = true, Margin = new Padding(3, 6, 3, 6) };

        var enterButton = new Button
        {
            Text = "Ввести данные",
            AutoSize = true,
            Padding = new Padding(10, 4, 10, 4),
            Anchor = AnchorStyles.Right
        };
        enterButton.Click += (_, _) => EnterDataRequested?.Invoke(this, EventArgs.Empty);

        layout.Controls.Add(titleLabel, 0, 0);
        layout.Controls.Add(CreateCaption("Введённые сигналы:"), 0, 1);
        layout.Controls.Add(_signalsBox, 0, 2);
        layout.Controls.Add(_alphabetLabel, 0, 3);
        layout.Controls.Add(CreateCaption("Расшифрованный текст:"), 0, 4);
        layout.Controls.Add(_resultBox, 0, 5);
        layout.Controls.Add(_statsLabel, 0, 6);
        layout.Controls.Add(enterButton, 0, 7);
        Controls.Add(layout);

        AcceptButton = enterButton;

        _model.Attach(this);
        FormClosed += (_, _) => _model.Detach(this);
        ModelChanged();
    }

    public void ModelChanged()
    {
        if (!_model.HasResult)
        {
            _signalsBox.Text = NoDataText;
            _alphabetLabel.Text = "Алфавит: —";
            _resultBox.Text = NoDataText;
            _statsLabel.Text = "Нажмите «Ввести данные», чтобы ввести сигналы.";
            return;
        }

        _signalsBox.Text = _model.Signals.Replace("\r\n", "\n").Replace("\n", Environment.NewLine);
        _alphabetLabel.Text = $"Алфавит: {_model.Alphabet.ToDisplayName()}";
        _resultBox.Text = _model.DecodedText;
        _statsLabel.Text = $"Знаков: {_model.SymbolCount}    Слов: {_model.WordCount}";
    }

    public MorseInput? RequestInput(MorseInput initial)
    {
        using var dialog = new InputForm(initial);
        return dialog.ShowDialog(this) == DialogResult.OK ? dialog.Result : null;
    }

    public void ShowError(string message) =>
        MessageBox.Show(this, message, "Ошибка ввода", MessageBoxButtons.OK, MessageBoxIcon.Error);

    private static Label CreateCaption(string text) => new()
    {
        Text = text,
        AutoSize = true,
        Font = new Font("Segoe UI", 10F, FontStyle.Bold),
        Margin = new Padding(3, 3, 3, 3)
    };

    private static TextBox CreateReadOnlyBox(Font font) => new()
    {
        Multiline = true,
        ReadOnly = true,
        WordWrap = true,
        ScrollBars = ScrollBars.Vertical,
        BackColor = SystemColors.Window,
        Font = font,
        Dock = DockStyle.Fill,
        TabStop = false
    };
}
