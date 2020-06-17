using Redis.Redis_Helper;
using System;
using System.Collections.Generic;
using System.Text;
using Tool;

namespace Redis
{
    /// <summary>
    /// Redis存储用户登录状态类，实现分布式登录
    /// </summary>
    public class RedisSession
    {
        private CookieHelper cookieHelper;
        RedisHelper _Redis_Operate = new RedisHelper();

        public RedisSession(bool IsReadOnly, int Timeout)
        {
            this.cookieHelper = new CookieHelper();
            this.IsReadOnly = IsReadOnly;
            this.Timeout = Timeout;
            //更新缓存Key过期时间
            _Redis_Operate.KeyExpire(SessionID, TimeSpan.FromMinutes(Timeout));
        }

        /// <summary>
        /// SessionId标识符
        /// </summary>
        public static string SessionName = "Redis_SessionId";


        /// <summary>
        /// 摘要:获取一个值，该值指示会话是否为只读。
        /// 返回结果: 如果会话为只读，则为 true；否则为 false。
        /// </summary>
        public bool IsReadOnly { get; set; }


        /// <summary>
        /// 摘要:获取会话的唯一标识符。
        /// 返回结果:唯一会话标识符。
        /// </summary>
        public string SessionID
        {
            get
            {
                return GetSessionID();
            }
        }

        //
        // 摘要:
        //     获取并设置在会话状态提供程序终止会话之前各请求之间所允许的时间（以分钟为单位）。
        //
        // 返回结果:
        //     超时期限（以分钟为单位）。
        public int Timeout { get; set; }

        /// <summary>
        /// 获取SessionID
        /// </summary>
        /// <param name="key">SessionId标识符</param>
        /// <returns>HttpCookie值</returns>
        private string GetSessionID()
        {
            var cookie = cookieHelper.GetValue(SessionName);
            if (cookie == null || string.IsNullOrEmpty(cookie))
            {
                string newSessionID = Guid.NewGuid().ToString();
                cookieHelper.AddCookie(SessionName, newSessionID, Timeout);
                return "Session_" + newSessionID;
            }
            else
            {
                return "Session_" + cookie;
            }
        }

        //
        // 摘要:
        //     按名称获取或设置会话值。
        //
        // 参数:
        //   name:
        //     会话值的键名。
        //
        // 返回结果:
        //     具有指定名称的会话状态值；如果该项不存在，则为 null。
        public object this[string name]
        {
            get
            {
                return _Redis_Operate.HashGet<object>(SessionID, name);
            }
            set
            {
                _Redis_Operate.HashSet<object>(SessionID, name, value);
            }
        }


        /// <summary>
        /// 摘要:判断会话中是否存在指定key
        /// </summary>
        /// <param name="name">键值</param>
        /// <returns></returns>
        public bool IsExistKey(string name)
        {
            return _Redis_Operate.HashExists(SessionID, name);
        }


        /// <summary>
        /// 向会话状态集合添加一个新项
        /// </summary>
        /// <param name="name">要添加到会话状态集合的项的名称</param>
        /// <param name="value">要添加到会话状态集合的项的值</param>
        public void Add(string name, object value)
        {
            _Redis_Operate.HashSet<object>(SessionID, name, value);
        }


        /// <summary>
        ///  摘要:从会话状态集合中移除所有的键和值。
        /// </summary>
        public void Clear()
        {
            _Redis_Operate.HashDelete(SessionID);
        }


        /// <summary>
        /// 摘要:删除会话状态集合中的项。
        /// </summary>
        /// <param name="name">要从会话状态集合中删除的项的名称</param>
        public void Remove(string name)
        {
            _Redis_Operate.HashDelete(SessionID, name);
        }


        /// <summary>
        ///  摘要:从会话状态集合中移除所有的键和值。
        /// </summary>
        public void RemoveAll()
        {
            Clear();
        }
    }
}

