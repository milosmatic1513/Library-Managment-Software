using System;
using System.ComponentModel.DataAnnotations;

namespace ClassProject.Models
{
    public class AuthorMetadata
    {
        [Display(Name = "Last Name")]
        public string au_lname { get; set; }
        
        [Display(Name = "First Name")]
        public string au_fname { get; set; }

        [Display(Name = "Phone")]
        public string phone { get; set; }

        [Display(Name = "Address")]
        public string address { get; set; }

        [Display(Name = "City")]
        public string city { get; set; }

        [Display(Name = "State")]
        public string state { get; set; }

        [Display(Name = "Zip")]
        public string zip { get; set; }
        
        
    }
    public class DiscountMetadata
    {
        [Display(Name = "Discount Type")]
        public string discounttype { get; set; }
        [Display(Name = "Store Id")]
        public string stor_id { get; set; }
        [Display(Name = "Low Quality")]
        public Nullable<short> lowqty { get; set; }
        [Display(Name = "High Quality")]
        public Nullable<short> highqty { get; set; }
        [Display(Name = "Discount")]
        public decimal discount1 { get; set; }
        [Display(Name = "Store")]
        public virtual store store { get; set; }
    }
    public class EmpolyeeMetadata
    {
        [Display(Name = "Emp Id")]
        public string emp_id { get; set; }
        [Display(Name = "Firstname")]
        public string fname { get; set; }
        [Display(Name = "Minit")]
        public string minit { get; set; }
        [Display(Name = "Last Name")]
        public string lname { get; set; }
        [Display(Name = "Job Id")]
        public short job_id { get; set; }
        [Display(Name = "Job Level")]
        public Nullable<byte> job_lvl { get; set; }
        [Display(Name = "Publisher Id")]
        public string pub_id { get; set; }
        [Display(Name = "Hire Date")]
        public System.DateTime hire_date { get; set; }
        [Display(Name = "Job")]
        public virtual job job { get; set; }
        [Display(Name = "Publisher")]
        public virtual publisher publisher { get; set; }
    }
    public class JobMetadata
    {
        [Display(Name = "Job Id")]
        public short job_id { get; set; }
        [Display(Name = "Job Desc")]
        public string job_desc { get; set; }
        [Display(Name = "Minimum Level")]
        public byte min_lvl { get; set; }
        [Display(Name = "Max  Level")]
        public byte max_lvl { get; set; }

    }
    public class PubInfoMetadata
    {
        [Display(Name = "Publisher Id")]
        public string pub_id { get; set; }
        [Display(Name = "Logo")]
        public byte[] logo { get; set; }
        [Display(Name = "Pr Info")]
        public string pr_info { get; set; }
        [Display(Name = "Publisher")]
        public virtual publisher publisher { get; set; }
   

    }
    public class PublisherMetadata
    {
        [Display(Name = "Publisher info")]
        public string pub_id { get; set; }
        [Display(Name = "Publisher Name")]
        public string pub_name { get; set; }
        [Display(Name = "City")]
        public string city { get; set; }
        [Display(Name = "State")]
        public string state { get; set; }
        [Display(Name = "Country")]
        public string country { get; set; }
    }
    public class RoyschedMetadata
    {
        [Display(Name = "Title Id")]
        public string title_id { get; set; }
        [Display(Name = "Low Range")]
        public Nullable<int> lorange { get; set; }
        [Display(Name = "High Range")]
        public Nullable<int> hirange { get; set; }
        [Display(Name = "Royalty")]
        public Nullable<int> royalty { get; set; }
        [Display(Name = "Title")]
        public virtual title title { get; set; }
    }
    public class SaleMetadata
    {
        [Display(Name = "Store Id")]
        public string stor_id { get; set; }
        [Display(Name = "Order Number")]
        public string ord_num { get; set; }
        [Display(Name = "Order Date")]
        public System.DateTime ord_date { get; set; }
        [Display(Name = "Quantity")]
        public short qty { get; set; }
        [Display(Name = "Pay Terms")]
        public string payterms { get; set; }
        [Display(Name = "Title Id")]
        public string title_id { get; set; }
        [Display(Name = "Store")]
        public virtual store store { get; set; }
        [Display(Name = "title")]
        public virtual title title { get; set; }
    }
    public class StoreMetadata {
        [Display(Name = "Store Id")]
        public string stor_id { get; set; }
        [Display(Name = "Store Name")]
        public string stor_name { get; set; }
        [Display(Name = "Store address")]
        public string stor_address { get; set; }
        [Display(Name = "City")]
        public string city { get; set; }
        [Display(Name = "State")]
        public string state { get; set; }
        [Display(Name = "Zip")]
        public string zip { get; set; }
    }
    public class TitleMetadata 
    {
        [Display(Name = "Title id")]
        public string title_id { get; set; }
        [Display(Name = "Title")]
        public string title1 { get; set; }
        [Display(Name = "Type")]
        public string type { get; set; }        
        [Display(Name = "Publisher id")]
        public string pub_id { get; set; }
        [Display(Name = "Price")]
        public Nullable<decimal> price { get; set; }
        [Display(Name = "Andvance")]
        public Nullable<decimal> advance { get; set; }
        [Display(Name = "Royalty")]
        public Nullable<int> royalty { get; set; }
        [Display(Name = "YTD Sales")]
        public Nullable<int> ytd_sales { get; set; }
        [Display(Name = "Notes")]
        public string notes { get; set; }
        [Display(Name = "Date Published")]
        public System.DateTime pubdate { get; set; }
        [Display(Name = "Publisher")]
        public virtual publisher publisher { get; set; }
    }
    public class TitleAuthorMetadata
    {
        [Display(Name = "Author Id")]
        public string au_id { get; set; }
        [Display(Name = "Title ID")]
        public string title_id { get; set; }
        [Display(Name = "Author Order")]
        public Nullable<byte> au_ord { get; set; }
        [Display(Name = "Royalty Per")]
        public Nullable<int> royaltyper { get; set; }
        [Display(Name = "Author")]
        public virtual author author { get; set; }
        [Display(Name = "Title")]
        public virtual title title { get; set; }
    }
}