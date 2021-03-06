﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ClownFish.Base.Http;
using ClownFish.HttpServer.Web;

namespace ClownFish.HttpServer.Handlers
{
	/// <summary>
	/// 执行请求发生异常时的处理器类型，
	/// 可重写这个类型中的ProcessRequest方法实现自己的异常处理方式
	/// </summary>
	public class ErrorHandler : IHttpHandler
	{
		/// <summary>
		/// 当时已发生的异常
		/// </summary>
		public Exception Exception { get; internal set; }

		/// <summary>
		/// 重写基类方法，处理异常的请求
		/// </summary>
		/// <param name="context"></param>
		public virtual void ProcessRequest(HttpContext context)
		{
            ResetHeaderes(context);
            WriteException(context);
        }

        /// <summary>
        /// 重新设置请求头
        /// </summary>
        public virtual void ResetHeaderes(HttpContext context)
        {
            context.Response.Headers.Clear();
            context.Response.SetBasicHeaders();
        }


        /// <summary>
        /// 输出异常内容
        /// </summary>
        /// <param name="context"></param>
        public virtual void WriteException(HttpContext context)
        {
            System.Web.HttpException httpException = this.Exception as System.Web.HttpException;
            if( httpException != null ) {
                context.Response.StatusCode = httpException.GetHttpCode();
                context.Response.ContentType = ResponseContentType.Text;
                context.Response.Write(httpException.Message);
            }
            else {
                context.Response.StatusCode = 500;
                context.Response.ContentType = ResponseContentType.Text;

                // 将错误消息写入响应流
                context.Response.Write(this.Exception.ToString());
            }
        }


	}
}
