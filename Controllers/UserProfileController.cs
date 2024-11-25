using Microsoft.AspNetCore.Mvc;
using v1Remastered.Services;
using v1Remastered.Dto;

namespace v1Remastered.Controllers
{
    [Route("UserProfile")]
    public class UserProfileController : Controller
    {
        private readonly IUserProfileService _userProfileService;
        private readonly IHospitalService _hospitalService;
        public UserProfileController(IUserProfileService userProfileService, IHospitalService hospitalService)
        {
            _userProfileService = userProfileService;
            _hospitalService = hospitalService;
        }

        [HttpGet("{userid}")]
        public IActionResult UserProfile(string userid)
        {
            if(!string.IsNullOrEmpty(userid))
            {
                UserDetailsDto_UserProfile userProfileDetails = _userProfileService.FetchUserProfileDetails(userid);

                ViewBag.D1HospitalName = _hospitalService.FetchHospitalNameById(userProfileDetails.UserBookingDetails.D1HospitalId);
                ViewBag.D2HospitalName = _hospitalService.FetchHospitalNameById(userProfileDetails.UserBookingDetails.D2HospitalId);

                if(userProfileDetails.UserId != null)
                {
                    return View(userProfileDetails);
                }
            }

            return View("~/Shared/_Error.cshtml");
        }
    }

}