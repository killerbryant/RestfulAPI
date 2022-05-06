using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace RestfulAPI.Utils
{
    public class JwtHelper
    {
        public static string IssueJWT(TokenModelJWT tokenModel)
        {
            var dateTime = DateTime.UtcNow;
            string iss = AppSettings.GetConfigure("Audience:Issuer");
            string aud = AppSettings.GetConfigure("Audience:Audience");
            string secret = AppSettings.GetConfigure("Audience:Secret");

            //var claims = new Claim[] //old
            var claims = new List<Claim>
                {
                    //下邊為Claim的預設配置
                new Claim(JwtRegisteredClaimNames.Jti, tokenModel.Uid.ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, $"{new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds()}"),
                new Claim(JwtRegisteredClaimNames.Nbf,$"{new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds()}") ,
                //這個就是過期時間，目前是過期100秒，可自訂，注意JWT有自己的緩衝過期時間
                new Claim (JwtRegisteredClaimNames.Exp,$"{new DateTimeOffset(DateTime.Now.AddSeconds(100)).ToUnixTimeSeconds()}"),
                new Claim(JwtRegisteredClaimNames.Iss,iss),
                new Claim(JwtRegisteredClaimNames.Aud,aud),
                
                //new Claim(ClaimTypes.Role,tokenModel.Role),//為瞭解決一個用戶多個角色(比如：Admin,System)，用下邊的方法
               };

            // 可以將一個使用者的多個角色全部賦予；
            // 作者：DX 提供技術支援；
            claims.AddRange(tokenModel.Role.Split(',').Select(s => new Claim(ClaimTypes.Role, s)));


            //秘鑰 (SymmetricSecurityKey 對安全性的要求，金鑰的長度太短會報出異常)
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var jwt = new JwtSecurityToken(
                issuer: iss,
                claims: claims,
                signingCredentials: creds);

            var jwtHandler = new JwtSecurityTokenHandler();
            var encodedJwt = jwtHandler.WriteToken(jwt);

            return encodedJwt;
        }

    }
    /// <summary>
    /// 權杖
    /// </summary>
    public class TokenModelJWT
    {
        /// <summary>
        /// Id
        /// </summary>
        public long Uid { get; set; }
        /// <summary>
        /// 角色
        /// </summary>
        public string Role { get; set; }
        /// <summary>
        /// 職能
        /// </summary>
        public string Work { get; set; }

    }

}
