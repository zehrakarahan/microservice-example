namespace AuthorizeCenter
{
    /// <summary>
    /// 请求响应实体
    /// </summary>
    public class ApiResultBase
    {
        /// <summary>
        /// 请求响应实体类
        /// </summary>
        public ApiResultBase()
        {
            Code = (int)ResultCode.Success;
            Message = "操作成功";
        }
       
        /// <summary>
        /// 响应代码
        /// </summary>
        public int Code { get; set; }
        /// <summary>
        /// 响应消息内容
        /// </summary>
        public string Message { get; set; }


        /// <summary>
        /// 设置响应状态为成功
        /// </summary>
        /// <param name="message"></param>
        public void SetSuccess(string message = "成功")
        {
            Code = (int)ResultCode.Success;
            Message = message;
        }
        /// <summary>
        /// 设置响应状态为失败
        /// </summary>
        /// <param name="message"></param>
        public void SetFailed(string message = "失败")
        {
            Message = message;
            Code = 999;
        }

        /// <summary>
        /// 设置响应状态为体验版(返回失败结果)
        /// </summary>
        /// <param name="message"></param>
        public void SetIsTrial(string message = "体验版,功能已被关闭")
        {
            Message = message;
            Code = 999;
        }

        /// <summary>
        /// 设置响应状态为错误
        /// </summary>
        /// <param name="message"></param>
        public void SetError(string message = "错误")
        {
            Code = (int)ResultCode.Error;
            Message = message;
        }

        /// <summary>
        /// 设置响应状态为未知资源
        /// </summary>
        /// <param name="message"></param>
        public void SetNotFound(string message = "未知资源")
        {
            Message = message;
            Code = (int)ResultCode.NotFound;
        }

        /// <summary>
        /// 设置响应状态为无权限
        /// </summary>
        /// <param name="message"></param>
        public void SetNoPermission(string message = "无权限")
        {
            Message = message;
            Code = (int)ResultCode.NoPermission;
        }

       
    }


    /// <summary>
    /// 请求响应实体，data包含返回数据
    /// </summary>
    public class ApiResult<T>: ApiResultBase
    {
        /// <summary>
        /// 请求响应实体类
        /// </summary>
        public ApiResult()
        {
            Code = (int)ResultCode.Success;
            Message = "操作成功";
        }

       
        /// <summary>
        /// 返回的响应数据
        /// </summary>
        public T Data { get; set; }


        /// <summary>
        /// 设置响应数据
        /// </summary>
        /// <param name="data"></param>
        public void SetData(T data)
        {
            Data = data;
        }
    }


    /// <summary>
    /// 状态码
    /// </summary>
    public enum ResultCode
    {
        /// <summary>
        /// 成功
        /// </summary>
        Success = 200,
        /// <summary>
        /// 无权限
        /// </summary>
        NoPermission = 401,
        /// <summary>
        /// 未知资源
        /// </summary>
        NotFound = 404,
        /// <summary>
        /// 一般错误
        /// </summary>
        Error = 400


    }
}
