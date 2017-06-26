using MVCSportsStore.Domain.Entities;

namespace MVCSportsStore.Models
{
    public class CartIndexViewModel
    {
        public Cart Cart { get; set; }

        public string ReturnUrl { get; set; }
    }
}