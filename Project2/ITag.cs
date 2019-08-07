using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace KVDrive
{
    public abstract class ITag : IComparable<ITag>
    {
        public ITag(short id,Storage value)
        {
            this._id = id;
            this._value = value;
            
        }
        //private DateTime _timeStamp;
        private short _id;
        

        //public DateTime TimeStamp { get { return _timeStamp; } }
        public short ID { get { return _id; } }
        protected Storage _value;
        public Storage Value
        {
            get
            {
                return _value;
            }
        }

        public string Address { get { return _address; } set { _address = value; } }
        public void Update(Storage newValue)
        {
            if (_value.Equals(newValue)) return;
            if (ValueChangeEvent != null)
            {
                ValueChangeEvent(this, new ValueChangeEventArgs(newValue));
            }
        }

        public abstract Storage Read();

        public abstract int Write(object value);
        public int CompareTo(ITag other)
        {
            if (this.Equals(other))
            {
                return 0;
            }
            else
            {
                return 1;
            }
        }
        public ValueChangeHandler ValueChangeEvent;
        private string _address;
    }

    [StructLayout(LayoutKind.Explicit, Size = 4)]
    public struct Storage
    {
        // Fields
        [FieldOffset(0)]
        public bool Boolean;
        [FieldOffset(0)]
        public byte Byte;
        [FieldOffset(0)]
        public short Int16;
        [FieldOffset(0)]
        public int Int32;
        [FieldOffset(0)]
        public float Single;
        [FieldOffset(0)]
        public ushort Word;
        [FieldOffset(0)]
        public uint DWord;

        public static readonly Storage Empty;

        static Storage()
        {
            Empty = new Storage();
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            Type type = obj.GetType();
            if (type == typeof(Storage))
                return this.Int32 == ((Storage)obj).Int32;
            else
            {
                if (type == typeof(int))
                    return this.Int32 == (int)obj;
                if (type == typeof(short))
                    return this.Int16 == (short)obj;
                if (type == typeof(byte))
                    return this.Byte == (byte)obj;
                if (type == typeof(bool))
                    return this.Boolean == (bool)obj;
                if (type == typeof(float))
                    return this.Single == (float)obj;
                if (type == typeof(ushort))
                    return this.Word == (ushort)obj;
                if (type == typeof(uint))
                    return this.DWord == (uint)obj;
                if (type == typeof(string))
                    return this.ToString() == obj.ToString();
            }
            return false;
        }
    }
    public delegate void ValueChangeHandler(ITag sender, ValueChangeEventArgs e);
    public class ValueChangeEventArgs : EventArgs
    {
        public ValueChangeEventArgs(Storage value)
        {
            this.Value = value;
        }

        public Storage Value;
    }
}
