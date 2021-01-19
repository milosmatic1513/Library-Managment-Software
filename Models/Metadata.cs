using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Web.Mvc;

namespace ClassProject.Models
{
    public class AuthorMetadata
    {
        [Key]
        [Display(Name = "Author Id")]
        [Required]
        [MaxLength(11, ErrorMessage = "Author Id can't be longer than 11 characters")]
        [RegularExpression(@"^[0-9]{3}-[0-9]{2}-[0-9]{4}", ErrorMessage = "Author Id must match this format 000-00-0000")]
        [Remote("VerifyAuthorId", "authors", AdditionalFields = "editMode", ErrorMessage = "Author Id already exists")]
        public string au_id { get; set; }

        [Display(Name = "Last Name")]
        [Required]
        [MaxLength(40, ErrorMessage = "Last Name can't be longer than 40 characters")]
        public string au_lname { get; set; }
        
        [Display(Name = "First Name")]
        [Required]
        [MaxLength(20, ErrorMessage = "First Name can't be longer than 20 characters")]
        public string au_fname { get; set; }

        [Display(Name = "Phone")]
        [Required]
        [DataType(DataType.PhoneNumber)]
        public string phone { get; set; }

        [Display(Name = "Address")]
        [MaxLength(40, ErrorMessage = "Address can't be longer than 40 characters")]
        public string address { get; set; }

        [Display(Name = "City")]
        [MaxLength(20, ErrorMessage = "City can't be longer than 20 characters")]
        public string city { get; set; }

        [Display(Name = "State")]
        [MaxLength(2, ErrorMessage = "State can't be longer than 2 characters")]
        public string state { get; set; }

        [Display(Name = "Zip")]
        [RegularExpression(@"^[0-9]{5}", ErrorMessage = "Zip must match this format 00000")]
        public string zip { get; set; }
    }
    public class DiscountMetadata
    {
        [Display(Name = "Discount Type")]
        [Required]
        [MaxLength(40, ErrorMessage = "Discount Type can't be longer than 40 characters")]
        [Remote("VerifyDiscountKeys", "discounts", AdditionalFields = "stor_id, editMode", ErrorMessage = "This Discount Type already exists for this Store")]
        public string discounttype { get; set; }

        [Display(Name = "Store")]
        [Required]
        [Remote("VerifyDiscountKeys", "discounts", AdditionalFields = "discounttype, editMode", ErrorMessage = "This Store already has this Discount Type")]
        public string stor_id { get; set; }

        [Display(Name = "Low Quality")]
        public Nullable<short> lowqty { get; set; }

        [Display(Name = "High Quality")]
        public Nullable<short> highqty { get; set; }

        [Display(Name = "Discount")]
        [Required]
        public decimal discount1 { get; set; }

        [ForeignKey("stor_id")]
        [Display(Name = "Store")]
        public virtual store store { get; set; }
    }
    public class EmployeeMetadata
    {
        [Key]
        [Display(Name = "Employee Id")]
        [Required]
        [RegularExpression(@"^[A-Z][A-Z|-][A-Z][1-9][0-9]{4}[FM]$", ErrorMessage = "Employee Id must match one of these formats AAA[1-9]0000[F or M] or A-A[1-9]0000[F or M]")]
        [Remote("VerifyEmployeeId", "employees", AdditionalFields = "editMode", ErrorMessage = "Employee Id already exists")]
        public string emp_id { get; set; }

        [Display(Name = "First Name")]
        [Required]
        [MaxLength(20, ErrorMessage = "First Name can't be longer than 20 characters")]
        public string fname { get; set; }

        [Display(Name = "Minit")]
        [MaxLength(1, ErrorMessage = "Minit can't be longer than 1 characters")]
        public string minit { get; set; }

        [Display(Name = "Last Name")]
        [MaxLength(30, ErrorMessage = "Last Name can't be longer than 30 characters")]
        [Required]
        public string lname { get; set; }

        [Display(Name = "Job Id")]
        [Required]
        public short job_id { get; set; }

        [Display(Name = "Job Level")]
        public Nullable<byte> job_lvl { get; set; }

        [Display(Name = "Publisher")]
        [Required]
        public string pub_id { get; set; }

        [Display(Name = "Hire Date")]
        [Required]
        [DisplayFormat(DataFormatString = "{0:dd/MMM/yyyy}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public System.DateTime hire_date { get; set; }

        [Display(Name = "Job")]
        public virtual job job { get; set; }

        [Display(Name = "Publisher")]
        public virtual publisher publisher { get; set; }
    }
    public class JobMetadata
    {
        [Key]
        [Display(Name = "Job Id")]
        [Required]
        public short job_id { get; set; }

        [Display(Name = "Job Descreption")]
        [Required]
        [DataType(DataType.MultilineText)]
        [MaxLength(50, ErrorMessage = "Job Description can't be longer than 50 characters")]
        public string job_desc { get; set; }

        [Display(Name = "Minimum Level")]
        [Required]
        [Range(10, 250, ErrorMessage = "Minimum level must be an integer between 10 and 250")]
        [MinLowerThanMax("max_lvl", "Minimum level must be lower than the Maximum level")]
        public byte min_lvl { get; set; }

        [Display(Name = "Maximum Level")]
        [Required]
        [Range(10, 250, ErrorMessage = "Maximum level must be an integer between 10 and 250")]
        [MinLowerThanMax("min_lvl", "Minimum level must be lower than the Maximum level")]
        public byte max_lvl { get; set; }


        [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
        public class MinLowerThanMax : ValidationAttribute
        {
            string otherPropertyName;

            public MinLowerThanMax(string otherPropertyName, string errorMessage) : base(errorMessage)
            {
                this.otherPropertyName = otherPropertyName;
            }

            protected override ValidationResult IsValid(object value, ValidationContext validationContext)
            {
                ValidationResult validationResult = ValidationResult.Success;
                try
                {
                    // Using reflection we can get a reference to the other value property
                    var otherPropertyInfo = validationContext.ObjectType.GetProperty(this.otherPropertyName);
                    // Let's check that otherProperty is of type byte as we expect it to be
                    if (otherPropertyInfo.PropertyType.Equals(new Byte().GetType()) && value != null)
                    {
                        int toValidate = Convert.ToInt16((byte)value);
                        int referenceProperty = Convert.ToInt16((byte)otherPropertyInfo.GetValue(validationContext.ObjectInstance, null));
                        // if the max level is lower than the min level, then the validationResult will be set to false and return
                        // a properly formatted error message
                        if (otherPropertyName.Equals("min_lvl"))
                        {
                            if (toValidate <= referenceProperty)  // max_lvl <= min_lvl
                            {
                                validationResult = new ValidationResult(ErrorMessageString);
                            }
                        }
                        else if (otherPropertyName.Equals("max_lvl"))
                        {
                            if (toValidate >= referenceProperty) // min_lvl >= max_lvl
                            {
                                validationResult = new ValidationResult(ErrorMessageString);
                            }
                        }
                    }
                    else
                    {
                        validationResult = new ValidationResult("An error occurred while validating the property");
                    }
                }
                catch (Exception ex)
                {
                    // Do stuff, i.e. log the exception
                    // Let it go through the upper levels, something bad happened
                    throw ex;
                }

                return validationResult;
            }
        }
    }
    public class PubInfoMetadata
    {
        [Key]
        [Display(Name = "Publisher Id")]
        [Required]
        public string pub_id { get; set; }

        [Display(Name = "Logo")]
        public byte[] logo { get; set; }

        [Display(Name = "Publisher Info")]
        [DataType(DataType.MultilineText)]
        public string pr_info { get; set; }

        [Display(Name = "Publisher")]
        public virtual publisher publisher { get; set; }
    }
    public class PublisherMetadata
    {
        [Key]
        [Display(Name = "Publisher Id")]
        [Required]
        [RegularExpression(@"1389|0736|0877|1622|1756|99[0-9][0-9]", ErrorMessage = "Publisher Id must match this format 99[0-9][0-9] or be one of these 1389, 0736, 0877, 1622, 1756")]
        [Remote("VerifyPublisherId", "publishers", AdditionalFields = "editMode", ErrorMessage = "Publisher Id already exists")]
        public string pub_id { get; set; }

        [Display(Name = "Publisher Name")]
        [Required]
        [MaxLength(40, ErrorMessage = "Publisher Name can't be longer than 40 characters")]
        public string pub_name { get; set; }

        [Display(Name = "City")]
        [MaxLength(20, ErrorMessage = "City can't be longer than 20 characters")]
        public string city { get; set; }

        [Display(Name = "State")]
        [MaxLength(2, ErrorMessage = "State can't be longer than 2 characters")]
        public string state { get; set; }

        [Display(Name = "Country")]
        [MaxLength(30, ErrorMessage = "Country can't be longer than 30 characters")]
        public string country { get; set; }
    }
    public class RoyschedMetadata
    {
        [Display(Name = "Title")]
        [Required]
        [Remote("VerifyRoychedKeys", "royscheds", AdditionalFields = "lorange, hirange, editMode", ErrorMessage = "Royched with for this Title with these Ranges already exists")]
        public string title_id { get; set; }

        [Display(Name = "Low Range")]
        [Required]
        [NonNegative("Low Range must be greater than 0")]
        [MinLowerThanMax("hirange", "Low Range must lower than High Range")]
        [Remote("VerifyRoychedKeys", "royscheds", AdditionalFields = "title_id, hirange, editMode", ErrorMessage = "Royched with for this Title with these Ranges already exists")]
        public Nullable<int> lorange { get; set; }

        [Display(Name = "High Range")]
        [Required]
        [MinLowerThanMax("lorange", "Low Range must lower than High Range")]
        [Remote("VerifyRoychedKeys", "royscheds", AdditionalFields = "lorange, title_id, editMode", ErrorMessage = "Royched with for this Title with these Ranges already exists")]
        public Nullable<int> hirange { get; set; }

        [Display(Name = "Royalty")]
        public Nullable<int> royalty { get; set; }

        [ForeignKey("title_id")]
        [Display(Name = "Title")]
        public virtual title title { get; set; }


        [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
        public class MinLowerThanMax : ValidationAttribute
        {
            string otherPropertyName;

            public MinLowerThanMax(string otherPropertyName, string errorMessage) : base(errorMessage)
            {
                this.otherPropertyName = otherPropertyName;
            }

            protected override ValidationResult IsValid(object value, ValidationContext validationContext)
            {
                ValidationResult validationResult = ValidationResult.Success;
                try
                {
                    // Using reflection we can get a reference to the other value property
                    var otherPropertyInfo = validationContext.ObjectType.GetProperty(this.otherPropertyName);
                    // Let's check that otherProperty is of type byte as we expect it to be
                    if (value != null)
                    {
                        Nullable<int> toValidate = (Nullable<int>)value;
                        Nullable<int> referenceProperty = (Nullable<int>)otherPropertyInfo.GetValue(validationContext.ObjectInstance, null);
                        // if the max level is lower than the min level, then the validationResult will be set to false and return
                        // a properly formatted error message
                        if (otherPropertyName.Equals("lorange"))
                        {
                            if (toValidate <= referenceProperty)  // hirange <= lorange
                            {
                                validationResult = new ValidationResult(ErrorMessageString);
                            }
                        }
                        else if (otherPropertyName.Equals("hirange"))
                        {
                            if (toValidate >= referenceProperty)  // lorange >= hirange
                            {
                                validationResult = new ValidationResult(ErrorMessageString);
                            }
                        }
                    }
                    else
                    {
                        validationResult = new ValidationResult("An error occurred while validating the property");
                    }
                }
                catch (Exception ex)
                {
                    // Do stuff, i.e. log the exception
                    // Let it go through the upper levels, something bad happened
                    throw ex;
                }
                return validationResult;
            }
        }

        [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
        public class NonNegative : ValidationAttribute
        {
            public NonNegative(string errorMessage) : base(errorMessage)
            {
            }

            protected override ValidationResult IsValid(object value, ValidationContext validationContext)
            {
                ValidationResult validationResult = ValidationResult.Success;
                try
                {
                    if (value != null)
                    {
                        Nullable<int> toValidate = (Nullable<int>)value;
                        if (toValidate < 0)
                        {
                            validationResult = new ValidationResult(ErrorMessageString);
                        }
                    }
                    else
                    {
                        validationResult = new ValidationResult("An error occurred while validating the property");
                    }
                }
                catch (Exception ex)
                {
                    // Do stuff, i.e. log the exception
                    // Let it go through the upper levels, something bad happened
                    throw ex;
                }
                return validationResult;
            }
        }
    }
    public class SaleMetadata
    {
        [Key]
        [Column(Order = 0)]
        [Display(Name = "Store")]
        [Required]
        [Remote("VerifySaleKeys", "sales", AdditionalFields = "ord_num, title_id, editMode", ErrorMessage = "This Store has already registered this Order Number for this Title")]
        public string stor_id { get; set; }

        [Key]
        [Column(Order = 1)]
        [Display(Name = "Order Number")]
        [Required]
        [MaxLength(20, ErrorMessage = "Order Number can't be longer than 20 characters")]
        [Remote("VerifySaleKeys", "sales", AdditionalFields = "stor_id, title_id, editMode", ErrorMessage = "This Order Number has already been registered by this Store for this Title")]
        public string ord_num { get; set; }

        [Display(Name = "Order Date")]
        [Required]
        [DisplayFormat(DataFormatString = "{0:dd/MMM/yyyy}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public System.DateTime ord_date { get; set; }

        [Display(Name = "Quantity")]
        [Required]
        public short qty { get; set; }

        [Display(Name = "Pay Terms")]
        [Required]
        [MaxLength(12, ErrorMessage = "Pay Terms can't be longer than 12 characters")]
        public string payterms { get; set; }

        [Key]
        [Column(Order = 2)]
        [Display(Name = "Title")]
        [Remote("VerifySaleKeys", "sales", AdditionalFields = "ord_num, stor_id, editMode", ErrorMessage = "This Title has already been registered by this Store with this Order Number")]
        [Required]
        public string title_id { get; set; }

        [Display(Name = "Store")]
        public virtual store store { get; set; }

        [Display(Name = "Title")]
        public virtual title title { get; set; }
    }
    public class StoreMetadata {
        [Key]
        [Display(Name = "Store Id")]
        [Required]
        [StringLength(4, MinimumLength = 4, ErrorMessage = "Store Id must have length 4")]
        [Remote("VerifyStoreId", "stores", AdditionalFields = "editMode", ErrorMessage = "Store Id already exists")]
        public string stor_id { get; set; }

        [Display(Name = "Store Name")]
        [MaxLength(40, ErrorMessage = "Store Name can't be longer than 40 characters")]
        public string stor_name { get; set; }

        [Display(Name = "Store address")]
        [MaxLength(40, ErrorMessage = "Store Address can't be longer than 40 characters")]
        public string stor_address { get; set; }

        [Display(Name = "City")]
        [MaxLength(20, ErrorMessage = "City can't be longer than 20 characters")]
        public string city { get; set; }

        [Display(Name = "State")]
        [MaxLength(2, ErrorMessage = "State can't be longer than 4 characters")]
        public string state { get; set; }

        [Display(Name = "Zip")]
        [RegularExpression(@"^[0-9]{5}", ErrorMessage = "Zip must match this format 00000")]
        public string zip { get; set; }
    }
    public class TitleMetadata 
    {
        [Key]
        [Display(Name = "Title Id")]
        [Required]
        [MaxLength(6, ErrorMessage = "Title Id can't be longer than 6 characters")]
        [Remote("VerifyTitleId", "titles", AdditionalFields = "editMode", ErrorMessage = "Title Id already exists")]
        public string title_id { get; set; }

        [Display(Name = "Title")]
        [Required]
        [MaxLength(80, ErrorMessage = "Title can't be longer than 80 characters")]
        public string title1 { get; set; }

        [Display(Name = "Type")]
        [Required]
        [MaxLength(12, ErrorMessage = "Type can't be longer than 12 characters")]
        public string type { get; set; }
        
        [Display(Name = "Publisher")]
        public string pub_id { get; set; }

        [Display(Name = "Price")]
        public Nullable<decimal> price { get; set; }

        [Display(Name = "Advance")]
        public Nullable<decimal> advance { get; set; }

        [Display(Name = "Royalty")]
        public Nullable<int> royalty { get; set; }

        [Display(Name = "YTD Sales")]
        public Nullable<int> ytd_sales { get; set; }

        [Display(Name = "Notes")]
        [DataType(DataType.MultilineText)]
        [MaxLength(200, ErrorMessage = "Notes can't be longer than 200 characters")]
        public string notes { get; set; }

        [Display(Name = "Date Published")]
        [Required]
        [DisplayFormat(DataFormatString = "{0:dd/MMM/yyyy}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        public System.DateTime pubdate { get; set; }

        [Display(Name = "Publisher")]
        public virtual publisher publisher { get; set; }
    }
    public class TitleAuthorMetadata
    {
        [Key]
        [Column(Order = 0)]
        [Display(Name = "Author")]
        [Required]
        [Remote("VerifyTitleAuthorId", "titleauthors", AdditionalFields = "title_id, editMode", ErrorMessage = "This Author has already registered this Title")]
        public string au_id { get; set; }

        [Key]
        [Column(Order = 1)]
        [Display(Name = "Title")]
        [Required]
        [Remote("VerifyTitleAuthorId", "titleauthors", AdditionalFields = "title_id, editMode", ErrorMessage = "This Title is already registered by this Author")]
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