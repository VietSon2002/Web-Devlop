using System.Globalization;

namespace SV20T1020242.Web
{
    public static class Converter
    {
        /// <summary>
        /// Chuyển chuổi s sang kiểu dữa liệu DateTime theo các fomat được quy định
        /// Hàm trả về null nếu ko thành công
        /// </summary>
        /// <param name="s"></param>
        /// <param name="formats"></param>
        /// <returns></returns>
        public static DateTime? ToDateTime(this string s, string formats = "d/M/yyyy;d-M-yyyy;d.M.yyyy")
        {
            try
            {
                return DateTime.ParseExact(s, formats.Split(';'), CultureInfo.InvariantCulture);

            }
            catch
            {
                return null;
            }
        }
    }
}
