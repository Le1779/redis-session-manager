// <copyright file="JwtManager.cs" company="DynaComware Corp. All Rights Reserved.">
// Copyright 2021, DynaComware Corp. All Rights Reserved.
//
// All rights reserved. DynaComware source code is an unpublished work and the
// use of a copyright notice does not imply otherwise. This source code contains
// confidential, trade secret material of DynaComware. Any attempt or participation
// in deciphering, decoding, reverse engineering or in any way altering the source
// code is strictly prohibited, unless the prior written consent of Company Name
// is obtained.
// </copyright>
// <date>       2021/06/17 </date>
// <brief>
//
// </brief>
// <author>     Kevin Le
// </author>
namespace AuthServer.Models
{
    using JWT.Algorithms;
    using JWT.Builder;

    /// <summary>
    /// JwtManager
    /// </summary>
    public class JwtManager
    {
        private readonly string secret = "gYU9HulYRCJGYO3F1ohFtjXwZ9lUIcL1HZxIpjJSgBtxQVF00I629vNkMP8wWak";

        /// <summary>
        /// 產生JWT
        /// </summary>
        /// <param name="data">打包的資料</param>
        /// <returns>JWT</returns>
        public string Generate(string data)
        {
            var token = JwtBuilder.Create()
                .WithAlgorithm(new HMACSHA256Algorithm())
                .WithSecret(secret)
                .AddClaim("data", data)
                .Encode();
            return token;
        }

        /// <summary>
        /// 驗證JWT
        /// </summary>
        /// <param name="token">jwt</param>
        /// <returns>如果通過驗證，回傳打包的資料，否則丟出例外</returns>
        public string Validate(string token) 
        {
            var json = JwtBuilder.Create()
                     .WithAlgorithm(new HMACSHA256Algorithm())
                     .WithSecret(secret)
                     .MustVerifySignature()
                     .Decode(token);
            return json;
        }
    }
}