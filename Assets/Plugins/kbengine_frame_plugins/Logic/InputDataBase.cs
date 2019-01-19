using System;
using System.Collections.Generic;

namespace KBEngine
{
	[Serializable]
	public  class InputDataBase : ObjectPool<InputDataBase> 
    {
		public int ownerID;

		public InputDataBase()
		{
		}

		public virtual void Serialize(List<byte> bytes) { }

		public virtual void Deserialize(byte[] data, ref int offset) { }

        public virtual FS_ENTITY_DATA Serialize() { return new FS_ENTITY_DATA(); }

        public virtual void Deserialize(FS_ENTITY_DATA e) { }

        public virtual bool EqualsData(InputDataBase otherBase) { return false; }

        public virtual void CleanUp() { }

        public virtual void CopyFrom(InputDataBase fromBase) { }
    }



}
