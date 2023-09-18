namespace PGR
{
    public class LoadingUI : SceneUI
    {
        public void Loading(bool value)
        {
            images["Loading Image"].gameObject.SetActive(value);
        }
    }
}
