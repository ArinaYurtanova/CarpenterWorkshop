using CarpenterWorkshopService.BindingModels;

namespace CarpenterWorkshopView
{
    internal class CustomerBindingModel : CustomerBidingModel
    {
        public int Id { get; set; }
        public object ClientFIO { get; set; }
    }
}