using System.Globalization;

namespace chip_counter
{
    /**
     * chip counter ini is read here
     */
  

    class ChipCounterInfo
    {
        public CultureInfo cul = null;
        public configure config = null;
        private static ChipCounterInfo _instance;


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
