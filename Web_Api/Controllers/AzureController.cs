using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using azureapi.Models;
using Newtonsoft.Json;
using System.Web.Http.Cors;
using azureapi.IUtilities;
namespace azureapi.Controllers
{
    //[EnableCors(origins: "https://ifn701hms.azurewebsites.net", headers: "*", methods: "*")]
    [MyCorsPolicy]
    public class ValuesController : ApiController
    {
        RegisterDetailController registdetailctl = new RegisterDetailController();
        MessagesController mctrl = new MessagesController();
        stepController stepctrl = new stepController();

        [AllowAnonymous]
        [Route("api/values/getuserinfo")]
        
        public HttpResponseMessage Get(string userid)
        {
        
           string jsonstr = registdetailctl.getuserinfo(userid);


            return Request.CreateResponse(HttpStatusCode.OK, jsonstr);
        
        }

        
        
        [AllowAnonymous]
        [HttpGet]
        public HttpResponseMessage HelloWorld()
        {
            return Request.CreateResponse(HttpStatusCode.OK, "Hello Wor123!");
        }


        [AllowAnonymous]
        [HttpGet]
        [Route("api/values/GetCalories")]
        public HttpResponseMessage GetCalories()
        {
            HttpResponseMessage msg;
            try
            {

                double[] data = stepctrl.GetWeeklyCalories();

                msg = Request.CreateResponse(HttpStatusCode.OK, data);

            }
            catch (Exception e)
            {
                msg = Request.CreateResponse(HttpStatusCode.InternalServerError, e.ToString());

            }
            return msg;
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("api/values/goalSetting")]
        public HttpResponseMessage goalSetting(string userid)
        {

            string message = stepctrl.goalSetting(userid);

            return Request.CreateResponse(HttpStatusCode.OK, message);

        }

        [AllowAnonymous]
        [HttpPost]
        [Route("api/values/goalsetting")]
        public HttpResponseMessage goalSetting()
        {

            HttpContent httpcontent = Request.Content;
            var goalsetting = httpcontent.ReadAsStringAsync().Result;
            string ans = stepctrl.goalsetting(goalsetting);



            return Request.CreateResponse(HttpStatusCode.OK, ans);
        }


        [AllowAnonymous]
        [HttpGet]
        [Route("api/values/getWeeklyGoals")]
        public HttpResponseMessage getWeeklyGoals(string userid)
        {
            HttpResponseMessage msg;



            try
            {
                Goals goals = stepctrl.getWeeklyCalories(userid);

                string goalsjson = JsonConvert.SerializeObject(goals);
                msg = Request.CreateResponse(HttpStatusCode.OK, goalsjson);
            }
            catch (Exception e)
            {
                msg = Request.CreateResponse(HttpStatusCode.InternalServerError, "");
            }
            return msg;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("api/values/saveRegistDetail")]
        public HttpResponseMessage saveRegistDetail()
        {

            HttpContent httpcontent = Request.Content;
            var registdetail = httpcontent.ReadAsStringAsync().Result;
            string message = registdetailctl.saveregistdetail(registdetail);
            return Request.CreateResponse(HttpStatusCode.OK, message);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("api/values/getTrainers")]
        public HttpResponseMessage getTrainers(string userid)
        {


            string[] trainers = registdetailctl.GetTrainersList(userid);

            return Request.CreateResponse(HttpStatusCode.OK, trainers);

        }

        [AllowAnonymous]
        [HttpGet]
        [Route("api/values/getstudents")]
        
        public HttpResponseMessage getstudents(string userid)
        {

            string[] studentlist = registdetailctl.getstudentsinfo(userid);
            List<Goals> studentgoals = new List<Goals>();

            foreach (string student in studentlist)
            {

                Goals goal = stepctrl.getWeeklyCalories(student);
                studentgoals.Add(goal);
            }
            if (studentgoals.Count > 0)
            {
                string jsonobj = JsonConvert.SerializeObject(studentgoals);
                return Request.CreateResponse(HttpStatusCode.OK, jsonobj);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, string.Empty);
            }

        }


        //Messages
        [AllowAnonymous]
        [HttpPost]
        [Route("api/values/sendMessage")]
        public HttpResponseMessage sendMessage()
        {
            
            HttpContent httpcontent = Request.Content;
            var messagecontent = httpcontent.ReadAsStringAsync().Result;
            string result = mctrl.saveMessage(messagecontent);

            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("api/values/readMessage")]
        public HttpResponseMessage readMsg(string userid) {

            List<string> msg = mctrl.readMsg(userid);
            if (msg.Count != 0)
            {
                //return response
            }
            else {

            }
            return Request.CreateResponse(HttpStatusCode.OK, "");
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("api/values/UploadInfo")]
        public HttpResponseMessage UploadInfo() {

            return Request.CreateResponse(HttpStatusCode.OK,"Hello Gary");
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("api/values/Login")]
        public HttpResponseMessage Login() {
            

            return Request.CreateResponse(HttpStatusCode.OK,"");
        }

        
    }
}

