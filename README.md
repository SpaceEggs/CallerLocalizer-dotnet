# 手机号码查询API服务

这是一个基于ASP.NET Core的手机号码查询API服务，可以根据输入的11位手机号码查询其归属地信息。

## 功能特点

- 支持查询以1开头的11位手机号码
- 通过号段匹配手机号码的前7位获取归属地信息
- 返回手机号码的省区、城市、服务商、区号、邮编、区划代码等信息
- 采用JSON格式输出查询结果
- 具有良好的错误处理和输入验证机制

## 技术实现

- 使用ASP.NET Core Web API框架
- 数据存储在PhoneNumber.csv文件中
- 采用内存缓存提高查询性能
- 实现了完善的输入验证和错误处理

## API使用说明

### 查询手机号码归属地

```
GET /api/PhoneNumber/{phoneNumber}
```

#### 参数说明

- `phoneNumber`: 11位手机号码，必须以1开头

#### 返回示例

成功响应：

```json
{
  "success": true,
  "message": null,
  "data": {
    "prefix": "130",
    "segment": "1300000",
    "province": "山东",
    "city": "济南",
    "serviceProvider": "中国联通",
    "areaCode": "0531",
    "postalCode": "250000",
    "areaNumber": "370100"
  }
}
```

错误响应：

```json
{
  "success": false,
  "message": "手机号码必须为11位",
  "data": null
}
```

## 运行说明

1. 确保已安装.NET SDK
2. 克隆项目到本地
3. 进入项目目录
4. 运行以下命令启动服务：

```bash
dotnet run
```

5. 服务默认运行在 https://localhost:8081 和 http://localhost:8080

## 项目结构

- `Controllers/PhoneNumberController.cs`: API控制器
- `Models/PhoneNumberInfo.cs`: 数据模型
- `Services/PhoneNumberService.cs`: 业务逻辑服务
- `PhoneNumber.csv`: 手机号码数据文件
- `Program.cs`: 应用程序入口和配置