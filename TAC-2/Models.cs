using System;
using System.Collections.Generic;
namespace TAC_2
{
    public class Auth
    {
        public string Name;
        public string Code;
        public int TAC;
        public int Type;
        public int Base;
        public string Version;
    }
    public class TaraRest
    {
        public string TaraName { get; set; }
        public int TaraCode { get; set; }
        public int KlientCode { get; set; }
        public int DotCode { get; set; }
        public double Qty { get; set; }
        public double Dalay { get; set; }
        public double Facing { get; set; }
    }
    public class Tara
    {
        public int TaraCode { get; set; }
        public string TaraName { get; set; }
    }
    public class OborudRest
    {
        public string OborudName { get; set; }
        public string OborudCode { get; set; }
        public int KlientCode { get; set; }
        public int DotCode { get; set; }
        public double Qty { get; set; }
        public double Facing { get; set; }
        public string SerialNumber { get; set; }
    }
    public class Oborud
    {
        public string OborudCode { get; set; }
        public string OborudName { get; set; }
        public string OborudInventCode { get; set; }
    }
    public class Klient
    {
        public int KlientCode;
        public string KlientName;
        public string KlientWarn;
        public double KlientDebet;
    }
    public class Dot
    {
        public int KlientCode { get; set; }
        public int DotCode { get; set; }
        public string DotName { get; set; }
        public string DotAddress { get; set; }
    }
    public class Price
    {
        public int GoodCode;
        public string GoodName;
        public double PriceUAH;
        public double Quantity;
    }
    public class Good
    {
        public int Code;
        public string Name;
        public int ParentID;
        public int Box;
        public string GoodView;
    }
    public class GoodRest
    {
        public int Code;
        public double Quantity;
        public double Price;
    }
    public class GoodView
    {
        public string Сode;
        public string Name;
    }
    public class Debet
    {
        public string NumDoc;
        public string DateDoc;
        public int KlientCode;
        public int DotCode;
        public double Dolg;
        public DateTime DatePay;
    }
    public class PKO
    {
        public string GUID;
        public string NumDoc;
        public string DateDoc;
        public int KlientCode;
        public int DotCode;
        public double Summ;
        public string DatePay;
        public int Status;
        public string KlientName;
    }
    public class DocReturn
    {
        public string GUID;
        public int Status;
    }
    public class Update
    {
        public Auth Auth;
        public List<Good> Good;
        public List<Model.GoodsDirectory> GoodsDirectory;
        public List<Tara> Tara;
        public List<Oborud> Oborud;
        public List<Klient> Klient;
        public List<Dot> Dot;
        public List<GoodView> GoodView;
        public List<GoodRest> GoodRests;
        public List<TaraRest> TaraRests;
        public List<OborudRest> OborudRests;
        public List<Debet> Debet;
        public string Version;
        
        public List<PKO> PKO;
        public List<Order> Order;
        public List<TaraFacing> TaraFacing;
        public List<OborudFacing> OborudFacing;
        
        public List<DocReturn> PKOReturn;
        public List<DocReturn> OrderReturn;
        public List<DocReturn> TaraFacingReturn;
        public List<DocReturn> OborudFacingReturn;
    }
    public class Order
    {
        public string GUID;
        public DateTime DateDoc;
        public int KlientCode;
        public int DotCode;
        public int Status;
        public int FlagA;
        public int FlagF;
        public int Form;
        public string Comment;
        public string KlientName;
        public double Summ;
        public List<OrderTab> OrderTab;
    }
    public class OrderTab
    {
        public string GUID;
        public int GoodCode;
        public string GoodName;
        public int Quantity;
        public double PriceUAH;
        public double Summ;
    }
    public class TaraFacing
    {
        public string DateDoc;
        public string GUID;
        public int KlientCode;
        public int DotCode;
        public int TaraCode;
        public double Quantity;
        public int Status;
    }
    public class OborudFacing 
    {
        public string DateDoc;
        public string GUID;
        public int KlientCode;
        public int DotCode;
        public string OborudCode;
        public double Quantity;
        public int Status;
    }
}