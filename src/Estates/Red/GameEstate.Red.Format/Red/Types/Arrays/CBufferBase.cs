using GameEstate.Red.Formats.Red.CR2W;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;

namespace GameEstate.Red.Formats.Red.Types.Arrays
{
    [REDMeta]
    public abstract class CBufferBase<T> : CVariable, IList<T>, IList, IBufferAccessor where T : CVariable
    {
        public CBufferBase(CR2WFile cr2w, CVariable parent, string name) : base(cr2w, parent, name) { }

        public List<T> elements { get; set; } = new List<T>();

        [Browsable(false)] public List<int> Flags { get; set; }

        public string Elementtype
        {
            get => REDReflection.GetREDTypeString(typeof(T));
            set => throw new NotImplementedException();
        }

        public Type InnerType => GetType().GetGenericArguments().Single();

        [Browsable(false)]
        public override string REDType => Flags != null
            ? REDReflection.GetREDTypeString(GetType(), Flags.ToArray())
            : REDReflection.GetREDTypeString(GetType());

        public override List<IEditableVariable> GetEditableVariables() => elements.Cast<IEditableVariable>().ToList();

        public override void Read(BinaryReader r, uint size) => throw new NotImplementedException();

        public void Read(BinaryReader r, uint size, int elementcount)
        {
            var redtype = Flags == null
                ? REDReflection.GetREDTypeString(typeof(T))
                : REDReflection.GetREDTypeString(typeof(T), Flags.ToArray());
            for (var i = 0; i < elementcount; i++)
            {
                var element = CR2WTypeManager.Create(redtype, i.ToString(), cr2w, this);
                element.Read(r, 0);
                element.IsSerialized = true;
                if (element is T te) elements.Add(te);
            }
        }

        public override void Write(BinaryWriter file)
        {
            foreach (var element in elements) element.Write(file);
        }

        public override bool CanAddVariable(IEditableVariable newvar) => newvar == null || newvar is T;

        public override void AddVariable(CVariable variable)
        {
            if (variable is T tvar) { variable.SetREDName(elements.Count.ToString()); tvar.IsSerialized = true; elements.Add(tvar); }
        }

        public void AddVariableWithName(CVariable variable)
        {
            if (variable is T tvar) { tvar.IsSerialized = true; elements.Add(tvar); }
        }
        public override bool CanRemoveVariable(IEditableVariable child) => child is T && elements.Count > 0;

        public override bool RemoveVariable(IEditableVariable child)
        {
            if (child is T tvar) { elements.Remove(tvar); UpdateNames(); return true; }
            return false;
        }

        public override string ToString()
        {
            var b = new StringBuilder().Append(elements.Count);
            if (elements.Count > 0)
            {
                b.Append(":");
                foreach (var element in elements)
                {
                    b.Append(" <").Append(element.ToString()).Append(">");
                    if (b.Length > 100) { b.Remove(100, b.Length - 100); break; }
                }
            }
            return b.ToString();
        }

        void UpdateNames()
        {
            for (var i = 0; i < elements.Count; i++)
                elements[i].SetREDName(i.ToString());
        }

        public override CVariable Copy(CR2WCopyAction context)
        {
            var copy = base.Copy(context) as CBufferBase<T>;
            context.Parent = copy;
            foreach (var element in elements)
            {
                var ccopy = element.Copy(context);
                if (ccopy is T copye) copy.elements.Add(copye);
            }
            return copy;
        }


        [Browsable(false)]
        public T this[int index]
        {
            get => ((IList<T>)elements)[index];
            set => ((IList<T>)elements)[index] = value;
        }

        public int Count => ((IList<T>)elements).Count;

        public bool IsReadOnly => ((IList<T>)elements).IsReadOnly;

        public bool IsFixedSize => ((IList)elements).IsFixedSize;

        public object SyncRoot => ((IList)elements).SyncRoot;

        public bool IsSynchronized => ((IList)elements).IsSynchronized;

        object IList.this[int index]
        {
            get => ((IList)elements)[index];
            set => ((IList)elements)[index] = value;
        }

        public IEnumerator<T> GetEnumerator() => elements.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public int IndexOf(T item) => ((IList<T>)elements).IndexOf(item);

        public void Insert(int index, T item)
        {
            throw new NotImplementedException();
            //((IList<T>)elements).Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            throw new NotImplementedException();
            //((IList<T>)elements).RemoveAt(index);
        }

        public void Add(T item) => AddVariable(item);

        public void Clear() => ((IList<T>)elements).Clear();

        public bool Contains(T item) => ((IList<T>)elements).Contains(item);

        public void CopyTo(T[] array, int arrayIndex) => ((IList<T>)elements).CopyTo(array, arrayIndex);

        public bool Remove(T item) => RemoveVariable(item);

        public int Add(object value)
        {
            if (value is T tvar) { AddVariable(tvar); return elements.Count; }
            return -1;
        }

        public bool Contains(object value) => ((IList)elements).Contains(value);

        public int IndexOf(object value) => ((IList)elements).IndexOf(value);

        public void Insert(int index, object value)
        {
            throw new NotImplementedException();
            //((IList)elements).Insert(index, value);
        }

        public void Remove(object value)
        {
            if (value is T cvar) RemoveVariable(cvar);
        }

        public void CopyTo(Array array, int index) => ((IList)elements).CopyTo(array, index);
    }
}