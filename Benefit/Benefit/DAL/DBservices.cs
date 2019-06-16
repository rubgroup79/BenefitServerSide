using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.Data;
using System.Text;
using Benefit.Models;
using System.Dynamic;
using Newtonsoft.Json.Linq;

/// <summary>
/// DBServices is a class created by me to provides some DataBase Services
/// </summary>
public class DBservices
{
    public SqlDataAdapter da;
    public DataTable dt;

    public DBservices()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    //--------------------------------------------------------------------------------------------------
    // This method creates a connection to the database according to the connectionString name in the web.config 
    //--------------------------------------------------------------------------------------------------
    public SqlConnection connect(String conString)
    {


        string pStr = WebConfigurationManager.ConnectionStrings[conString].ConnectionString;
        SqlConnection con = new SqlConnection(pStr);
        con.Open();
        return con;
    }

    //--------------------------------------------------------------------------------------------------
    // Sign In
    //--------------------------------------------------------------------------------------------------

    public int SignInTrainee(Trainee t)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("BenefitConnectionStringName");
        }
        catch (Exception ex)
        {
            throw (ex);
        }

        String pStr1 = BuildInsertUserCommand(t);
        cmd = CreateCommand(pStr1, con);

        try
        {
            int UserCode = Convert.ToInt32(cmd.ExecuteScalar());
            InsertSportCategories(t.SportCategories, UserCode);
            String pStr2 = BuildInsertTraineeCommand(UserCode, t);
            cmd = CreateCommand(pStr2, con);
            cmd.ExecuteNonQuery();
            return UserCode;
        }
        catch (Exception ex)
        {
            return 0;
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                con.Close();
            }
        }

    }

    public int SignInTrainer(Trainer t)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("BenefitConnectionStringName");
        }
        catch (Exception ex)
        {
            throw (ex);
        }

        String pStr = BuildInsertUserCommand(t);
        cmd = CreateCommand(pStr, con);

        try
        {
            int UserCode = Convert.ToInt32(cmd.ExecuteScalar());
            InsertSportCategories(t.SportCategories, UserCode);
            pStr = BuildInsertTrainerCommand(UserCode, t);
            cmd = CreateCommand(pStr, con);
            cmd.ExecuteNonQuery();
            return UserCode;
        }
        catch (Exception ex)
        {
            return 0;
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                con.Close();
            }
        }

    }

    public bool InsertSportCategories(List<SportCategory> SportCategories, int UserCode)
    {
        SqlConnection con;
        SqlCommand cmd;
        try
        {
            con = connect("BenefitConnectionStringName");
        }
        catch (Exception ex)
        {
            throw (ex);
        }
        try
        {
            for (int i = 0; i < SportCategories.Count(); i++)
            {
                String str = BuildInsertSportCategoriesCommand(UserCode, SportCategories[i].CategoryCode);
                cmd = CreateCommand(str, con);
                int numEffected = cmd.ExecuteNonQuery();

            }
            return true;
        }
        catch (Exception ex)
        {
            return false;
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                con.Close();
            }

        }


    }

    public string GetToken(int UserCode)
    {

        SqlConnection con = null;
        string token = "";

        try
        {
            con = connect("BenefitConnectionStringName");

            String selectSTR = "select Users.Token from Users where Users.UserCode='" + UserCode + "'";
            SqlCommand cmd = new SqlCommand(selectSTR, con);

            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            if (dr.Read())
            {

                token = Convert.ToString(dr["Token"]);

                return token;
            }
            else
            {

                return null;
            }
        }
        catch (Exception ex)
        {
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                con.Close();
            }
        }
    }
    //--------------------------------------------------------------------------------------------------
    // Login
    //--------------------------------------------------------------------------------------------------

    public bool CheckIfEmailExists(string UserEmail)
    {

        SqlConnection con = null;

        try
        {
            con = connect("BenefitConnectionStringName");

            String selectSTR = "select * from Users where Users.Email='" + UserEmail + "'";
            SqlCommand cmd = new SqlCommand(selectSTR, con);

            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            if (dr.Read())
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        catch (Exception ex)
        {
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                con.Close();
            }

        }

    }

    public Trainee CheckIfPasswordMatches(string UserEmail, string Password)
    {

        SqlConnection con = null;

        try
        {
            con = connect("BenefitConnectionStringName");

            String selectSTR = "select Users.UserCode, Users.IsTrainer from Users where Users.Email='" + UserEmail + "' and Users.Password= '" + Password + "'";
            SqlCommand cmd = new SqlCommand(selectSTR, con);

            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            Trainee t = new Trainee();
            if (dr.Read())
            {

                t.UserCode = Convert.ToInt32(dr["UserCode"]);
                t.IsTrainer = Convert.ToInt32(dr["IsTrainer"]);
                return t;
            }
            else
            {
                t.UserCode = 0;
                return t;
            }

        }
        catch (Exception ex)
        {
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                con.Close();
            }

        }

    }

    public void UpdateToken(string Token, int UserCode)
    {

        SqlConnection con = null;
        SqlCommand cmd;

        try
        {
            con = connect("BenefitConnectionStringName");
            //Get trainee's details that needed for the search
            String selectSTR = "Update Users set Token='" + Token + "' where Users.UserCode=" + UserCode;
            cmd = new SqlCommand(selectSTR, con);
            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
        }

        catch (Exception ex)
        {
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                con.Close();
            }
        }

    }

    //--------------------------------------------------------------------------------------------------
    // Home Trainee - Search
    //--------------------------------------------------------------------------------------------------

    public List<Result> SearchCoupleTraining(OnlineHistoryTrainee o)
    {
        List<Result> Partners = null;
        List<Result> Trainers = null;
        if (o.WithPartner == 1 && o.WithTrainer == 1)
        {
            Partners = SearchPartners(o);
            Trainers = SearchTrainers(o);
            Partners.AddRange(Trainers);
            return Partners;
        }
        else if (o.WithPartner == 1)
            return SearchPartners(o);
        else if (o.WithTrainer == 1)
            return SearchTrainers(o);
        else return null;
    }

    public List<Result> SearchPartners(OnlineHistoryTrainee o)
    {

        SqlConnection con = null;
        //SqlConnection con1 = null;
        SqlCommand cmd;

        try
        {
            con = connect("BenefitConnectionStringName");

            //Get trainee's details that needed for the search
            String selectSTR = "select U.Gender, U.SearchRadius, datediff(year, U.DateOfBirth, getdate()) as Age, T.PartnerGender, T.MinPartnerAge, T.MaxPartnerAge, USC.CategoryCode from Users as U inner join Trainees as T on U.UserCode = T.TraineeCode inner join UserSportCategories as USC on U.UserCode = USC.UserCode where U.UserCode = '" + o.UserCode + "'";
            cmd = new SqlCommand(selectSTR, con);
            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            List<int> scl = new List<int>();
            string search_Gender = null;
            int search_SearchRadius = 0;
            int search_Age = 0;
            string search_PartnerGender = null;
            int search_MinPartnerAge = 0;
            int search_MaxPartnerAge = 0;

            while (dr.Read())
            {
                search_Gender = Convert.ToString(dr["Gender"]);
                search_SearchRadius = Convert.ToInt32(dr["SearchRadius"]);
                search_Age = Convert.ToInt32(dr["Age"]);
                search_PartnerGender = Convert.ToString(dr["PartnerGender"]);
                search_MinPartnerAge = Convert.ToInt32(dr["MinPartnerAge"]);
                search_MaxPartnerAge = Convert.ToInt32(dr["MaxPartnerAge"]);
                int sc = Convert.ToInt32(dr["CategoryCode"]);
                scl.Add(sc);
            }

            //check if the user have active training or suggestions with result user
            List<int> ul = new List<int>();
            ul = CheckActiveResults(o.UserCode);

            //if user doesnt care of the partner's gender
            string partnerGenderStr = null;
            if (search_PartnerGender == "Both")
                partnerGenderStr = " ";
            else partnerGenderStr = "and (U.gender = '" + search_PartnerGender + "') ";

            string sportCategoriesStr = "and (USC.CategoryCode = " + scl[0];
            for (int i = 1; i < scl.Count; i++)
            {
                sportCategoriesStr += " or USC.CategoryCode = " + scl[i];
            }
            sportCategoriesStr += ") ";

            con.Close();
            con = connect("BenefitConnectionStringName");

            selectSTR = "select distinct U.UserCode, U.FirstName, U.LastName, datediff(year,  U.DateOfBirth, getdate()) as Age, U.Gender, OHT.Latitude, OHT.Longitude, OHT.StartTime, OHT.EndTime, T.PartnerGender, U.SearchRadius, U.Picture, U.IsTrainer, U.Rate   " +
               "from Users as U inner join Trainees as T on U.UserCode = T.TraineeCode " +
               "inner join OnlineHistoryTrainee as OHT on OHT.TraineeCode = U.UserCode " +
               "inner join CurrentOnlineTrainee as COT on COT.OnlineCode = OHT.OnlineCode " +
               "inner join UserSportCategories as USC on USC.UserCode= U.UserCode " +
               "where OHT.WithPartner = 1 " +
               "and (U.UserCode <> " + o.UserCode + ") " +
               "and (datediff(year, U.DateOfBirth, getdate()) between " + search_MinPartnerAge + " and " + search_MaxPartnerAge + ") " +
               "and (" + search_Age + ">= T.MinPartnerAge and " + search_Age + "<= T.MaxPartnerAge) " +
               partnerGenderStr + " " +
               "and (OHT.StartTime <= '" + o.EndTime + "' and OHT.EndTime >= '" + o.StartTime + "') " +
               sportCategoriesStr;

            cmd = new SqlCommand(selectSTR, con);
            dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            List<Result> tl = new List<Result>();
            int counter;
            while (dr.Read())
            {
                counter = 0;
                int UserCode = Convert.ToInt32(dr["UserCode"]);

                for (int i = 0; i < ul.Count; i++)
                {
                    if (UserCode == ul[i])
                    {
                        counter = 1;
                        break;
                    }
                }

                if (counter == 0)
                {
                    string result_PartnerGender = Convert.ToString(dr["PartnerGender"]);
                    double result_Longitude = Convert.ToDouble(dr["Longitude"]);
                    double result_Latitude = Convert.ToDouble(dr["Latitude"]);
                    double distance = distances(result_Latitude, result_Longitude, Convert.ToDouble(o.Latitude), Convert.ToDouble(o.Longitude), 'K');
                    int SearchRaduis = Convert.ToInt32(dr["SearchRadius"]);
                    if ((distance <= SearchRaduis + search_SearchRadius) && (result_PartnerGender == "Both" || result_PartnerGender == search_Gender))
                    {
                        Result rt = new Result();
                        rt.UserCode = Convert.ToInt32(dr["UserCode"]);
                        rt.FirstName = Convert.ToString(dr["FirstName"]);
                        rt.LastName = Convert.ToString(dr["LastName"]);
                        rt.Age = Convert.ToInt32(dr["Age"]);
                        rt.Gender = Convert.ToString(dr["Gender"]);
                        rt.Longitude = Convert.ToSingle(result_Longitude);
                        rt.Latitude = Convert.ToSingle(result_Latitude);
                        rt.StartTime = Convert.ToString(dr["StartTime"]);
                        rt.EndTime = Convert.ToString(dr["EndTime"]);
                        rt.Picture = Convert.ToString(dr["Picture"]);
                        rt.IsTrainer = Convert.ToInt32(dr["IsTrainer"]);
                        rt.Distance = distance;
                        rt.Rate = Convert.ToSingle(dr["Rate"]);
                        tl.Add(rt);

                    }

                }

            }

            return tl;
        }

        catch (Exception ex)
        {
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                con.Close();
            }
        }

    }

    public List<Result> SearchTrainers(OnlineHistoryTrainee o)
    {

        SqlConnection con = null;
        SqlCommand cmd;

        try
        {
            con = connect("BenefitConnectionStringName");

            //Get trainee's details that needed for the search
            String selectSTR = "select U.Gender, U.SearchRadius, datediff(year, U.DateOfBirth, getdate()) as Age, T.MaxBudget, T.TrainerGender, USC.CategoryCode from Users as U inner join Trainees as T on U.UserCode = T.TraineeCode inner join UserSportCategories as USC on U.UserCode = USC.UserCode where U.UserCode = '" + o.UserCode + "'";
            cmd = new SqlCommand(selectSTR, con);
            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            List<int> scl = new List<int>();
            string search_Gender = null;
            int search_SearchRadius = 0;
            int search_Age = 0;
            int search_MaxBudget = 0;
            string search_TrainerGender = null;

            while (dr.Read())
            {
                search_Gender = Convert.ToString(dr["Gender"]);
                search_SearchRadius = Convert.ToInt32(dr["SearchRadius"]);
                search_Age = Convert.ToInt32(dr["Age"]);
                search_MaxBudget = Convert.ToInt32(dr["MaxBudget"]);
                search_TrainerGender = Convert.ToString(dr["TrainerGender"]);
                int sc = Convert.ToInt32(dr["CategoryCode"]);
                scl.Add(sc);
            }

            //if user doesnt care of the partner's gender
            string trainerGenderStr = null;
            if (search_TrainerGender == "Both")
                trainerGenderStr = " ";
            else trainerGenderStr = " (U.gender = '" + search_TrainerGender + "') and ";

            string sportCategoriesStr = "and (USC.CategoryCode = " + scl[0];
            for (int i = 1; i < scl.Count; i++)
            {
                sportCategoriesStr += " or USC.CategoryCode = " + scl[i];
            }
            sportCategoriesStr += ") ";

            con.Close();
            con = connect("BenefitConnectionStringName");
            selectSTR = "select distinct U.UserCode, U.FirstName, U.LastName, datediff(year, U.DateOfBirth, getdate()) as Age, U.Gender, OHT.Latitude, OHT.Longitude, OHT.StartTime, OHT.EndTime, U.SearchRadius, T.PersonalTrainingPrice, U.Picture, U.IsTrainer, U.Rate " +
                "from Users as U inner join Trainers as T on U.UserCode = T.TrainerCode " +
                "inner join OnlineHistoryTrainer as OHT on OHT.TrainerCode = U.UserCode " +
                "inner join CurrentOnlineTrainer as COT on COT.OnlineCode = OHT.OnlineCode " +
                "inner join UserSportCategories as USC on USC.UserCode = U.UserCode " +
                "where " + trainerGenderStr +
                "(T.PersonalTrainingPrice <= " + search_MaxBudget + ") " +
                "and(OHT.StartTime <= '" + o.EndTime + "' and OHT.EndTime >= '" + o.StartTime + "') " +
                sportCategoriesStr;

            cmd = new SqlCommand(selectSTR, con);
            dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            List<Result> tl = new List<Result>();

            List<int> ul = new List<int>();

            ul = CheckActiveResults(o.UserCode);
            int counter;

            while (dr.Read())
            {
                counter = 0;
                int UserCode = Convert.ToInt32(dr["UserCode"]);

                for (int i = 0; i < ul.Count; i++)
                {
                    if (UserCode == ul[i])
                    {
                        counter = 1;
                        break;
                    }
                }

                if (counter == 0)
                {
                    double result_Longitude = Convert.ToDouble(dr["Longitude"]);
                    double result_Latitude = Convert.ToDouble(dr["Latitude"]);
                    double distance = distances(result_Latitude, result_Longitude, Convert.ToDouble(o.Latitude), Convert.ToDouble(o.Longitude), 'K');
                    int SearchRaduis = Convert.ToInt32(dr["SearchRadius"]);
                    if ((distance <= SearchRaduis + search_SearchRadius))
                    {
                        Result rt = new Result();
                        rt.UserCode = Convert.ToInt32(dr["UserCode"]);
                        rt.FirstName = Convert.ToString(dr["FirstName"]);
                        rt.LastName = Convert.ToString(dr["LastName"]);
                        rt.Age = Convert.ToInt32(dr["Age"]);
                        rt.Gender = Convert.ToString(dr["Gender"]);
                        rt.Longitude = Convert.ToSingle(result_Longitude);
                        rt.Latitude = Convert.ToSingle(result_Latitude);
                        rt.StartTime = Convert.ToString(dr["StartTime"]);
                        rt.EndTime = Convert.ToString(dr["EndTime"]);
                        rt.Picture = Convert.ToString(dr["Picture"]);
                        rt.Price = Convert.ToInt32(dr["PersonalTrainingPrice"]);
                        rt.IsTrainer = Convert.ToInt32(dr["IsTrainer"]);
                        rt.Distance = distance;
                        rt.Rate = Convert.ToSingle(dr["Rate"]);
                        tl.Add(rt);

                    }
                }
            }
            return tl;

        }

        catch (Exception ex)
        {
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                con.Close();
            }
        }

    }

    // מחזיר רשימה של כל היוזרים שכבר יש להם פעילות - אימון או הצעה פעילים עם המשתשמש שחיפש
    public List<int> CheckActiveResults(int UserCode)
    {
        SqlConnection con = null;
        SqlCommand cmd;
        try
        {
            con = connect("BenefitConnectionStringName");
            String selectSTR = "select U.UserCode" +
                " from Users as U" +
                " where U.UserCode in (" +
                " select case when CTS.SenderCode = " + UserCode + " then CTS.ReceiverCode when CTS.ReceiverCode = " + UserCode + " then CTS.SenderCode end as UserCode" +
                " from CoupleTraining as CT inner join CoupleTrainingSuggestions CTS on CT.SuggestionCode = CTS.SuggestionCode" +
                " where(CTS.SenderCode = " + UserCode + " OR CTS.ReceiverCode = " + UserCode + ")" +
                " and CT.StatusCode = 1)" +
                " or U.UserCode in (" +
                "  select case when CTS.SenderCode = " + UserCode + " then CTS.ReceiverCode when CTS.ReceiverCode = " + UserCode + " then CTS.SenderCode end as UserCode" +
                " from CoupleTrainingSuggestions CTS" +
                " where (CTS.SenderCode = " + UserCode + " OR CTS.ReceiverCode = " + UserCode + ")" +
                " and(CTS.StatusCode = 4 OR CTS.StatusCode = 5))";

            cmd = new SqlCommand(selectSTR, con);
            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            List<int> ul = new List<int>();

            while (dr.Read())
            {
                int UserCodeRes = Convert.ToInt32(dr["UserCode"]);
                ul.Add(UserCodeRes);
            }

            return ul;
        }

        catch (Exception ex)
        {
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                con.Close();
            }
        }



    }

    public List<HistoryGroupTraining> SearchGroups(OnlineHistoryTrainee o)
    {

        SqlConnection con = null;
        SqlCommand cmd;

        try
        {
            con = connect("BenefitConnectionStringName");

            //Get trainee's details that needed for the search
            String selectSTR = "select U.SearchRadius, T.MaxBudget, USC.CategoryCode from Users as U inner join Trainees as T on U.UserCode = T.TraineeCode inner join UserSportCategories as USC on U.UserCode = USC.UserCode where U.UserCode = '" + o.UserCode + "'";
            cmd = new SqlCommand(selectSTR, con);
            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            List<int> scl = new List<int>();
            int search_SearchRadius = 0;
            int search_MaxBudget = 0;

            while (dr.Read())
            {
                search_SearchRadius = Convert.ToInt32(dr["SearchRadius"]);
                search_MaxBudget = Convert.ToInt32(dr["MaxBudget"]);
                int sc = Convert.ToInt32(dr["CategoryCode"]);
                scl.Add(sc);
            }

            string sportCategoriesStr = "and (HGT.SportCategoryCode = " + scl[0];
            for (int i = 1; i < scl.Count; i++)
            {
                sportCategoriesStr += " or HGT.SportCategoryCode = " + scl[i];
            }
            sportCategoriesStr += ") ";

            string GruopWith = null;
            if (o.GroupWithPartners == 1 && o.GroupWithTrainer == 1)
                GruopWith = " (HGT.Price <= " + search_MaxBudget + ")";
            else if (o.GroupWithPartners == 1)
                GruopWith = " HGT.WithTrainer=0 ";
            else GruopWith = " HGT.WithTrainer=1 and (HGT.Price <= " + search_MaxBudget + ")";

            con.Close();
            con = connect("BenefitConnectionStringName");
            selectSTR = "select AGT.GroupTrainingCode" +
                " from ActiveGroupTraining as AGT inner join HistoryGroupTraining as HGT on AGT.GroupTrainingCode = HGT.GroupTrainingCode" +
                " inner join GroupParticipants GP on GP.GroupTrainingCode = AGT.GroupTrainingCode" +
                " where GP.UserCode = " + o.UserCode + " and GP.StatusCode = 1 ";
            cmd = new SqlCommand(selectSTR, con);
            dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            List<int> gl = new List<int>();

            while (dr.Read())
            {
                int GroupCode = Convert.ToInt32(dr["GroupTrainingCode"]);
                gl.Add(GroupCode);
            }

            con.Close();
            con = connect("BenefitConnectionStringName");

            selectSTR = "select distinct AGT.GroupTrainingCode, HGT.Latitude, HGT.Longitude, HGT.TrainingTime, HGT.MaxParticipants, HGT.CurrentParticipants, HGT.SportCategoryCode,SC.Description, HGT.Price, HGT.WithTrainer, HGT.CreatorCode " +
                " from HistoryGroupTraining as HGT inner join ActiveGroupTraining as AGT on HGT.GroupTrainingCode = AGT.GroupTrainingCode " +
                " inner join SportCategories as SC on SC.CategoryCode=HGT.SportCategoryCode" +
                " where " + GruopWith +
                " and( HGT.TrainingTime between '" + o.StartTime + "' and '" + o.EndTime + "') " +
                " and HGT.StatusCode<>'3' " +
                sportCategoriesStr;

            cmd = new SqlCommand(selectSTR, con);
            dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            List<HistoryGroupTraining> gtl = new List<HistoryGroupTraining>();

            int counter;

            while (dr.Read())
            {
                counter = 0;
                int GroupCode = Convert.ToInt32(dr["GroupTrainingCode"]);

                for (int i = 0; i < gl.Count; i++)
                {
                    if (GroupCode == gl[i])
                    {
                        counter = 1;
                        break;
                    }
                }

                if (counter == 0)
                {

                    double group_Longitude = Convert.ToSingle(dr["Longitude"]);
                    double group_Latitude = Convert.ToSingle(dr["Latitude"]);
                    double distance = distances(group_Latitude, group_Longitude, Convert.ToDouble(o.Latitude), Convert.ToDouble(o.Longitude), 'K');
                    if ((distance <= search_SearchRadius))
                    {
                        HistoryGroupTraining hgt = new HistoryGroupTraining();
                        hgt.Longitude = Convert.ToSingle(group_Longitude);
                        hgt.Latitude = Convert.ToSingle(group_Latitude);
                        hgt.TrainingCode = Convert.ToInt32(dr["GroupTrainingCode"]);
                        hgt.TrainingTime = Convert.ToString(dr["TrainingTime"]);
                        hgt.Price = Convert.ToInt32(dr["Price"]);
                        hgt.WithTrainer = Convert.ToInt32(dr["WithTrainer"]);
                        hgt.CreatorCode = Convert.ToInt32(dr["CreatorCode"]);
                        hgt.SportCategory = Convert.ToString(dr["Description"]);
                        hgt.Distance = distance;
                        gtl.Add(hgt);
                    }

                }


            }
            return gtl;
        }

        catch (Exception ex)
        {
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                con.Close();
            }
        }

    }

    //השאילתה מחשבת את המרחק בין 2 קואורדינטות
    private double distances(double lat1, double lon1, double lat2, double lon2, char unit)
    {
        if ((lat1 == lat2) && (lon1 == lon2))
        {
            return 0;
        }
        else
        {
            double theta = lon1 - lon2;
            double dist = Math.Sin(deg2rad(lat1)) * Math.Sin(deg2rad(lat2)) + Math.Cos(deg2rad(lat1)) * Math.Cos(deg2rad(lat2)) * Math.Cos(deg2rad(theta));
            dist = Math.Acos(dist);
            dist = rad2deg(dist);
            dist = dist * 60 * 1.1515;
            if (unit == 'K')
            {
                dist = dist * 1.609344;
            }
            else if (unit == 'N')
            {
                dist = dist * 0.8684;
            }
            return (dist);
        }
    }

    private double rad2deg(double rad)
    {
        return (rad / Math.PI * 180.0);
    }

    private double deg2rad(double deg)
    {
        return (deg * Math.PI / 180.0);
    }

    //--------------------------------------------------------------------------------------------------
    // Home Trainee && Home Trainer - Online\Offline
    //--------------------------------------------------------------------------------------------------
    
    //הפונקציה מכניסה חיפוש של מתאמן למערכת ובנוסף מבצעת את החיפושים בהתאם למה שהמשתמש הכניס
    public void InsertOnlineTrainee(OnlineHistoryTrainee o)
    {
        SqlConnection con;
        SqlCommand cmd;

        //delete all trainees/trainers/groups that are not active (end time is over)
        try
        {
            DeleteNotActive();
        }

        catch (Exception ex)
        {
            throw (ex);
        }
        try
        {
            con = connect("BenefitConnectionStringName");

            //Get onlines and check if there is one open for this trainee, if yes, delete it.
            String selectSTR = "select * from CurrentOnlineTrainee as COT inner join OnlineHistoryTrainee as OHT on COT.OnlineCode = OHT.OnlineCode where OHT.TraineeCode = '" + o.UserCode + "'";
            cmd = new SqlCommand(selectSTR, con);
            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            String selectSTR1 = "";
            while (dr.Read())
            {
                int OnlineCode = Convert.ToInt32(dr["OnlineCode"]);
                //deletes previous online (need to do the function)
                selectSTR1 += "DELETE FROM CurrentOnlineTrainee WHERE OnlineCode = " + OnlineCode + "; ";
            }
            con.Close();
            con = connect("BenefitConnectionStringName");
            cmd = new SqlCommand(selectSTR1, con);

            if (selectSTR1 != "")
                cmd.ExecuteNonQuery();

        }
        catch (Exception ex)
        {
            throw (ex);
        }
        con.Close();

        try
        {
            con = connect("BenefitConnectionStringName");
            String pStr = BuildInsertOnlineHistoryTraineeCommand(o);
            cmd = CreateCommand(pStr, con);
            int OnlineCode = Convert.ToInt32(cmd.ExecuteScalar());
            con.Close();
            con = connect("BenefitConnectionStringName");
            pStr = BuildInsertCurrentTraineeCommand(OnlineCode);
            cmd = CreateCommand(pStr, con);
            cmd.ExecuteNonQuery();

        }

        catch (Exception ex)
        {
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                con.Close();
            }
        }

    }

    //הפונקציה מכניסה מאמן פעיל למערכת
    public void InsertOnlineTrainer(OnlineHistoryTrainer o)
    {
        SqlConnection con;
        SqlCommand cmd;
        String selectSTR;
        String selectSTR1 = "";
        try
        {
            con = connect("BenefitConnectionStringName");

            //Get onlines and check if there is one open for this trainer, if yes, delete it.
             selectSTR = "select * from CurrentOnlineTrainer as COT inner join OnlineHistoryTrainer as OHT on COT.OnlineCode = OHT.OnlineCode where OHT.TrainerCode = '" + o.UserCode + "'";
            cmd = new SqlCommand(selectSTR, con);
            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
           
            
            while (dr.Read())
            {
                int OnlineCode = Convert.ToInt32(dr["OnlineCode"]);
                //deletes previous online (need to do the function)
                selectSTR1 += "DELETE FROM CurrentOnlineTrainer WHERE OnlineCode = " + OnlineCode+"; ";
               
            }
            con.Close();
            con = connect("BenefitConnectionStringName");
            cmd = new SqlCommand(selectSTR1, con);
            
            if (selectSTR1 != "")
                 cmd.ExecuteNonQuery();

        }
        catch (Exception ex)
        {
            throw (ex);
        }
        con.Close();
        
        try
        {
            con = connect("BenefitConnectionStringName");
        selectSTR = BuildInsertOnlineHistoryTrainerCommand(o);
        cmd = CreateCommand(selectSTR, con);
        
            int OnlineCode = Convert.ToInt32(cmd.ExecuteScalar());
            con.Close();
            con = connect("BenefitConnectionStringName");
            selectSTR = BuildInsertCurrentTrainerCommand(OnlineCode);
            cmd = CreateCommand(selectSTR, con);
            cmd.ExecuteNonQuery();
        }


        catch (Exception ex)
        {
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                con.Close();
            }
        }

    }

    public void GoOffline(int UserCode, int IsTrainer)
    {
        SqlConnection con = null;
        SqlCommand cmd;
        String selectSTR;

        try
        {
            con = connect("BenefitConnectionStringName");
            if (IsTrainer == 1)
            {
                selectSTR = "delete" +
              " from CurrentOnlineTrainer" +
              " where CurrentOnlineTrainer.OnlineCode in" +
              " (select OHT.OnlineCode" +
              " from OnlineHistoryTrainer as OHT inner join CurrentOnlineTrainer as COT on OHT.OnlineCode = COT.OnlineCode" +
              " where OHT.TrainerCode = " + UserCode + ")";
            }

            else
            {
                selectSTR = "delete" +
                  " from CurrentOnlineTrainee" +
                  " where CurrentOnlineTrainee.OnlineCode in" +
                  " (select OHT.OnlineCode" +
                  " from OnlineHistoryTrainee as OHT inner join CurrentOnlineTrainee as COT on OHT.OnlineCode = COT.OnlineCode" +
                  " where OHT.TraineeCode = " + UserCode + ")";
            }

            cmd = new SqlCommand(selectSTR, con);
            cmd.ExecuteNonQuery();
        }

        catch (Exception ex)
        {
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                con.Close();
            }
        }
    }

    public OnlineHistoryTrainee CheckIfTraineeOnline(int UserCode)
    {
        SqlConnection con = null;

        try
        {
            DeleteNotActive();
            con = connect("BenefitConnectionStringName");
            String selectSTR;
            //if (IsTrainer == 1)
            //{
            //    selectSTR = "select *" +
            // " from CurrentOnlineTrainer as COT inner Join OnlineHistoryTrainer as OHT on COT.OnlineCode = OHT.OnlineCode" +
            // " where OHT.TrainerCode = " + UserCode + "";
            //}

            selectSTR = "select *" +
                " from CurrentOnlineTrainee as COT inner Join OnlineHistoryTrainee as OHT on COT.OnlineCode = OHT.OnlineCode" +
                " where OHT.TraineeCode = " + UserCode + "";


            SqlCommand cmd = new SqlCommand(selectSTR, con);

            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            OnlineHistoryTrainee o = new OnlineHistoryTrainee();
            while (dr.Read())
            {
                o.OnlineCode = Convert.ToInt32(dr["OnlineCode"]);
                o.UserCode = Convert.ToInt32(dr["TraineeCode"]);
                o.Longitude = Convert.ToSingle(dr["Longitude"]);
                o.Latitude = Convert.ToSingle(dr["Latitude"]);
                o.StartTime = Convert.ToString(dr["StartTime"]);
                o.EndTime = Convert.ToString(dr["EndTime"]);
                o.WithTrainer = Convert.ToInt32(dr["WithTrainer"]);
                o.WithPartner = Convert.ToInt32(dr["WithPartner"]);
                o.GroupWithTrainer = Convert.ToInt32(dr["GroupWithTrainer"]);
                o.GroupWithPartners = Convert.ToInt32(dr["GroupWithPartners"]);
            }
            return o;

        }
        catch (Exception ex)
        {
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                con.Close();
            }
        }

    }

    public OnlineHistoryTrainer CheckIfTrainerOnline(int UserCode)
    {
        SqlConnection con = null;

        try
        {
            DeleteNotActive();
            con = connect("BenefitConnectionStringName");
            String selectSTR;
            selectSTR = "select *" +
         " from CurrentOnlineTrainer as COT inner Join OnlineHistoryTrainer as OHT on COT.OnlineCode = OHT.OnlineCode" +
         " where OHT.TrainerCode = " + UserCode + "";



            SqlCommand cmd = new SqlCommand(selectSTR, con);

            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            OnlineHistoryTrainer o = new OnlineHistoryTrainer();
            while (dr.Read())
            {
                o.OnlineCode = Convert.ToInt32(dr["OnlineCode"]);
                o.UserCode = Convert.ToInt32(dr["TrainerCode"]);
                o.Longitude = Convert.ToSingle(dr["Longitude"]);
                o.Latitude = Convert.ToSingle(dr["Latitude"]);
                o.StartTime = Convert.ToString(dr["StartTime"]);
                o.EndTime = Convert.ToString(dr["EndTime"]);
            }
            return o;

        }
        catch (Exception ex)
        {
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                con.Close();
            }
        }

    }

    //--------------------------------------------------------------------------------------------------
    // Home Trainee && Home Trainer - Get functions
    //--------------------------------------------------------------------------------------------------

    public List<HistoryGroupTraining> GetFutureGroupTrainings(int UserCode)
    {
        SqlConnection con = null;
        SqlCommand cmd;
        try
        {
            con = connect("BenefitConnectionStringName");

            String selectSTR = "select HGT.GroupTrainingCode, HGT.TrainingTime, HGT.Latitude, HGT.Longitude, HGT.WithTrainer, SC.Description as SportCategory, HGT.Price, HGT.CreatorCode, HGT.StatusCode" +
                 " from HistoryGroupTraining as HGT" +
                 " inner join GroupParticipants as GP on GP.GroupTrainingCode = HGT.GroupTrainingCode" +
                 " inner join SportCategories SC on HGT.SportCategoryCode = SC.CategoryCode" +
                 " where GP.UserCode = " + UserCode +
                 " and GP.StatusCode = 1" +
                 " and(HGT.StatusCode = 1 OR HGT.StatusCode = 2)" +
                 " and(datediff(minute, HGT.TrainingTime, getdate()) < 60)";

            cmd = new SqlCommand(selectSTR, con);
            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            List<HistoryGroupTraining> hgtl = new List<HistoryGroupTraining>();
            while (dr.Read())
            {
                HistoryGroupTraining hgt = new HistoryGroupTraining();
                hgt.TrainingCode = Convert.ToInt32(dr["GroupTrainingCode"]);
                hgt.TrainingTime = Convert.ToString(dr["TrainingTime"]);
                string _lat = Convert.ToString(dr["Latitude"]);
                hgt.Latitude = float.Parse(_lat);
                string _long = Convert.ToString(dr["Longitude"]);
                hgt.Longitude = float.Parse(_long);
                hgt.WithTrainer = Convert.ToInt32(dr["WithTrainer"]);
                hgt.SportCategory = Convert.ToString(dr["SportCategory"]);
                hgt.Price = Convert.ToInt32(dr["Price"]);
                hgt.CreatorCode = Convert.ToInt32(dr["CreatorCode"]);
                hgtl.Add(hgt);
            }

            return hgtl;
        }

        catch (Exception ex)
        {
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                con.Close();
            }
        }

    }

    public List<SuggestionResult> GetSuggestions(int UserCode, bool IsApproved)
    {
        UpdateSuggestionsStatus(UserCode);
        SqlConnection con = null;
        SqlCommand cmd;
        String selectSTR = null;

        string IsApprovedStr = null;
        if (IsApproved) IsApprovedStr = "5"; else IsApprovedStr = "4";

        try
        {
            con = connect("BenefitConnectionStringName");
            selectSTR = "select distinct CTS.SuggestionCode, CTS.ReceiverCode, CTS.StatusCode, CTS.SendingTime, U.FirstName, U.LastName, U.Gender, U.Picture, DATEDIFF(year, U.DateOfBirth, getdate()) as Age, U.IsTrainer, RES.Latitude, RES.Longitude, U.Rate, RES.Price " +
                " from CoupleTrainingSuggestions CTS inner join Users U on CTS.ReceiverCode = U.UserCode" +
                " inner join" +
                " (select OHT1.TraineeCode as UserCode, OHT1.Latitude, OHT1.Longitude , '0' as Price" +
                " from OnlineHistoryTrainee OHT1 inner join CurrentOnlineTrainee CO1 on CO1.OnlineCode = OHT1.OnlineCode" +
                " UNION" +
                " select OHT1.TrainerCode as UserCode, OHT1.Latitude, OHT1.Longitude, T.PersonalTrainingPrice as Price" +
                " from OnlineHistoryTrainer OHT1 inner join CurrentOnlineTrainer CO1 on CO1.OnlineCode = OHT1.OnlineCode" +
                " inner join Trainers as T on T.TrainerCode = OHT1.TrainerCode" +
                " ) as RES on RES.UserCode = CTS.ReceiverCode" +
                 " where((CTS.SenderCode = " + UserCode + ") and(CTS.StatusCode = " + IsApprovedStr + ")" +
                 " and(CTS.SuggestionCode not in" +
                 " (select CT.SuggestionCode from CoupleTraining CT  )))";

            cmd = new SqlCommand(selectSTR, con);
            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            List<SuggestionResult> srl = new List<SuggestionResult>();


            while (dr.Read())
            {

                SuggestionResult sr = new SuggestionResult();
                sr.SuggestionCode = Convert.ToInt32(dr["SuggestionCode"]);
                sr.ReceiverCode = Convert.ToInt32(dr["ReceiverCode"]);
                sr.SenderCode = UserCode;
                sr.StatusCode = Convert.ToInt32(dr["StatusCode"]);
                sr.SendingTime = Convert.ToString(dr["SendingTime"]);
                sr.FirstName = Convert.ToString(dr["FirstName"]);
                sr.LastName = Convert.ToString(dr["LastName"]);
                sr.Gender = Convert.ToString(dr["Gender"]);
                sr.Age = Convert.ToInt32(dr["Age"]);
                sr.Picture = Convert.ToString(dr["Picture"]);
                sr.IsTrainer = Convert.ToBoolean(dr["IsTrainer"]);
                sr.Latitude = Convert.ToSingle(dr["Latitude"]);
                sr.Longitude = Convert.ToSingle(dr["Longitude"]);
                sr.Rate = Convert.ToSingle(dr["Rate"]);
                sr.Price = Convert.ToInt32(dr["Price"]);
                srl.Add(sr);
            }
            con.Close();
            con = connect("BenefitConnectionStringName");

            selectSTR = "select distinct CTS.SuggestionCode, CTS.SenderCode, CTS.StatusCode, CTS.SendingTime, U.FirstName, U.LastName, U.Gender, U.Picture, DATEDIFF(year, U.DateOfBirth, getdate()) as Age, U.IsTrainer, OHT.Latitude, OHT.Longitude, U.Rate" +
                " from CoupleTrainingSuggestions CTS inner" +
                " join Users U on CTS.SenderCode = U.UserCode" +
                " inner join OnlineHistoryTrainee OHT on OHT.TraineeCode = CTS.SenderCode " +
                " inner join CurrentOnlineTrainee CO on CO.OnlineCode = OHT.OnlineCode" +
                " where((CTS.ReceiverCode = " + UserCode + ") and(CTS.StatusCode = " + IsApprovedStr + ")" +
                 " and(CTS.SuggestionCode not in" +
                 " (select CT.SuggestionCode from CoupleTraining CT  )))";

            cmd = new SqlCommand(selectSTR, con);
            dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            while (dr.Read())
            {
                SuggestionResult sr = new SuggestionResult();
                sr.SuggestionCode = Convert.ToInt32(dr["SuggestionCode"]);
                sr.ReceiverCode = UserCode;
                sr.SenderCode = Convert.ToInt32(dr["SenderCode"]);
                sr.StatusCode = Convert.ToInt32(dr["StatusCode"]);
                sr.SendingTime = Convert.ToString(dr["SendingTime"]);
                sr.FirstName = Convert.ToString(dr["FirstName"]);
                sr.LastName = Convert.ToString(dr["LastName"]);
                sr.Gender = Convert.ToString(dr["Gender"]);
                sr.Age = Convert.ToInt32(dr["Age"]);
                sr.Picture = Convert.ToString(dr["Picture"]);
                sr.IsTrainer = Convert.ToBoolean(dr["IsTrainer"]);
                sr.Latitude = Convert.ToSingle(dr["Latitude"]);
                sr.Longitude = Convert.ToSingle(dr["Longitude"]);
                sr.Rate = Convert.ToSingle(dr["Rate"]);

                srl.Add(sr);
            }
            return srl;
        }

        catch (Exception ex)
        {
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                con.Close();
            }
        }

    }

    //--------------------------------------------------------------------------------------------------
    // Groups
    //--------------------------------------------------------------------------------------------------

    public void InsertGroupTraining(HistoryGroupTraining h)
    {
        SqlConnection con;
        SqlCommand cmd;

        con = connect("BenefitConnectionStringName");
        try
        {
            String pStr = BuildInsertHistoryGroupTrainingCommand(h);
            cmd = CreateCommand(pStr, con);
            int HistoryGroupTrainingCode = Convert.ToInt32(cmd.ExecuteScalar());
            con.Close();
            con = connect("BenefitConnectionStringName");
            pStr = BuildInsertActiveGroupTrainingCommand(HistoryGroupTrainingCode);
            cmd = CreateCommand(pStr, con);
            cmd.ExecuteNonQuery();

            JoinGroup(h.CreatorCode, HistoryGroupTrainingCode);
        }
        catch (Exception ex)
        {
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                con.Close();
            }
        }

    }

    public void JoinGroup(int UserCode, int GroupTrainingCode)
    {


        SqlConnection con;
        SqlCommand cmd;
        String pStr;
        try
        {
            con = connect("BenefitConnectionStringName");
        }
        catch (Exception ex)
        {
            throw (ex);
        }


        try
        {
            pStr = "delete GroupParticipants where UserCode=" + UserCode + " and GroupTrainingCode=" + GroupTrainingCode;
            cmd = CreateCommand(pStr, con);
            cmd.ExecuteNonQuery();
            con.Close();
            con = connect("BenefitConnectionStringName");
            pStr = BuildInsertGroupParticipantsCommand(UserCode, GroupTrainingCode);
            cmd = CreateCommand(pStr, con);
            cmd.ExecuteNonQuery();
            UpdateNumOfParticipants(1, GroupTrainingCode);
        }
        catch (Exception ex)
        {

            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                con.Close();
            }
        }

    }

    //this function gets num=1 if added a new participants, num=-1 if deleting one participant
    private void UpdateNumOfParticipants(int Num, int GroupTrainingCode)
    {

        SqlConnection con = null;
        SqlCommand cmd;

        try
        {
            con = connect("BenefitConnectionStringName");
            //update num
            String selectSTR = "Update HistoryGroupTraining set CurrentParticipants=CurrentParticipants+'" + Num + "' where GroupTrainingCode='" + GroupTrainingCode +
                "'  Update HistoryGroupTraining set StatusCode=3 where CurrentParticipants=MaxParticipants";
            cmd = new SqlCommand(selectSTR, con);
            //int CurrentParticipants = Convert.ToInt32(cmd.ExecuteScalar());
            cmd.ExecuteNonQuery();
        }

        catch (Exception ex)
        {
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                con.Close();
            }
        }
    }
    
    public List<User> CancelGroupParticipant(int GroupTrainingCode, int UserCode)
    {
        UpdateNumOfParticipants(-1, GroupTrainingCode);
        SqlConnection con = null;
        SqlCommand cmd;
        String selectSTR;
        List<User> ul = new List<User>();

        try
        {

            con = connect("BenefitConnectionStringName");
            // update status for the participant
            selectSTR = "Update GroupParticipants set StatusCode=2" +
               " where UserCode=" + UserCode + " and GroupTrainingCode= " + GroupTrainingCode +
               " Select CreatorCode from HistoryGroupTraining HGT" +
               " where GroupTrainingCode= " + GroupTrainingCode;
            cmd = new SqlCommand(selectSTR, con);
            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            while (dr.Read())   //if the user who canceled is the creator the group is canceled
            {
                if (Convert.ToInt32(dr["CreatorCode"]) == UserCode)
                {
                    ul = CancelGroupTraining(GroupTrainingCode);

                }
                else
                {
                    ul = CheckNumOfParticipants(GroupTrainingCode);
                }//check if current < min and its 15m befor the training 
            }
            return ul;

        }

        catch (Exception ex)
        {
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                con.Close();
            }
        }
    }

    public List<User> CheckNumOfParticipants(int GroupTrainingCode)
    {
        SqlConnection con = null;
        SqlCommand cmd;
        List<User> ul = new List<User>();
        try
        {
            con = connect("BenefitConnectionStringName");
            // update status for the participant and then check if current < min and its 15m befor the training 
            String selectSTR =
                " select HGT.GroupTrainingCode from HistoryGroupTraining HGT" +
                " where (HGT.GroupTrainingCode = " + GroupTrainingCode + ") and (HGT.CurrentParticipants < HGT.MinParticipants) " +
                " and (DATEDIFF(MINUTE, getdate(), HGT.TrainingTime) <= 15) and(HGT.StatusCode = 1) ";
            // if it returns a value, cancel group 
            cmd = new SqlCommand(selectSTR, con);

            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            // if the group is less then min & 15m befor training 
            if (dr.Read())
            {
                // int group = Convert.ToInt32(dr["GroupTrainingCode"]);
                return CancelGroupTraining(GroupTrainingCode);

            }
            else return ul;


        }

        catch (Exception ex)
        {
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                con.Close();
            }
        }
    }

    public HistoryGroupTraining GetGroupDetails(int GroupCode)
    {
        SqlConnection con = null;
        SqlCommand cmd;
        try
        {
            con = connect("BenefitConnectionStringName");

            String selectSTR = "select *" +
                " from HistoryGroupTraining HGT inner join SportCategories SC on SC.CategoryCode=HGT.SportCategoryCode" +
                " where HGT.GroupTrainingCode = " + GroupCode;


            cmd = new SqlCommand(selectSTR, con);
            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            HistoryGroupTraining hgt = new HistoryGroupTraining();
            while (dr.Read())
            {
                hgt.TrainingCode = Convert.ToInt32(dr["GroupTrainingCode"]);
                hgt.TrainingTime = Convert.ToString(dr["TrainingTime"]);
                hgt.Latitude = Convert.ToSingle(dr["Latitude"]);
                hgt.Longitude = Convert.ToSingle(dr["Longitude"]);
                hgt.WithTrainer = Convert.ToInt32(dr["WithTrainer"]);
                hgt.CreatorCode = Convert.ToInt32(dr["CreatorCode"]);
                hgt.MinParticipants = Convert.ToInt32(dr["MinParticipants"]);
                hgt.MaxParticipants = Convert.ToInt32(dr["MaxParticipants"]);
                hgt.CurrentParticipants = Convert.ToInt32(dr["CurrentParticipants"]);
                hgt.SportCategory = Convert.ToString(dr["Description"]);
                hgt.Price = Convert.ToInt32(dr["Price"]);

            }

            return hgt;
        }

        catch (Exception ex)
        {
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                con.Close();
            }
        }

    }

    public List<User> GetGroupParticipants(int GroupCode)
    {
        SqlConnection con = null;
        SqlCommand cmd;
        try
        {
            con = connect("BenefitConnectionStringName");

            String selectSTR = "select U.UserCode, U.FirstName, U.LastName, U.Picture, U.Token, U.IsTrainer, U.Rate, datediff(year, U.DateOfBirth, getdate()) as Age" +
                " from Users U inner join GroupParticipants GP on GP.UserCode = U.UserCode" +
                " where GP.GroupTrainingCode = " + GroupCode + " and GP.StatusCode = 1";


            cmd = new SqlCommand(selectSTR, con);
            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            List<User> ul = new List<User>();
            while (dr.Read())
            {
                User u = new User();
                u.UserCode = Convert.ToInt32(dr["UserCode"]);
                u.FirstName = Convert.ToString(dr["FirstName"]);
                u.LastName = Convert.ToString(dr["LastName"]);
                u.Picture = Convert.ToString(dr["Picture"]);
                u.IsTrainer = Convert.ToInt32(dr["IsTrainer"]);
                u.Token = Convert.ToString(dr["Token"]);
                u.Rate = Convert.ToSingle(dr["Rate"]);
                u.Age = Convert.ToInt32(dr["Age"]);
                ul.Add(u);
            }

            return ul;
        }

        catch (Exception ex)
        {
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                con.Close();
            }
        }

    }

    public List<User> CancelGroupTraining(int GroupCode)
    {

        SqlConnection con = null;
        SqlCommand cmd;
        List<User> Participants = new List<User>();
        Participants = GetGroupParticipants(GroupCode);


        try
        {
            con = connect("BenefitConnectionStringName");

            String selectSTR =
                " update HistoryGroupTraining SET StatusCode=2" + // cancel group
                " where GroupTrainingCode = " + GroupCode +
                " update GroupParticipants SET StatusCode=2" + //change all participants status to canceled
                " where GroupTrainingCode = " + GroupCode +
                " DELETE FROM ActiveGroupTraining " +  // delete from active
                " where GroupTrainingCode = " + GroupCode;

            cmd = new SqlCommand(selectSTR, con);
            cmd.ExecuteNonQuery();


            // to send push notification at client side
            return Participants;
        }

        catch (Exception ex)
        {
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                con.Close();
            }
        }
    }

 

    //--------------------------------------------------------------------------------------------------
    // Suggestions
    //--------------------------------------------------------------------------------------------------
    
    public string SendSuggestion(int SenderCode, int ReceiverCode)
    {

        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("BenefitConnectionStringName");
        }
        catch (Exception ex)
        {
            throw (ex);
        }

        try
        {
            String pStr = BuildInsertSuggestionCommand(SenderCode, ReceiverCode);
            cmd = CreateCommand(pStr, con);
            cmd.ExecuteNonQuery();
            return GetToken(ReceiverCode);

        }
        catch (Exception ex)
        {
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                con.Close();
            }
        }
    }

    public void ReplySuggestion(int SuggestionCode, bool Reply)
    {

        SqlConnection con = null;
        SqlCommand cmd;
        int StatusCode = 0;
        if (Reply == true) StatusCode = 5;
        else StatusCode = 6;

        try
        {
            con = connect("BenefitConnectionStringName");
            String selectSTR = "Update CoupleTrainingSuggestions set StatusCode='" + StatusCode + "' where SuggestionCode='" + SuggestionCode + "'";
            cmd = new SqlCommand(selectSTR, con);
            cmd.ExecuteNonQuery();
        }

        catch (Exception ex)
        {
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                con.Close();
            }
        }
    }

    public void CancelSuggestion(int SuggestionCode)
    {

        SqlConnection con = null;
        SqlCommand cmd;


        try
        {
            con = connect("BenefitConnectionStringName");
            String selectSTR = "Update CoupleTrainingSuggestions set StatusCode=2 where SuggestionCode='" + SuggestionCode + "'";
            cmd = new SqlCommand(selectSTR, con);
            cmd.ExecuteNonQuery();
        }

        catch (Exception ex)
        {
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                con.Close();
            }
        }
    }

    //קורה בכל פעם שמציגים הצעות
    //proccess
    public void UpdateSuggestionsStatus(int UserCode)
    {

        SqlConnection con = null;
        SqlCommand cmd;

        try
        {
            con = connect("BenefitConnectionStringName");

            String selectSTR = "UPDATE CoupleTrainingSuggestions " +
                "SET StatusCode = 7 " +
                "where(CoupleTrainingSuggestions.SenderCode = " + UserCode + " or CoupleTrainingSuggestions.ReceiverCode = " + UserCode + ") " +
                "and(CoupleTrainingSuggestions.SenderCode not in " +
                "(select OHT.TraineeCode " +
                "from OnlineHistoryTrainee as OHT inner join CurrentOnlineTrainee as C  on OHT.OnlineCode = C.OnlineCode) " +
                "or(CoupleTrainingSuggestions.ReceiverCode not in " +
                "(select OHT.TraineeCode from OnlineHistoryTrainee as OHT inner join CurrentOnlineTrainee as C on OHT.OnlineCode = C.OnlineCode) " +
                "and CoupleTrainingSuggestions.ReceiverCode not in " +
                "(select OHT.TrainerCode " +
                "from OnlineHistoryTrainer as OHT inner join CurrentOnlineTrainer as C on OHT.OnlineCode = C.OnlineCode)))";


            cmd = new SqlCommand(selectSTR, con);
            //int CurrentParticipants = Convert.ToInt32(cmd.ExecuteScalar());
            cmd.ExecuteNonQuery();
        }

        catch (Exception ex)
        {
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                con.Close();
            }
        }
    }

    // קורה בשליחת הצעה 
    //  בודקת האם יש כבר הצעה מאושרת או ממתינה לאישור או אימון קיים עבור אותם 2 משתשמשים
    // מחזירה סטרינג שמוצג למשתמש 
    public string CheckActiveSuggestions(int SenderCode, int ReceiverCode)
    {
        SqlConnection con = null;
        SqlCommand cmd;
        String selectSTR = null;
        string CheckStr = "Suggestion Sent!";

        try
        {
            con = connect("BenefitConnectionStringName");


            selectSTR = "select CTS.SenderCode,CTS.ReceiverCode,CTS.StatusCode" +
                " from CoupleTrainingSuggestions as CTS" +
                " where ((CTS.SenderCode =" + SenderCode + " and CTS.ReceiverCode = " + ReceiverCode + ") or" +
                " (CTS.SenderCode = " + ReceiverCode + " and CTS.ReceiverCode = " + SenderCode + ")) and" +
                " (CTS.StatusCode = 4 or CTS.StatusCode = 5)";
            cmd = new SqlCommand(selectSTR, con);
            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            while (dr.Read())
            {
                int Sender = Convert.ToInt32(dr["SenderCode"]);
                int Receiver = Convert.ToInt32(dr["ReceiverCode"]);
                int StatusCode = Convert.ToInt32(dr["StatusCode"]);
                if (Sender == SenderCode && StatusCode == 4) return "You have already sent this user a suggestion. please wait for his response :)";
                if (Sender == ReceiverCode && StatusCode == 4) return "This user already sent you a suggestion, you just need to approve it";
                if (StatusCode == 5) return "You already have an approved suggestion! all you have to do is to chat";
            }
            con.Close();
            con = connect("BenefitConnectionStringName");
            selectSTR = "select CTS.SuggestionCode" +
                " from CoupleTrainingSuggestions as CTS inner join CoupleTraining as CT" +
                " on CTS.SuggestionCode = CT.SuggestionCode" +
                " where ((CTS.SenderCode =" + SenderCode + " and CTS.ReceiverCode = " + ReceiverCode + ") or" +
                " (CTS.SenderCode = " + ReceiverCode + " and CTS.ReceiverCode = " + SenderCode + ")) and" +
                " CT.StatusCode = 1";
            cmd = new SqlCommand(selectSTR, con);
            dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            while (dr.Read())
            {

                return "You already have a training with this user :)";

            }
            SendSuggestion(SenderCode, ReceiverCode);
            return CheckStr;
        }


        catch (Exception ex)
        {
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                con.Close();
            }
        }

    }

    public int GetSuggestionCode(int UserCode1, int UserCode2)
    {
        SqlConnection con = null;
        SqlCommand cmd;
        try
        {
            con = connect("BenefitConnectionStringName");

            String selectSTR = "select CTS.SuggestionCode, CT.CoupleTrainingCode" +
                " from CoupleTrainingSuggestions CTS left outer join CoupleTraining CT on CT.SuggestionCode = CTS.SuggestionCode" +
                " where((CTS.SenderCode = " + UserCode1 + " and CTS.ReceiverCode =  " + UserCode2 + ") OR(CTS.SenderCode = " + UserCode2 + " and CTS.ReceiverCode =" + UserCode1 + "))" +
                " and(CTS.StatusCode = 5)";

            cmd = new SqlCommand(selectSTR, con);
            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            int SuggestionCode = 0;
            while (dr.Read())
            {
                string CoupleTrainingCode = Convert.ToString(dr["CoupleTrainingCode"]);
                if (CoupleTrainingCode == "")
                    SuggestionCode = Convert.ToInt32(dr["SuggestionCode"]);
            }

            return SuggestionCode;
        }

        catch (Exception ex)
        {
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                con.Close();
            }
        }

    }

    //--------------------------------------------------------------------------------------------------
    // Couple Training
    //--------------------------------------------------------------------------------------------------

    public void InsertCoupleTraining(CoupleTraining ct)
    {
        SqlConnection con;
        SqlCommand cmd;
        String pStr;

        con = connect("BenefitConnectionStringName");
        try
        {
            if (ct.WithTrainer == 1)
            {
                pStr = "select T.PersonalTrainingPrice" +
                      " from Trainers T inner join CoupleTrainingSuggestions CTS" +
                      " on CTS.ReceiverCode = T.TrainerCode" +
                      " Where CTS.SuggestionCode = " + ct.SuggestionCode;

                cmd = new SqlCommand(pStr, con);
                SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);


                while (dr.Read())
                {
                    ct.Price = Convert.ToInt32(dr["PersonalTrainingPrice"]);
                }
                con.Close();
            }

            pStr = BuildInsertCoupleTrainingCommand(ct);
            cmd = CreateCommand(pStr, con);
            cmd.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                con.Close();
            }
        }

    }

    public int CancelCoupleTraining(int CoupleTrainingCode, int UserCode)
    {

        SqlConnection con = null;
        SqlCommand cmd;
        int PartnerCode = 0;


        try
        {
            con = connect("BenefitConnectionStringName");
            String selectSTR = "Update CoupleTraining set StatusCode=2 where CoupleTrainingCode='" + CoupleTrainingCode + "'" +
                " select CTS.SuggestionCode, case when CTS.SenderCode = " + UserCode + " then CTS.ReceiverCode when CTS.ReceiverCode = " + UserCode + " then CTS.SenderCode end as PartnerCode" +
                " from CoupleTraining as CT inner join CoupleTrainingSuggestions as CTS on CT.SuggestionCode = CTS.SuggestionCode" +
                " Where CT.CoupleTrainingCode = " + CoupleTrainingCode;
            cmd = new SqlCommand(selectSTR, con);
            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            
            int SuggestionCode=0;
            while (dr.Read())
            {
                SuggestionCode = Convert.ToInt32(dr["SuggestionCode"]);
                PartnerCode = Convert.ToInt32(dr["PartnerCode"]);
            }
            con.Close();
            con = connect("BenefitConnectionStringName");
            selectSTR = "Update CoupleTrainingSuggestions set StatusCode=2 where SuggestionCode=" + SuggestionCode;
            cmd = new SqlCommand(selectSTR, con);
            cmd.ExecuteNonQuery();
            return PartnerCode;
        }

        catch (Exception ex)
        {
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                con.Close();
            }
        }
    }

    public List<CoupleTraining> GetFutureCoupleTrainings(int UserCode)
    {
        SqlConnection con = null;
        SqlCommand cmd;
        try
        {
            con = connect("BenefitConnectionStringName");

            String selectSTR = "select  CT.CoupleTrainingCode, CT.Latitude,CT.Longitude,CT.TrainingTime,CT.WithTrainer, case when CTS.SenderCode = " + UserCode + " then CTS.ReceiverCode when CTS.ReceiverCode = " + UserCode + " then CTS.SenderCode end as PartnerCode, DATEDIFF(year, U.DateOfBirth, getdate()) as Age, U.FirstName, U.LastName, U.Picture, U.Rate, CT.Price, CT.StatusCode" +
                " from CoupleTraining as CT inner join CoupleTrainingSuggestions AS CTS on CT.SuggestionCode = CTS.SuggestionCode" +
                " inner join Users as U on U.UserCode = case when CTS.SenderCode = " + UserCode + " then CTS.ReceiverCode when CTS.ReceiverCode = " + UserCode + " then CTS.SenderCode end" +
                " where(CTS.SenderCode = " + UserCode + " or CTS.ReceiverCode = " + UserCode + ") and(datediff(minute, CT.TrainingTime, getdate())  < 60) and (CT.StatusCode = 1 or CT.StatusCode=2)";

            cmd = new SqlCommand(selectSTR, con);
            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            List<CoupleTraining> ctl = new List<CoupleTraining>();
            while (dr.Read())
            {
                CoupleTraining c = new CoupleTraining();
                c.TrainingCode = Convert.ToInt32(dr["CoupleTrainingCode"]);
                c.TrainingTime = Convert.ToString(dr["TrainingTime"]);
                string _lat = Convert.ToString(dr["Latitude"]);
                c.Latitude = float.Parse(_lat);
                string _long = Convert.ToString(dr["Longitude"]);
                c.Longitude = float.Parse(_long);
                c.WithTrainer = Convert.ToInt32(dr["WithTrainer"]);
                c.PartnerUserCode = Convert.ToInt32(dr["PartnerCode"]);
                c.PartnerFirstName = Convert.ToString(dr["FirstName"]);
                c.PartnerLastName = Convert.ToString(dr["LastName"]);
                c.PartnerAge = Convert.ToInt32(dr["Age"]);
                c.PartnerPicture = Convert.ToString(dr["Picture"]);
                c.Rate = Convert.ToSingle(dr["Rate"]);
                if (c.WithTrainer == 1) c.Price = Convert.ToInt32(dr["Price"]);
                c.StatusCode = Convert.ToInt32(dr["StatusCode"]);

                ctl.Add(c);
            }

            return ctl;
        }

        catch (Exception ex)
        {
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                con.Close();
            }
        }

    }

    public bool checkForCloseTrainings(int UserCode, string TrainingTime)
    {
        SqlConnection con = null;
        SqlCommand cmd;
        try
        {
            con = connect("BenefitConnectionStringName");

            String selectSTR = "select *" +
                " from CoupleTraining as CT inner join CoupleTrainingSuggestions AS CTS on CT.SuggestionCode = CTS.SuggestionCode" +
                " inner join Users as U on U.UserCode = case when CTS.SenderCode = " + UserCode + " then CTS.ReceiverCode when CTS.ReceiverCode = " + UserCode + " then CTS.SenderCode end" +
                " where(CTS.SenderCode = " + UserCode + " or CTS.ReceiverCode = " + UserCode + ") and(datediff(minute, CT.TrainingTime, '" + TrainingTime + "') between - 60 and 60) and(CT.StatusCode = 1)";

            cmd = new SqlCommand(selectSTR, con);
            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            if (dr.Read())
            {
                return true;
            }
            else
            {
                con.Close();
                con = connect("BenefitConnectionStringName");
                selectSTR = "select *" +
                    " from HistoryGroupTraining as HGT" +
                    " inner join GroupParticipants as GP on GP.GroupTrainingCode = HGT.GroupTrainingCode" +
                    " inner join SportCategories SC on HGT.SportCategoryCode = SC.CategoryCode" +
                    " where GP.UserCode = " + UserCode +
                    " and GP.StatusCode = 1" +
                    " and(HGT.StatusCode = 1)" +
                    " and(datediff(minute, HGT.TrainingTime, '" + TrainingTime + "') BETWEEN - 60 and 60)";

                cmd = new SqlCommand(selectSTR, con);
                dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                if (dr.Read())
                    return true;
                else return false;
            }

        }

        catch (Exception ex)
        {
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                con.Close();
            }
        }

    }

    //--------------------------------------------------------------------------------------------------
    // Profile
    //--------------------------------------------------------------------------------------------------

    public User ShowProfile(int UserCode)

    {
        SqlConnection con = null;
        SqlCommand cmd;
        try

        {
            con = connect("BenefitConnectionStringName");
            String selectSTR = "select U.UserCode, U.FirstName, U.LastName, U.Gender, datediff(year, U.DateOfBirth, getdate()) as Age, U.Picture , U.Rate, U.IsTrainer" +
                " from Users as U" +
                " where U.UserCode = " + UserCode;

            cmd = new SqlCommand(selectSTR, con);
            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            User u = new Trainee();

            if (dr.Read())
            {
                u.UserCode = Convert.ToInt32(dr["UserCode"]);
                u.FirstName = Convert.ToString(dr["FirstName"]);
                u.LastName = Convert.ToString(dr["LastName"]);
                u.Gender = Convert.ToString(dr["Gender"]);
                u.Picture = Convert.ToString(dr["Picture"]);
                u.Age = Convert.ToInt32(dr["Age"]);
                u.Rate = Convert.ToSingle(dr["Rate"]);
                u.IsTrainer = Convert.ToInt32(dr["IsTrainer"]);

            }

            selectSTR = "select SC.Description" +
                " from UserSportCategories as USC inner join SportCategories as SC on USC.CategoryCode = SC.CategoryCode" +
                " where UserCode = " + UserCode;
            con.Close();
            con = connect("BenefitConnectionStringName");
            cmd = new SqlCommand(selectSTR, con);
            dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            List<SportCategory> scl = new List<SportCategory>();
            while (dr.Read())
            {
                SportCategory sc = new SportCategory();
                sc.Description = Convert.ToString(dr["Description"]);
                scl.Add(sc);
            }
            u.SportCategories = scl;
            return u;
        }

        catch (Exception ex)
        {
            throw (ex);
        }


        finally
        {
            if (con != null)
            {
                con.Close();
            }
        }

    }

    public TraineeDetails GetTraineeProfileDetails(int UserCode)
    {

        SqlConnection con = null;

        try
        {
            con = connect("BenefitConnectionStringName");

            String selectSTR = "select U.FirstName , U.LastName, U.Email, datediff(year, U.DateOfBirth, getdate()) as Age, U.Picture, U.Rate, U.SearchRadius, T.MaxBudget,T.PartnerGender, T.TrainerGender, T.MinPartnerAge, T.MaxPartnerAge" +
                " from Users as U inner join Trainees as T on U.UserCode = T.TraineeCode" +
                " where U.UserCode =" + UserCode;
            SqlCommand cmd = new SqlCommand(selectSTR, con);
            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            TraineeDetails td = new TraineeDetails();

            while (dr.Read())
            {
                td.FirstName = Convert.ToString(dr["FirstName"]);
                td.LastName = Convert.ToString(dr["LastName"]);
                td.Email = Convert.ToString(dr["Email"]);
                td.Age = Convert.ToInt32(dr["Age"]);
                td.Picture = Convert.ToString(dr["Picture"]);
                td.Rate = Convert.ToSingle(dr["Rate"]);
                td.SearchRadius = Convert.ToInt32(dr["SearchRadius"]);
                td.MaxBudget = Convert.ToInt32(dr["MaxBudget"]);
                td.PartnerGender = Convert.ToString(dr["PartnerGender"]);
                td.TrainerGender = Convert.ToString(dr["TrainerGender"]);
                td.MinPartnerAge = Convert.ToInt32(dr["MinPartnerAge"]);
                td.MaxPartnerAge = Convert.ToInt32(dr["MaxPartnerAge"]);
            }

            td.SportCategories = GetUserSportCategories(UserCode);
            return td;
        }
        catch (Exception ex)
        {
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                con.Close();
            }
        }
    }

    public void UpdateTraineeDetails(TraineeDetails td)
    {

        SqlConnection con = null;

        try
        {
            con = connect("BenefitConnectionStringName");

            String selectSTR = "UPDATE Users" +
                " SET Picture = '" + td.Picture + "', SearchRadius = " + td.SearchRadius + "" +
                " WHERE UserCode = " + td.TraineeCode + ";" +
                " UPDATE Trainees" +
                " SET MaxBudget = " + td.MaxBudget + ", PartnerGender = '" + td.PartnerGender + "', TrainerGender = '" + td.TrainerGender + "', MinPartnerAge = " + td.MinPartnerAge + ", MaxPartnerAge = " + td.MaxPartnerAge +
                " WHERE TraineeCode = " + td.TraineeCode + ";" +
                " DELETE FROM UserSportCategories WHERE UserCode = " + td.TraineeCode + "";
            SqlCommand cmd = new SqlCommand(selectSTR, con);
            cmd.ExecuteNonQuery();
            InsertSportCategories(td.SportCategories, td.TraineeCode);

        }
        catch (Exception ex)
        {
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                con.Close();
            }
        }
    }

    public void UpdateTrainerDetails(TrainerDetails td)
    {

        SqlConnection con = null;

        try
        {
            con = connect("BenefitConnectionStringName");

            String selectSTR = "UPDATE Users" +
                " SET Picture = '" + td.Picture + "', SearchRadius = " + td.SearchRadius + "" +
                " WHERE UserCode = " + td.TrainerCode + ";" +
                " UPDATE Trainers" +
                " SET PersonalTrainingPrice = " + td.PersonalTrainingPrice +
                " WHERE TrainerCode = " + td.TrainerCode + ";" +
                " DELETE FROM UserSportCategories WHERE UserCode = " + td.TrainerCode + "";
            SqlCommand cmd = new SqlCommand(selectSTR, con);
            cmd.ExecuteNonQuery();
            InsertSportCategories(td.SportCategories, td.TrainerCode);

        }
        catch (Exception ex)
        {
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                con.Close();
            }
        }
    }
    
    public List<SportCategory> GetUserSportCategories(int UserCode)
    {

        SqlConnection con = null;

        try
        {
            con = connect("BenefitConnectionStringName");

            String selectSTR = "select SC.CategoryCode, SC.Description" +
                " from UserSportCategories as USC inner join SportCategories as SC on SC.CategoryCode = USC.CategoryCode" +
                " where UserCode = " + UserCode;

            SqlCommand cmd = new SqlCommand(selectSTR, con);

            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            List<SportCategory> scl = new List<SportCategory>();
            while (dr.Read())
            {
                SportCategory sc = new SportCategory();
                sc.CategoryCode = Convert.ToInt32(dr["CategoryCode"]);
                sc.Description = Convert.ToString(dr["Description"]);
                scl.Add(sc);

            }

            return scl;
        }
        catch (Exception ex)
        {
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                con.Close();
            }
        }
    }

    public List<SportCategory> GetSportCategories()
    {
        SqlConnection con = null;

        try
        {
            con = connect("BenefitConnectionStringName");

            String selectSTR = "select * from SportCategories";
            SqlCommand cmd = new SqlCommand(selectSTR, con);

            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            List<SportCategory> scl = new List<SportCategory>();
            while (dr.Read())
            {
                SportCategory sc = new SportCategory();
                sc.CategoryCode = Convert.ToInt32(dr["CategoryCode"]);
                sc.Description = Convert.ToString(dr["Description"]);
                scl.Add(sc);
            }
            return scl;

        }

        catch (Exception ex)
        {
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                con.Close();
            }
        }
    }


    //--------------------------------------------------------------------------------------------------
    // Rates
    //--------------------------------------------------------------------------------------------------
    
    public List<AverageRateParameters> GetAvarageParametersRate(int UserCode)
    {
        SqlConnection con = null;

        try
        {
            con = connect("BenefitConnectionStringName");

            String selectSTR = "select PR.ParameterCode,  RP.Description, AVG(PR.Rate) as 'AverageRate'" +
                " from Ratings as R inner join ParametersRate as PR ON R.RatingCode = PR.RatingCode" +
                " inner join RateParameters as RP ON RP.ParameterCode = PR.ParameterCode" +
                " where R.RatedCode = " + UserCode +
                " group by PR.ParameterCode , RP.Description";

            SqlCommand cmd = new SqlCommand(selectSTR, con);

            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            List<AverageRateParameters> arpl = new List<AverageRateParameters>();
            while (dr.Read())
            {
                AverageRateParameters arp = new AverageRateParameters();
                RateParameter rateParameter = new RateParameter();
                rateParameter.ParameterCode = Convert.ToInt32(dr["ParameterCode"]);
                rateParameter.Description = Convert.ToString(dr["Description"]);
                arp.Parameter = rateParameter;
                arp.AverageRate = Convert.ToSingle(dr["AverageRate"]);
                arpl.Add(arp);
            }
            return arpl;

        }

        catch (Exception ex)
        {
            throw (ex);
        }
        finally
        {
            if (con != null)
            {
                con.Close();
            }
        }
    }
    
    public int InsertNewRating(Rating r)
    {
        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("BenefitConnectionStringName");
        }
        catch (Exception ex)
        {
            throw (ex);
        }

        String pStr = BuildInsertRatingCommand(r);
        cmd = CreateCommand(pStr, con);

        try
        {
            int RatingCode = Convert.ToInt32(cmd.ExecuteScalar());
            UpdateUserRate(r.RatedCode);
            return RatingCode;
        }
        catch (Exception ex)
        {
            return 0;
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                con.Close();
            }
        }

    }

    public void UpdateExistingParametersRate(ParameterRate pr)
    {
        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("BenefitConnectionStringName");
        }
        catch (Exception ex)
        {
            throw (ex);
        }

        String pStr = "UPDATE ParametersRate" +
            " SET Rate = " + pr.Rate +
            " WHERE RatingCode = " + pr.RatingCode + " and ParameterCode = " + pr.ParameterCode;

        try
        {
            cmd = CreateCommand(pStr, con);
            cmd.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                con.Close();
            }
        }

    }

    public void UpdateExistingAvarageRate(Rating r)
    {
        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("BenefitConnectionStringName");
        }
        catch (Exception ex)
        {
            throw (ex);
        }

        String pStr = "UPDATE Ratings" +
            " SET AvgTotalRate = " + r.AvgTotalRate +
            " WHERE RatingCode = " + r.RatingCode;

        try
        {
            cmd = CreateCommand(pStr, con);
            cmd.ExecuteNonQuery();
            UpdateUserRate(r.RatedCode);
        }
        catch (Exception ex)
        {
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                con.Close();
            }
        }

    }

    public void UpdateUserRate(int RatedCode)
    {
        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("BenefitConnectionStringName");
        }
        catch (Exception ex)
        {
            throw (ex);
        }

        String pStr = "select AVG(R.AvgTotalRate) as 'Rate'" +
            " from Ratings R" +
            " where R.RatedCode = " + RatedCode;

        cmd = CreateCommand(pStr, con);
        SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

        float newRate = 0;
        while (dr.Read())
        {
            newRate = Convert.ToSingle(dr["Rate"]);
        }
        con.Close();

        
        try
        {
            con = connect("BenefitConnectionStringName");
            pStr = "UPDATE Users" +
            " SET Rate =" + newRate +
            " WHERE Users.UserCode =" + RatedCode;

        cmd = CreateCommand(pStr, con);
            cmd.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                con.Close();
            }
        }

    }

    public void InsertParametersRate(ParameterRate pr)
    {
        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("BenefitConnectionStringName");
        }
        catch (Exception ex)
        {
            throw (ex);
        }

        String pStr = BuildInsertParameterRateCommand(pr);
        cmd = CreateCommand(pStr, con);

        try
        {
            cmd.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                con.Close();
            }
        }

    }

    public Rating CheckIfRateExists(int UserCode, int RatedUserCode)
    {
        SqlConnection con;
        SqlCommand cmd;

        try
        {
            con = connect("BenefitConnectionStringName");
        }
        catch (Exception ex)
        {
            throw (ex);
        }

        String pStr = "select R.AvgTotalRate, R.RatingCode" +
            " from Ratings as R" +
            " where R.TraineeCode = " + UserCode + " and R.RatedCode = " + RatedUserCode;
        cmd = CreateCommand(pStr, con);

        try
        {
            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            Rating r = new Rating();
            while (dr.Read())
            {
                r.AvgTotalRate = Convert.ToSingle(dr["AvgTotalRate"]);
                r.RatingCode = Convert.ToInt32(dr["RatingCode"]);
            }

            return r;
        }
        catch (Exception ex)
        {
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                con.Close();
            }
        }

    }

    //--------------------------------------------------------------------------------------------------
    // Chat
    //--------------------------------------------------------------------------------------------------

    public int OpenChat(int UserCode1, int UserCode2)
    {
        SqlConnection con;
        SqlCommand cmd;
        String pStr = null;

        try
        {
            con = connect("BenefitConnectionStringName");
        }
        catch (Exception ex)
        {
            throw (ex);
        }

        pStr = "select C.ChatCode" +
            " from Chats as C" +
            " where (C.UserCode1 =" +UserCode1+ "and C.UserCode2 = " + UserCode2 + ") or(C.UserCode1 = " + UserCode2 + " and C.UserCode2 = " + UserCode1 + ")";
        cmd = CreateCommand(pStr, con);

        try
        {
            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            if (dr.Read())
            {
                return Convert.ToInt32(dr["ChatCode"]);
            }

            else
            {
                con.Close();
                con = connect("BenefitConnectionStringName");
                pStr = BuildInsertNewChat(UserCode1, UserCode2);
                cmd = CreateCommand(pStr, con);
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }
        catch (Exception ex)
        {
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                con.Close();
            }
        }
    }

    public List<Chat> GetAllChats(int UserCode)
    {
        SqlConnection con = null;
        SqlCommand cmd;
        try
        {
            con = connect("BenefitConnectionStringName");

            String selectSTR = "select C.ChatCode, U.UserCode as PartnerCode ,U.FirstName, U.LastName,U.Picture, LM.Content, LM.LatestMessageDate" +
                " from( " +
                " select M.ChatCode, Res.LatestMessageDate, M.Content" +
                " from(" +
                " select  M.ChatCode, max(M.SendingTime) as LatestMessageDate" +
                " from Messages M" +
                " group by M.ChatCode) as Res inner join Messages M" +
                " on Res.ChatCode = M.ChatCode and M.SendingTime = Res.LatestMessageDate) as LM" +
                " inner join Chats C on C.ChatCode = LM.ChatCode" +
                " inner join Users U on U.UserCode =case when C.UserCode1 = " + UserCode + " then C.UserCode2 when C.UserCode2 =" + UserCode + " then C.UserCode1 end" +
                " order by LM.LatestMessageDate desc";


            cmd = new SqlCommand(selectSTR, con);
            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            List<Chat> cl = new List<Chat>();
            while (dr.Read())
            {
                Chat c = new Chat();
                c.ChatCode = Convert.ToInt32(dr["ChatCode"]);
                c.UserCode1 = UserCode;
                c.UserCode2 = Convert.ToInt32(dr["PartnerCode"]);
                c.FirstName = Convert.ToString(dr["FirstName"]);
                c.LastName = Convert.ToString(dr["LastName"]);
                c.Picture = Convert.ToString(dr["Picture"]);
                c.LastMessage = Convert.ToString(dr["Content"]);
                cl.Add(c);
            }

            return cl;
        }

        catch (Exception ex)
        {
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                con.Close();
            }
        }

    }

    public List<Message> GetMessages(int ChatCode)
    {
        SqlConnection con = null;
        SqlCommand cmd;
        try
        {
            con = connect("BenefitConnectionStringName");

            String selectSTR = "select * from Messages M" +
                " where M.ChatCode = " + ChatCode;

            cmd = new SqlCommand(selectSTR, con);
            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            List<Message> ml = new List<Message>();
            while (dr.Read())
            {
                Message m = new Message();
                m.MessageCode = Convert.ToInt32(dr["MessageCode"]);
                m.ChatCode = Convert.ToInt32(dr["ChatCode"]);
                m.SenderCode = Convert.ToInt32(dr["SenderCode"]);
                m.SendingTime = Convert.ToString(dr["SendingTime"]);
                m.Content = Convert.ToString(dr["Content"]);
                ml.Add(m);
            }

            return ml;
        }

        catch (Exception ex)
        {
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                con.Close();
            }
        }

    }

    public void SendMessage(Message m)
    {
        SqlConnection con;
        SqlCommand cmd;


        con = connect("BenefitConnectionStringName");
        try
        {
            String pStr = BuildInsertMessageCommand(m);
            cmd = CreateCommand(pStr, con);
            cmd.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                con.Close();
            }
        }
    }

    //--------------------------------------------------------------------------------------------------
    // Get Past Trainings
    //--------------------------------------------------------------------------------------------------
    
    public List<CoupleTraining> GetPastCoupleTrainings(int UserCode)
    {

        //status code = 8 --> completed training 
        // to get canceled trainings --> status code = 2 
        SqlConnection con = null;
        SqlCommand cmd;
        try
        {
            con = connect("BenefitConnectionStringName");

            String selectSTR = "select CT.CoupleTrainingCode, CT.Latitude, CT.Longitude, U.FirstName, U.LastName, U.Picture,U.UserCode as 'PartnerCode', CT.TrainingTime, CT.WithTrainer, CT.Price" +
                " from CoupleTraining CT inner join CoupleTrainingSuggestions CTS " +
                " on CTS.SuggestionCode = CT.SuggestionCode" +
                " inner join Users U " +
                " on U.UserCode = case when CTS.SenderCode = " + UserCode + " then CTS.ReceiverCode when CTS.ReceiverCode =" + UserCode + " then CTS.SenderCode end" +
                " where CT.StatusCode = 8 and(CTS.SenderCode = " + UserCode + " or CTS.ReceiverCode = " + UserCode + ")";

            cmd = new SqlCommand(selectSTR, con);
            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            List<CoupleTraining> ctl = new List<CoupleTraining>();
            while (dr.Read())
            {
                CoupleTraining ct = new CoupleTraining();
                ct.TrainingCode = Convert.ToInt32(dr["CoupleTrainingCode"]);
                ct.TrainingTime = Convert.ToString(dr["TrainingTime"]);
                ct.Latitude = Convert.ToSingle(dr["Latitude"]);
                ct.Longitude = Convert.ToSingle(dr["Longitude"]);
                ct.PartnerUserCode = Convert.ToInt32(dr["PartnerCode"]);
                ct.PartnerFirstName = Convert.ToString(dr["FirstName"]);
                ct.PartnerLastName = Convert.ToString(dr["LastName"]);
                ct.PartnerPicture = Convert.ToString(dr["Picture"]);
                ct.WithTrainer = Convert.ToInt32(dr["WithTrainer"]);
                if (ct.WithTrainer == 1)
                    ct.Price = Convert.ToInt32(dr["Price"]);
                else ct.Price = 0;
                ctl.Add(ct);
            }

            return ctl;
        }

        catch (Exception ex)
        {
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                con.Close();
            }
        }

    }

    public List<HistoryGroupTraining> GetPastGroupTrainings(int UserCode)
    {
        //statusCode=8 -> completed training
        SqlConnection con = null;
        SqlCommand cmd;
        try
        {
            con = connect("BenefitConnectionStringName");

            String selectSTR = "select HGT.GroupTrainingCode, HGT.Latitude, HGT.Longitude, HGT.Price, S.Description as 'SportCategory', HGT.TrainingTime, HGT.WithTrainer" +
                " from HistoryGroupTraining HGT inner join GroupParticipants GP" +
                " on HGT.GroupTrainingCode = GP.GroupTrainingCode" +
                " inner join SportCategories S " +
                " on S.CategoryCode = HGT.SportCategoryCode" +
                " where GP.UserCode = " + UserCode + " and HGT.StatusCode = 8";
            cmd = new SqlCommand(selectSTR, con);
            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            List<HistoryGroupTraining> hgtl = new List<HistoryGroupTraining>();
            while (dr.Read())
            {
                HistoryGroupTraining hgt = new HistoryGroupTraining();
                hgt.TrainingCode = Convert.ToInt32(dr["GroupTrainingCode"]);
                hgt.TrainingTime = Convert.ToString(dr["TrainingTime"]);
                hgt.Latitude = Convert.ToSingle(dr["Latitude"]);
                hgt.Longitude = Convert.ToSingle(dr["Longitude"]);
                hgt.SportCategory = Convert.ToString(dr["SportCategory"]);
                hgt.WithTrainer = Convert.ToInt32(dr["WithTrainer"]);
                if (hgt.WithTrainer == 1)
                    hgt.Price = Convert.ToInt32(dr["Price"]);
                else hgt.Price = 0;
                hgtl.Add(hgt);
            }

            return hgtl;
        }

        catch (Exception ex)
        {
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                con.Close();
            }
        }

    }

    //--------------------------------------------------------------------------------------------------
    // Procceses - clean DB and change statuses
    //--------------------------------------------------------------------------------------------------

    // הפונקציה רצה על טבלת האונליין ומוחקת את מי שזמן סיום הפעילות שלו חלף
    // לעשות את זה בפרוסס
    public void DeleteNotActive()
    {

        SqlConnection con = null;

        SqlCommand cmd;

        try
        {
            con = connect("BenefitConnectionStringName");

            //Get trainee's details that needed for the search
            String selectSTR = "DELETE FROM CurrentOnlineTrainee  WHERE OnlineCode in (select OHT.OnlineCode" +
                " from OnlineHistoryTrainee as OHT inner join CurrentOnlineTrainee as CO on OHT.OnlineCode = CO.OnlineCode" +
                " where DATEDIFF(second, OHT.EndTime, getdate()) > 0)" +
                " DELETE FROM CurrentOnlineTrainer WHERE OnlineCode in (select OHT.OnlineCode" +
                " from OnlineHistoryTrainer as OHT inner join CurrentOnlineTrainer as CO on OHT.OnlineCode = CO.OnlineCode" +
                " where DATEDIFF(second, OHT.EndTime, getdate()) > 0)" +
                " DELETE FROM ActiveGroupTraining WHERE GroupTrainingCode in (select AGT.GroupTrainingCode" +
                " from ActiveGroupTraining as AGT inner join HistoryGroupTraining as HGT on AGT.GroupTrainingCode = HGT.GroupTrainingCode" +
                " where DATEDIFF(second, HGT.TrainingTime, getdate()) > 0)";

            cmd = new SqlCommand(selectSTR, con);
            cmd.ExecuteNonQuery();
            //    SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
        }

        catch (Exception ex)
        {
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                con.Close();
            }
        }
    }

    // changes status code from 1 (active) or 3 (full group) to 8 (completed) for past time
    // canceled trainings stay with status canceled (2) 
    public void UpdateTrainingsStatus()
    {

        SqlConnection con = null;
        SqlCommand cmd;

        try
        {
            con = connect("BenefitConnectionStringName");

            String selectSTR = "UPDATE CoupleTraining set StatusCode=8" +
                " where datediff(hour, TrainingTime, getdate())> 0 and StatusCode = 1" +
                "update HistoryGroupTraining set StatusCode=8" +
                " where datediff(hour, TrainingTime, getdate())> 0 and((StatusCode = 1) OR(StatusCode = 3))";

            cmd = new SqlCommand(selectSTR, con);
            cmd.ExecuteNonQuery();
        }

        catch (Exception ex)
        {
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                con.Close();
            }
        }
    }

    public void UpdateAllSuggestionsStatus()
    {

        SqlConnection con = null;
        SqlCommand cmd;

        try
        {
            con = connect("BenefitConnectionStringName");

            String selectSTR = "UPDATE CoupleTrainingSuggestions " +
                "SET StatusCode = 7 " +
                "where (CoupleTrainingSuggestions.SenderCode not in " +
                "(select OHT.TraineeCode " +
                "from OnlineHistoryTrainee as OHT inner join CurrentOnlineTrainee as C  on OHT.OnlineCode = C.OnlineCode) " +
                "or (CoupleTrainingSuggestions.ReceiverCode not in " +
                "(select OHT.TraineeCode from OnlineHistoryTrainee as OHT inner join CurrentOnlineTrainee as C on OHT.OnlineCode = C.OnlineCode) " +
                "and CoupleTrainingSuggestions.ReceiverCode not in " +
                "(select OHT.TrainerCode " +
                "from OnlineHistoryTrainer as OHT inner join CurrentOnlineTrainer as C on OHT.OnlineCode = C.OnlineCode)))";


            cmd = new SqlCommand(selectSTR, con);
            //int CurrentParticipants = Convert.ToInt32(cmd.ExecuteScalar());
            cmd.ExecuteNonQuery();
        }

        catch (Exception ex)
        {
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                con.Close();
            }
        }
    }

    // called by a proccess to check if its 15m befor training 
    //returns list of users to sent them push 
    public List<User> CheckMinParticipants_AllGroups()
    {
        SqlConnection con = null;
        SqlCommand cmd;

        try
        {
            con = connect("BenefitConnectionStringName");
            // check if current < min and its 15m befor the training 
            String selectSTR =
                " select HGT.GroupTrainingCode from HistoryGroupTraining HGT" +
                " where (HGT.CurrentParticipants < HGT.MinParticipants) " +
                " and(DATEDIFF(MINUTE, HGT.TrainingTime, getdate()) <= 15) and(HGT.StatusCode = 1) ";
            // if it returns a value, cancel group 
            cmd = new SqlCommand(selectSTR, con);

            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            // if the group is less then min & 15m befor training 
            List<User> ul = new List<User>();
            int counter = 0;
            while (dr.Read())
            {
                counter++;
                List<User> gl = new List<User>();
                int GroupTrainingCode = Convert.ToInt32(dr["GroupTrainingCode"]);
                if (counter == 1) ul = CancelGroupTraining(GroupTrainingCode);
                else gl = CancelGroupTraining(GroupTrainingCode);
                ul.AddRange(gl);
            }
            return ul;

        }

        catch (Exception ex)
        {
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                con.Close();
            }
        }
    }

    //--------------------------------------------------------------------------------------------------
    // User learning 
    //--------------------------------------------------------------------------------------------------

    public List<Trainee> GetLazyTrainees()
    {

        SqlConnection con = null;
        SqlCommand cmd;

        try
        {
            con = connect("BenefitConnectionStringName");

            //Get trainee's details that needed for the search
            String selectSTR = "SELECT U.Token, U.FirstName " +
                " FROM Trainees as T inner join Users as U on T.TraineeCode = U.UserCode" +
                " where (datediff(day, U.SignUpDate, getdate()) >= 7) AND (T.TraineeCode NOT IN" +
                " (SELECT CTS.ReceiverCode" +
                " from CoupleTraining as CT inner join CoupleTrainingSuggestions as CTS on CT.SuggestionCode = CTS.SuggestionCode" +
                " where datediff(day, CT.TrainingTime, getdate()) <= 7))" +
                " AND(T.TraineeCode NOT IN" +
                " (SELECT CTS.SenderCode" +
                " from CoupleTraining as CT inner join CoupleTrainingSuggestions as CTS on CT.SuggestionCode = CTS.SuggestionCode" +
                " where datediff(day, CT.TrainingTime, getdate()) <= 7))" +
                " AND(T.TraineeCode NOT IN" +
                " (SELECT GP.UserCode" +
                " FROM HistoryGroupTraining as HGT inner join GroupParticipants as GP on HGT.GroupTrainingCode = GP.GroupTrainingCode" +
                " where(datediff(day, HGT.TrainingTime, getdate()) <= 7)  and(HGT.StatusCode <> 2)))";
            cmd = new SqlCommand(selectSTR, con);
            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            List<Trainee> tl = new List<Trainee>();

            //****returns list with trainee code and first name only****//

            while (dr.Read())
            {
                Trainee t = new Trainee();
                t.FirstName = Convert.ToString(dr["FirstName"]);
                t.Token = Convert.ToString(dr["Token"]);


                tl.Add(t);
            }

            return tl;
        }

        catch (Exception ex)
        {
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                con.Close();
            }
        }

    }

    public List<Trainer> GetLazyTrainers()
    {

        SqlConnection con = null;
        SqlCommand cmd;

        try
        {
            con = connect("BenefitConnectionStringName");

            //Get trainee's details that needed for the search
            String selectSTR = "SELECT U.Token, U.FirstName " +
                " FROM Trainers as T inner join Users as U on T.TrainerCode = U.UserCode" +
                " where (datediff(day, U.SignUpDate, getdate()) >= 7) AND (T.TrainerCode NOT IN" +
                " (SELECT CTS.ReceiverCode" +
                " from CoupleTraining as CT inner join CoupleTrainingSuggestions as CTS on CT.SuggestionCode = CTS.SuggestionCode" +
                " where datediff(day, CT.TrainingTime, getdate()) <= 7))" +
                " AND(T.TrainerCode NOT IN" +
                " (SELECT HGT.CreatorCode" +
                " FROM HistoryGroupTraining as HGT inner join Trainers as T on HGT.CreatorCode=T.TrainerCode" +
                " where(datediff(day, HGT.TrainingTime, getdate()) <= 7) and(HGT.StatusCode <> 2))" +
                ")";

            cmd = new SqlCommand(selectSTR, con);
            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            List<Trainer> tl = new List<Trainer>();

            //****returns list with trainee code and first name only****//

            while (dr.Read())
            {
                Trainer t = new Trainer();
                t.FirstName = Convert.ToString(dr["FirstName"]);
                t.Token = Convert.ToString(dr["Token"]);


                tl.Add(t);
            }

            return tl;
        }

        catch (Exception ex)
        {
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                con.Close();
            }
        }

    }

    public List<PrefferedDay> GetPrefferedTrainingDay()
    {

        SqlConnection con = null;
        SqlCommand cmd;
        String selectSTR;

        try
        {
            con = connect("BenefitConnectionStringName");

            // השאילתא מחזירה את כל המתאמנים שהתאמנו יותר מפעמיים באותו יום
            selectSTR = "SELECT distinct RES.TraineeCode, RES.Token " +
                "FROM(Select T.TraineeCode, U.Token, " +
                "case when DATENAME(dw, CT.TrainingTime) = 'Sunday' then count(T.TraineeCode) " +
                "when DATENAME(dw, CT.TrainingTime) = 'Monday' then count(T.TraineeCode) " +
                "when DATENAME(dw, CT.TrainingTime) = 'Tuesday' then count(T.TraineeCode) " +
                "when DATENAME(dw, CT.TrainingTime) = 'Wednesday' then count(T.TraineeCode) " +
                "when DATENAME(dw, CT.TrainingTime) = 'Thursday' then count(T.TraineeCode) " +
                "when DATENAME(dw, CT.TrainingTime) = 'Friday' then count(T.TraineeCode) " +
                "when DATENAME(dw, CT.TrainingTime) = 'Saturday' then count(T.TraineeCode) " +
                "end as 'NumOfTrainings' " +
                "from Trainees as T inner join CoupleTrainingSuggestions as CTS on T.TraineeCode = CTS.SenderCode or T.TraineeCode = CTS.ReceiverCode inner join CoupleTraining as CT ON CT.SuggestionCode = CTS.SuggestionCode inner join Users as U on U.UserCode = T.TraineeCode " +
                "GROUP BY T.TraineeCode, DATENAME(dw, CT.TrainingTime), U.Token)AS RES " +
                "GROUP BY RES.TraineeCode, RES.NumOfTrainings, RES.Token " +
                "HAVING RES.NumOfTrainings >= 2 ";

            cmd = new SqlCommand(selectSTR, con);
            SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            List<User> ul = new List<User>();
            List<PrefferedDay> ol = new List<PrefferedDay>();

            while (dr.Read())
            {
                User User = new User();
                User.UserCode = Convert.ToInt32(dr["TraineeCode"]);
                User.Token = Convert.ToString(dr["Token"]);
                ul.Add(User);
            }
            con.Close();

            con = connect("BenefitConnectionStringName");
            // מחזירה עבור כל מתאמן כמה פעמים הוא התאמן בכל יום 
            selectSTR = "SELECT * FROM(Select distinct T.TraineeCode, DATENAME(dw, CT.TrainingTime) as 'DayName', " +
                "case " +
                "when  DATENAME(dw, CT.TrainingTime) = 'Sunday' then count(T.TraineeCode) " +
                "when  DATENAME(dw, CT.TrainingTime) = 'Monday' then count(T.TraineeCode) " +
                "when  DATENAME(dw, CT.TrainingTime) = 'Tuesday' then count(T.TraineeCode) " +
                "when  DATENAME(dw, CT.TrainingTime) = 'Wednesday'  then count(T.TraineeCode) " +
                "when  DATENAME(dw, CT.TrainingTime) = 'Thursday' then count(T.TraineeCode) " +
                "when  DATENAME(dw, CT.TrainingTime) = 'Friday' then count(T.TraineeCode) " +
                "when  DATENAME(dw, CT.TrainingTime) = 'Saturday' then count(T.TraineeCode) " +
                "end as 'NumOfTrainings' " +
                "from Trainees as T inner join CoupleTrainingSuggestions as CTS on T.TraineeCode = CTS.SenderCode or  T.TraineeCode = CTS.ReceiverCode inner join CoupleTraining as CT ON CT.SuggestionCode = CTS.SuggestionCode " +
                "GROUP BY  T.TraineeCode, DATENAME(dw, CT.TrainingTime))AS RES " +
                "GROUP BY RES.TraineeCode, RES.DayName, RES.NumOfTrainings " +
                "HAVING RES.NumOfTrainings >= 2";

            cmd = new SqlCommand(selectSTR, con);
            dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            List<PrefferedDay> returnList = new List<PrefferedDay>();

            while (dr.Read())
            {
                PrefferedDay pd = new PrefferedDay();
                pd.UserCode = Convert.ToInt32(dr["TraineeCode"]);
                pd.DayName = Convert.ToString(dr["DayName"]);
                pd.NumOfTrainings = Convert.ToInt32(dr["NumOfTrainings"]);
                ol.Add(pd);

            }
            for (int i = 0; i < ul.Count; i++)
            {
                int numberOfTrainings = 0;
                PrefferedDay p = new PrefferedDay();
                for (int j = 0; j < ol.Count; j++)
                {

                    if (ol[j].UserCode == ul[i].UserCode)
                    {

                        if (numberOfTrainings < ol[j].NumOfTrainings)
                        {
                            numberOfTrainings = ol[j].NumOfTrainings;

                            p.DayName = ol[j].DayName;
                            p.UserCode = ol[j].UserCode;
                        }
                    }
                }
                numberOfTrainings = 0;
                returnList.Add(p);
            }
            return returnList;
        }

        catch (Exception ex)
        {
            throw (ex);
        }

        finally
        {
            if (con != null)
            {
                con.Close();
            }
        }
    }

    //--------------------------------------------------------------------
    // Build the Insert command String
    //--------------------------------------------------------------------

    private String BuildInsertUserCommand(User u)
    {
        String command;
        StringBuilder sb = new StringBuilder();

        sb.AppendFormat("Values('{0}','{1}','{2}','{3}','{4}','{5}','{6}',{7},{8},{9}, {10})", u.Email, u.FirstName, u.LastName, u.Password, u.Gender, u.DateOfBirth, u.Picture, u.SearchRadius.ToString(), u.IsTrainer.ToString(), u.Rate.ToString(), "getdate()");
        String prefix = "INSERT INTO Users (Email, FirstName, LastName, Password, Gender, DateOfBirth, Picture, SearchRadius, IsTrainer, Rate, SignUpDate) output INSERTED.UserCode ";
        command = prefix + sb.ToString();
        return command;
    }

    private String BuildInsertTraineeCommand(int UserCode, Trainee t)
    {
        String command;
        StringBuilder sb = new StringBuilder();

        sb.AppendFormat("Values({0},{1},'{2}','{3}', {4}, {5})", UserCode.ToString(), t.MaxBudget.ToString(), t.PartnerGender, t.TrainerGender, t.MinPartnerAge.ToString(), t.MaxPartnerAge.ToString());
        String prefix = "INSERT INTO Trainees (TraineeCode, MaxBudget, PartnerGender, TrainerGender, MinPartnerAge, MaxPartnerAge) ";
        command = prefix + sb.ToString();
        return command;
    }

    private String BuildInsertTrainerCommand(int UserCode, Trainer t)
    {
        String command;
        StringBuilder sb = new StringBuilder();

        sb.AppendFormat("Values({0},{1})", UserCode.ToString(), t.PersonalTrainingPrice.ToString());
        String prefix = "INSERT INTO Trainers (TrainerCode, PersonalTrainingPrice) ";
        command = prefix + sb.ToString();
        return command;
    }

    private String BuildInsertSportCategoriesCommand(int UserCode, int CategoryCode)
    {
        String command;
        StringBuilder sb = new StringBuilder();

        sb.AppendFormat("Values({0},{1})", UserCode.ToString(), CategoryCode.ToString());
        String prefix = "INSERT INTO UserSportCategories (UserCode, CategoryCode) ";
        command = prefix + sb.ToString();
        return command;
    }

    private String BuildInsertOnlineHistoryTraineeCommand(OnlineHistoryTrainee o)
    {
        String command;
        StringBuilder sb = new StringBuilder();
        sb.AppendFormat("Values({0},{1},{2},{3},'{4}', '{5}', {6}, {7}, {8}, {9} )", o.UserCode.ToString(), "getdate()", o.Latitude.ToString(), o.Longitude.ToString(), o.StartTime, o.EndTime, o.WithTrainer.ToString(), o.WithPartner.ToString(), o.GroupWithTrainer.ToString(), o.GroupWithPartners.ToString());
        String prefix = "INSERT INTO OnlineHistoryTrainee (TraineeCode, InsertTime, Latitude, Longitude, StartTime, EndTime, WithTrainer,WithPartner, GroupWithTrainer, GroupWithPartners) output INSERTED.OnlineCode  ";
        command = prefix + sb.ToString();
        return command;
    }

    private String BuildInsertCurrentTraineeCommand(int OnlineCode)
    {
        String command;
        StringBuilder sb = new StringBuilder();

        sb.AppendFormat("Values({0})", OnlineCode.ToString());
        String prefix = "INSERT INTO CurrentOnlineTrainee (OnlineCode)";
        command = prefix + sb.ToString();
        return command;
    }

    private String BuildInsertOnlineHistoryTrainerCommand(OnlineHistoryTrainer o)
    {
        String command;
        StringBuilder sb = new StringBuilder();

        sb.AppendFormat("Values({0},{1},{2},{3},'{4}', '{5}')", o.UserCode.ToString(), "getdate()", o.Latitude.ToString(), o.Longitude.ToString(), o.StartTime, o.EndTime);
        String prefix = "INSERT INTO OnlineHistoryTrainer (TrainerCode, InsertTime, Latitude, Longitude, StartTime, EndTime) output INSERTED.OnlineCode  ";
        command = prefix + sb.ToString();
        return command;
    }

    private String BuildInsertCurrentTrainerCommand(int OnlineCode)
    {
        String command;
        StringBuilder sb = new StringBuilder();

        sb.AppendFormat("Values({0})", OnlineCode.ToString());
        String prefix = "INSERT INTO CurrentOnlineTrainer (OnlineCode)";
        command = prefix + sb.ToString();
        return command;
    }

    private String BuildInsertHistoryGroupTrainingCommand(HistoryGroupTraining h)
    {
        String command;
        StringBuilder sb = new StringBuilder();

        sb.AppendFormat("Values('{0}',{1},{2},{3},{4},{5},{6},'{7}','{8}',{9},{10})", h.TrainingTime, h.Latitude.ToString(), h.Longitude.ToString(), h.WithTrainer.ToString(), h.CreatorCode.ToString(), h.MinParticipants.ToString(), h.MaxParticipants.ToString(), "0", "1", h.SportCategoryCode.ToString(), h.Price.ToString());
        String prefix = "INSERT INTO HistoryGroupTraining (TrainingTime, Latitude, Longitude, WithTrainer, CreatorCode, MinParticipants, MaxParticipants, CurrentParticipants, StatusCode,SportCategoryCode, Price ) output INSERTED.GroupTrainingCode  ";
        command = prefix + sb.ToString();
        return command;
    }

    private String BuildInsertActiveGroupTrainingCommand(int HistoryGroupTrainingCode)
    {
        String command;
        StringBuilder sb = new StringBuilder();

        sb.AppendFormat("Values({0})", HistoryGroupTrainingCode.ToString());
        String prefix = "INSERT INTO ActiveGroupTraining (GroupTrainingCode) ";
        command = prefix + sb.ToString();
        return command;
    }

    private String BuildInsertGroupParticipantsCommand(int UserCode, int GroupTrainingCode)
    {
        String command;
        StringBuilder sb = new StringBuilder();

        sb.AppendFormat("Values({0},{1},'{2}')", UserCode.ToString(), GroupTrainingCode.ToString(), '1');
        String prefix = "INSERT INTO GroupParticipants (UserCode, GroupTrainingCode, StatusCode) ";
        command = prefix + sb.ToString();
        return command;
    }
    
    private String BuildInsertSuggestionCommand(int SenderCode, int ReceiverCode)
    {
        String command;
        StringBuilder sb = new StringBuilder();

        sb.AppendFormat("Values({0},{1},{2}, {3})", SenderCode.ToString(), ReceiverCode.ToString(), "getdate()", 4);
        String prefix = "INSERT INTO CoupleTrainingSuggestions (SenderCode, ReceiverCode, SendingTime, StatusCode) ";
        command = prefix + sb.ToString();
        return command;
    }

    private String BuildInsertCoupleTrainingCommand(CoupleTraining ct)
    {
        String command;
        StringBuilder sb = new StringBuilder();

        sb.AppendFormat("Values('{0}',{1},{2},{3},'{4}',{5},{6})", ct.TrainingTime, ct.Latitude.ToString(), ct.Longitude.ToString(), ct.WithTrainer.ToString(), '1', ct.SuggestionCode.ToString(), ct.Price.ToString());
        String prefix = "INSERT INTO CoupleTraining (TrainingTime, Latitude, Longitude, WithTrainer, StatusCode, SuggestionCode, Price ) ";
        command = prefix + sb.ToString();
        return command;
    }

    private String BuildInsertMessageCommand(Message m)
    {
        String command;
        StringBuilder sb = new StringBuilder();

        sb.AppendFormat("Values({0},{1},{2},'{3}')", m.ChatCode.ToString(), m.SenderCode.ToString(), " getdate() ", m.Content);
        String prefix = "INSERT INTO Messages (ChatCode, SenderCode, SendingTime,Content ) ";
        command = prefix + sb.ToString();
        return command;
    }

    private String BuildInsertRatingCommand(Rating r)
    {
        String command;
        StringBuilder sb = new StringBuilder();

        sb.AppendFormat("Values({0},{1},{2})", r.TraineeCode.ToString(), r.RatedCode.ToString(), r.AvgTotalRate.ToString());
        String prefix = "INSERT INTO Ratings (TraineeCode, RatedCode, AvgTotalRate ) output INSERTED.RatingCode ";
        command = prefix + sb.ToString();
        return command;
    }

    private String BuildInsertParameterRateCommand(ParameterRate pr)
    {
        String command;
        StringBuilder sb = new StringBuilder();

        sb.AppendFormat("Values({0},{1}, {2})", pr.RatingCode.ToString(), pr.ParameterCode.ToString(), pr.Rate.ToString());
        String prefix = "INSERT INTO ParametersRate (RatingCode, ParameterCode, Rate ) ";
        command = prefix + sb.ToString();
        return command;
    }

    private String BuildInsertNewChat(int UserCode1, int UserCode2)
    {
        String command;
        StringBuilder sb = new StringBuilder();

        sb.AppendFormat(" Values({0},{1})", UserCode1.ToString(), UserCode2.ToString());
        String prefix = "INSERT INTO Chats (UserCode1, UserCode2) output INSERTED.ChatCode ";
        command = prefix + sb.ToString();
        return command;
    }

    
    //---------------------------------------------------------------------------------
    // Create the SqlCommand
    //---------------------------------------------------------------------------------
    private SqlCommand CreateCommand(String CommandSTR, SqlConnection con)
    {

        SqlCommand cmd = new SqlCommand(); // create the command object

        cmd.Connection = con;              // assign the connection to the command object

        cmd.CommandText = CommandSTR;      // can be Select, Insert, Update, Delete 

        cmd.CommandTimeout = 10;           // Time to wait for the execution' The default is 30 seconds

        cmd.CommandType = System.Data.CommandType.Text; // the type of the command, can also be stored procedure

        return cmd;
    }



}

