namespace ProniaWebApp.ViewModels
{
    public record HomeVm
    {
        public List<SliderVm> SliderVms { get; set; }
        public List<HomeProductVm> HomeProductVms { get; set; }

    }
}
