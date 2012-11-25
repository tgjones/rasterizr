using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Security;

namespace Rasterizr.Platform.WindowsForms
{
	/// <summary>
	/// Internal class to interact with Native Message
	/// </summary>
	internal class Win32Native
	{
		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
		public class LogFont
		{
			public int lfHeight;
			public int lfWidth;
			public int lfEscapement;
			public int lfOrientation;
			public int lfWeight;
			public byte lfItalic;
			public byte lfUnderline;
			public byte lfStrikeOut;
			public byte lfCharSet;
			public byte lfOutPrecision;
			public byte lfClipPrecision;
			public byte lfQuality;
			public byte lfPitchAndFamily;
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 0x20)]
			public string lfFaceName;
		}

		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		public struct TextMetric
		{
			public int tmHeight;
			public int tmAscent;
			public int tmDescent;
			public int tmInternalLeading;
			public int tmExternalLeading;
			public int tmAveCharWidth;
			public int tmMaxCharWidth;
			public int tmWeight;
			public int tmOverhang;
			public int tmDigitizedAspectX;
			public int tmDigitizedAspectY;
			public char tmFirstChar;
			public char tmLastChar;
			public char tmDefaultChar;
			public char tmBreakChar;
			public byte tmItalic;
			public byte tmUnderlined;
			public byte tmStruckOut;
			public byte tmPitchAndFamily;
			public byte tmCharSet;
		}

		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
		public struct TextMetricA
		{
			public int tmHeight;
			public int tmAscent;
			public int tmDescent;
			public int tmInternalLeading;
			public int tmExternalLeading;
			public int tmAveCharWidth;
			public int tmMaxCharWidth;
			public int tmWeight;
			public int tmOverhang;
			public int tmDigitizedAspectX;
			public int tmDigitizedAspectY;
			public byte tmFirstChar;
			public byte tmLastChar;
			public byte tmDefaultChar;
			public byte tmBreakChar;
			public byte tmItalic;
			public byte tmUnderlined;
			public byte tmStruckOut;
			public byte tmPitchAndFamily;
			public byte tmCharSet;
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct NativeMessage
		{
			public IntPtr handle;
			public uint msg;
			public IntPtr wParam;
			public IntPtr lParam;
			public uint time;
			public Point p;
		}

		[DllImport("user32.dll", EntryPoint = "PeekMessage"), SuppressUnmanagedCodeSecurity]
		public static extern int PeekMessage(out NativeMessage lpMsg, IntPtr hWnd, int wMsgFilterMin,
											  int wMsgFilterMax, int wRemoveMsg);

		[DllImport("user32.dll", EntryPoint = "GetMessage"), SuppressUnmanagedCodeSecurity]
		public static extern int GetMessage(out NativeMessage lpMsg, IntPtr hWnd, int wMsgFilterMin,
											 int wMsgFilterMax);

		[DllImport("user32.dll", EntryPoint = "TranslateMessage"), SuppressUnmanagedCodeSecurity]
		public static extern int TranslateMessage(ref NativeMessage lpMsg);

		[DllImport("user32.dll", EntryPoint = "DispatchMessage"), SuppressUnmanagedCodeSecurity]
		public static extern int DispatchMessage(ref NativeMessage lpMsg);

		public enum WindowLongType : int
		{
			WndProc = (-4),
			HInstance = (-6),
			HwndParent = (-8),
			Style = (-16),
			ExtendedStyle = (-20),
			UserData = (-21),
			Id = (-12)
		}

		public delegate IntPtr WndProc(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

		public static IntPtr GetWindowLong(HandleRef hWnd, WindowLongType index)
		{
			if (IntPtr.Size == 4)
			{
				return GetWindowLong32(hWnd, index);
			}
			return GetWindowLong64(hWnd, index);
		}

		[DllImport("user32.dll", EntryPoint = "GetWindowLong", CharSet = CharSet.Ansi)]
		private static extern IntPtr GetWindowLong32(HandleRef hwnd, WindowLongType index);

		[DllImport("user32.dll", EntryPoint = "GetWindowLongPtr", CharSet = CharSet.Ansi)]
		private static extern IntPtr GetWindowLong64(HandleRef hwnd, WindowLongType index);

		public static IntPtr SetWindowLong(HandleRef hwnd, WindowLongType index, WndProc wndProc)
		{
			if (IntPtr.Size == 4)
			{
				return SetWindowLong32(hwnd, index, wndProc);
			}
			return SetWindowLongPtr64(hwnd, index, wndProc);
		}

		[DllImport("user32.dll", EntryPoint = "SetWindowLong", CharSet = CharSet.Ansi)]
		private static extern IntPtr SetWindowLong32(HandleRef hwnd, WindowLongType index, WndProc wndProc);

		[DllImport("user32.dll", EntryPoint = "SetWindowLongPtr", CharSet = CharSet.Ansi)]
		private static extern IntPtr SetWindowLongPtr64(HandleRef hwnd, WindowLongType index, WndProc wndProc);

		[DllImport("user32.dll", EntryPoint = "CallWindowProc", CharSet = CharSet.Ansi)]
		public static extern IntPtr CallWindowProc(IntPtr wndProc, IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam);

		[DllImport("kernel32.dll", EntryPoint = "GetModuleHandle", CharSet = CharSet.Ansi)]
		public static extern IntPtr GetModuleHandle(string lpModuleName);
	}
}