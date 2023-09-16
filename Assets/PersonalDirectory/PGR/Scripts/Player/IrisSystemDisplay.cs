namespace PGR
{
    public class IrisSystemDisplay : SceneUI
    {
        void Start()
        {
            UseSocket(0, false);
            UseSocket(1, false);
            UseSocket(2, false);
            ModifyText("");
        }

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

        public void UseSocket(int num, bool isUsing)
        {
            switch (num)
            {
                case 0:
                    images["Left"].gameObject.SetActive(isUsing);
                    break;
                case 1:
                    images["Back"].gameObject.SetActive(isUsing);
                    break;
                case 2:
                    images["Right"].gameObject.SetActive(isUsing);
                    break;
            }
        }
    }
}
