namespace SharedSwdCore.Pages
{
    public abstract class SelfTestingCorePage : CorePage, INvokable, ICheckExpectedWebElements
    {
        public abstract void Invoke();

        public abstract bool IsDisplayed();

        public abstract void VerifyExpectedElementsAreDisplayed();
    }
}
