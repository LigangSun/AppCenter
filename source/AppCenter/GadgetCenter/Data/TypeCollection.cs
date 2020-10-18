using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using SoonLearning.AppCenter.Resources;
using System.Threading;
using SoonLearning.AppCenter.Data;
using SoonLearning.AppCenter.Utility;

namespace SoonLearning.AppCenter.Data
{
    public class TypeCollection : ObservableCollection<TypeItem>
    {
        //private static TypeCollection instance;
        //private static object instanceLocker;

        //public static TypeCollection Instance
        //{
        //    get
        //    {
        //        Interlocked.CompareExchange<object>(ref instanceLocker, new object(), null);

        //        lock (instanceLocker)
        //        {
        //            try
        //            {
        //                if (!System.IO.File.Exists(DataMgr.Instance.TypeConfigFile))
        //                {
        //                    initDefault();
        //                }
        //                else
        //                {
        //                    instance = SerializerHelper<TypeCollection>.XmlDeserialize(DataMgr.Instance.TypeConfigFile);
        //                }
        //            }
        //            catch
        //            {
        //                initDefault();
        //            }
        //        }

        //        return instance;
        //    }
        //}

        public TypeCollection()
        {
        }

        internal void initDefault()
        {
            initMemorizeType();
            initIntelligenceType();
            initMathType();
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

        private void initIntelligenceType()
        {
            TypeItem intelligenceItem = new TypeItem("益智", string.Empty, "", 100, TypeItem.Root);
            this.Add(intelligenceItem);

            intelligenceItem.SubTypeItems.Add(new TypeItem("探索", string.Empty, @"", 101, 100));
            intelligenceItem.SubTypeItems.Add(new TypeItem("记忆", string.Empty, @"", 102, 100));
            intelligenceItem.SubTypeItems.Add(new TypeItem("谜题", string.Empty, @"", 103, 100));
        //    intelligenceItem.SubTypeItems.Add(new TypeItem("电脑探索", string.Empty, @"", 104, 100));
        //    intelligenceItem.SubTypeItems.Add(new TypeItem("其他益智", string.Empty, @"", 199, 100));

        }

        private void initMemorizeType()
        {
            TypeItem memorizeItem = new TypeItem("记忆", string.Empty, "", 102, TypeItem.Root);
            this.Add(memorizeItem);

            memorizeItem.SubTypeItems.Add(new TypeItem("数学", string.Empty, "", 10201, 102));
            memorizeItem.SubTypeItems.Add(new TypeItem("英语", string.Empty, "", 10202, 102));
            memorizeItem.SubTypeItems.Add(new TypeItem("语文", string.Empty, "", 10203, 102));
            memorizeItem.SubTypeItems.Add(new TypeItem("事物认知", string.Empty, "", 10204, 102));
            memorizeItem.SubTypeItems.Add(new TypeItem("记忆其他", string.Empty, "", 10299, 102));
        }

        private void initMathType()
        {
            TypeItem mathItem = new TypeItem(Strings.Math, string.Empty, "", 200, TypeItem.Root);
            this.Add(mathItem);

            mathItem.SubTypeItems.Add(new TypeItem("少儿数学", string.Empty, @"", 201, 200));
            mathItem.SubTypeItems.Add(new TypeItem("小学数学", string.Empty, @"", 202, 200));
         //   mathItem.SubTypeItems.Add(new TypeItem("初中数学", string.Empty, @"", 203, 200));
         //   mathItem.SubTypeItems.Add(new TypeItem("高中数学", string.Empty, @"", 204, 200));
         //   mathItem.SubTypeItems.Add(new TypeItem("奥林匹克数学", string.Empty, @"", 205, 200));
         //   mathItem.SubTypeItems.Add(new TypeItem("数学游戏", string.Empty, @"", 206, 200));
            mathItem.SubTypeItems.Add(new TypeItem("数学速算", string.Empty, @"", 207, 200));
         //   mathItem.SubTypeItems.Add(new TypeItem("其他数学", string.Empty, @"", 299, 200));
        }

        //private void InitExploreType()
        //{
        //    TypeItem exploreType = new TypeItem(Strings.Explore, string.Empty, @"/Resources/Images/explore.png", GadgetType.Explore);
        //    this.Add(exploreType);

        //    exploreType.SubTypeItems.Add(new SubTypeItem(Strings.Color, string.Empty, @"/Resources/Images/Color.png", GadgetSubType.Explore_Color, GadgetType.Explore));
        //    exploreType.SubTypeItems.Add(new SubTypeItem(Strings.Sound, string.Empty, @"/Resources/Images/Sound.png", GadgetSubType.Explore_Sound, GadgetType.Explore));
        //}

        //private void InitMemoryType()
        //{
        //    TypeItem memoryType = new TypeItem(Strings.Memory, string.Empty, @"/Resources/Images/explore.png", GadgetType.Memory);
        //    this.Add(memoryType);

        //    memoryType.SubTypeItems.Add(new SubTypeItem(Strings.Color, string.Empty, @"/Resources/Images/Color.png", GadgetSubType.Memory_Color, GadgetType.Memory));
        //    memoryType.SubTypeItems.Add(new SubTypeItem(Strings.Sound, string.Empty, @"/Resources/Images/Sound.png", GadgetSubType.Memory_Sound, GadgetType.Memory));
        //}
    }
}
