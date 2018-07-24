using ManagerApplication.Constant;
using ManagerApplication.Service;
using ManagerApplication.Utility;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ManagerApplication.Controllers.PartnerControler
{
    public class PartnerController : Controller
    {
        //
        // GET: /Partner/
        public ActionResult Home()
        {
            return View("~/Views/Partner/partnerHome.cshtml");
        }

        public async Task<JsonResult> TestCrawl()
        {
            var obj = new
            {
                message = ""
            };
            return Json(obj);
        }

        public JsonResult GetTypeOfSeat(string groupIdStr)
        {
            int groupId = Convert.ToInt32(groupIdStr);
            List<TypeOfSeat> list = new TypeOfSeatService().FindBy(t => t.groupId == groupId);
            var obj = list.Select(item => new
            {
                id = item.typeSeatId,
                name = item.typeName,
                capacity = item.capacity,
                groupId = item.groupId,
                price = item.price
            });
            return Json(obj);
        }

        public JsonResult GetGroupCinemaInfor(string groupIdStr)
        {
            int groupId = Convert.ToInt32(groupIdStr);
            string serverPath = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));
            GroupCinema group = new GroupCinemaServcie().FindByID(groupId);
            var jsonObj = new
            {
                logoImg = group.logoImg.Contains("http") ? group.logoImg : serverPath + group.logoImg,
                name = group.name,
                address = group.address,
                email = group.email,
                phone = group.phone
            };
            return Json(jsonObj);
        }

        public JsonResult GetAllEmployee(string groupIdStr)
        {
            int groupId = Convert.ToInt32(groupIdStr);
            List<CinemaManager> employeeList = new CinemaManagerService().FindCinemaManagerByGroupId(groupId);
            var jsonObj = employeeList.Select(item => new
            {
                username = item.managerId,
                password = item.managerPassword,
                name = item.managerName,
                phone = item.phone,
                email = item.email,
                cinemaId = item.cinemaId,
                cinemaName = new CinemaService().FindByID(item.cinemaId).cinemaName,
                isAvailable = item.isAvailable,
            });
            return Json(jsonObj);
        }

        public JsonResult SaveImage()
        {
            string message = "success!";
            if (System.Web.HttpContext.Current.Request.Files.AllKeys.Any())
            {
                System.Web.HttpPostedFile pic = System.Web.HttpContext.Current.Request.Files["imageUpload"];
                string fileName = pic.FileName;
                Stream fs = pic.InputStream;
                BinaryReader br = new BinaryReader(fs);
                byte[] bytes = br.ReadBytes((Int32)fs.Length);
                string uriString = @"ftp://waws-prod-dm1-039.ftp.azurewebsites.windows.net/site/wwwroot/Content/img/cinemaLogo/" + fileName;
                bool isSuccess = UploadUtility.Upload(bytes, uriString);
                if (!isSuccess) message = "fail"; 
                }
            var obj = new
            {
                message = message,
            };
            return Json(obj);
        }

        public JsonResult GetAllCinemaInGroup(string groupIdStr)
        {
            int groupId = Convert.ToInt32(groupIdStr);
            string serverPath = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));
            List<Cinema> cinemaList = new CinemaService().FindBy(c => c.groupId == groupId);
            var jsonObj = cinemaList.Select(item => new
            {
                cinemaId = item.cinemaId,
                cinemaName = item.cinemaName,
                address = item.cinemaAddress,
                email = item.email,
                phone = item.phone,
                introduction = item.introduction,
                openTime = item.openTime,
                profilePicture = item.profilePicture.Contains("http") ? item.profilePicture : (serverPath + item.profilePicture),
                rooms = new RoomService().FindBy(r => r.cinemaId == item.cinemaId)
                                         .Select(room => new
                                         {
                                             id = room.roomId,
                                             name = room.name,
                                             matrixSizeX = room.matrixSizeX,
                                             matrixSizeY = room.matrixSizeY,
                                             digType = room.digTypeId,
                                             capacity = room.capacity,
                                         })
            });
            return Json(jsonObj);
        }

        public JsonResult UpdateEmployee(string empObj)
        {
            JObject emp = JObject.Parse(empObj);
            string username = (string)emp.GetValue("username");
            string name = (string)emp.GetValue("name");
            string phone = (string)emp.GetValue("phone");
            string email = (string)emp.GetValue("email");
            CinemaManager cm = new CinemaManagerService().FindByID(username);
            cm.email = email;
            cm.phone = phone;
            cm.managerName = name;
            new CinemaManagerService().Update(cm);
            return null;
        }



        public JsonResult DeleteEmployee(string username)
        {
            var obj = new
            {
                isExist = "true",
            };
            CinemaManager cm = new CinemaManagerService().FindByID(username);
            if (cm != null)
            {
                cm.isAvailable = false;
                new CinemaManagerService().Update(cm);
                obj = new
                {
                    isExist = "false",
                };
                return Json(obj);
            }
            return Json(obj);
        }

        public bool IsEmpExist(string username)
        {
            CinemaManager cm = new CinemaManagerService().FindByID(username);
            if (cm != null) return true;
            else return false;
        }

        public JsonResult AddNewEmployee(string empObj)
        {
            JObject emp = JObject.Parse(empObj);
            string username = (string)emp.GetValue("username");
            string name = (string)emp.GetValue("name");
            string phone = (string)emp.GetValue("phone");
            string email = (string)emp.GetValue("email");
            string password = (string)emp.GetValue("password");
            int cinemaId = (int)emp.GetValue("cinemaId");

            string encrytedPassword = EncryptUtility.EncryptString(password);
            CinemaManager cm = new CinemaManager();
            cm.managerId = username;
            cm.managerPassword = encrytedPassword;
            cm.email = email;
            cm.phone = phone;
            cm.managerName = name;
            cm.cinemaId = cinemaId;
            cm.isAvailable = true;

            new CinemaManagerService().Create(cm);
            return null;
        }

        public JsonResult UpdateCinema(string cinemaObj)
        {
            JObject cinema = JObject.Parse(cinemaObj);
            int id = (int)cinema.GetValue("id");
            string cinemaName = (string)cinema.GetValue("cinemaName");
            string cinemaAddress = (string)cinema.GetValue("cinemaAddress");
            string phone = (string)cinema.GetValue("phone");
            string email = (string)cinema.GetValue("email");
            string openTime = (string)cinema.GetValue("openTime");
            string introduction = (string)cinema.GetValue("introduction");
            string imagePath = (string)cinema.GetValue("imagePath");

            Cinema c = new CinemaService().FindByID(id);
            c.email = email;
            c.phone = phone;
            c.cinemaName = cinemaName;
            c.cinemaAddress = cinemaAddress;
            c.openTime = openTime;
            c.introduction = introduction;
            if (imagePath != "")
            {
                c.profilePicture = "Content/img/cinemaLogo/" + imagePath;
            }
            new CinemaService().Update(c);
            return null;
        }
        public JsonResult UpdatePictureForCinema(string cinemaId, string fileName)
        {
            int cineId = Convert.ToInt32(cinemaId);
            Cinema c = new CinemaService().FindByID(cineId);
            c.profilePicture = "Content/img/cinemaLogo/" + fileName;
            new CinemaService().Update(c);
            return null;
        }


        public JsonResult CreateCinema(string cinemaObj)
        {
            JObject cinema = JObject.Parse(cinemaObj);
            string cinemaName = (string)cinema.GetValue("cinemaName");
            string cinemaAddress = (string)cinema.GetValue("cinemaAddress");
            string phone = (string)cinema.GetValue("phone");
            string email = (string)cinema.GetValue("email");
            string openTime = (string)cinema.GetValue("openTime");
            string introduction = (string)cinema.GetValue("introduction");
            string imagePath = (string)cinema.GetValue("imagePath");
            int groupId = (int)cinema.GetValue("groupId");

            Cinema c = new Cinema();
            c.email = email;
            c.phone = phone;
            c.cinemaName = cinemaName;
            c.cinemaAddress = cinemaAddress;
            c.openTime = openTime;
            c.introduction = introduction;
            c.groupId = groupId;
            if (imagePath != "")
            {
                c.profilePicture = "Content/img/cinemaLogo/" + imagePath;
            }
            new CinemaService().Create(c);
            return null;
        }

        public JsonResult FindAllSeatByRoomId(string roomIdstr)
        {
            int roomId = Convert.ToInt32(roomIdstr);
            List<Seat> seats = new SeatService().FindBy(s => s.roomId == roomId);
            var obj = seats.Select(s => new
            {
                id = s.seatId,
                type = s.typeSeatId,
                px = s.px,
                py = s.py,
                locationX = s.locationX,
                locationY = s.locationY
            });
            return Json(obj);
        }

        public JsonResult SaveRoom()
        {
            string roomInfor = Request.Params["roomInfor"];
            JObject data = JObject.Parse(roomInfor);
            JObject roomData = (JObject)data.GetValue("roomData");
            int cinemaId = (int)roomData.GetValue("cinemaId");
            string roomName = (string)roomData.GetValue("name");
            int digType = (int)roomData.GetValue("digType");
            int capacity = (int)roomData.GetValue("capacity");
            int matrixSizeX = (int)roomData.GetValue("matrixSizeX");
            int matrixSizeY = (int)roomData.GetValue("matrixSizeY");
            Room room = new Room();
            room.cinemaId = cinemaId;
            room.digTypeId = digType;
            room.capacity = capacity;
            room.matrixSizeX = matrixSizeX;
            room.matrixSizeY = matrixSizeY;
            room.name = roomName;
            int roomId = new RoomService().CreateRoom(room);

            JArray seatData = (JArray)data.GetValue("seatData");
            foreach (JObject item in seatData)
            {
                int py = (int)item.GetValue("py");
                int px = (int)item.GetValue("px");
                int locationX = (int)item.GetValue("locationX");
                int locationY = (int)item.GetValue("locationY");
                int typeSeatId = (int)item.GetValue("typeOfSeatId");
                Seat seat = new Seat();
                seat.px = px; seat.py = py;
                seat.locationX = locationX; seat.locationY = locationY;
                seat.roomId = roomId;
                seat.typeSeatId = typeSeatId;

                new SeatService().Create(seat);
            }
            var obj = new
            {
                isSucess = "true",
            };
            return Json(obj);
        }

        public JsonResult UploadImage(object data, string fileName)
        {
            string uriString = @"ftp://waws-prod-dm1-039.ftp.azurewebsites.windows.net/site/wwwroot/Content/img/film/testPic1.jpg";
            //byte[] array = Encoding.ASCII.GetBytes(data);
            return null;
        }
    }
}
