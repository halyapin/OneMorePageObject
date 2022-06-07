using System;
using System.Collections.Generic;
using System.Text;

namespace OneMorePageObject
{
	public enum GuidVersion
	{
		TimeBased = 0x01,
		Reserved = 0x02,
		NameBased = 0x03,
		Random = 0x04
	}

	public static class GuidGenerator
	{
		public const int ByteArraySize = 16;

		public const int VariantByte = 8;
		public const int VariantByteMask = 0x3f;
		public const int VariantByteShift = 0x80;

		public const int VersionByte = 7;
		public const int VersionByteMask = 0x0f;
		public const int VersionByteShift = 4;

		private const byte TimestampByte = 0;
		private const byte GuidClockSequenceByte = 8;
		private const byte NodeByte = 10;

		private static readonly DateTimeOffset GregorianCalendarStart = new DateTimeOffset(1582, 10, 15, 0, 0, 0, TimeSpan.Zero);

		public static byte[] DefaultClockSequence { get; set; }
		public static byte[] DefaultNode { get; set; }

		static GuidGenerator()
		{
			DefaultClockSequence = new byte[2];
			DefaultNode = new byte[6];

			var random = new Random();
			random.NextBytes(DefaultClockSequence);
			random.NextBytes(DefaultNode);
		}

		public static DateTimeOffset GetDateTimeOffset(Guid guid)
		{
			byte[] bytes = guid.ToByteArray();

			bytes[VersionByte] &= (byte)VersionByteMask;
			bytes[VersionByte] |= (byte)((byte)GuidVersion.TimeBased >> VersionByteShift);

			byte[] timestampBytes = new byte[8];
			Array.Copy(bytes, TimestampByte, timestampBytes, 0, 8);

			long timestamp = BitConverter.ToInt64(timestampBytes, 0);
			long ticks = timestamp + GregorianCalendarStart.Ticks;

			return new DateTimeOffset(ticks, TimeSpan.Zero);
		}

		public static DateTime GetDateTime(Guid guid)
		{
			return GetDateTimeOffset(guid).DateTime;
		}

		public static DateTime GetLocalDateTime(Guid guid)
		{
			return GetDateTimeOffset(guid).LocalDateTime;
		}

		public static DateTime GetUtcDateTime(Guid guid)
		{
			return GetDateTimeOffset(guid).UtcDateTime;
		}

		public static Guid GenerateTimeBasedGuid()
		{
			return GenerateTimeBasedGuid(DateTimeOffset.UtcNow, DefaultClockSequence, DefaultNode);
		}

		public static Guid GenerateTimeBasedGuid(DateTime dateTime)
		{
			return GenerateTimeBasedGuid(dateTime, DefaultClockSequence, DefaultNode);
		}

		public static Guid GenerateTimeBasedGuid(DateTimeOffset dateTime)
		{
			return GenerateTimeBasedGuid(dateTime, DefaultClockSequence, DefaultNode);
		}

		public static Guid GenerateTimeBasedGuid(DateTime dateTime, byte[] clockSequence, byte[] node)
		{
			return GenerateTimeBasedGuid(new DateTimeOffset(dateTime), clockSequence, node);
		}

		public static Guid GenerateTimeBasedGuid(DateTimeOffset dateTime, byte[] clockSequence, byte[] node)
		{
			if (clockSequence == null)
				throw new ArgumentNullException("clockSequence");

			if (node == null)
				throw new ArgumentNullException("node");

			if (clockSequence.Length != 2)
				throw new ArgumentOutOfRangeException("clockSequence", "The clockSequence must be 2 bytes.");

			if (node.Length != 6)
				throw new ArgumentOutOfRangeException("node", "The node must be 6 bytes.");

			long ticks = (dateTime - GregorianCalendarStart).Ticks;
			byte[] guid = new byte[ByteArraySize];
			byte[] timestamp = BitConverter.GetBytes(ticks);

			Array.Copy(node, 0, guid, NodeByte, Math.Min(6, node.Length));

			Array.Copy(clockSequence, 0, guid, GuidClockSequenceByte, Math.Min(2, clockSequence.Length));

			Array.Copy(timestamp, 0, guid, TimestampByte, Math.Min(8, timestamp.Length));

			guid[VariantByte] &= (byte)VariantByteMask;
			guid[VariantByte] |= (byte)VariantByteShift;

			guid[VersionByte] &= (byte)VersionByteMask;
			guid[VersionByte] |= (byte)((byte)GuidVersion.TimeBased << VersionByteShift);

			return new Guid(guid);
		}
	}
}
