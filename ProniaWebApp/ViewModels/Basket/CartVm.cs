namespace ProniaWebApp.ViewModels.Basket
{
    public record CartVm
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string ImgUrl { get; set; }
        public int Count { get; set; }
    }
}
