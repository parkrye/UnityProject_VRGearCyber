public class PopUpUI : BaseUI
{
    protected override void Awake()
    {
        base.Awake();
    }

    public override void CloseUI()
    {
        base.CloseUI();

        GameManager.UI.ClosePopupUI();
    }
}