////using System;
////using System.Collections.Generic;
////using System.Linq;
////using System.Text;
////using System.Threading.Tasks;
////using DTO;
////using DAL;


////namespace TestBUS
////{
////    public class BUS_TaiKhoan
////    {
////        TaiKhoanAcess tkAccess = new TaiKhoanAcess();
////        public string CheckLogic(DTO_TaiKhoan taikhoan)
////        {
////            // Kiểm tra nghiệp vụ
////            if (taikhoan.EMAIL == "")
////            {
////                return "requeid_taikhoan";
////            }
//namespace TestBUS
//{
//    public class BUS_TaiKhoan
//    {
//        DAL_TaiKhoan tkAccess = new DAL_TaiKhoan();
//        public string CheckLogic(DTO_TaiKhoan taikhoan)
//        {
//            // Kiểm tra nghiệp vụ
//            if (taikhoan.EMAIL == "")
//            {
//                return "requeid_taikhoan";
//            }

////            if (taikhoan.PASS == "")
////            {
////                return "requeid_password";
////            }

////            string info = tkAccess.CheckLogic(taikhoan);
////            return info;
////        }
////        public string TaoTaiKhoan(DTO_TaiKhoan taiKhoan)
////        {
////            // Kiểm tra tính hợp lệ của thông tin tài khoản nếu cần
////            if (!IsValidAccount(taiKhoan))
////            {
////                return "Thông tin tài khoản không hợp lệ!";
////            }
////            // Tạo một instance của lớp DAL_TaiKhoan
////            TaoTaiKhoan dalTaiKhoan = new TaoTaiKhoan();

//            // Tạo một instance của lớp DAL_TaiKhoan
//            DAL_TaiKhoan dalTaiKhoan = new DAL_TaiKhoan();
            
////            // Gọi phương thức tạo tài khoản trong lớp DAL_TaiKhoan
////            string result = dalTaiKhoan.TaoMoiTaiKhoan(taiKhoan);

////            // Trả về kết quả từ phương thức trong lớp DAL_TaiKhoan
////            return result;
////        }


////        private bool IsValidAccount(DTO_TaiKhoan taiKhoan)
////        {
////            // Thực hiện các kiểm tra tính hợp lệ của thông tin tài khoản
////            // Ví dụ: kiểm tra độ dài của mật khẩu, định dạng email, v.v.
////            // Trả về true nếu thông tin hợp lệ, ngược lại trả về false
////            // Bạn có thể cải tiến hàm này tùy theo yêu cầu của ứng dụng của bạn
////            return true;
////        }
////    }
////    public class DoiMatKhauBUS
////    {
////        private readonly DoiMatKhau taiKhoanDAL = new DoiMatKhau();

////        public string ChangePassword(string email, string oldPassword, string newPassword)
////        {
////            return taiKhoanDAL.ChangePassword(email, oldPassword, newPassword);
////        }
////    }
////}

//        private bool IsValidAccount(DTO_TaiKhoan taiKhoan)
//        {
//            // Thực hiện các kiểm tra tính hợp lệ của thông tin tài khoản
//            // Ví dụ: kiểm tra độ dài của mật khẩu, định dạng email, v.v.
//            // Trả về true nếu thông tin hợp lệ, ngược lại trả về false
//            // Bạn có thể cải tiến hàm này tùy theo yêu cầu của ứng dụng của bạn
//            return true;
//        }
//    }
//    public class DoiMatKhauBUS
//    {
//        private readonly DAL_TaiKhoan taiKhoanDAL = new DAL_TaiKhoan();

//        public (string, DTO_TaiKhoan) ChangePassword(string email, string oldPassword, string newPassword)
//        {
//            return taiKhoanDAL.ChangePassword(email, oldPassword, newPassword);
//        }
//    }
//}

