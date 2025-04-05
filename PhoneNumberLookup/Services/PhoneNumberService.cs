using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using PhoneNumberLookup.Models;

namespace PhoneNumberLookup.Services
{
    /// <summary>
    /// 提供手机号码查询服务
    /// </summary>
    public interface IPhoneNumberService
    {
        /// <summary>
        /// 根据手机号码查询归属地信息
        /// </summary>
        /// <param name="phoneNumber">11位手机号码</param>
        /// <returns>手机号码信息</returns>
        Task<PhoneNumberLookupResult> LookupPhoneNumberAsync(string phoneNumber);

        /// <summary>
        /// 初始化服务，加载数据
        /// </summary>
        Task InitializeAsync();
    }

    /// <summary>
    /// 手机号码查询服务实现
    /// </summary>
    public class PhoneNumberService : IPhoneNumberService
    {
        private readonly ILogger<PhoneNumberService> _logger;
        private readonly Dictionary<string, PhoneNumberInfo> _phoneNumberData;
        private readonly string _csvFilePath;

        public PhoneNumberService(ILogger<PhoneNumberService> logger)
        {
            _logger = logger;
            _phoneNumberData = new Dictionary<string, PhoneNumberInfo>();
            _csvFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "PhoneNumber.csv");
        }

        /// <summary>
        /// 初始化服务，加载CSV数据
        /// </summary>
        public async Task InitializeAsync()
        {
            try
            {
                await LoadPhoneNumberDataAsync();
                _logger.LogInformation("手机号码数据加载完成，共加载 {Count} 条记录", _phoneNumberData.Count);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "加载手机号码数据时发生错误");
                throw;
            }
        }

        /// <summary>
        /// 根据手机号码查询归属地信息
        /// </summary>
        /// <param name="phoneNumber">11位手机号码</param>
        /// <returns>手机号码信息</returns>
        public Task<PhoneNumberLookupResult> LookupPhoneNumberAsync(string phoneNumber)
        {
            // 验证手机号码格式
            if (string.IsNullOrWhiteSpace(phoneNumber))
            {
                return Task.FromResult(new PhoneNumberLookupResult
                {
                    Success = false,
                    Message = "手机号码不能为空"
                });
            }

            // 验证手机号码长度
            if (phoneNumber.Length != 11)
            {
                return Task.FromResult(new PhoneNumberLookupResult
                {
                    Success = false,
                    Message = "手机号码必须为11位"
                });
            }

            // 验证手机号码是否以1开头
            if (!phoneNumber.StartsWith("1"))
            {
                return Task.FromResult(new PhoneNumberLookupResult
                {
                    Success = false,
                    Message = "手机号码必须以1开头"
                });
            }

            // 验证手机号码是否全为数字
            if (!phoneNumber.All(char.IsDigit))
            {
                return Task.FromResult(new PhoneNumberLookupResult
                {
                    Success = false,
                    Message = "手机号码必须全为数字"
                });
            }

            // 获取前7位作为查询键
            string segment = phoneNumber.Substring(0, 7);

            // 查询手机号码信息
            if (_phoneNumberData.TryGetValue(segment, out var phoneNumberInfo))
            {
                return Task.FromResult(new PhoneNumberLookupResult
                {
                    Success = true,
                    Data = phoneNumberInfo
                });
            }

            return Task.FromResult(new PhoneNumberLookupResult
            {
                Success = false,
                Message = "未找到该手机号码的归属地信息"
            });
        }

        /// <summary>
        /// 加载CSV文件中的手机号码数据
        /// </summary>
        private async Task LoadPhoneNumberDataAsync()
        {
            if (!File.Exists(_csvFilePath))
            {
                throw new FileNotFoundException("手机号码数据文件不存在", _csvFilePath);
            }

            try
            {
                // 读取CSV文件
                string[] lines = await File.ReadAllLinesAsync(_csvFilePath);

                // 跳过标题行
                for (int i = 1; i < lines.Length; i++)
                {
                    string line = lines[i];
                    if (string.IsNullOrWhiteSpace(line))
                    {
                        continue;
                    }

                    try
                    {
                        // 解析CSV行
                        string[] fields = line.Split(',');
                        if (fields.Length < 8)
                        {
                            _logger.LogWarning("第 {LineNumber} 行数据格式不正确: {Line}", i + 1, line);
                            continue;
                        }

                        // 创建手机号码信息对象
                        var phoneNumberInfo = new PhoneNumberInfo
                        {
                            Prefix = fields[0],
                            Segment = fields[1],
                            Province = fields[2],
                            City = fields[3],
                            ServiceProvider = fields[4],
                            AreaCode = fields[5],
                            PostalCode = fields[6],
                            AreaNumber = fields[7]
                        };

                        // 添加到字典中，使用号段作为键
                        _phoneNumberData[phoneNumberInfo.Segment] = phoneNumberInfo;
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "解析第 {LineNumber} 行数据时发生错误: {Line}", i + 1, line);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "读取手机号码数据文件时发生错误");
                throw;
            }
        }
    }
}