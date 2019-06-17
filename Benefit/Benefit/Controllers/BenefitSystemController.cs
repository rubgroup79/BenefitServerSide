using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Benefit.Models;
using Benefit.Controllers;

namespace Benefit.Controllers
{

    public class BenefitSystemController : ApiController
    {
        public BenefitSystemController()
        {

        }

        [HttpPost]
        [Route("api/CheckIfEmailExists")]
        public bool CheckIfEmailExists(string Email)
        {
            BenefitSystem s = new BenefitSystem();
            return s.CheckIfEmailExists(Email);
        }


        [HttpPost]
        [Route("api/CheckIfPasswordMatches")]
        public Trainee CheckIfPasswordMatches([FromBody]Trainee t)
        {
            BenefitSystem s = new BenefitSystem();
            return s.CheckIfPasswordMatches(t.Email, t.Password);
        }

        [HttpGet]
        [Route("api/getLazyTrainees")]
        public IEnumerable<Trainee> GetLazyTrainees()
        {
            BenefitSystem s = new BenefitSystem();
            List<Trainee> tl = s.GetLazyTrainees();
            return tl;


        }

        [HttpGet]
        [Route("api/getLazyTrainers")]
        public IEnumerable<Trainer> GetLazyTrainers()
        {
            BenefitSystem s = new BenefitSystem();
            return s.GetLazyTrainers();

        }



        [HttpPost]
        [Route("api/UpdateToken")]
        public void UpdadteToken(string Token, int UserCode)
        {
            BenefitSystem s = new BenefitSystem();
            s.UpdateToken(Token, UserCode);

        }

        //returns token for user 
        [HttpGet]
        [Route("api/GetToken")]
        public string GetToken(int UserCode)
        {
            User u = new User();
            return u.GetToken(UserCode);
        }


        [HttpGet]
        [Route("api/GetPrefferedDay")]
        public List<PrefferedDay> GetPrefferedTrainingDay()
        {
            BenefitSystem s = new BenefitSystem();
            return s.GetPrefferedTrainingDay();

        }
        //--------------------------------------------------------------------
        // upload pictures 
        //--------------------------------------------------------------------


        [Route("uploadpicture")]
        public Task<HttpResponseMessage> Post()
        {
            string outputForNir = "start---";
            List<string> savedFilePath = new List<string>();
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }
            string rootPath = HttpContext.Current.Server.MapPath("~/uploadFiles");
            var provider = new MultipartFileStreamProvider(rootPath);
            var task = Request.Content.ReadAsMultipartAsync(provider).
                ContinueWith<HttpResponseMessage>(t =>
                {
                    if (t.IsCanceled || t.IsFaulted)
                    {
                        Request.CreateErrorResponse(HttpStatusCode.InternalServerError, t.Exception);
                    }
                    foreach (MultipartFileData item in provider.FileData)
                    {
                        try
                        {
                            outputForNir += " ---here";
                            string name = item.Headers.ContentDisposition.FileName.Replace("\"", "");
                            outputForNir += " ---here2=" + name;

                            //need the guid because in react native in order to refresh an inamge it has to have a new name
                            string newFileName = Path.GetFileNameWithoutExtension(name) + "_" + CreateDateTimeWithValidChars() + Path.GetExtension(name);
                            //string newFileName = Path.GetFileNameWithoutExtension(name) + "_" + Guid.NewGuid() + Path.GetExtension(name);
                            //string newFileName = name + "" + Guid.NewGuid();
                            outputForNir += " ---here3" + newFileName;

                            //delete all files begining with the same name
                            string[] names = Directory.GetFiles(rootPath);
                            //foreach (var fileName in names)
                            //{
                            //    if (Path.GetFileNameWithoutExtension(fileName).IndexOf(Path.GetFileNameWithoutExtension(name)) != -1)
                            //    {
                            //        File.Delete(fileName);
                            //    }
                            //}

                            //File.Move(item.LocalFileName, Path.Combine(rootPath, newFileName));
                            File.Copy(item.LocalFileName, Path.Combine(rootPath, newFileName), true);
                            File.Delete(item.LocalFileName);
                            outputForNir += " ---here4";

                            Uri baseuri = new Uri(Request.RequestUri.AbsoluteUri.Replace(Request.RequestUri.PathAndQuery, string.Empty));
                            outputForNir += " ---here5";
                            string fileRelativePath = "~/uploadFiles/" + newFileName;
                            outputForNir += " ---here6 imageName=" + fileRelativePath;
                            Uri fileFullPath = new Uri(baseuri, VirtualPathUtility.ToAbsolute(fileRelativePath));
                            outputForNir += " ---here7" + fileFullPath.ToString();
                            savedFilePath.Add(fileFullPath.ToString());
                        }
                        catch (Exception ex)
                        {
                            outputForNir += " ---excption=" + ex.Message;
                            string message = ex.Message;
                        }
                    }

                    return Request.CreateResponse(HttpStatusCode.Created, savedFilePath[0]);
                });
            return task;
        }

        private string CreateDateTimeWithValidChars()
        {
            return DateTime.Now.ToString().Replace('/', '_').Replace(':', '-').Replace(' ', '_');
        }

        [HttpGet]
        [Route("api/ShowProfile")]
        public User ShowProfile(int UserCode)
        {
            BenefitSystem s = new BenefitSystem();
            return s.ShowProfile(UserCode);
        }

        [HttpPost]
        [Route("api/GoOffline")]
        public void GoOffline(int UserCode, int IsTrainer)
        {
            BenefitSystem s = new BenefitSystem();
            s.GoOffline(UserCode, IsTrainer);
        }

        [HttpGet]
        [Route("api/CheckIfTraineeOnline")]
        public OnlineHistoryTrainee CheckIfTraineeOnline(int UserCode)
        {
            BenefitSystem s = new BenefitSystem();
            return s.CheckIfTraineeOnline(UserCode);
        }

        [HttpGet]
        [Route("api/CheckIfTrainerOnline")]
        public OnlineHistoryTrainer CheckIfTrainerOnline(int UserCode)
        {
            BenefitSystem s = new BenefitSystem();
            return s.CheckIfTrainerOnline(UserCode);
        }


        [HttpPost]
        [Route("api/UpdateTrainingsStatus")]
        public void UpdateTrainingsStatus()
        {
            BenefitSystem b = new BenefitSystem();
            b.UpdateTrainingsStatus();
        }



      

        [HttpPost]
        [Route("api/InsertNewRating")]
        public int InsertNewRating([FromBody]Rating r)
        {
            BenefitSystem b = new BenefitSystem();
            return b.InsertNewRating(r);
        }


        [HttpPost]
        [Route("api/InsertParametersRate")]
        public void InsertParametersRate([FromBody]ParameterRate pr)
        {
            BenefitSystem b = new BenefitSystem();
            b.InsertParametersRate(pr);
        }

        [HttpGet]
        [Route("api/CheckIfRateExists")]
        public Rating CheckIfRateExists(int UserCode, int RatedUserCode)
        {
            BenefitSystem b = new BenefitSystem();
            return b.CheckIfRateExists(UserCode, RatedUserCode);
        }

        [HttpPost]
        [Route("api/UpdateExistingParametersRate")]
        public void UpdateExistingParametersRate([FromBody]ParameterRate pr)
        {
            BenefitSystem b = new BenefitSystem();
            b.UpdateExistingParametersRate(pr);
        }

        //[HttpPost]
        //[Route("api/UpdateUserRate")]
        //public void UpdateUserRate(int RatedCode)
        //{
        //    BenefitSystem b = new BenefitSystem();
        //    b.UpdateUserRate(RatedCode);
        //}

        [HttpPost]
        [Route("api/UpdateExistingAvarageRate")]
        public void UpdateExistingAvarageRate(Rating r)
        {
            BenefitSystem b = new BenefitSystem();
            b.UpdateExistingAvarageRate(r);
        }
        
        [HttpGet]
        [Route("api/GetSportCategories")]
        public IEnumerable<SportCategory> GetSportCategories()
        {
            BenefitSystem b = new BenefitSystem();
           return  b.GetSportCategories();
        }

        [HttpGet]
        [Route("api/GetAvarageParametersRate")]
        public IEnumerable<AverageRateParameters> GetAvarageParametersRate(int UserCode)
        {
            BenefitSystem b = new BenefitSystem();
            return b.GetAvarageParametersRate(UserCode);
        }

        [HttpGet]
        [Route("api/GetTraineeProfileDetails")]
        public TraineeDetails GetTraineeProfileDetails(int UserCode)
        {
            BenefitSystem b = new BenefitSystem();
            return b.GetTraineeProfileDetails(UserCode);
        }


        [HttpGet]
        [Route("api/GetTrainerProfileDetails")]
        public TrainerDetails GetTrainerProfileDetails(int UserCode)
        {
            BenefitSystem b = new BenefitSystem();
            return b.GetTrainerProfileDetails(UserCode);
        }

        [HttpPost]
        [Route("api/UpdateTraineeDetails")]
        public void UpdateTraineeDetails([FromBody]TraineeDetails td)
        {
            BenefitSystem b = new BenefitSystem();
            b.UpdateTraineeDetails(td);
        }

        [HttpPost]
        [Route("api/UpdateTrainerDetails")]
        public void UpdateTrainerDetails([FromBody]TrainerDetails td)
        {
            BenefitSystem b = new BenefitSystem();
            b.UpdateTrainerDetails(td);
        }

        [HttpPost]
        [Route("api/OpenChat")]
        public int OpenChat(int UserCode1, int UserCode2)
        {
            BenefitSystem b = new BenefitSystem();
            return b.OpenChat(UserCode1, UserCode2);
        }

        [HttpGet]
        [Route("api/checkForCloseTrainings")]
        public bool checkForCloseTrainings(int UserCode, string TrainingTime)
        {
            BenefitSystem b = new BenefitSystem();
            return b.checkForCloseTrainings(UserCode, TrainingTime);
        }
        

    }



}

