using Microsoft.AspNetCore.Mvc;
using WebApi.Model.Room;
using WebApi.Model;
using Microsoft.AspNetCore.Authorization;
using Application.Interface.IDomainServices;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Application.Implementation.DomainServices;
using Application.Interface.ApplicationServices;

namespace WebApi.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RoomController : Controller
    {
        private readonly IBranchService _branchService;
        private readonly UserManager<AppUser> _userManager;
        private readonly ILandlordService _landlordService;
        private readonly IMapper _mapper;
        private readonly IBoundaryService _boundaryService;
        private readonly IRoomService _roomService;

        public RoomController(IBranchService branchService, UserManager<AppUser> userManager, ILandlordService landlordService, IMapper mapper, IBoundaryService boundaryService, IRoomService roomService)
        {
            _branchService=branchService;
            _userManager=userManager;
            _landlordService=landlordService;
            _mapper=mapper;
            _boundaryService=boundaryService;
            _roomService=roomService;
        }

        [HttpGet]
        [Route("detail")]
        public IActionResult GetRoom(int roomid) { 
            var Identity = HttpContext.User;
            string CurrentUserId = "";
            string CurrentLandlordId = "";
            int landlordId = 0;
            if (Identity.HasClaim(c => c.Type == "userid"))
            {
                CurrentUserId = Identity.Claims.FirstOrDefault(c => c.Type == "userid").Value.ToString();
                CurrentLandlordId = Identity.Claims.FirstOrDefault(c => c.Type == "landlordid").Value.ToString();
            }
            var result = int.TryParse(CurrentLandlordId, out landlordId);
            if (string.IsNullOrEmpty(CurrentUserId) && string.IsNullOrEmpty(CurrentLandlordId) && !result)
            {
                return Unauthorized();
            }

            try
            {
                var room = _roomService.GetRoomById(landlordId,roomid);
                var roomResult = _mapper.Map<RoomModel>(room);
                if (roomResult != null)
                {
                    foreach (var imageItem in roomResult.ImageRooms)
                    {
                        imageItem.Url =  "http://localhost:5135/contents/room/image/"+ imageItem.Url;
                    }
                }
                
                return Ok(roomResult);
            }
            catch
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseMessage { Status = "Error", Message = "can't create room!" });
            }
        }

		[HttpGet]
		[Route("detail/full")]
		public IActionResult GetRoomForDetail(int roomid)
		{
			var Identity = HttpContext.User;
			string CurrentUserId = "";
			string CurrentLandlordId = "";
			int landlordId = 0;
			if (Identity.HasClaim(c => c.Type == "userid"))
			{
				CurrentUserId = Identity.Claims.FirstOrDefault(c => c.Type == "userid").Value.ToString();
				CurrentLandlordId = Identity.Claims.FirstOrDefault(c => c.Type == "landlordid").Value.ToString();
			}
			var result = int.TryParse(CurrentLandlordId, out landlordId);
			if (string.IsNullOrEmpty(CurrentUserId) && string.IsNullOrEmpty(CurrentLandlordId) && !result)
			{
				return Unauthorized();
			}

			try
			{
				var room = _roomService.GetRoomForDetailById(landlordId, roomid);
				var roomResult = _mapper.Map<RoomModel>(room);
				if (roomResult != null)
				{
					foreach (var imageItem in roomResult.ImageRooms)
					{
						imageItem.Url =  "http://localhost:5135/contents/room/image/"+ imageItem.Url;
					}
				}

				return Ok(roomResult);
			}
			catch
			{
				return StatusCode(StatusCodes.Status400BadRequest, new ResponseMessage { Status = "Error", Message = "can't create room!" });
			}
		}



		[HttpPost]
        [Route("add")]
        public IActionResult CreateRoom(RoomCreateModel model)
        {
            var Identity = HttpContext.User;
            string CurrentUserId = "";
            string CurrentLandlordId = "";
            int landlordId = 0;
            if (Identity.HasClaim(c => c.Type == "userid"))
            {
                CurrentUserId = Identity.Claims.FirstOrDefault(c => c.Type == "userid").Value.ToString();
                CurrentLandlordId = Identity.Claims.FirstOrDefault(c => c.Type == "landlordid").Value.ToString();
            }
            var result = int.TryParse(CurrentLandlordId, out landlordId);
            if (string.IsNullOrEmpty(CurrentUserId) && string.IsNullOrEmpty(CurrentLandlordId) && !result)
            {
                return Unauthorized();
            }
          
            

            try
            {    var room = _mapper.Map<Room>(model);
                _roomService.CreateRoom(landlordId,model.BranchId,model.AreaId, room);
                _roomService.SaveChanges();
                
                return Ok(room.Id);
            }
            catch
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseMessage { Status = "Error", Message = "can't create room!" });
            }
        }

        [HttpPut]
        [Route("edit")]
        public IActionResult UpdateRoom(RoomEditModel model)
        {
            var Identity = HttpContext.User;
            string CurrentUserId = "";
            string CurrentLandlordId = "";
            int landlordId = 0;
            if (Identity.HasClaim(c => c.Type == "userid"))
            {
                CurrentUserId = Identity.Claims.FirstOrDefault(c => c.Type == "userid").Value.ToString();
                CurrentLandlordId = Identity.Claims.FirstOrDefault(c => c.Type == "landlordid").Value.ToString();
            }
            var result = int.TryParse(CurrentLandlordId, out landlordId);
            if (string.IsNullOrEmpty(CurrentUserId) && string.IsNullOrEmpty(CurrentLandlordId) && !result)
            {
                return Unauthorized();
            }
               
            try
            {
                 var room = _mapper.Map<Room>(model);
                _roomService.UpdateRoom(landlordId, room);
                _roomService.SaveChanges();

                return Ok(room.Id);
            }
            catch
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseMessage { Status = "Error", Message = "lỗi không thể cập nhật thông tin phòng trọ !" });
            }
        }

        [HttpDelete]
        [Route("delete")]
        public IActionResult DeleteRoom([FromQuery] int roomid)
        {
            var Identity = HttpContext.User;
            string CurrentLandlordId = "";
            int landlordId = 0;
            if (Identity.HasClaim(c => c.Type == "userid"))
            {

                CurrentLandlordId = Identity.Claims.FirstOrDefault(c => c.Type == "landlordid").Value.ToString();
            }

            var result = int.TryParse(CurrentLandlordId, out landlordId);
            if (string.IsNullOrEmpty(CurrentLandlordId) && !result)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseMessage { Status = "Error", Message = "Tìm thấy nhà trọ!" });
            }
            try
            {

                var room = _roomService.GetRoomById(landlordId, roomid);
                if(room != null)
                {
                    foreach(var image in room.ImageRooms)
                    {
                        var filePath = "Uploads/room/image/"+image.Url;
                        System.IO.File.Delete(filePath);
                    }

                }

                var deleteResult = _roomService.DeleteRoom(landlordId, roomid);
                _roomService.SaveChanges();
                if (!deleteResult.Success) { return StatusCode(StatusCodes.Status400BadRequest, new ResponseMessage { Status = "Error", Message = deleteResult.Message }); }
                return Ok();
            }
            catch
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseMessage { Status = "Error", Message = "Không thể xóa khu vực!" });
            }
        }


        [HttpDelete]
        [Route("image/delete")]
        public IActionResult DeleteImageRoom([FromQuery] int imageid)
        {
            var Identity = HttpContext.User;
            string CurrentLandlordId = "";
            int landlordId = 0;
            if (Identity.HasClaim(c => c.Type == "userid"))
            {
                CurrentLandlordId = Identity.Claims.FirstOrDefault(c => c.Type == "landlordid").Value.ToString();
            }

            var result = int.TryParse(CurrentLandlordId, out landlordId);
            if (string.IsNullOrEmpty(CurrentLandlordId) && !result)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseMessage { Status = "Error", Message = "Tìm thấy nhà trọ!" });
            }
            try
            {
                var deleteResult = _roomService.DeleteImageRoom(landlordId,imageid);
                _roomService.SaveChanges();

                if (deleteResult.Success)
                {
                    var filePath = "Uploads/room/image/"+deleteResult.Message;
                    System.IO.File.Delete(filePath);

                }
                

                return Ok();
            }
            catch
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseMessage { Status = "Error", Message = "Không thể xóa khu vực!" });
            }

        }



        [HttpPost]
        [Route("uploadimage")]
        public async Task<IActionResult> UploadImageRoom(int roomid)
        {

            var Identity = HttpContext.User;
            string CurrentUserId = "";
            string CurrentLandlordId = "";
            int landlordId = 0;
            if (Identity.HasClaim(c => c.Type == "userid"))
            {
                CurrentUserId = Identity.Claims.FirstOrDefault(c => c.Type == "userid").Value.ToString();
                CurrentLandlordId = Identity.Claims.FirstOrDefault(c => c.Type == "landlordid").Value.ToString();
            }
            var result = int.TryParse(CurrentLandlordId, out landlordId);
            if (string.IsNullOrEmpty(CurrentUserId) && string.IsNullOrEmpty(CurrentLandlordId) && !result)
            {
                return Unauthorized();
            }

          
            try
            {
                var httpRequest = HttpContext.Request;
                if (httpRequest.Form.Files.Count>0)
                {
                    var room = _roomService.GetRoomById(landlordId,roomid);
                    if (room == null) {return StatusCode(StatusCodes.Status400BadRequest, new ResponseMessage { Status = "Error", Message = "Can't find room  !" } );}
                    List<string> files = new List<string>();

                    foreach(var file in httpRequest.Form.Files)
                    {

                        if (file != null) { 
                             var fileNameRandom = Path.GetFileNameWithoutExtension(Path.GetRandomFileName()) + Path.GetExtension(file.FileName);//tên file random + extension file upload

                             var filePath = Path.Combine("Uploads", "room", "image", fileNameRandom); //đường đẫn đến file
                            files.Add(fileNameRandom);

                             using (var filestream = new FileStream(filePath, FileMode.Create))
                             {
                                  await file.CopyToAsync(filestream); // copy file f vào filestream
                             }

                        }

                        
                    }

                    _roomService.UploadRoomImages(landlordId, room.Id, files.ToArray());

                    _roomService.SaveChanges();

                }

                

                return Ok();

            }
            catch (Exception ex)
            {
                return StatusCode(
                                StatusCodes.Status400BadRequest,
                                new ResponseMessage { Status = "Error", Message = "Can upload room image !" }
                       );

            }
        }


        [HttpPost]
        [Route("uploadoneimage")]
        public async Task<IActionResult> UploadOneImageRoom(int roomid)
        {

            var Identity = HttpContext.User;
            string CurrentUserId = "";
            string CurrentLandlordId = "";
            int landlordId = 0;
            if (Identity.HasClaim(c => c.Type == "userid"))
            {
                CurrentUserId = Identity.Claims.FirstOrDefault(c => c.Type == "userid").Value.ToString();
                CurrentLandlordId = Identity.Claims.FirstOrDefault(c => c.Type == "landlordid").Value.ToString();
            }
            var result = int.TryParse(CurrentLandlordId, out landlordId);
            if (string.IsNullOrEmpty(CurrentUserId) && string.IsNullOrEmpty(CurrentLandlordId) && !result)
            {
                return Unauthorized();
            }


            try
            {
                var httpRequest = HttpContext.Request;
                if (httpRequest.Form.Files.Count>0)
                {
                    var room = _roomService.GetRoomById(landlordId, roomid);
                    if (room == null) { return StatusCode(StatusCodes.Status400BadRequest, new ResponseMessage { Status = "Error", Message = "Can't find room  !" }); }
                  

                    var file = httpRequest.Form.Files[0];
                    if (file != null)
                    {
                        var fileNameRandom = Path.GetFileNameWithoutExtension(Path.GetRandomFileName()) + Path.GetExtension(file.FileName);//tên file random + extension file upload

                        var filePath = Path.Combine("Uploads", "room", "image", fileNameRandom); //đường đẫn đến file
                       

                        using (var filestream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(filestream); // copy file f vào filestream
                        }  

                        var imageRoom =  _roomService.UploadRoomImage(landlordId, room.Id,fileNameRandom);

                        _roomService.SaveChanges();
                        var imageResult = _mapper.Map<ImageRoomModel>(imageRoom);

                        imageResult.Url =  "http://localhost:5135/contents/room/image/"+ imageResult.Url;

                        return Ok(imageResult);
                    }

                }

                return Ok();

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status400BadRequest, new ResponseMessage { Status = "Error", Message = "Can upload room image !" });

            }
        }


        


    }
}
