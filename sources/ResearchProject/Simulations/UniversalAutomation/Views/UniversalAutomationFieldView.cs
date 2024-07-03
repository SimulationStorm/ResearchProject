using Godot;
using System.Collections.Generic;

public partial class UniversalAutomationFieldView : SimulationFieldView, IView<UniversalAutomationFieldVM>
{
    #region Fields
    private UniversalAutomationFieldVM _viewModel = null!;

    private IGetCell<byte> _fieldCellGetter = null!;

    private IReadOnlyDictionary<byte, Color> _cellColorsByStateNumber = null!;
    #endregion

    public void Setup(UniversalAutomationFieldVM viewModel)
    {
        _viewModel = viewModel;
        base.Setup(viewModel);

        _fieldCellGetter = _viewModel.FieldCellGetter;
        _cellColorsByStateNumber = _viewModel.CellColorsByStateNumber;
    }

    protected override Color GetCellColor(int x, int y) => _cellColorsByStateNumber[_fieldCellGetter.GetCell(x, y)];
}