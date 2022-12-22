using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarFilterTask.Entities
{
    public class Car
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int BrandId { get; set; }
        public int TypeId { get; set; }
        public double Price { get; set; }
        public double Kilometers { get; set; }
        public int Year { get; set; }
        public string Color { get; set; }
        public bool IsNew { get; set; }
        public virtual Brand Brand { get; set; }
        public virtual Type Type { get; set; }

    }
}
