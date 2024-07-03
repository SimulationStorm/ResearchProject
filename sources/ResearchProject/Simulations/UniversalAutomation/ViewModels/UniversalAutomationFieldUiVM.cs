public partial class UniversalAutomationFieldUiVM : AutomationFieldUiVM<byte>
{
    public UniversalAutomationFieldUiVM
    (
        FieldStateModel fieldStateModel,
        UniversalAutomationModel automationModel,
        UniversalAutomationPresentationModel presentationModel 
    )
    : base(fieldStateModel, automationModel, presentationModel) {}
}