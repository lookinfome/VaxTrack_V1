using Microsoft.AspNetCore.Mvc;
using v1Remastered.Services;
using v1Remastered.Dto;
using v1Remastered.Models;

namespace v1Remastered.Controllers
{
    [Route("Admin")]
    public class AdminController:Controller
    {
        private readonly IAdminService _adminService;
        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        [HttpGet("{userid}")]
        public IActionResult AdminPage()
        {
            // users list with pending approval
            List<AdminDetailsDto_UserWithPendingApproval> _userswWithPendingApproval = _adminService.FetchUsersWithPendingApproval();
            ViewBag.UsersWithPendingApproval = _userswWithPendingApproval;

            // hospitals list with name and slots details
            List<AdminDetailsDto_HospitalsList> _fetchedHospitalsDetails = _adminService.FetchHospitalsList();
            ViewBag.FetchedHospitalsDetails = _fetchedHospitalsDetails;

            // total users count
            int _totalUsersCount = _adminService.FetchTotalUsersCount();
            ViewBag.TotalUsersCount = _totalUsersCount;

            // total users count with slot booked
            int _totalUsersCountWithSlotBooked = _adminService.FetchTotalUsersCountWithSlotBooked();
            ViewBag.TotalUsersCountWithSlotBooked = _totalUsersCountWithSlotBooked;

            // total users count with approved slots
            int _totalUsersCountWithApprovedSlots = _adminService.FetchTotalUsersCountWithApprovedSlots();
            ViewBag.TotalUsersCountWithApprovedSlots = _totalUsersCountWithApprovedSlots;

            // total male users count
            int _totalMaleUsersCount = _adminService.FetchTotalMaleUsersCount();
            int _totalFemaleUsersCount = _adminService.FetchTotalFemaleUsersCount();
            ViewBag.TotalMaleUsersCount = _totalMaleUsersCount;
            ViewBag.TotalFemaleUsersCount = _totalFemaleUsersCount;

            return View();
        }

        [HttpPost("BookUserSlot")]
        public IActionResult ApproveSlotBook(string userId, string bookingId)
        {
            bool approvalStatus = _adminService.ApproveSlotBook(userId, bookingId);

            if(approvalStatus)
            {
                List<AdminDetailsDto_UserWithPendingApproval> _userswWithPendingApproval = _adminService.FetchUsersWithPendingApproval();
                return PartialView("_FilteredUsersTablePartial", _userswWithPendingApproval.ToList());

            }
            else
            {
                // return error message
                return NotFound($"Approval failed due to unexpected error for user - {userId}"); 
            }

        } 
    

        [HttpGet("IncreaseSlots")]
        public IActionResult IncreaseSlots(string hospitalId, int increaseBy)
        {
            _adminService.UpdateAvailableSlotsById(hospitalId, increaseBy);

            List<AdminDetailsDto_HospitalsList> _fetchedHospitalsDetails = _adminService.FetchHospitalsList();
            ViewBag.FetchedHospitalsDetails = _fetchedHospitalsDetails;

            if(_fetchedHospitalsDetails.Count > 0)
            {
                return PartialView("_UpdatedSlotHospitalTablePartial", _fetchedHospitalsDetails);
            }

            // return error page
            return PartialView("_AdminErrorMsgPartial", _fetchedHospitalsDetails);
        
        }

    
    }
}