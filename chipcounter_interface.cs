using System.Collections.Generic;
using System.Drawing;

namespace chip_counter
{
    public enum BARCODE_SCAN_STATE
    {
        NONE,
        SCAN,
        COMPLETE,
        MES_COMPLETE,
        EMPTY,
        SENSOR_SCAN
    }

    public enum BARCODE_SCAN_TYPE
    {
        UNUSE_SCAN = 0,
        ONE_SCAN,
        TWO_SCAN
    }

    public class barcode_info
    {
        public BARCODE_SCAN_STATE state = BARCODE_SCAN_STATE.NONE;
        public BARCODE_SCAN_TYPE scan_type = 0;
        public string barcode1 = "";
        public string barcode2 = "";
        public string lot = "";
        public string parts = "";
        public string title = "Chip Count";
        public string date = "";
        public string extend1 = "";
        public string extend2 = "";
        public string extend3 = "";
        public string extend4 = "";
        public int count_num = 0;
        public string resultFolder = "";
        public int idx = 0;
        public Color BarcodeColor = Color.FromArgb(255, 255, 255);
        public Dictionary<string, string> opt = new Dictionary<string, string>();

        public void Copy(ref barcode_info dest)
        {
            dest.barcode1 = this.barcode1;
            dest.barcode2 = this.barcode2;
            dest.title = this.title;
            dest.date = this.date;
            dest.extend1 = this.extend1;
            dest.extend2 = this.extend2;
            dest.extend3 = this.extend3;
            dest.extend4 = this.extend4;
            dest.count_num = this.count_num;
            dest.resultFolder = this.resultFolder;
            dest.lot = this.lot;
            dest.parts = this.parts;
            dest.idx = this.idx;
            dest.BarcodeColor = BarcodeColor;
            dest.opt.Clear();
            //dest.state = state;
            //dest.scan_type = BARCODE_SCAN_TYPE.UNUSE_SCAN;

            foreach (string key in opt.Keys)
            {
                dest.opt.Add(key, opt[key]);
            }
        }

        public bool IsEmpty()
        {
            if (barcode1.Length > 0 ||
                barcode2.Length > 0 ||
                lot.Length > 0 ||
                parts.Length > 0 ||
                extend1.Length > 0 ||
                extend2.Length > 0 ||
                extend3.Length > 0 ||
                extend4.Length > 0)
                return false;

            return true;
        }

        public void Clear()
        {
            barcode1 = "";
            barcode2 = "";
            title = "Chip Count";
            date = "";
            extend1 = "";
            extend2 = "";
            extend3 = "";
            extend4 = "";
            count_num = 0;
            resultFolder = "";
            lot = "";
            parts = "";
            idx = 0;
            BarcodeColor = Color.FromArgb(255, 255, 255);
            state = BARCODE_SCAN_STATE.NONE;
            opt.Clear();
        }
    }

    public interface IMesInterface
    {
        bool mes_init();

        bool mes_deinit();

        int mes_status(string dummy, int data);

        bool mes_barcode_validation(string barcode, out barcode_info bcode_info, out string error);

        bool mes_count_complete(int num, ref barcode_info bcode_info, out barcode_info out_bcode, out string error);
        bool mes_count_print(int num, ref barcode_info bcode_info, out barcode_info out_bcode, out string error);
        //bool mes_option(string msg, ref Dictionary<string, string> dic, out Dictionary<string, string> result_dic, ref string e);
    }

    public class Class1
    {
    }
}
