namespace Ecommerce.Core.src.Entity
{
    public class Image : BaseEntity
    {
        public byte[] Data { get; set; }

        // Add a parameterless constructor for Entity Framework
        public Image() { }

        public Image(byte[] data)
        {
            Data=data;
        }
    }
}
