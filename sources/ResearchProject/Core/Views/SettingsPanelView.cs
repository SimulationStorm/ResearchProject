using System.Linq;
using EasyBindings;
using Godot;

public partial class SettingsPanelView : PanelView, IView<SettingsPanelVM>
{
    private SettingsPanelVM _viewModel = null!;

    public void Setup(SettingsPanelVM viewModel)
    {
        _viewModel = viewModel;
        base.Setup(viewModel, true);

        SetupControls();
    }

    #region Controls
    private void SetupControls()
    {
        SetupLanguageOptionButton();
        SetupFieldBackgroundColorPicker();
        SetupVSyncCheckButton();
    }

    #region Language option button
    [Export] public NodePath LanguageOptionButtonPath { get; set; } = null!;
    private RichOptionButton _languageOptionButton = null!;

    public void SetupLanguageOptionButton()
    {
        _languageOptionButton = GetNode<RichOptionButton>(LanguageOptionButtonPath);

        _languageOptionButton.Items = AppSettings.LocalesByName.Values.Select(name => new OptionItem(name));
        _languageOptionButton.SelectedIndex = AppSettings.LocalesByName.Keys.ToList().IndexOf(AppSettings.InitialLocale);

        _languageOptionButton.ItemSelected += item =>
            _viewModel.ApplicationLocale = AppSettings.LocalesByName.First(kv => kv.Value == item.Text).Key;
    }
    #endregion

    #region Field background color picker
    [Export] public NodePath FieldBackgroundColorPickerPath { get; set; } = null!;
    private RichColorPickerButton _fieldBackgroundColorPicker = null!;

    private void SetupFieldBackgroundColorPicker()
    {
        _fieldBackgroundColorPicker = GetNode<RichColorPickerButton>(FieldBackgroundColorPickerPath);
        _fieldBackgroundColorPicker.SelectedColor = _viewModel.FieldBackgroundColor;

        PropertyBinder.BindOneWayToSource(this, _fieldBackgroundColorPicker, t => t.SelectedColor, _viewModel, s => s.FieldBackgroundColor);
    }
    #endregion

    #region V sync check button
    [Export] public NodePath VSyncCheckButtonPath { get; set; } = null!;
    private RichCheckButton _vSyncCheckButton = null!;

    private void SetupVSyncCheckButton()
    {
        _vSyncCheckButton = GetNode<RichCheckButton>(VSyncCheckButtonPath);
        _vSyncCheckButton.IsChecked = _viewModel.VSyncEnabled;

        PropertyBinder.BindOneWayToSource(this, _vSyncCheckButton, t => t.IsChecked, _viewModel, s => s.VSyncEnabled);
    }
    #endregion

    //{
    //	new("Установить настройки по умолчанию (Not implemented)", MenuItemType.Text),
    //	() => _viewModel.ResetSettingsCommand.Execute(null)
    //},
    #endregion

    public override void Unsubscribe()
    {
        base.Unsubscribe();
        PropertyBinder.Unbind(this);
    }
}