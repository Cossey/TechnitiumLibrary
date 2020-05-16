﻿/*
Technitium Library
Copyright (C) 2020  Shreyas Zare (shreyas@technitium.com)

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program.  If not, see <http://www.gnu.org/licenses/>.

*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TechnitiumLibrary.IO;

namespace TechnitiumLibrary.Net.Dns.ResourceRecords
{
    public class DnsHINFORecord : DnsResourceRecordData
    {
        #region variables

        string _cpu;
        string _os;

        #endregion

        #region constructors

        public DnsHINFORecord(string cpu, string os)
        {
            _cpu = cpu;
            _os = os;
        }

        public DnsHINFORecord(Stream s)
            : base(s)
        { }

        public DnsHINFORecord(dynamic jsonResourceRecord)
        {
            _length = Convert.ToUInt16(jsonResourceRecord.data.Value.Length);

            string value = DnsDatagram.DecodeCharacterString(jsonResourceRecord.data.Value);
            string[] parts;

            if (value.Contains("\" \""))
                parts = value.Split(new string[] { "\" \"" }, StringSplitOptions.None);
            else
                parts = value.Split(new char[] { ' ' }, StringSplitOptions.None);

            _cpu = parts[0];

            if (parts.Length > 1)
                _os = parts[1];
        }

        #endregion

        #region protected

        protected override void Parse(Stream s)
        {
            _cpu = Encoding.ASCII.GetString(s.ReadBytes(s.ReadByte()));
            _os = Encoding.ASCII.GetString(s.ReadBytes(s.ReadByte()));
        }

        protected override void WriteRecordData(Stream s, List<DnsDomainOffset> domainEntries)
        {
            s.WriteByte(Convert.ToByte(_cpu.Length));
            s.Write(Encoding.ASCII.GetBytes(_cpu));

            s.WriteByte(Convert.ToByte(_os.Length));
            s.Write(Encoding.ASCII.GetBytes(_os));
        }

        #endregion

        #region public

        public override bool Equals(object obj)
        {
            if (obj is null)
                return false;

            if (ReferenceEquals(this, obj))
                return true;

            DnsHINFORecord other = obj as DnsHINFORecord;
            if (other == null)
                return false;

            return _cpu.Equals(other._cpu) && _os.Equals(other._os);
        }

        public override string ToString()
        {
            return DnsDatagram.EncodeCharacterString(_cpu) + " " + DnsDatagram.EncodeCharacterString(_cpu);
        }

        public override int GetHashCode()
        {
            int hashCode = -597155466;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(_cpu);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(_os);
            return hashCode;
        }

        #endregion

        #region properties

        public string CPU
        { get { return _cpu; } }

        public string OS
        { get { return _os; } }

        #endregion
    }
}
