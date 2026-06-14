using RajasthanTourCabN.Data;
using RajasthanTourCabN.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web;
namespace RajasthanTourCabN.Data
{
    public class PageService
    {
        DbHelper db = new DbHelper();
        private readonly string _connectionString;
        public PageService()
        {
            _connectionString = Code.LIBS.SiteKey.SqlConn;
        }
        public List<Page> GetPages()
        {
            if (HttpContext.Current.Cache["pages"] != null)
                return (List<Page>)HttpContext.Current.Cache["pages"];

            List<Page> list = new List<Page>();

            DataTable dt = db.GetData("SELECT * FROM Pages WHERE IsActive = 1 and IsDeleted=0  ORDER BY DisplayOrder");

            foreach (DataRow row in dt.Rows)
            {
                list.Add(new Page
                {
                    Id = Convert.ToInt32(row["Id"]),
                    Title = row["Title"].ToString(),
                    Slug = row["Slug"].ToString(),
                    Content = row["Content"].ToString(),
                    ParentId = row["ParentId"] == DBNull.Value ? (int?)null : Convert.ToInt32(row["ParentId"]),
                    IsActive = Convert.ToBoolean(row["IsActive"]),
                    DisplayOrder = Convert.ToInt32(row["DisplayOrder"]),
                    MetaTitle = row.Table.Columns.Contains("MetaTitle") ? row["MetaTitle"].ToString() : "",
                    MetaDescription = row.Table.Columns.Contains("MetaDescription") ? row["MetaDescription"].ToString() : "",
                    MetaKeywords = row.Table.Columns.Contains("MetaKeywords") ? row["MetaKeywords"].ToString() : ""
                });
            }

            HttpContext.Current.Cache.Insert("pages", list, null, DateTime.Now.AddMinutes(60), TimeSpan.Zero);

            return list;
        }

        public void InsertPage(Page model)
        {
            string query = @"INSERT INTO Pages (Title,Slug,Content,ParentId,IsActive,DisplayOrder,MetaTitle,MetaDescription,MetaKeywords)
                            VALUES (@Title,@Slug,@Content,@ParentId,@IsActive,@DisplayOrder,@MetaTitle,@MetaDescription,@MetaKeywords)";

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Title", model.Title);
                    cmd.Parameters.AddWithValue("@Slug", model.Slug);
                    cmd.Parameters.AddWithValue("@Content", model.Content);
                    cmd.Parameters.AddWithValue("@ParentId", (object)model.ParentId ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@IsActive", model.IsActive);
                    cmd.Parameters.AddWithValue("@DisplayOrder", model.DisplayOrder);
                    cmd.Parameters.AddWithValue("@MetaTitle", model.MetaTitle ?? "");
                    cmd.Parameters.AddWithValue("@MetaDescription", model.MetaDescription ?? "");
                    cmd.Parameters.AddWithValue("@MetaKeywords", model.MetaKeywords ?? "");
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            // ✅ CLEAR CACHE
            CacheHelper.ClearPagesCache();
        }
        public void UpdatePage(Page model)
        {
            string query = @"UPDATE Pages SET 
        Title=@Title,
        Slug=@Slug,
        Content=@Content,
        ParentId=@ParentId,
        IsActive=@IsActive,
        DisplayOrder=@DisplayOrder,
        MetaTitle=@MetaTitle,
        MetaDescription=@MetaDescription,
        MetaKeywords=@MetaKeywords
        WHERE Id=@Id";

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Id", model.Id);
                    cmd.Parameters.AddWithValue("@Title", model.Title);
                    cmd.Parameters.AddWithValue("@Slug", model.Slug);
                    cmd.Parameters.AddWithValue("@Content", model.Content);
                    cmd.Parameters.AddWithValue("@ParentId", (object)model.ParentId ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@IsActive", model.IsActive);
                    cmd.Parameters.AddWithValue("@DisplayOrder", model.DisplayOrder);
                    cmd.Parameters.AddWithValue("@MetaTitle", model.MetaTitle ?? "");
                    cmd.Parameters.AddWithValue("@MetaDescription", model.MetaDescription ?? "");
                    cmd.Parameters.AddWithValue("@MetaKeywords", model.MetaKeywords ?? "");

                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            // ✅ CLEAR CACHE
            CacheHelper.ClearPagesCache();
        }
        public void DeletePage(int id)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("update  FROM Pages set IsDeleted=1 WHERE Id=@Id", con))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            // ✅ CLEAR CACHE
            CacheHelper.ClearPagesCache();
        }

        public string GetUserRole(string username, string password)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                string query = "SELECT Role FROM AdminUsers WHERE Username=@u AND Password=@p";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@u", username);
                    cmd.Parameters.AddWithValue("@p", password);

                    con.Open();
                    var role = cmd.ExecuteScalar();

                    return role != null ? role.ToString() : null;
                }
            }
        }
        public bool CheckLogin(string username, string password)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                string query = "SELECT COUNT(*) FROM AdminUsers WHERE Username=@u AND Password=@p";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@u", username);
                    cmd.Parameters.AddWithValue("@p", password);

                    con.Open();
                    int count = (int)cmd.ExecuteScalar();

                    return count > 0;
                }
            }
        }
        public List<AdminMenu> GetAdminMenu(string role)
        {
            List<AdminMenu> list = new List<AdminMenu>();

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                string query = "SELECT * FROM AdminMenu WHERE Role=@role ORDER BY DisplayOrder";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@role", role);

                    con.Open();
                    SqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        list.Add(new AdminMenu
                        {
                            Title = dr["Title"].ToString(),
                            Url = dr["Url"].ToString(),
                            Icon = dr["Icon"].ToString()
                        });
                    }
                }
            }

            return list;
        }


        public List<CabPricing> GetCabPricing()
        {
            if (HttpContext.Current.Cache["CabPricing"] != null)
                return (List<CabPricing>)HttpContext.Current.Cache["CabPricing"];
            List<CabPricing> list = new List<CabPricing>();

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                string query = @"SELECT Id, VehicleName, Fare4hr, Fare8hr, 
                                ExtraKm, ExtraHour, DisplayOrder,City
                         FROM CabPricing
                         WHERE IsActive = 1
                         ORDER BY DisplayOrder";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    con.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            CabPricing obj = new CabPricing();

                            obj.Id = Convert.ToInt32(dr["Id"]);
                            obj.VehicleName = dr["VehicleName"].ToString();
                            obj.City = Convert.ToString(dr["City"]);
                            obj.Fare4hr = Convert.ToDecimal(dr["Fare4hr"]);
                            obj.Fare8hr = Convert.ToDecimal(dr["Fare8hr"]);
                            obj.ExtraKm = Convert.ToDecimal(dr["ExtraKm"]);
                            obj.ExtraHour = Convert.ToDecimal(dr["ExtraHour"]);
                            obj.DisplayOrder = Convert.ToInt32(dr["DisplayOrder"]);

                            list.Add(obj);
                        }
                    }
                }
            }
            HttpContext.Current.Cache.Insert("CabPricing", list, null, DateTime.Now.AddMinutes(60), TimeSpan.Zero);

            return list;
        }
        public CabPricing GetById(int id)
        {
            CabPricing obj = new CabPricing();

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                string query = "SELECT * FROM CabPricing WHERE Id=@Id";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Id", id);

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    obj.Id = Convert.ToInt32(dr["Id"]);
                    obj.VehicleName = dr["VehicleName"].ToString();
                    obj.Fare4hr = Convert.ToDecimal(dr["Fare4hr"]);
                    obj.Fare8hr = Convert.ToDecimal(dr["Fare8hr"]);
                    obj.ExtraKm = Convert.ToDecimal(dr["ExtraKm"]);
                    obj.ExtraHour = Convert.ToDecimal(dr["ExtraHour"]);
                    obj.City = dr["City"].ToString();
                    obj.DisplayOrder = Convert.ToInt32(dr["DisplayOrder"]);
                }
            }
            return obj;
        }
        public void UpdateCab(CabPricing model)
        {
            string query = @"UPDATE CabPricing SET
    VehicleName=@VehicleName,
    Fare4hr=@Fare4hr,
    Fare8hr=@Fare8hr,
    ExtraKm=@ExtraKm,
    ExtraHour=@ExtraHour,
    City=@City,
    DisplayOrder=@DisplayOrder
    WHERE Id=@Id";
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Id", model.Id);
                cmd.Parameters.AddWithValue("@VehicleName", model.VehicleName);
                cmd.Parameters.AddWithValue("@Fare4hr", model.Fare4hr);
                cmd.Parameters.AddWithValue("@Fare8hr", model.Fare8hr);
                cmd.Parameters.AddWithValue("@ExtraKm", model.ExtraKm);
                cmd.Parameters.AddWithValue("@ExtraHour", model.ExtraHour);
                cmd.Parameters.AddWithValue("@DisplayOrder", model.DisplayOrder);
                cmd.Parameters.AddWithValue("@City", model.City);
                con.Open();
                cmd.ExecuteNonQuery();
            }
            // ✅ CLEAR CACHE
            CacheHelper.ClearPagesCache();
        }
        public void DeleteCab(int id)
        {
            string query = "UPDATE CabPricing SET IsActive=0 WHERE Id=@Id";
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Id", id);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }
        public void DeleteBookingInquiry(int id)
        {
            string query = "UPDATE BookingInquiry SET IsDeleteId=1 WHERE BookingId=@BookingId";
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@BookingId", id);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void InsertCab(CabPricing model)
        {
            string query = @"INSERT INTO CabPricing
    (VehicleName, Fare4hr, Fare8hr, ExtraKm, ExtraHour, DisplayOrder, City, IsActive)
    VALUES (@VehicleName, @Fare4hr, @Fare8hr, @ExtraKm, @ExtraHour, @DisplayOrder, @City, 1)";

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@VehicleName", model.VehicleName);
                cmd.Parameters.AddWithValue("@Fare4hr", model.Fare4hr);
                cmd.Parameters.AddWithValue("@Fare8hr", model.Fare8hr);
                cmd.Parameters.AddWithValue("@ExtraKm", model.ExtraKm);
                cmd.Parameters.AddWithValue("@ExtraHour", model.ExtraHour);
                cmd.Parameters.AddWithValue("@DisplayOrder", model.DisplayOrder);
                cmd.Parameters.AddWithValue("@City", model.City);

                con.Open();
                cmd.ExecuteNonQuery();
            }
            // ✅ CLEAR CACHE
            CacheHelper.ClearPagesCache();
        }
        public void SaveBooking(Booking model)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                string query = @"INSERT INTO Booking 
            (CustomerName, Mobile, City, VehicleName, KM, Hours, TotalAmount)
            VALUES (@CustomerName,@Mobile,@City,@VehicleName,@KM,@Hours,@TotalAmount)";

                SqlCommand cmd = new SqlCommand(query, con);

                cmd.Parameters.AddWithValue("@CustomerName", model.CustomerName);
                cmd.Parameters.AddWithValue("@Mobile", model.Mobile);
                cmd.Parameters.AddWithValue("@City", model.City);
                cmd.Parameters.AddWithValue("@VehicleName", model.VehicleName);
                cmd.Parameters.AddWithValue("@KM", model.KM);
                cmd.Parameters.AddWithValue("@Hours", model.Hours);
                cmd.Parameters.AddWithValue("@TotalAmount", model.TotalAmount);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }
        public void SaveBookingInquiry(BookingInquiryModel model)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand(@"
            INSERT INTO BookingInquiry
            (
                FullName,
                MobileNo,
                EmailId,
                PickupLocation,
                DropLocation,
                PickupDate,
                PickupTime,
                VehicleType,
                Message,
                BookingStatus
            )
            VALUES
            (
                @FullName,
                @MobileNo,
                @EmailId,
                @PickupLocation,
                @DropLocation,
                @PickupDate,
                @PickupTime,
                @VehicleType,
                @Message,'Pending'
            )", con);

                cmd.Parameters.AddWithValue("@FullName", model.FullName ?? "");
                cmd.Parameters.AddWithValue("@MobileNo", model.MobileNo ?? "");
                cmd.Parameters.AddWithValue("@EmailId", model.EmailId ?? "");
                cmd.Parameters.AddWithValue("@PickupLocation", model.PickupLocation ?? "");
                cmd.Parameters.AddWithValue("@DropLocation", model.DropLocation ?? "");
                cmd.Parameters.AddWithValue("@PickupDate",
                    (object)model.PickupDate ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@PickupTime", model.PickupTime ?? "");
                cmd.Parameters.AddWithValue("@VehicleType", model.VehicleType ?? "");
                cmd.Parameters.AddWithValue("@Message", model.Message ?? "");

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }
        public void SaveDriverBooking(DriverBooking model)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand(@"
            INSERT INTO DriverBooking
            (
                TripType,
                PickupDate,
                PickupTime,
                PickupLocation,
                DropLocation,
                PickupLat,
                PickupLng,
                DropLat,
                DropLng,
                DropoffDate,
                DropoffTime,
                CarBrand,
                CarModel,
                CarType,
                Transmission,
                FullName,
                MobileNo,
                EmailId,
                BookingStatus
            )
            VALUES
            (
                @TripType,
                @PickupDate,
                @PickupTime,
                @PickupLocation,
                @DropLocation,
                @PickupLat,
                @PickupLng,
                @DropLat,
                @DropLng,
                @DropoffDate,
                @DropoffTime,
                @CarBrand,
                @CarModel,
                @CarType,
                @Transmission,
                @FullName,
                @MobileNo,
                @EmailId,'Pending'
            )", con);

                cmd.Parameters.AddWithValue("@TripType", model.TripType ?? "");
                cmd.Parameters.AddWithValue("@PickupDate", model.PickupDate ?? "");
                cmd.Parameters.AddWithValue("@PickupTime", model.PickupTime ?? "");
                cmd.Parameters.AddWithValue("@PickupLocation", model.PickupLocation ?? "");
                cmd.Parameters.AddWithValue("@DropLocation", model.DropLocation ?? "");
                cmd.Parameters.AddWithValue("@PickupLat", model.PickupLat ?? "");
                cmd.Parameters.AddWithValue("@PickupLng", model.PickupLng ?? "");
                cmd.Parameters.AddWithValue("@DropLat", model.DropLat ?? "");
                cmd.Parameters.AddWithValue("@DropLng", model.DropLng ?? "");
                cmd.Parameters.AddWithValue("@DropoffDate", model.DropoffDate ?? "");
                cmd.Parameters.AddWithValue("@DropoffTime", model.DropoffTime ?? "");
                cmd.Parameters.AddWithValue("@CarBrand", model.CarBrand ?? "");
                cmd.Parameters.AddWithValue("@CarModel", model.CarModel ?? "");
                cmd.Parameters.AddWithValue("@CarType", model.CarType ?? "");
                cmd.Parameters.AddWithValue("@Transmission", model.Transmission ?? "");
                cmd.Parameters.AddWithValue("@FullName", model.FullName ?? "");
                cmd.Parameters.AddWithValue("@MobileNo", model.MobileNo ?? "");
                cmd.Parameters.AddWithValue("@EmailId", model.EmailId ?? "");

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }

        public List<DriverBooking> GetDriverBookings()
        {
            DataTable dt = db.GetData("SELECT * FROM DriverBooking WHERE IsDeleted=0 ORDER BY Id DESC");
            List<DriverBooking> list = new List<DriverBooking>();

            foreach (DataRow row in dt.Rows)
            {
                list.Add(new DriverBooking
                {
                    Id = Convert.ToInt32(row["Id"]),
                    TripType = row["TripType"].ToString(),
                    PickupDate = row["PickupDate"].ToString(),
                    PickupTime = row["PickupTime"].ToString(),
                    PickupLocation = row["PickupLocation"].ToString(),
                    DropLocation = row["DropLocation"].ToString(),
                    PickupLat = row.Table.Columns.Contains("PickupLat") ? row["PickupLat"].ToString() : "",
                    PickupLng = row.Table.Columns.Contains("PickupLng") ? row["PickupLng"].ToString() : "",
                    DropLat = row.Table.Columns.Contains("DropLat") ? row["DropLat"].ToString() : "",
                    DropLng = row.Table.Columns.Contains("DropLng") ? row["DropLng"].ToString() : "",
                    DropoffDate = row["DropoffDate"].ToString(),
                    DropoffTime = row["DropoffTime"].ToString(),
                    CarBrand = row.Table.Columns.Contains("CarBrand") ? row["CarBrand"].ToString() : "",
                    CarModel = row.Table.Columns.Contains("CarModel") ? row["CarModel"].ToString() : "",
                    CarType = row.Table.Columns.Contains("CarType") ? row["CarType"].ToString() : "",
                    Transmission = row.Table.Columns.Contains("Transmission") ? row["Transmission"].ToString() : "",
                    FullName = row["FullName"].ToString(),
                    MobileNo = row["MobileNo"].ToString(),
                    EmailId = row["EmailId"].ToString(),
                    DriverName = row.Table.Columns.Contains("DriverName") ? row["DriverName"].ToString() : "",
                    DriverMobile = row.Table.Columns.Contains("DriverMobile") ? row["DriverMobile"].ToString() : "",
                    BookingStatus = row["BookingStatus"].ToString(),
                    CreatedDate = Convert.ToDateTime(row["CreatedDate"])
                });
            }

            return list;
        }

        public void UpdateDriverBookingStatus(int id, string status)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                string query = "UPDATE DriverBooking SET BookingStatus=@Status WHERE Id=@Id";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.Parameters.AddWithValue("@Status", status);
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void UpdateDriverAssignment(int id, string driverName, string driverMobile)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                string query = "UPDATE DriverBooking SET DriverName=@DriverName, DriverMobile=@DriverMobile WHERE Id=@Id";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.Parameters.AddWithValue("@DriverName", driverName ?? "");
                    cmd.Parameters.AddWithValue("@DriverMobile", driverMobile ?? "");
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void DeleteDriverBooking(int id)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                string query = "UPDATE DriverBooking SET IsDeleted=1 WHERE Id=@Id";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void UpdateBookingInquiryStatus(int bookingId, string status)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                string query = "UPDATE BookingInquiry SET BookingStatus=@Status WHERE BookingId=@BookingId";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@BookingId", bookingId);
                    cmd.Parameters.AddWithValue("@Status", status);
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public List<BookingInquiryModel> GetBookingInquirys()
        {
            DataTable dt = db.GetData("SELECT * FROM BookingInquiry where IsDeleteId=0 ORDER BY BookingId DESC");
            List<BookingInquiryModel> list= new List<BookingInquiryModel>();

            foreach (DataRow row in dt.Rows)
            {
                list.Add(new BookingInquiryModel
                {
                    BookingId = Convert.ToInt32(row["BookingId"]),
                    FullName = row["FullName"].ToString(),
                    MobileNo = row["MobileNo"].ToString(),
                    EmailId = row["EmailId"].ToString(),
                    PickupLocation = row["PickupLocation"].ToString(),
                    PickupDate = row["PickupDate"] != DBNull.Value ? (DateTime?)Convert.ToDateTime(row["PickupDate"]) : null,
                    DropLocation = row["DropLocation"].ToString(),
                    PickupTime = row["PickupTime"].ToString(),
                    VehicleType = row["VehicleType"].ToString(),
                    Message = row["Message"].ToString(),
                    BookingStatus = row["BookingStatus"].ToString(),
                    CreatedDate = Convert.ToDateTime(row["CreatedDate"])
                });
            }


            return list;
        }
        public void InsertFeedback(Feedback model)
        {
            string query = @"INSERT INTO Feedback (Rating, Comments, SubmittedOn) VALUES (@Rating, @Comments, @SubmittedOn)";
            using (SqlConnection con = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@Rating", model.Rating);
                cmd.Parameters.AddWithValue("@Comments", (object)model.Comments ?? "");
                cmd.Parameters.AddWithValue("@SubmittedOn", model.SubmittedOn);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public List<Feedback> GetAllFeedback()
        {
            var list = new List<Feedback>();
            string query = "SELECT * FROM Feedback ORDER BY SubmittedOn DESC";
            using (SqlConnection con = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                con.Open();
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        list.Add(new Feedback
                        {
                            Id = Convert.ToInt32(dr["Id"]),
                            Rating = Convert.ToInt32(dr["Rating"]),
                            Comments = dr["Comments"].ToString(),
                            SubmittedOn = Convert.ToDateTime(dr["SubmittedOn"])
                        });
                    }
                }
            }
            return list;
        }
    }
}