﻿using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;

namespace PM_Ban_Do_An_Nhanh.Helpers
{
    public static class ImageHelper
    {
        private static readonly string ImageDirectory = Path.Combine(Application.StartupPath, "Images", "MenuItems");

        static ImageHelper()
        {
            // Tạo thư mục nếu chưa tồn tại
            if (!Directory.Exists(ImageDirectory))
            {
                Directory.CreateDirectory(ImageDirectory);
            }
        }

        public static string SaveMenuItemImage(string sourceImagePath, int menuItemId, string menuItemName)
        {
            try
            {
                if (string.IsNullOrEmpty(sourceImagePath) || !File.Exists(sourceImagePath))
                    return null;

                // Tạo tên file duy nhất
                string extension = Path.GetExtension(sourceImagePath);
                string safeFileName = GetSafeFileName(menuItemName);
                string fileName = $"{menuItemId}_{safeFileName}{extension}";
                string destinationPath = Path.Combine(ImageDirectory, fileName);

                // Xóa ảnh cũ nếu tồn tại
                DeleteOldImage(menuItemId);

                // Copy và resize ảnh
                using (var originalImage = Image.FromFile(sourceImagePath))
                {
                    using (var resizedImage = ResizeImage(originalImage, 300, 300))
                    {
                        resizedImage.Save(destinationPath, GetImageFormat(extension));
                    }
                }

                return Path.Combine("Images", "MenuItems", fileName); // Trả về đường dẫn tương đối
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lưu ảnh: {ex.Message}");
            }
        }

        public static void DeleteMenuItemImage(string imagePath)
        {
            try
            {
                if (string.IsNullOrEmpty(imagePath)) return;

                string fullPath = Path.Combine(Application.StartupPath, imagePath);
                if (File.Exists(fullPath))
                {
                    File.Delete(fullPath);
                }
            }
            catch (Exception)
            {
                // Log error nhưng không throw để không ảnh hưởng đến việc xóa dữ liệu
            }
        }

        public static Image LoadMenuItemImage(string imagePath)
        {
            try
            {
                if (string.IsNullOrEmpty(imagePath)) return null;

                string fullPath = Path.Combine(Application.StartupPath, imagePath);
                if (File.Exists(fullPath))
                {
                    return Image.FromFile(fullPath);
                }
            }
            catch (Exception)
            {
                // Return null nếu không load được ảnh
            }
            return null;
        }

        private static void DeleteOldImage(int menuItemId)
        {
            try
            {
                string pattern = $"{menuItemId}_*";
                string[] files = Directory.GetFiles(ImageDirectory, pattern);
                foreach (string file in files)
                {
                    File.Delete(file);
                }
            }
            catch (Exception)
            {
                // Ignore errors khi xóa ảnh cũ
            }
        }

        private static string GetSafeFileName(string fileName)
        {
            string safe = fileName;
            foreach (char c in Path.GetInvalidFileNameChars())
            {
                safe = safe.Replace(c, '_');
            }
            return safe.Length > 50 ? safe.Substring(0, 50) : safe;
        }

        private static Image ResizeImage(Image image, int maxWidth, int maxHeight)
        {
            int newWidth = image.Width;
            int newHeight = image.Height;

            // Calculate new dimensions while maintaining aspect ratio
            if (image.Width > maxWidth || image.Height > maxHeight)
            {
                double ratioX = (double)maxWidth / image.Width;
                double ratioY = (double)maxHeight / image.Height;
                double ratio = Math.Min(ratioX, ratioY);

                newWidth = (int)(image.Width * ratio);
                newHeight = (int)(image.Height * ratio);
            }

            Bitmap newImage = new Bitmap(newWidth, newHeight);
            using (Graphics graphics = Graphics.FromImage(newImage))
            {
                graphics.DrawImage(image, 0, 0, newWidth, newHeight);
            }
            return newImage;
        }

        private static ImageFormat GetImageFormat(string extension)
        {
            switch (extension.ToLower())
            {
                case ".jpg":
                case ".jpeg":
                    return ImageFormat.Jpeg;
                case ".png":
                    return ImageFormat.Png;
                case ".bmp":
                    return ImageFormat.Bmp;
                default:
                    return ImageFormat.Jpeg;
            }
        }
    }
}