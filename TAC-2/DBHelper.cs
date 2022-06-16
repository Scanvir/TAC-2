using Android.Content;
using Android.Database;
using Android.Database.Sqlite;
using Android.Text;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using static TAC_2.MainActivity;

namespace TAC_2
{
    class DBHelper : SQLiteOpenHelper, IDatabaseHandler
    {
        ICursor cursor;
        private new static string DatabaseName = "TAC-2";
        private const int DatabaseVersion = 33;
        public DBHelper(Context context) : base(context, DatabaseName, null, DatabaseVersion) { }
        public override void OnCreate(SQLiteDatabase db)
        {
            db.ExecSQL(@"CREATE TABLE IF NOT EXISTS Auth (
                    ID              INTEGER PRIMARY KEY,
                    Name            TEXT NOT NULL,
                    Code            TEXT NOT NULL,
                    TAC             INTEGER NOT NULL,
                    Teh             INTEGER NOT NULL,
                    Base            INTEGER NOT NULL,
                    Version         TEXT)");
            db.ExecSQL(@"CREATE TABLE IF NOT EXISTS Dot (
                    KlientCode      INTEGER NOT NULL,
                    DotCode         INTEGER NOT NULL,
                    DotName         TEXT NOT NULL,
                    DotAddress      TEXT NOT NULL,
                    DotFillial      TEXT NOT NULL,
                    PRIMARY KEY (KlientCode, DotCode))");
            db.ExecSQL(@"CREATE TABLE IF NOT EXISTS Klient (
                    KlientCode      INTEGER PRIMARY KEY,
                    KlientName      TEXT NOT NULL)");
            db.ExecSQL(@"CREATE TABLE IF NOT EXISTS Tara (
                    TaraCode        INTEGER PRIMARY KEY,
                    TaraName        TEXT NOT NULL)");
            db.ExecSQL(@"CREATE TABLE IF NOT EXISTS TaraRest (
                    TaraCode        INTEGER NOT NULL,
                    KlientCode      INTEGER NOT NULL,
                    DotCode         INTEGER NOT NULL,
                    Qty             REAL NOT NULL,
                    Delay           REAL NOT NULL,
                    PRIMARY KEY (TaraCode, KlientCode, DotCode))");
            db.ExecSQL(@"CREATE TABLE IF NOT EXISTS Oborud (
                    OborudCode      TEXT PRIMARY KEY,
                    OborudName      TEXT NOT NULL,
                    OborudInventCode    TEXT NOT NULL)");
            db.ExecSQL(@"CREATE TABLE IF NOT EXISTS GoodView (
                    Code            TEXT PRIMARY KEY,
                    Name            TEXT NOT NULL)");
            db.ExecSQL(@"CREATE TABLE IF NOT EXISTS Good (
                    Code            INT PRIMARY KEY,
                    Name            TEXT NOT NULL,
                    ParentID        INTEGER NOT NULL,
                    Box             INTEGER NOT NULL,
                    GoodView        STRING NOT NULL,
                    Price           REAL)");
            db.ExecSQL(@"CREATE TABLE IF NOT EXISTS GoodsDirectory (
                    Code            INT PRIMARY KEY,
                    Name            TEXT NOT NULL,
                    ParentCode      INTEGER NOT NULL,
                    Level           INTEGER NOT NULL)");
            db.ExecSQL(@"CREATE TABLE IF NOT EXISTS GoodRest (
                    Code            INTEGER PRIMARY KEY,
                    Quantity        REAL,
                    Fillial         TEXT NOT NULL)");
            db.ExecSQL(@"CREATE TABLE IF NOT EXISTS OborudRest (
                    OborudCode      TEXT NOT NULL,
                    KlientCode      INTEGER NOT NULL,
                    DotCode         INTEGER NOT NULL,
                    Qty             REAL NOT NULL,
                    PRIMARY KEY (OborudCode, KlientCode, DotCode))");
            db.ExecSQL(@"CREATE INDEX idx_klient_or ON OborudRest (KlientCode)");
            db.ExecSQL(@"CREATE INDEX idx_dot_or ON OborudRest (KlientCode, DotCode)");
            db.ExecSQL(@"CREATE TABLE IF NOT EXISTS Debet (
                    NumDoc          TEXT NOT NULL,
                    DateDoc         TEXT NOT NULL,
                    KlientCode      INTEGER NOT NULL,
                    DotCode         INTEGER NOT NULL,
                    Dolg            REAL NOT NULL,
                    DatePay         DATE NOT NULL,
                    PRIMARY KEY (NumDoc, DateDoc))");
            db.ExecSQL(@"CREATE INDEX idx_klient_d ON Debet (KlientCode)");
            db.ExecSQL(@"CREATE INDEX idx_dot_d ON Debet (KlientCode, DotCode)");
            db.ExecSQL(@"CREATE TABLE IF NOT EXISTS PKO (
                    GUID            TEXT PRIMARY KEY,
                    NumDoc          TEXT NOT NULL,
                    DateDoc         DATE NOT NULL,
                    KlientCode      INTEGER NOT NULL,
                    DotCode         INTEGER NOT NULL,
                    DatePay         DATE NOT NULL,
                    Status          INT NOT NULL,
                    Summ            REAL NOT NULL)");
            db.ExecSQL(@"CREATE TABLE IF NOT EXISTS Zakaz (
                    GUID            TEXT PRIMARY KEY,
                    DateDoc         DATE NOT NULL,
                    KlientCode      INTEGER NOT NULL,
                    DotCode         INTEGER NOT NULL,
                    FlagA           INTEGER NOT NULL,
                    FlagF           INTEGER NOT NULL,
                    Form            INTEGER NOT NULL,
                    Comment         STRING NOT NULL,
                    Status          INT NOT NULL)");
            db.ExecSQL(@"CREATE INDEX idx_Klient ON Zakaz (KlientCode)");
            db.ExecSQL(@"CREATE INDEX idx_Doc ON Zakaz (DotCode)");
            db.ExecSQL(@"CREATE TABLE IF NOT EXISTS ZakazTab (
                    GUID            TEXT NOT NULL,
                    GoodCode        INTEGER NOT NULL,
                    Quantity        REAL NOT NULL,
                    PriceUAH        REAL NOT NULL,
                    Summa           REAL NOT NULL,
                    PRIMARY KEY (GUID, GoodCode))");
            db.ExecSQL(@"CREATE INDEX idx_GUID ON ZakazTab (GUID)");
            db.ExecSQL(@"CREATE INDEX idx_Good ON ZakazTab (GoodCode)");
            db.ExecSQL(@"CREATE TABLE IF NOT EXISTS TaraFacing (
                    DateDoc         DATE NOT NULL,
                    GUID            TEXT NOT NULL,
                    KlientCode      INTEGER NOT NULL,
                    DotCode         INTEGER NOT NULL,
                    TaraCode        INTEGER NOT NULL,
                    Quantity        REAL NOT NULL,
                    Status          INTEGER NOT NULL)");
            db.ExecSQL(@"CREATE UNIQUE INDEX idx_TaFacing ON TaraFacing (DateDoc, KlientCode, DotCode, TaraCode)");
            db.ExecSQL(@"CREATE UNIQUE INDEX idx_GUIDTaraFacing ON TaraFacing (GUID)");
            db.ExecSQL(@"CREATE TABLE IF NOT EXISTS OborudFacing (
                    DateDoc         DATE NOT NULL,
                    GUID            TEXT NOT NULL,
                    KlientCode      INTEGER NOT NULL,
                    DotCode         INTEGER NOT NULL,
                    OborudCode      TEXT NOT NULL,
                    Quantity        REAL NOT NULL,
                    Status          INTEGER NOT NULL)");
            db.ExecSQL(@"CREATE UNIQUE INDEX idx_ObFacing ON OborudFacing (DateDoc, KlientCode, DotCode, OborudCode)");
            db.ExecSQL(@"CREATE UNIQUE INDEX idx_GUIDOborudFacing ON OborudFacing (GUID)");
        }
        public override void OnUpgrade(SQLiteDatabase db, int oldVersion, int newVersion)
        {
            db.ExecSQL(@"DROP TABLE IF EXISTS Auth");
            db.ExecSQL(@"DROP TABLE IF EXISTS Dot");
            db.ExecSQL(@"DROP TABLE IF EXISTS Klient");
            db.ExecSQL(@"DROP TABLE IF EXISTS Tara");
            db.ExecSQL(@"DROP TABLE IF EXISTS TaraRest");
            db.ExecSQL(@"DROP TABLE IF EXISTS Oborud");
            db.ExecSQL(@"DROP TABLE IF EXISTS OborudRest");
            db.ExecSQL(@"DROP TABLE IF EXISTS Good");
            db.ExecSQL(@"DROP TABLE IF EXISTS GoodsDirectory");
            db.ExecSQL(@"DROP TABLE IF EXISTS GoodView");
            db.ExecSQL(@"DROP TABLE IF EXISTS GoodRest");
            db.ExecSQL(@"DROP TABLE IF EXISTS Debet");
            db.ExecSQL(@"DROP TABLE IF EXISTS PKO");
            db.ExecSQL(@"DROP TABLE IF EXISTS Zakaz");
            db.ExecSQL(@"DROP TABLE IF EXISTS ZakazTab");
            db.ExecSQL(@"DROP TABLE IF EXISTS TaraFacing");
            db.ExecSQL(@"DROP TABLE IF EXISTS OborudFacing");
            OnCreate(db);
        }
        public void Delete(Context context)
        {
            using (SQLiteDatabase db = new DBHelper(context).WritableDatabase)
            {
                db.Delete("Dot", null, null);
                db.Delete("Klient", null, null);
                db.Delete("Tara", null, null);
                db.Delete("TaraRest", null, null);
                db.Delete("Oborud", null, null);
                db.Delete("OborudRest", null, null);
                db.Delete("Good", null, null);
                db.Delete("GoodsDirectory", null, null);
                db.Delete("GoodView", null, null);
                db.Delete("GoodRest", null, null);
                db.Delete("Debet", null, null);
                db.Delete("PKO", null, null);
                db.Delete("Zakaz", null, null);
                db.Delete("ZakazTab", null, null);
                db.Delete("TaraFacing", null, null);
                db.Delete("OborudFacing", null, null);
            }
        }
        public void AddOrUpdateTaraRest(Context context, int klientCode, int dotCode, int taraCode, int rest)
        {
            string today = DateTime.Now.ToString("yyyy-MM-dd");
            ContentValues Values = new ContentValues();

            using (SQLiteDatabase db = new DBHelper(context).ReadableDatabase)
            {
                cursor = db.Query("TaraFacing", new string[] { "GUID" }, "DateDoc = ? and KlientCode = ? and DotCode = ? and TaraCode = ?", new string[] { today, klientCode.ToString(), dotCode.ToString(), taraCode.ToString()}, null, null, null);                if (cursor != null && cursor.MoveToFirst() && cursor.Count > 0)
                {
                    for (int i = 0; i < cursor.Count; i++)
                    {
                        Values.Put("DateDoc", today);
                        Values.Put("GUID", cursor.GetString(0));
                        Values.Put("KlientCode", klientCode);
                        Values.Put("DotCode", dotCode);
                        Values.Put("TaraCode", taraCode);
                        Values.Put("Quantity", rest);
                        Values.Put("Status", 0);
                        db.Replace("TaraFacing", null, Values);
                    }
                } 
                else
                {
                    Values.Put("DateDoc", today);
                    Values.Put("GUID", Guid.NewGuid().ToString());
                    Values.Put("KlientCode", klientCode);
                    Values.Put("DotCode", dotCode);
                    Values.Put("TaraCode", taraCode);
                    Values.Put("Quantity", rest);
                    Values.Put("Status", 0);
                    db.Insert("TaraFacing", null, Values);
                }
            }
        }
        public void AddOrUpdateOborudRest(Context context, int klientCode, int dotCode, string oborudCode, int rest)
        {
            string today = DateTime.Now.ToString("yyyy-MM-dd");
            ContentValues Values = new ContentValues();

            using (SQLiteDatabase db = new DBHelper(context).ReadableDatabase)
            {
                cursor = db.Query("OborudFacing", new string[] { "GUID" }, "DateDoc = ? and KlientCode = ? and DotCode = ? and OborudCode = ?", new string[] { today, klientCode.ToString(), dotCode.ToString(), oborudCode.ToString() }, null, null, null); 
                if (cursor != null && cursor.MoveToFirst() && cursor.Count > 0)
                {
                    for (int i = 0; i < cursor.Count; i++)
                    {
                        Values.Put("DateDoc", today);
                        Values.Put("GUID", cursor.GetString(0));
                        Values.Put("KlientCode", klientCode);
                        Values.Put("DotCode", dotCode);
                        Values.Put("OborudCode", oborudCode);
                        Values.Put("Quantity", rest);
                        Values.Put("Status", 0);
                        db.Replace("OborudFacing", null, Values);
                    }
                }
                else
                {
                    Values.Put("DateDoc", today);
                    Values.Put("GUID", Guid.NewGuid().ToString());
                    Values.Put("KlientCode", klientCode);
                    Values.Put("DotCode", dotCode);
                    Values.Put("OborudCode", oborudCode);
                    Values.Put("Quantity", rest);
                    Values.Put("Status", 0);
                    db.Insert("OborudFacing", null, Values);
                }
            }
        }
        
        public void AddPKO(SQLiteDatabase db, PKO pko)
        {
            string dd = pko.DateDoc;
            if (dd.Length == 8)
                dd = DateTime.ParseExact(dd, "yyyyMMdd", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");
            else if (dd.Length == 10)
                dd = DateTime.ParseExact(dd, "yyyy-MM-dd", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd");

            ContentValues Values = new ContentValues();
            Values.Put("GUID", pko.GUID);
            Values.Put("NumDoc", pko.NumDoc);
            Values.Put("DateDoc", dd);
            Values.Put("KlientCode", pko.KlientCode);
            Values.Put("DotCode", pko.DotCode);
            Values.Put("Summ", pko.Summ);
            Values.Put("DatePay", pko.DatePay);
            Values.Put("Status", pko.Status);
            db.Insert("PKO", null, Values);
        }
        public void AddPKO(Context context, PKO pko)
        {
            using (SQLiteDatabase db = new DBHelper(context).WritableDatabase)
            {
                AddPKO(db, pko);
            }
        }

        public void DelPKO(Context context, string GUID)
        {
            using (SQLiteDatabase db = new DBHelper(context).WritableDatabase)
            {
                db.Delete("PKO", "GUID = ?", new string[] { GUID });
            }
        }
        
        public void AddOrder(SQLiteDatabase db, Order order)
        {
            ContentValues Values = new ContentValues();
            Values.Put("GUID", order.GUID);
            Values.Put("DateDoc", order.DateDoc.ToString("yyyy-MM-dd"));
            Values.Put("KlientCode", order.KlientCode);
            Values.Put("DotCode", order.DotCode);
            Values.Put("Status", order.Status);
            Values.Put("Comment", order.Comment);
            Values.Put("FlagA", order.FlagA);
            Values.Put("FlagF", order.FlagF);
            Values.Put("Form", order.Form);
            db.Replace("Zakaz", null, Values);
            foreach (OrderTab tab in order.OrderTab)
            {
                AddOrderTab(db, tab);
            }
        }
        public void AddOrder(Context context, Order order)
        {
            using (SQLiteDatabase db = new DBHelper(context).WritableDatabase)
            {
                AddOrder(db, order);
            }
        }
        
        public void AddOrderTab(SQLiteDatabase db, OrderTab tab)
        {
            ContentValues Values = new ContentValues();
            Values.Put("GUID", tab.GUID);
            Values.Put("GoodCode", tab.GoodCode);
            Values.Put("Quantity", tab.Quantity);
            Values.Put("PriceUAH", tab.PriceUAH);
            Values.Put("Summa", tab.Summ);
            db.Replace("ZakazTab", null, Values);
        }
        public void AddOrderTab(Context context, OrderTab tab)
        {
            using (SQLiteDatabase db = new DBHelper(context).WritableDatabase)
            {
                AddOrderTab(db, tab);
            }
        }
        
        public void DelOrder(Context context, string GUID)
        {
            using (SQLiteDatabase db = new DBHelper(context).WritableDatabase)
            {
                db.Delete("Zakaz", "GUID = ?", new string[] { GUID });
                db.Delete("ZakazTab", "GUID = ?", new string[] { GUID });
            }
        }
        public void DelZakazTab(Context context, string GUID, string GoodCode)
        {
            using (SQLiteDatabase db = new DBHelper(context).WritableDatabase)
            {
                db.Delete("ZakazTab", "GUID = ? and GoodCode = ?", new string[] { GUID, GoodCode });
            }
        }
        public void UpdateAuth(Context context, Auth auth)
        {
            using (SQLiteDatabase db = new DBHelper(context).WritableDatabase)
            {
                ContentValues Values = new ContentValues();
                Values.Put("ID", 1);
                Values.Put("Name", auth.Name);
                Values.Put("Code", auth.Code);
                Values.Put("TAC", auth.TAC);
                Values.Put("Teh", auth.Type);
                Values.Put("Base", auth.Base);
                Values.Put("Version", auth.Version);
                db.Replace("Auth", null, Values);
            };
        }
        private void UpdateKlients(SQLiteDatabase db, List<Klient> klients)
        {
            db.Delete("Klient", null, null);
            foreach (Klient klient in klients)
            {
                ContentValues Values = new ContentValues();
                Values.Put("KlientCode", klient.KlientCode);
                Values.Put("KlientName", klient.KlientName);
                db.Insert("Klient", null, Values);
            }
        }
        private void UpdateDots(SQLiteDatabase db, List<Dot> dots)
        {
            foreach (Dot dot in dots)
            {
                ContentValues Values = new ContentValues();
                Values.Put("KlientCode", dot.KlientCode);
                Values.Put("DotCode", dot.DotCode);
                Values.Put("DotName", dot.DotName);
                Values.Put("DotAddress", dot.DotAddress);
                Values.Put("DotFillial", dot.DotFillial);
                db.Replace("Dot", null, Values);
            }
        }
        private void UpdateGoods(SQLiteDatabase db, List<Good> goods)
        {
            db.Delete("Good", null, null);

            foreach (Good good in goods)
            {
                ContentValues Values = new ContentValues();
                Values.Put("Code", good.Code);
                Values.Put("Name", good.Name);
                Values.Put("ParentID", good.ParentID);
                Values.Put("Box", good.Box);
                Values.Put("GoodView", good.GoodView);
                Values.Put("Price", good.Price);
                db.Insert("Good", null, Values);
            }
        }
        private void UpdateGoodsDirectory(SQLiteDatabase db, List<Model.GoodsDirectory> goodsDirectory)
        {
            foreach (Model.GoodsDirectory directory in goodsDirectory)
            {
                ContentValues Values = new ContentValues();
                Values.Put("Code", directory.Code);
                Values.Put("ParentCode", directory.ParentCode);
                Values.Put("Name", directory.Name);
                Values.Put("Level", directory.Level);

                db.Insert("GoodsDirectory", null, Values);
                UpdateGoodsDirectory(db, directory.Child);
            }
        }
        private void UpateGoodView(SQLiteDatabase db, List<GoodView> goodViews)
        {
            foreach (GoodView goodView in goodViews)
            {
                ContentValues Values = new ContentValues();
                Values.Put("Code", goodView.Сode);
                Values.Put("Name", goodView.Name);
                db.Replace("GoodView", null, Values);
            }
        }
        private void UpdateGoodRests(SQLiteDatabase db, List<GoodRest> goodRests)
        {
            db.Delete("GoodRest", null, null);
            foreach (GoodRest goodRest in goodRests)
            {
                ContentValues Values = new ContentValues();
                Values.Put("Code", goodRest.Code);
                Values.Put("Quantity", goodRest.Quantity);
                Values.Put("Fillial", goodRest.Fillial);
                db.Insert("GoodRest", null, Values);
            }
        }
        private void UpdateDebets(SQLiteDatabase db, List<Debet> debets)
        {
            db.Delete("Debet", null, null);
            foreach (Debet debet in debets)
            {
                ContentValues Values = new ContentValues();
                Values.Put("NumDoc", debet.NumDoc);
                Values.Put("DateDoc", debet.DateDoc);
                Values.Put("KlientCode", debet.KlientCode);
                Values.Put("DotCode", debet.DotCode);
                Values.Put("Dolg", debet.Dolg);
                Values.Put("DatePay", debet.DatePay.ToString("yyyy-MM-dd"));
                db.Insert("Debet", null, Values);
            }
        }
        private void UpateTaras(SQLiteDatabase db, List<Tara> taras)
        {
            foreach (Tara tara in taras)
            {
                ContentValues Values = new ContentValues();
                Values.Put("TaraCode", tara.TaraCode);
                Values.Put("TaraName", tara.TaraName);
                db.Replace("Tara", null, Values);
            }
        }
        private void UpateTarasRests(SQLiteDatabase db, List<TaraRest> taraRests)
        {
            db.Delete("TaraRest", null, null);
            foreach (TaraRest taraRest in taraRests)
            {
                ContentValues Values = new ContentValues();
                Values.Put("TaraCode", taraRest.TaraCode);
                Values.Put("KlientCode", taraRest.KlientCode);
                Values.Put("DotCode", taraRest.DotCode);
                Values.Put("Qty", taraRest.Qty);
                Values.Put("Delay", taraRest.Dalay);
                db.Insert("TaraRest", null, Values);
            }
        }
        private void UpateOboruds(SQLiteDatabase db, List<Oborud> oboruds)
        {
            foreach (Oborud oborud in oboruds)
            {
                ContentValues Values = new ContentValues();
                Values.Put("OborudCode", oborud.OborudCode);
                Values.Put("OborudName", oborud.OborudName);
                Values.Put("OborudInventCode", oborud.OborudInventCode);
                db.Replace("Oborud", null, Values);
            }
        }
        private void UpateOborudsRests(SQLiteDatabase db, List<OborudRest> oborudRests)
        {
            db.Delete("OborudRest", null, null);
            foreach (OborudRest oborudRest in oborudRests)
            {
                ContentValues Values = new ContentValues();
                Values.Put("OborudCode", oborudRest.OborudCode);
                Values.Put("KlientCode", oborudRest.KlientCode);
                Values.Put("DotCode", oborudRest.DotCode);
                Values.Put("Qty", oborudRest.Qty);
                db.Insert("OborudRest", null, Values);
            }
        }

        public void UpdateDatabase(Context context, Update update, TextView log)
        {
            using (SQLiteDatabase db = new DBHelper(context).WritableDatabase)
            {
                List<Dot> dots = update.Dot;
                List<Klient> klients = update.Klient;
                List<Good> goods = update.Good;
                List<Model.GoodsDirectory> goodsDirectory = update.GoodsDirectory;
                List<GoodRest> goodRests = update.GoodRests;
                List<GoodView> goodViews = update.GoodView;
                List<Debet> debets = update.Debet;
                List<Tara> taras = update.Tara;
                List<TaraRest> taraRests = update.TaraRests;
                List<Oborud> oboruds = update.Oborud;
                List<OborudRest> oborudRests = update.OborudRests;
                List<DocReturn> pkoReturn = update.PKOReturn;
                List<DocReturn> orderReturn = update.OrderReturn;
                List<DocReturn> taraReturn = update.TaraFacingReturn;
                List<DocReturn> oborudReturn = update.OborudFacingReturn;
                List<PKO> pkos = update.PKO;
                List<Order> orders = update.Order;

                if (dots != null && dots.Count > 0)
                {
                    UpdateDots(db, dots);
                    log.Text += " - оновлено Точки " + dots.Count + "\n";
                }
                if (klients != null && klients.Count > 0)
                {
                    UpdateKlients(db, klients);
                    log.Text += " - оновлено Клієнти " + klients.Count + "\n";
                }
                if (goods != null && goods.Count > 0)
                {
                    UpdateGoods(db, goods);
                    log.Text += " - оновлено Товари " + goods.Count + "\n";
                }
                if (goodsDirectory != null && goodsDirectory.Count > 0)
                {
                    db.Delete("GoodsDirectory", null, null);
                    UpdateGoodsDirectory(db, goodsDirectory);
                    log.Text += " - оновлено Дерево товарів " + goodsDirectory.Count + "\n";
                }
                if (goodViews != null && goodViews.Count > 0)
                {
                    UpateGoodView(db, goodViews);
                    log.Text += " - оновлено Види товарів " + goodViews.Count + "\n";
                }
                if (goodRests != null && goodRests.Count > 0)
                {
                    UpdateGoodRests(db, goodRests);
                    log.Text += " - оновлено Залишки товарів " + goodRests.Count + "\n";
                }
                if (debets != null && debets.Count > 0)
                {
                    UpdateDebets(db, debets);
                    log.Text += " - оновлено Борги " + debets.Count + "\n";
                }
                if (taras != null && taras.Count > 0)
                {
                    UpateTaras(db, taras);
                    log.Text += " - оновлено Тара " + taras.Count + "\n";
                }
                if (taraRests != null && taraRests.Count > 0)
                {
                    UpateTarasRests(db, taraRests);
                    log.Text += " - оновлено Залишки тари " + taraRests.Count + "\n";
                }
                if (oboruds != null && oboruds.Count > 0)
                {
                    UpateOboruds(db, oboruds);
                    log.Text += " - оновлено Обладнання " + oboruds.Count + "\n";
                }
                if (oborudRests != null && oborudRests.Count > 0)
                {
                    UpateOborudsRests(db, oborudRests);
                    log.Text += " - оновлено Залишки обладнання " + oborudRests.Count + "\n";
                }
                if (pkoReturn != null && pkoReturn.Count > 0)
                {
                    foreach (DocReturn pko in pkoReturn)
                    {
                        ContentValues Values = new ContentValues();
                        Values.Put("Status", pko.Status);
                        db.Update("PKO", Values, "GUID = ?", new string[] { pko.GUID });
                    }
                    log.Text += " - оновлено ПКО " + pkoReturn.Count + "\n";
                }
                if (orderReturn != null && orderReturn.Count > 0)
                {
                    foreach (DocReturn order in orderReturn)
                    {
                        ContentValues Values = new ContentValues();
                        Values.Put("Status", order.Status);
                        db.Update("Zakaz", Values, "GUID = ?", new string[] { order.GUID });
                    }
                    log.Text += " - оновлено Замовлення " + oborudReturn.Count + "\n";
                }
                if (taraReturn != null && taraReturn.Count > 0)
                {
                    foreach (DocReturn tara in taraReturn)
                    {
                        ContentValues Values = new ContentValues();
                        Values.Put("Status", tara.Status);
                        db.Update("TaraFacing", Values, "GUID = ?", new string[] { tara.GUID });
                    }
                    log.Text += " - оновлено Фейсинг тари " + taraReturn.Count + "\n";
                }
                if (oborudReturn != null && oborudReturn.Count > 0)
                {
                    foreach (DocReturn oborud in oborudReturn)
                    {
                        ContentValues Values = new ContentValues();
                        Values.Put("Status", oborud.Status);
                        db.Update("OborudFacing", Values, "GUID = ?", new string[] { oborud.GUID });
                    }
                    log.Text += " - оновлено Фейсинг обладнання " + oborudReturn.Count + "\n";
                }
                if (pkos != null && pkos.Count > 0)
                {
                    foreach (PKO pko in pkos)
                    {
                        AddPKO(db, pko);
                    }
                    log.Text += " - додано ПКО " + pkos.Count + "\n";
                }
                if (orders != null && orders.Count > 0)
                {
                    foreach (Order order in orders)
                    {
                        AddOrder(db, order);
                    }
                    log.Text += " - додано Замовлення " + orders.Count + "\n";
                }
            }
        }
        public Klient GetKlient(Context context, int klientCode)
        {
            Klient klient = new Klient();
            using (SQLiteDatabase db = new DBHelper(context).ReadableDatabase)
            {
                cursor = db.Query("Klient", new string[] { "KlientCode", "KlientName" }, "KlientCode = " + klientCode, null, null, null, null);
                if (cursor != null && cursor.MoveToFirst() && cursor.Count > 0)
                {
                    for (int i = 0; i < cursor.Count; i++)
                    {
                        klient.KlientCode = int.Parse(cursor.GetString(0));
                        klient.KlientName = cursor.GetString(1);
                        cursor.MoveToNext();
                    }
                }
            }
            return klient;
        }
        public Dot GetDot(Context context, int klientCode)
        {
            Dot dot = new Dot();
            using (SQLiteDatabase db = new DBHelper(context).ReadableDatabase)
            {
                cursor = db.Query("Dot", new string[] { "DotCode", "DotName", "DotAddress", "DotFillial" }, "KlientCode = " + klientCode, null, null, null, "DotName");
                if (cursor != null && cursor.MoveToFirst() && cursor.Count > 0)
                {
                    for (int i = 0; i < cursor.Count; i++)
                    {
                        dot.DotCode = int.Parse(cursor.GetString(0));
                        dot.DotName = cursor.GetString(1);
                        dot.DotAddress = cursor.GetString(2);
                        dot.DotFillial = cursor.GetString(3);
                        cursor.MoveToNext();
                    }
                }
            }
            return dot;
        }
        public Update GetDocForExport(Context context)
        {
            Update update = new Update();
            List<PKO> pko = new List<PKO>();
            List<Order> order = new List<Order>();
            List<TaraFacing> taraFacing = new List<TaraFacing>();
            List<OborudFacing> oborudFacing = new List<OborudFacing>();

            using (SQLiteDatabase db = new DBHelper(context).ReadableDatabase)
            {
                cursor = db.Query("PKO", new string[] { "GUID", "NumDoc", "DateDoc", "KlientCode", "DotCode", "Summ", "DatePay" }, "Status < 2", null, null, null, null);

                if (cursor != null && cursor.MoveToFirst() && cursor.Count > 0)
                {
                    for (int i = 0; i < cursor.Count; i++)
                    {
                        pko.Add(new PKO()
                        {
                            GUID = cursor.GetString(0),
                            NumDoc = cursor.GetString(1),
                            DateDoc = cursor.GetString(2),
                            KlientCode = cursor.GetInt(3),
                            DotCode = cursor.GetInt(4),
                            Summ = cursor.GetDouble(5),
                            DatePay = cursor.GetString(6),
                            Status = 0,
                        });
                        cursor.MoveToNext();
                    }
                }

                cursor = db.Query("Zakaz", new string[] { "GUID", "DateDoc", "KlientCode", "DotCode", "FlagA", "FlagF", "Comment", "Form" }, "Status < 2", null, null, null, null);

                if (cursor != null && cursor.MoveToFirst() && cursor.Count > 0)
                {
                    for (int i = 0; i < cursor.Count; i++)
                    {
                        List<OrderTab> orderTab = new List<OrderTab>();

                        ICursor cursor1 = db.Query("ZakazTab", new string[] { "GoodCode", "Quantity", "PriceUAH", "Summa" }, "GUID = ?", new string[] { cursor.GetString(0) }, null, null, null);
                        if (cursor1 != null && cursor1.MoveToFirst() && cursor1.Count > 0)
                        {
                            for (int j = 0; j < cursor1.Count; j++)
                            {
                                orderTab.Add(new OrderTab()
                                {
                                    GoodCode = cursor1.GetInt(0),
                                    Quantity = cursor1.GetInt(1),
                                    PriceUAH = cursor1.GetDouble(2),
                                    Summ = cursor1.GetDouble(3)
                                });
                                cursor1.MoveToNext();
                            }
                        }

                        order.Add(new Order()
                        {
                            GUID = cursor.GetString(0),
                            DateDoc = DateTime.Parse(cursor.GetString(1)),
                            KlientCode = cursor.GetInt(2),
                            DotCode = cursor.GetInt(3),
                            Status = 0,
                            FlagA = cursor.GetInt(4),
                            FlagF = cursor.GetInt(5),
                            Comment = cursor.GetString(6),
                            Form = cursor.GetInt(7),
                            OrderTab = orderTab
                        });
                        cursor.MoveToNext();
                    }
                }

                cursor = db.Query("TaraFacing", new string[] { "GUID", "DateDoc", "KlientCode", "DotCode", "TaraCode", "Quantity" }, "Status < 1", null, null, null, null);

                if (cursor != null && cursor.MoveToFirst() && cursor.Count > 0)
                {
                    for (int i = 0; i < cursor.Count; i++)
                    {
                        taraFacing.Add(new TaraFacing()
                        {
                            GUID = cursor.GetString(0),
                            DateDoc = cursor.GetString(1),
                            KlientCode = cursor.GetInt(2),
                            DotCode = cursor.GetInt(3),
                            TaraCode = cursor.GetInt(4),
                            Quantity = cursor.GetDouble(5),
                            Status = 0
                        });
                        cursor.MoveToNext();
                    }
                }

                cursor = db.Query("OborudFacing", new string[] { "GUID", "DateDoc", "KlientCode", "DotCode", "OborudCode", "Quantity" }, "Status < 1", null, null, null, null);

                if (cursor != null && cursor.MoveToFirst() && cursor.Count > 0)
                {
                    for (int i = 0; i < cursor.Count; i++)
                    {
                        oborudFacing.Add(new OborudFacing()
                        {
                            GUID = cursor.GetString(0),
                            DateDoc = cursor.GetString(1),
                            KlientCode = cursor.GetInt(2),
                            DotCode = cursor.GetInt(3),
                            OborudCode = cursor.GetString(4),
                            Quantity = cursor.GetDouble(5),
                            Status = 0
                        });
                        cursor.MoveToNext();
                    }
                }
            }
            update.PKO = pko;
            update.Order = order;
            update.TaraFacing = taraFacing;
            update.OborudFacing = oborudFacing;

            return update;
        }
        public List<PKO> GetPKOJourn(Context context, DateTime date)
        {
            List<PKO> pko = new List<PKO>();
            using (SQLiteDatabase db = new DBHelper(context).ReadableDatabase)
            {
                cursor = db.Query("PKO as P left join Klient as K on K.KlientCode = P.KlientCode", new string[] { "GUID", "DateDoc", "NumDoc", "Summ", "DatePay", "Status", "KlientName" }, "DatePay = ?", new String[]{ date.ToString("yyyy-MM-dd") }, null, null, "KlientName");

                if (cursor != null && cursor.MoveToFirst() && cursor.Count > 0)
                {
                    for (int i = 0; i < cursor.Count; i++)
                    {
                        pko.Add(new PKO()
                        {
                            GUID = cursor.GetString(0),
                            DateDoc = cursor.GetString(1),
                            NumDoc = cursor.GetString(2),
                            Summ = cursor.GetDouble(3),
                            DatePay = cursor.GetString(4),
                            Status = cursor.GetInt(5),
                            KlientName = cursor.GetString(6)
                        });
                        cursor.MoveToNext();
                    }
                }
            }
            return pko;
        }
        public List<PKO> GetPKOList(Context context, int klientCode, int dotCode)
        {
            List<PKO> pko = new List<PKO>();
            using (SQLiteDatabase db = new DBHelper(context).ReadableDatabase)
            {
                if (dotCode > 0)
                    cursor = db.Query("PKO", new string[] { "GUID", "NumDoc", "DateDoc", "DatePay", "Status", "Summ" }, "KlientCode = " + klientCode + " and DotCode = " + dotCode, null, null, null, "NumDoc");
                else
                    cursor = db.Query("PKO", new string[] { "GUID", "NumDoc", "DateDoc", "DatePay", "Status", "Summ" }, "KlientCode = " + klientCode, null, null, null, "NumDoc");

                if (cursor != null && cursor.MoveToFirst() && cursor.Count > 0)
                {
                    for (int i = 0; i < cursor.Count; i++)
                    {
                        pko.Add(new PKO()
                        {
                            GUID = cursor.GetString(0),
                            NumDoc = cursor.GetString(1),
                            DateDoc = cursor.GetString(2),
                            DatePay = cursor.GetString(3),
                            Status = cursor.GetInt(4),
                            Summ = cursor.GetDouble(5)
                        });
                        cursor.MoveToNext();
                    }
                }
            }
            return pko;
        }
        public Order GetOrder(Context context, string GUID)
        {
            Order zakaz = new Order();

            using (SQLiteDatabase db = new DBHelper(context).ReadableDatabase)
            {
                cursor = db.Query("Zakaz", new string[] { "GUID", "DateDoc", "KlientCode", "DotCode", "Status", "Comment", "FlagA", "FlagF", "Form" }, "GUID = ?", new string[] { GUID }, null, null, "DateDoc");

                if (cursor != null && cursor.MoveToFirst() && cursor.Count > 0)
                {
                    for (int i = 0; i < cursor.Count; i++)
                    {
                        zakaz = new Order()
                        {
                            GUID = cursor.GetString(0),
                            DateDoc = DateTime.Parse(cursor.GetString(1)),
                            KlientCode = cursor.GetInt(2),
                            DotCode = cursor.GetInt(3),
                            Status = cursor.GetInt(4),
                            Comment = cursor.GetString(5),
                            FlagA = cursor.GetInt(6),
                            FlagF = cursor.GetInt(7),
                            Form = cursor.GetInt(8)
                        };
                        cursor.MoveToNext();
                    }
                }
            }
            return zakaz;
        }
        public List<Order> GetOrderJourn(Context context, DateTime date)
        {
            List<Order> order = new List<Order>();
            using (SQLiteDatabase db = new DBHelper(context).ReadableDatabase)
            {
                cursor = db.Query("ZakazTab as T left join Zakaz as D on T.GUID = D.GUID left join Klient as K on K.KlientCode = D.KlientCode", new string[] { "D.GUID", "SUM(T.Summa)", "D.Status", "D.KlientCode", "K.KlientName" }, "D.DateDoc = ?", new String[] { date.ToString("yyyy-MM-dd") }, "D.GUID, D.Status, D.KlientCode, K.KlientName", null, "KlientName");

                if (cursor != null && cursor.MoveToFirst() && cursor.Count > 0)
                {
                    for (int i = 0; i < cursor.Count; i++)
                    {
                        order.Add(new Order()
                        {
                            GUID = cursor.GetString(0),
                            Summ = cursor.GetDouble(1),
                            Status = cursor.GetInt(2),
                            KlientCode = cursor.GetInt(3),
                            KlientName = cursor.GetString(4)
                        });
                        cursor.MoveToNext();
                    }
                }
            }
            return order;
        }
        public List<Order> GetZakazList(Context context, int klientCode, int dotCode)
        {
            List<Order> zakaz = new List<Order>();
            using (SQLiteDatabase db = new DBHelper(context).ReadableDatabase)
            {
                if (dotCode > 0)
                    cursor = db.Query("ZakazTab as T left join Zakaz as D on D.GUID = T.GUID", new string[] { "D.GUID", "DateDoc", "KlientCode", "DotCode", "Status", "SUM(Summa)" }, "KlientCode = ? and DotCode = ?", new string[] { klientCode.ToString(), dotCode.ToString() }, "D.GUID,DateDoc,KlientCode,DotCode,Status", null, "DateDoc");
                else
                    cursor = db.Query("ZakazTab as T left join Zakaz as D on D.GUID = T.GUID", new string[] { "D.GUID", "DateDoc", "KlientCode", "DotCode", "Status", "SUM(Summa)" }, "KlientCode = ?", new string[] { klientCode.ToString() }, "D.GUID,DateDoc,KlientCode,DotCode,Status", null, "DateDoc");

                if (cursor != null && cursor.MoveToFirst() && cursor.Count > 0)
                {
                    for (int i = 0; i < cursor.Count; i++)
                    {
                        zakaz.Add(new Order()
                        {
                            GUID = cursor.GetString(0),
                            DateDoc = DateTime.Parse(cursor.GetString(1)),
                            KlientCode = cursor.GetInt(2),
                            DotCode = cursor.GetInt(3),
                            Status = cursor.GetInt(4),
                            Summ = cursor.GetInt(5)
                        });
                        cursor.MoveToNext();
                    }
                }
            }
            return zakaz;
        }
        public List<OrderTab> GetZakazTabList(Context context, string GUID)
        {
            List<OrderTab> zakazTab = new List<OrderTab>();
            using (SQLiteDatabase db = new DBHelper(context).ReadableDatabase)
            {
                cursor = db.Query("ZakazTab as T left join Good G on G.Code = T.GoodCode", new string[] { "GUID", "GoodCode", "Name", "Quantity", "PriceUAH", "Summa" }, "GUID = ?", new string[] { GUID }, null, null, "Name");

                if (cursor != null && cursor.MoveToFirst() && cursor.Count > 0)
                {
                    for (int i = 0; i < cursor.Count; i++)
                    {
                        zakazTab.Add(new OrderTab()
                        {
                            GUID = cursor.GetString(0),
                            GoodCode = cursor.GetInt(1),
                            GoodName = cursor.GetString(2),
                            Quantity = cursor.GetInt(3),
                            PriceUAH = cursor.GetDouble(4),
                            Summ = cursor.GetDouble(5)
                        });
                        cursor.MoveToNext();
                    }
                }
            }
            return zakazTab;
        }
        public List<Klient> GetKlientList(Context context, string searchText)
        {
            List<Klient> klients = new List<Klient>();
            using (SQLiteDatabase db = new DBHelper(context).ReadableDatabase)
            {
                ICursor cursor = db.Query("Klient", new string[] { "KlientCode", "KlientName" }, null, null, null, null, "KlientName");
                if (cursor != null && cursor.MoveToFirst() && cursor.Count > 0)
                {
                    for (int i = 0; i < cursor.Count; i++)
                    {   
                        klients.Add(new Klient()
                        {
                            KlientCode = int.Parse(cursor.GetString(0)),
                            KlientName = cursor.GetString(1)
                        });
                        cursor.MoveToNext();
                    }
                }
            }
            return klients;
        }
        public List<Klient> GetKlientDebetList(Context context, string searchText)
        {
            List<Klient> klients = new List<Klient>();
            using (SQLiteDatabase db = new DBHelper(context).ReadableDatabase)
            {
                string filter = "";
                if (TextUtils.IsEmpty(searchText))
                {
                    filter += "";
                }
                else
                {
                    filter += " and k.KlientName like '%" + searchText + "%'";
                }

                cursor = db.Query("Klient as k left join Debet as d on d.KlientCode = k.KlientCode", new string[] { "k.KlientCode", "k.KlientName", "SUM(d.Dolg)" }, "1=1" + filter, null, "k.KlientCode,k.KlientName", null, "KlientName");

                if (cursor != null && cursor.MoveToFirst() && cursor.Count > 0)
                {
                    for (int i = 0; i < cursor.Count; i++)
                    {
                        klients.Add(new Klient()
                        {
                            KlientCode = int.Parse(cursor.GetString(0)),
                            KlientName = cursor.GetString(1),
                            KlientDebet = cursor.GetDouble(2),
                            KlientWarn = ""
                        });
                        cursor.MoveToNext();
                    }
                }
                cursor = db.Query("Klient as k left join Debet as d on d.KlientCode = k.KlientCode", new string[] { "k.KlientCode", "k.KlientName", "SUM(d.Dolg)" }, "DatePay < date('now')" + filter, null, "k.KlientCode,k.KlientName", null, null);
                if (cursor != null && cursor.MoveToFirst() && cursor.Count > 0)
                {
                    for (int i = 0; i < cursor.Count; i++)
                    {
                        int code = int.Parse(cursor.GetString(0));
                        if (klients.Exists(x => x.KlientCode == code))
                        {
                            int ix = klients.FindIndex(x => x.KlientCode == code);
                            klients[ix].KlientWarn = "!";
                        }
                        cursor.MoveToNext();
                    }
                }
            }
            return klients;
        }
        public List<Dot> GetDotList(Context context, int klientCode, string searchText)
        {
            List<Dot> dots = new List<Dot>();
            using (SQLiteDatabase db = new DBHelper(context).ReadableDatabase)
            {
                string filter = "";
                if (TextUtils.IsEmpty(searchText))
                {
                    filter += "";
                }
                else
                {
                    filter += " and (DotName like '%" + searchText + "%' or DotAddress like '%" + searchText + "%')";
                }

                cursor = db.Query("Dot", new string[] { "DotCode", "DotName", "DotAddress", "DotFillial" }, "KlientCode = " + klientCode + filter, null, null, null, "DotAddress");
                if (cursor != null && cursor.MoveToFirst() && cursor.Count > 0)
                {
                    for (int i = 0; i < cursor.Count; i++)
                    {
                        dots.Add(new Dot()
                        {
                            DotCode = int.Parse(cursor.GetString(0)),
                            DotName = cursor.GetString(1),
                            DotAddress = cursor.GetString(2),
                            DotFillial = cursor.GetString(3)
                        });
                        cursor.MoveToNext();
                    }
                }
            }
            return dots;
        }
        public List<TaraRest> GetTaraList(Context context, int klientCode, int dotCode)
        {
            string today = DateTime.Now.ToString("yyyy-MM-dd");

            List<TaraRest> taraList = new List<TaraRest>();
            using (SQLiteDatabase db = new DBHelper(context).ReadableDatabase)
            {

                if (dotCode > 0)
                    cursor = db.Query("Tara as T " +
                        "left join TaraRest as R on R.TaraCode = T.TaraCode and R.KlientCode = " + klientCode + " and R.DotCode = " + dotCode + " " +
                        "left join TaraFacing as F on F.TaraCode = T.TaraCode and F.KlientCode = " + klientCode + " and F.DotCode = " + dotCode + " and F.DateDoc = date('now')", 
                        new string[] { "T.TaraCode as TaraCode", "T.TaraName as TaraName", "Sum(R.Qty) as Qty", "Sum(F.Quantity) as Fac", "Sum(R.Delay) as Delay" }, 
                        null,
                        null,
                        "T.TaraCode, T.TaraName", 
                        null, 
                        "TaraName");
                else
                    cursor = db.Query("Tara as T " +
                        "left join TaraRest as R on R.TaraCode = T.TaraCode and R.KlientCode = " + klientCode + " " +
                        "left join TaraFacing as F on F.TaraCode = T.TaraCode and F.KlientCode = " + klientCode + " and F.DateDoc = date('now')", 
                        new string[] { "T.TaraCode as TaraCode", "T.TaraName as TaraName", "Sum(R.Qty) as Qty", "Sum(F.Quantity) as Fac", "Sum(R.Delay) as Delay" }, 
                        null, 
                        null, 
                        "T.TaraCode, T.TaraName", 
                        null, 
                        "TaraName");

                if (cursor != null && cursor.MoveToFirst() && cursor.Count > 0)
                {
                    for (int i = 0; i < cursor.Count; i++)
                    {
                        taraList.Add(new TaraRest()
                        {
                            TaraCode = int.Parse(cursor.GetString(0)),
                            TaraName = cursor.GetString(1),
                            Qty = cursor.GetDouble(2),
                            Facing = cursor.GetDouble(3),
                            Dalay = cursor.GetDouble(4),
                        });
                        cursor.MoveToNext();
                    }
                }
            }
            return taraList;
        }
        public List<OborudRest> GetOborudList(Context context, int klientCode, int dotCode)
        {
            List<OborudRest> oborudList = new List<OborudRest>();
            using (SQLiteDatabase db = new DBHelper(context).ReadableDatabase)
            {
                if (dotCode > 0)
                    cursor = db.Query("OborudRest as R left join Oborud as T on R.OborudCode = T.OborudCode left join OborudFacing as F on F.OborudCode = R.OborudCode and F.KlientCode = R.KlientCode and F.DotCode = R.DotCode and F.DateDoc = date('now')", new string[] { "T.OborudCode as OborudCode", "T.OborudName as OborudName", "OborudInventCode as SerialNumber", "Sum(R.Qty) as Qty", "Sum(F.Quantity) as Fac" }, "R.KlientCode = " + klientCode + " and R.DotCode = " + dotCode, null, "T.OborudCode, T.OborudName, OborudInventCode", "Sum(R.Qty) > 0", "OborudName");
                else
                    cursor = db.Query("OborudRest as R left join Oborud as T on R.OborudCode = T.OborudCode left join OborudFacing as F on F.OborudCode = R.OborudCode and F.KlientCode = R.KlientCode and F.DotCode = R.DotCode and F.DateDoc = date('now')", new string[] { "T.OborudCode as OborudCode", "T.OborudName as OborudName", "OborudInventCode as SerialNumber", "Sum(R.Qty) as Qty", "Sum(F.Quantity) as Fac" }, "R.KlientCode = " + klientCode, null, "T.OborudCode, T.OborudName, OborudInventCode", "Sum(R.Qty) > 0", "OborudName");
                
                if (cursor != null && cursor.MoveToFirst() && cursor.Count > 0)
                {
                    for (int i = 0; i < cursor.Count; i++)
                    {
                        oborudList.Add(new OborudRest()
                        {
                            OborudCode = cursor.GetString(0),
                            OborudName = cursor.GetString(1),
                            Qty = double.Parse(cursor.GetString(3)),
                            Facing = cursor.GetDouble(4),
                            SerialNumber = cursor.GetString(2)
                        });
                        cursor.MoveToNext();
                    }
                }
            }
            return oborudList;
        }
        public List<Model.GoodsDirectory> GetGoodsDirectory(Context context)
        {
            List<Model.GoodsDirectory> directory = new List<Model.GoodsDirectory>();

            using (SQLiteDatabase db = new DBHelper(context).ReadableDatabase)
            {
                directory = GetDir(db, directory, 1, 0);
            };
            return directory;
        }
        private List<Model.GoodsDirectory> GetDir(SQLiteDatabase db, List<Model.GoodsDirectory> directory, int level, int parentCode)
        {
            ICursor cursor = db.Query("GoodsDirectory as D left outer join Good as G on G.ParentID = D.Code", new string[] { "D.Code", "D.Name", "D.ParentCode", "D.Level", "COUNT(G.Code)" }, "level = ? and ParentCode = ?", new string[] { level.ToString(), parentCode.ToString() }, "D.Code,D.Name,D.ParentCode,D.Level", null, "D.Name");

            if (cursor != null && cursor.MoveToFirst() && cursor.Count > 0)
            {
                for (int i = 0; i < cursor.Count; i++)
                {
                    directory.Add(new Model.GoodsDirectory()
                    {
                        Code = cursor.GetInt(0),
                        Name = cursor.GetString(1),
                        ParentCode = cursor.GetInt(2),
                        Level = cursor.GetInt(3),
                        GoodCount = cursor.GetInt(4)
                    });
                    directory = GetDir(db, directory, level + 1, cursor.GetInt(0));
                    cursor.MoveToNext();
                }
            }
            return directory;
        }
        public List<Price> GetPriceList(Context context, string searchText, int dirCode, string fillial)
        {
            List<Price> prices = new List<Price>();
            using (SQLiteDatabase db = new DBHelper(context).ReadableDatabase)
            {
                string filter = "";
                if (TextUtils.IsEmpty(searchText))
                    filter += "";
                else
                    filter += " and Good.Name like '%" + searchText + "%'";

                string dir = "";
                if (dirCode != 0)
                    dir = " and ParentId = " + dirCode;
                //and GoodRest.Fillial = " + fillial
                cursor = db.Query("Good as Good left join GoodRest as GoodRest on Good.Code = GoodRest.Code and GoodRest.Fillial = '" + fillial + "'", new string[] { "Good.Code", "Good.Name", "GoodRest.Quantity", "Good.Price", "GoodRest.Fillial" }, "1 = 1" + filter + dir, null, null, null, "Good.Name");

                if (cursor != null && cursor.MoveToFirst() && cursor.Count > 0)
                {
                    for (int i = 0; i < cursor.Count; i++)
                    {
                        prices.Add(new Price()
                        {
                            GoodCode = cursor.GetInt(0),
                            GoodName = cursor.GetString(1),
                            Quantity = cursor.GetDouble(2),
                            PriceUAH = cursor.GetDouble(3),
                        });
                        cursor.MoveToNext();
                    }
                }
            }
            return prices;
        }
        public List<Debet> GetDebetList(Context context, int klientCode, int dotCode)
        {
            List<Debet> debets = new List<Debet>();
            using (SQLiteDatabase db = new DBHelper(context).ReadableDatabase)
            {
                if (dotCode > 0)
                    cursor = db.Query("Debet", new string[] { "NumDoc", "DateDoc", "DatePay", "KlientCode", "DotCode", "Sum(Dolg)" }, "KlientCode = " + klientCode + " and DotCode = " + dotCode, null, "NumDoc, DateDoc, DatePay, KlientCode, DotCode", "Sum(Dolg) < 0", "DatePay");
                else
                    cursor = db.Query("Debet", new string[] { "NumDoc", "DateDoc", "DatePay", "KlientCode", "DotCode", "Sum(Dolg)" }, "KlientCode = " + klientCode, null, "NumDoc, DateDoc, DatePay, KlientCode, DotCode", "Sum(Dolg) < 0", "DatePay");
                
                if (cursor != null && cursor.MoveToFirst() && cursor.Count > 0)
                {
                    for (int i = 0; i < cursor.Count; i++)
                    {
                        debets.Add(new Debet()
                        {
                            NumDoc = cursor.GetString(0),
                            DateDoc = cursor.GetString(1),
                            DatePay = DateTime.Parse(cursor.GetString(2)),
                            KlientCode = cursor.GetInt(3),
                            DotCode = cursor.GetInt(4),
                            Dolg = cursor.GetDouble(5)
                        });
                        cursor.MoveToNext();
                    }
                }
            }
            return debets;
        }
        public Auth GetAuth(Context context)
        {
            Auth auth = new Auth();
            using (SQLiteDatabase db = new DBHelper(context).ReadableDatabase)
            {
                cursor = db.Query("Auth", new string[] { "Name", "Code", "TAC", "Teh", "Base", "Version" }, "ID" + "=?", new string[] { "1" }, null, null, null);
                if (cursor != null && cursor.MoveToFirst() && cursor.Count > 0)
                {
                    auth.Name = cursor.GetString(0);
                    auth.Code = cursor.GetString(1);
                    auth.TAC = cursor.GetInt(2);
                    auth.Type = cursor.GetInt(3);
                    auth.Base= cursor.GetInt(4);
                    auth.Version = cursor.GetString(5);
                }
            }
            return auth;
        }
    }
}