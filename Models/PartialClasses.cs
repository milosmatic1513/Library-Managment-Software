using System;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Linq;

namespace ClassProject.Models
{
    [MetadataType(typeof(AuthorMetadata))]
    public partial class author
    {
        // Deletes self and titleauthors
        public bool Delete(pubsEntities db)
        {
            try
            {
                // Remove author's titles
                var titleauthors = db.titleauthors.Where(item => item.au_id == this.au_id);
                foreach (var titleauthor in titleauthors)
                    titleauthor.Delete(db);
                // Remove author
                db.authors.Remove(this);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }

    [MetadataType(typeof(DiscountMetadata))]
    public partial class discount
    {
        // Deletes self
        public bool Delete(pubsEntities db)
        {
            try
            {
                // Delete the entry
                db.Database.ExecuteSqlCommand("DELETE FROM discounts WHERE discounttype = @discounttype and stor_id = @stor_id",
                    new SqlParameter("@discounttype", this.discounttype),
                    new SqlParameter("@stor_id", this.stor_id)
                    );
                return true;
            }
            catch
            {
                return false;
            }
        }

        // Deletes self 
        public static bool Delete(pubsEntities db, string stor_id)
        {
            try
            {
                // Delete the entry
                db.Database.ExecuteSqlCommand("DELETE FROM discounts WHERE stor_id = @stor_id",
                    new SqlParameter("@stor_id", stor_id)
                    );
                return true;
            }
            catch
            {
                return false;
            }
        }
    }

    [MetadataType(typeof(EmployeeMetadata))]
    public partial class employee
    {
        // Deletes self 
        public bool Delete(pubsEntities db)
        {
            try
            {
                db.employees.Remove(this);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }

    [MetadataType(typeof(JobMetadata))]
    public partial class job
    {
        // Deletes self and employees
        public bool Delete(pubsEntities db)
        {
            try
            {
                // Remove employees
                var employees = db.employees.Where(item => item.job_id == this.job_id);
                foreach (var employee in employees)
                    employee.Delete(db);
                // Remove job
                db.jobs.Remove(this);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }

    [MetadataType(typeof(PubInfoMetadata))]
    public partial class pub_info
    {
        // Deletes self 
        public bool Delete(pubsEntities db)
        {
            try
            {
                db.pub_info.Remove(this);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }

    [MetadataType(typeof(PublisherMetadata))]
    public partial class publisher
    {
        // Deletes self, employees and publisher info
        public bool Delete(pubsEntities db)
        {
            try
            {
                // Remove titles
                Models.title.Delete(db, this.pub_id);
                // Remove employees
                var employees = db.employees.Where(item => item.pub_id == this.pub_id);
                foreach (var employee in employees)
                    employee.Delete(db);
                // Remove publisher info
                var pubInfo = db.pub_info.Where(item => item.pub_id == this.pub_id); // Since it is 0 or 1
                foreach (var info in pubInfo)
                    info.Delete(db);
                // Remove publisher
                db.publishers.Remove(this);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }

    [MetadataType(typeof(RoyschedMetadata))]
    public partial class roysched
    {
        // Deletes self 
        public bool Delete(pubsEntities db)
        {
            try
            {
                // Delete the entry
                db.Database.ExecuteSqlCommand("DELETE FROM roysched WHERE title_id = @title_id and lorange = @lorange and hirange = @hirange",
                    new SqlParameter("@title_id", this.title_id),
                    new SqlParameter("@lorange", this.lorange),
                    new SqlParameter("@hirange", this.hirange)
                    );

                return true;
            }
            catch
            {
                return false;
            }
        }

        // Deletes self 
        public static bool Delete(pubsEntities db, string title_id)
        {
            try
            {
                // Delete the entry
                db.Database.ExecuteSqlCommand("DELETE FROM roysched WHERE title_id = @title_id",
                    new SqlParameter("@title_id", title_id)
                    );
                return true;
            }
            catch
            {
                return false;
            }
        }
    }

    [MetadataType(typeof(SaleMetadata))]
    public partial class sale
    {
        // Deletes self 
        public bool Delete(pubsEntities db)
        {
            try
            {
                db.sales.Remove(this);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }

    [MetadataType(typeof(StoreMetadata))]
    public partial class store
    {
        // Deletes self, discounts and sales
        public bool Delete(pubsEntities db)
        {
            try
            {
                // Remove discounts
                discount.Delete(db, this.stor_id);
                // Remove sales
                var sales = db.sales.Where(item => item.stor_id == this.stor_id);
                foreach (var sale in sales)
                    sale.Delete(db);
                // Remove store
                db.stores.Remove(this);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }

    [MetadataType(typeof(TitleMetadata))]
    public partial class title
    {
        // Deletes self, royscheds, sales and titleauthors
        public bool Delete(pubsEntities db)
        {
            try
            {
                // Delete the royscheds
                Models.roysched.Delete(db, this.title_id);
                // Remove sales
                var sales = db.sales.Where(item => item.title_id == this.title_id);
                foreach (var sale in sales)
                    sale.Delete(db);
                // Remove author's titles
                var titleauthors = db.titleauthors.Where(item => item.title_id == this.title_id);
                foreach (var titleauthor in titleauthors)
                    titleauthor.Delete(db);
                // Remove title
                db.titles.Remove(this);
                return true;
            }
            catch
            {
                return false;
            }
        }
        
        public static bool Delete(pubsEntities db, string pub_id)
        {
            try
            {
                // Delete the royscheds
                db.Database.ExecuteSqlCommand("DELETE FROM roysched FROM roysched JOIN titles ON roysched.title_id = titles.title_id WHERE pub_id = @pub_id",
                    new SqlParameter("@pub_id", pub_id)
                    );

                // Delete the entry
                var titles = db.titles.Where(item => item.pub_id == pub_id);
                foreach (var title in titles)
                    title.Delete(db);

                return true;
            }
            catch
            {
                return false;
            }
        }
    }

    [MetadataType(typeof(TitleAuthorMetadata))]
    public partial class titleauthor
    {
        // Deletes self 
        public bool Delete(pubsEntities db)
        {
            try
            {
                db.titleauthors.Remove(this);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}

