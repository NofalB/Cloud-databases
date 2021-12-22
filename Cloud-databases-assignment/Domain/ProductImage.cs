using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class ProductImage
    {
        public string ImageURL { get; set; }

        public ProductImage(string imageURL)
        {
            ImageURL = imageURL;
        }

        public ProductImage()
        {

        }
    }
}
