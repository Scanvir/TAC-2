using System.Collections.Generic;

namespace TAC_2.Model
{
    public class GoodsDirectory
    {
        public int Code;
        public int ParentCode;
        public string Name;
        public int Level;
        public int GoodCount;
        public List<GoodsDirectory> Child;
        public List<GoodItem> Goods;
    }
}