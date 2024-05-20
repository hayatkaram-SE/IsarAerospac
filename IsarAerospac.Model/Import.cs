using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IsarAerospac.Model
{
    public class Import
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public string Price { get; set; }
        public string Date { get; set; }
        public List<string> BindingCombo { get; set; }
        public bool InStock { get; set; }
        public int Qty { get; set; }
        public string Description { get; set; }
        public bool IsEnabled { get; set; }
        public Import(string title, string author, string date, string price,  string bindingCombo,int qty, bool inStock, string description)
        {
            try
            {
                Title = title;
                Author = author;
                Price = price;
                Date = date;
                if (BindingCombo == null)
                    BindingCombo = new List<string>();
                BindingCombo.Add(bindingCombo);
                Qty = qty;
                InStock = inStock;
                Description = description;
                IsEnabled = !inStock;
            }
            catch (Exception ex)
            {

                throw ex;
            }
            
        }
    }
}
