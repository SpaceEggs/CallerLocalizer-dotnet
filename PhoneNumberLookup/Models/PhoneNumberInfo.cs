using System;

namespace PhoneNumberLookup.Models
{
    /// <summary>
    /// 表示手机号码信息的模型类
    /// </summary>
    public class PhoneNumberInfo
    {
        /// <summary>
        /// 号码前缀（前3位）
        /// </summary>
        // 属性示例
        public string? Prefix { get; set; }

        /// <summary>
        /// 号段（前7位）
        /// </summary>
        public string Segment { get; set; } = string.Empty;

        /// <summary>
        /// 省区
        /// </summary>
        public string Province { get; set; } = string.Empty;

        /// <summary>
        /// 城市
        /// </summary>
        public string City { get; set; } = string.Empty;

        /// <summary>
        /// 服务商
        /// </summary>
        public string ServiceProvider { get; set; } = string.Empty;

        /// <summary>
        /// 区号
        /// </summary>
        public string AreaCode { get; set; } = string.Empty;

        /// <summary>
        /// 邮编
        /// </summary>
        public string PostalCode { get; set; } = string.Empty;

        /// <summary>
        /// 区划代码
        /// </summary>
        public string AreaNumber { get; set; } = string.Empty;
    }

    /// <summary>
    /// 表示手机号码查询结果的模型类
    /// </summary>
    public class PhoneNumberLookupResult
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// 错误消息（如果有）
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// 手机号码信息
        /// </summary>
        public PhoneNumberInfo? Data { get; set; }
    }
}