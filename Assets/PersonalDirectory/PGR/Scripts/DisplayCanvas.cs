namespace PGR
{
    /// <summary>
    /// 홍채 디스플레이를 활성화했을 때 보이는 World Space UI
    /// 위치를 적절히 조정하고, 텍스트를 입력하면 됨
    /// </summary>
    public class DisplayCanvas : SceneUI
    {
        public void ChangeMainText(string context)
        {
            texts["MainText"].text = context;
        }

        public void ChangeSubText(string context)
        {
            texts["SubText"].text = context;
        }
    }

}