using PM_Ban_Do_An_Nhanh.Entities;
using System;
using System.Collections.Generic;
using System.Data;

namespace PM_Ban_Do_An_Nhanh.Helpers
{
    /// <summary>
    /// Cache helper để lưu danh sách món/danh mục
    /// </summary>
    public static class CacheManager
    {
        private static Dictionary<string, CacheItem> _cache = new Dictionary<string, CacheItem>();
        private static readonly int DefaultCacheDuration = 5; // 5 phút

        private class CacheItem
        {
            public object Data { get; set; }
            public DateTime ExpiryTime { get; set; }
        }

        /// <summary>
        /// Lấy data từ cache
        /// </summary>
        public static T Get<T>(string key) where T : class
        {
            if (_cache.ContainsKey(key))
            {
                var item = _cache[key];
                if (DateTime.Now < item.ExpiryTime)
                {
                    return item.Data as T;
                }
                else
                {
                    // Cache expired, remove it
                    _cache.Remove(key);
                }
            }
            return null;
        }

        /// <summary>
        /// Lưu data vào cache
        /// </summary>
        public static void Set(string key, object data, int durationMinutes = -1)
        {
            if (durationMinutes == -1)
                durationMinutes = DefaultCacheDuration;

            var item = new CacheItem
            {
                Data = data,
                ExpiryTime = DateTime.Now.AddMinutes(durationMinutes)
            };

            if (_cache.ContainsKey(key))
                _cache[key] = item;
            else
                _cache.Add(key, item);
        }

        /// <summary>
        /// Xóa cache theo key
        /// </summary>
        public static void Remove(string key)
        {
            if (_cache.ContainsKey(key))
                _cache.Remove(key);
        }

        /// <summary>
        /// Xóa tất cả cache
        /// </summary>
        public static void Clear()
        {
            _cache.Clear();
        }

        /// <summary>
        /// Invalidate cache khi có thay đổi data
        /// </summary>
        public static void InvalidateMenuCache()
        {
            Remove("MonAn_All");
            Remove("DanhMuc_All");
        }

        public static void InvalidateCustomerCache()
        {
            Remove("KhachHang_All");
        }
    }
}
