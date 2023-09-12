namespace PGR
{
    public class IrisSystemDisplay : SceneUI
    {
        public void ModifyHP(float hpRatio)
        {
            images["Life Image"].fillAmount = hpRatio;
        }

        public void ModifyCable(bool isOff)
        {
            images["Cable Image"].enabled = !isOff;
        }

        public void ModifyText(string text)
        {
            images["Text Image"].enabled = text.Length > 0;
            texts["Direct Text"].text = text;
        }
    }
}
