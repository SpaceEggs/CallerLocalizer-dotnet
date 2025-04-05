using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PhoneNumberLookup.Models;
using PhoneNumberLookup.Services;

namespace PhoneNumberLookup.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PhoneNumberController : ControllerBase
    {
        private readonly ILogger<PhoneNumberController> _logger;
        private readonly IPhoneNumberService _phoneNumberService;

        public PhoneNumberController(ILogger<PhoneNumberController> logger, IPhoneNumberService phoneNumberService)
        {
            _logger = logger;
            _phoneNumberService = phoneNumberService;
        }

        /// <summary>
        /// 查询手机号码归属地信息
        /// </summary>
        /// <param name="phoneNumber">11位手机号码</param>
        /// <returns>手机号码归属地信息</returns>
        [HttpGet("{phoneNumber}")]
        public async Task<IActionResult> Get(string phoneNumber)
        {
            try
            {
                _logger.LogInformation("接收到手机号码查询请求: {PhoneNumber}", phoneNumber);
                var result = await _phoneNumberService.LookupPhoneNumberAsync(phoneNumber);

                if (result.Success)
                {
                    return Ok(result);
                }
                else
                {
                    return BadRequest(result);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "查询手机号码时发生错误: {PhoneNumber}", phoneNumber);
                return StatusCode(500, new PhoneNumberLookupResult
                {
                    Success = false,
                    Message = "服务器内部错误"
                });
            }
        }
    }
}