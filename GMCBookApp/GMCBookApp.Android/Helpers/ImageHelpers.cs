﻿using System;
using Android.Media;
using Android.Graphics;
using Android.Hardware.Camera2;
using System.IO;

namespace SelectMultiIpleImagesApp.Droid.Helpers
{
	public static class ImageHelpers
	{
		public static string SaveFile(string collectionName, byte[] imageByte, string fileName)
		{
			var fileDir = new Java.IO.File(Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryPictures), collectionName);
			if (!fileDir.Exists())
			{
				fileDir.Mkdirs();
			}

			var file = new Java.IO.File(fileDir, fileName);
			System.IO.File.WriteAllBytes(file.Path, imageByte);

			return file.Path;

		}

		public static byte[] RotateImage(string path)
		{
			byte[] imageBytes;

			var originalImage = BitmapFactory.DecodeFile(path);
			var rotation = GetRotation(path);
			var width = (originalImage.Width * 0.25);
			var height = (originalImage.Height * 0.25);
			var scaledImage = Bitmap.CreateScaledBitmap(originalImage, (int)width, (int)height, true);

			Bitmap rotatedImage = scaledImage;
			if (rotation != 0)
			{
				var matrix = new Matrix();
				matrix.PostRotate(rotation);
				rotatedImage = Bitmap.CreateBitmap(scaledImage, 0, 0, scaledImage.Width, scaledImage.Height, matrix, true);
				scaledImage.Recycle();
				scaledImage.Dispose();
			}

			using (var ms = new MemoryStream())
			{
				rotatedImage.Compress(Bitmap.CompressFormat.Jpeg, 90, ms);
				imageBytes = ms.ToArray();
			}

			originalImage.Recycle();
			rotatedImage.Recycle();
			originalImage.Dispose();
			rotatedImage.Dispose();
			GC.Collect();

			return imageBytes;
		}

		private static int GetRotation(string filePath)
		{
			using (var ei = new ExifInterface(filePath))
			{
				var orientation = (Android.Media.Orientation)ei.GetAttributeInt(ExifInterface.TagOrientation, (int)Android.Media.Orientation.Normal);

				switch (orientation)
				{
					case Android.Media.Orientation.Rotate90:
						return 90;
					case Android.Media.Orientation.Rotate180:
						return 180;
					case Android.Media.Orientation.Rotate270:
						return 270;
					default:
						return 0;
				}
			}
		}
	}
}
