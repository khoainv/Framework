#region

using System;
using System.Collections.Generic;
using System.ComponentModel;

#endregion

namespace SSD.Framework.Collections
{

    public class BaseList<T> : List<T>, IBindingList
    {
        private int addNewPos = -1;
        private bool raiseListChangedEvents = true;

        [NonSerialized()]
        private AddingNewEventHandler onAddingNew;

        [NonSerialized()]
        private ListChangedEventHandler onListChanged;

        private bool allowNew = true;
        private bool allowEdit = true;
        private bool allowRemove = true;
        private bool userSetAllowNew = false;

        #region AddingNew event

        /// <include file="doc\BindingList.uex" path="docs/doc[@for="BindingList.AddingNew"]/*">
        /// <devdoc>
        ///     Event that allows a custom item to be provided as the new item added to the list by AddNew().
        /// </devdoc>
        public event AddingNewEventHandler AddingNew
        {
            add
            {
                bool allowNewWasTrue = AllowNew;
                onAddingNew += value;
                if (allowNewWasTrue != AllowNew)
                {
                    FireListChanged(ListChangedType.Reset, -1);
                }
            }
            remove
            {
                bool allowNewWasTrue = AllowNew;
                onAddingNew -= value;
                if (allowNewWasTrue != AllowNew)
                {
                    FireListChanged(ListChangedType.Reset, -1);
                }
            }
        }

        /// <include file="doc\BindingList.uex" path="docs/doc[@for="BindingList.OnAddingNew"]/*">
        /// <devdoc>
        ///     Raises the AddingNew event.
        /// </devdoc>
        protected virtual void OnAddingNew(AddingNewEventArgs e)
        {
            if (onAddingNew != null)
            {
                onAddingNew(this, e);
            }
        }

        // Private helper method
        private object FireAddingNew()
        {
            AddingNewEventArgs e = new AddingNewEventArgs(null);
            OnAddingNew(e);
            return e.NewObject;
        }

        #endregion

        #region ListChanged event

        /// <include file="doc\BindingList.uex" path="docs/doc[@for="BindingList.ListChanged"]/*">
        /// <devdoc>
        ///     Event that reports changes to the list or to items in the list.
        /// </devdoc>
        public event ListChangedEventHandler ListChanged
        {
            add
            {
                onListChanged += value;
            }
            remove
            {
                onListChanged -= value;
            }
        }

        /// <include file="doc\BindingList.uex" path="docs/doc[@for="BindingList.OnListChanged"]/*">
        /// <devdoc>
        ///     Raises the ListChanged event.
        /// </devdoc>
        protected virtual void OnListChanged(ListChangedEventArgs e)
        {
            if (onListChanged != null)
            {
                onListChanged(this, e);
            }
        }

        /// <include file="doc\BindingList.uex" path="docs/doc[@for="BindingList.RaiseListChangedEvents"]/* />
        public bool RaiseListChangedEvents
        {
            get
            {
                return this.raiseListChangedEvents;
            }

            set
            {
                if (this.raiseListChangedEvents != value)
                {
                    this.raiseListChangedEvents = value;
                }
            }
        }

        /// <include file=" doc\bindinglist.uex'="">
        /// <devdoc>
        /// </devdoc>
        public void ResetBindings()
        {
            FireListChanged(ListChangedType.Reset, -1);
        }

        /// <include file="doc\BindingList.uex" path="docs/doc[@for="BindingList.ResetItem"]/*">
        /// <devdoc>
        /// </devdoc>
        public void ResetItem(int position)
        {
            FireListChanged(ListChangedType.ItemChanged, position);
        }

        // Private helper method
        private void FireListChanged(ListChangedType type, int index)
        {
            if (this.raiseListChangedEvents)
            {
                OnListChanged(new ListChangedEventArgs(type, index));
            }
        }

        #endregion

        #region IBindingList interface

        /// <include file="doc\BindingList.uex" path="docs/doc[@for="BindingList.AddNew"]/*">
        /// <devdoc>
        ///     Adds a new item to the list. Calls <see cref="AddNewCore"> to create and add the item.
        ///
        ///     Add operations are cancellable via the <see cref="ICancelAddNew"> interface. The position of the
        ///     new item is tracked until the add operation is either cancelled by a call to <see cref="CancelNew">,
        ///     explicitly commited by a call to <see cref="EndNew">, or implicitly commmited some other operation
        ///     that changes the contents of the list (such as an Insert or Remove). When an add operation is
        ///     cancelled, the new item is removed from the list.
        /// </see></see></see></see></devdoc>
        public T AddNew()
        {
            return (T)((this as IBindingList).AddNew());
        }

        object IBindingList.AddNew()
        {
            // Create new item and add it to list
            object newItem = AddNewCore();

            // Record position of new item (to support cancellation later on)
            addNewPos = (newItem != null) ? IndexOf((T)newItem) : -1;

            // Return new item to caller
            return newItem;
        }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2113:SecureLateBindingMethods")]
        protected virtual object AddNewCore()
        {
            // Allow event handler to supply the new item for us
            object newItem = FireAddingNew();

            // If event hander did not supply new item, create one ourselves
            if (newItem == null)
            {
                Type type = typeof(T);
                //newItem = SecurityUtils.SecureCreateInstance(type);Activator.CreateInstance
                newItem = Activator.CreateInstance(type);
            }

            // Add item to end of list. Note: If event handler returned an item not of type T,
            // the cast below will trigger an InvalidCastException. This is by design.
            Add((T)newItem);

            // Return new item to caller
            return newItem;
        }
        private bool AddingNewHandled
        {
            get
            {
                return onAddingNew != null && onAddingNew.GetInvocationList().Length > 0;
            }
        }

        /// <include file="doc\BindingList.uex" path="docs/doc[@for="BindingList.AllowNew"]/*">
        /// <devdoc>
        /// </devdoc>
        public bool AllowNew
        {
            get
            {
                //If the user set AllowNew, return what they set.  If we have a default constructor, allowNew will be
                //true and we should just return true.
                if (userSetAllowNew || allowNew)
                {
                    return this.allowNew;
                }
                //Even if the item doesn't have a default constructor, the user can hook AddingNew to provide an item.
                //If there's a handler for this, we should allow new.
                return AddingNewHandled;
            }
            set
            {
                bool oldAllowNewValue = AllowNew;
                userSetAllowNew = true;
                //Note that we don't want to set allowNew only if AllowNew didn't match value,
                //since AllowNew can depend on onAddingNew handler
                this.allowNew = value;
                if (oldAllowNewValue != value)
                {
                    FireListChanged(ListChangedType.Reset, -1);
                }
            }
        }

        /* private */
        bool IBindingList.AllowNew
        {
            get
            {
                return AllowNew;
            }
        }

        /// <include file="doc\BindingList.uex" path="docs/doc[@for="BindingList.AllowEdit"]/*">
        /// <devdoc>
        /// </devdoc>
        public bool AllowEdit
        {
            get
            {
                return this.allowEdit;
            }
            set
            {
                if (this.allowEdit != value)
                {
                    this.allowEdit = value;
                    FireListChanged(ListChangedType.Reset, -1);
                }
            }
        }

        /* private */
        bool IBindingList.AllowEdit
        {
            get
            {
                return AllowEdit;
            }
        }

        /// <include file="doc\BindingList.uex" path="docs/doc[@for="BindingList.AllowRemove"]/*">
        /// <devdoc>
        /// </devdoc>
        public bool AllowRemove
        {
            get
            {
                return this.allowRemove;
            }
            set
            {
                if (this.allowRemove != value)
                {
                    this.allowRemove = value;
                    FireListChanged(ListChangedType.Reset, -1);
                }
            }
        }

        /* private */
        bool IBindingList.AllowRemove
        {
            get
            {
                return AllowRemove;
            }
        }

        bool IBindingList.SupportsChangeNotification
        {
            get
            {
                return SupportsChangeNotificationCore;
            }
        }

        protected virtual bool SupportsChangeNotificationCore
        {
            get
            {
                return true;
            }
        }

        bool IBindingList.SupportsSearching
        {
            get
            {
                return SupportsSearchingCore;
            }
        }

        protected virtual bool SupportsSearchingCore
        {
            get
            {
                return false;
            }
        }

        bool IBindingList.SupportsSorting
        {
            get
            {
                return SupportsSortingCore;
            }
        }

        protected virtual bool SupportsSortingCore
        {
            get
            {
                return false;
            }
        }

        bool IBindingList.IsSorted
        {
            get
            {
                return IsSortedCore;
            }
        }

        protected virtual bool IsSortedCore
        {
            get
            {
                return false;
            }
        }

        PropertyDescriptor IBindingList.SortProperty
        {
            get
            {
                return SortPropertyCore;
            }
        }

        protected virtual PropertyDescriptor SortPropertyCore
        {
            get
            {
                return null;
            }
        }

        ListSortDirection IBindingList.SortDirection
        {
            get
            {
                return SortDirectionCore;
            }
        }

        protected virtual ListSortDirection SortDirectionCore
        {
            get
            {
                return ListSortDirection.Ascending;
            }
        }

        void IBindingList.ApplySort(PropertyDescriptor prop, ListSortDirection direction)
        {
            ApplySortCore(prop, direction);
        }

        protected virtual void ApplySortCore(PropertyDescriptor prop, ListSortDirection direction)
        {
            throw new NotSupportedException();
        }

        void IBindingList.RemoveSort()
        {
            RemoveSortCore();
        }

        protected virtual void RemoveSortCore()
        {
            throw new NotSupportedException();
        }

        int IBindingList.Find(PropertyDescriptor prop, object key)
        {
            return FindCore(prop, key);
        }

        protected virtual int FindCore(PropertyDescriptor prop, object key)
        {
            throw new NotSupportedException();
        }

        void IBindingList.AddIndex(PropertyDescriptor prop)
        {
            // Not supported
        }

        void IBindingList.RemoveIndex(PropertyDescriptor prop)
        {
            // Not supported
        }

        #endregion
    }
}
