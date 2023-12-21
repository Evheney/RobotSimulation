using System.Globalization;

namespace chip_counter
{
    /**
     * chip counter ini is read here
     */
    public enum chip_counter_mode
    {
        AUTO,
        MANUAL
    }
    public enum chip_counter_type { DIV1, DIV4, ETC }

    public enum REEL_SENSOR
    {
        NOT_USE = 0,
        USE = 1
    }


    public enum ImageViewerNotify
    {
        REQUEST_BARCODE_SCAN = 0,
        REQUEST_PREPARE_BARCODE,
        REQUEST_PRINT,
        REQUEST_REGISTER,
        REQUEST_UPLOAD,
        REQUEST_INSPECT,
        REQUEST_MANUAL_BARCODE,
        REQUEST_EDIT_QUANTITY,
        REQUEST_IMAGE_LUT_CONTROL,
        REQUEST_PRESS_EMPTY,
        REQUEST_EDIT_RESULT_COUNT
    }
    public enum PRINT_TYPE
    {
        MANUAL,
        AUTO
    }

    public enum ImageViewerZoom
    {
        ZoomIn,
        ZoomOut
    }

    public enum AUTOSMART_SCANNER_VALIDATION_RESULT { OK, FILTER_ERROR, MES_VALIDATION_ERROR, UNKNOWN_ERROR };

    public class reel_stage_position
    {
        public int no = 0;
        //public barcode_info barcode = new barcode_info();
    }

    public class userinfo
    {
        public string id = "Administrator";
        public string passwd = "...";
        public string department = "";
        public string info = "";
        public string opt = "";
        public string descriptions = "";
        public string role = "";
    }
    public enum UserRole
    {
        Administrator, //0
        User,          //1
        Guest          //2
    }

    class ChipCounterInfo
    {
        public CultureInfo cul = null;
        public configure config = null;
        private static ChipCounterInfo _instance;
        public userinfo select_user;
        public bool is_administrator = false;
        public bool is_user = false;
        public bool xray_error_status = false;
        public bool is_stage_error = false;
        public bool need_0402r_mode = false;
        public bool use_mes = false;
        public chip_counter_type type = chip_counter_type.DIV1;
        public chip_counter_mode mode = chip_counter_mode.AUTO;


        public static ChipCounterInfo Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ChipCounterInfo();
                }

                return _instance;
            }
        }


    }
}
