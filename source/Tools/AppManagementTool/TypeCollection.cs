using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Threading;

namespace SoonLearning.AppManagementTool
{
    public class TypeItem
    {
        public const int Root = 0;
        public const int All = -1;
        public const int New = -2;

        public const int Installing = int.MinValue;

        private string title;
        private string description;
        private string thumbnail;
        private int type;
        private int parentType;
        private ObservableCollection<TypeItem> subTypeItems = new ObservableCollection<TypeItem>();

        public string Title
        {
            get { return this.title; }
        }

        public string Description
        {
            get { return this.description; }
        }

        public string Thumbnail
        {
            get { return this.thumbnail; }
        }

        public int Type
        {
            get { return this.type; }
        }

        public int ParentType
        {
            get { return this.parentType; }
        }

        public ObservableCollection<TypeItem> SubTypeItems
        {
            get { return this.subTypeItems; }
        }

        public TypeItem(string title, string description, string thumbnailUrl, int type, int parentType)
        {
            this.title = title;
            this.description = description;
            this.thumbnail = thumbnailUrl;
            this.type = type;
            this.parentType = parentType;
        }

        public override string ToString()
        {
            return this.Title;
        }
    }

    public class TypeCollection : ObservableCollection<TypeItem>
    {
        private static TypeCollection instance;

        public static TypeCollection Instance
        {
            get
            {
                Interlocked.CompareExchange<TypeCollection>(ref instance, new TypeCollection(), null);
                return instance;
            }
        }

        private TypeCollection()
        {
            //this.InitMathType();
            //this.InitPuzzleType();
            //this.InitExploreType();
            //this.InitMemoryType();
        }

        internal TypeItem GetById(int id)
        {
            foreach (TypeItem item in this)
            {
                if (item.Type == id)
                    return item;

                foreach (TypeItem subItem in item.SubTypeItems)
                {
                    if (subItem.Type == id)
                        return subItem;
                }
            }

            return null;
        }
    }
}
