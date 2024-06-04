using DAL;
using DTO;
using System;
using System.Text.RegularExpressions;
//using MailKit.Net.Smtp;
//using MailKit.Security;
//using MimeKit;

namespace BUS
{
    public class BUS_TaiKhoan
    {
        //--Duoc goi khi event handling cua button dang nhap duoc goi
        //--Kiem tra tai khoan co ton tai hay khong, neu ton tai thi tra ve mot doi tuong cua lop DTO_NhanVien
        //
        static BUS_TaiKhoan _instance = new BUS_TaiKhoan();
        public static BUS_TaiKhoan Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new BUS_TaiKhoan();
                return _instance;
            }
        }

        DAL_TaiKhoan dalTK = new DAL_TaiKhoan();
        DAL_QuyenHan dalQH = new DAL_QuyenHan();
        DAL_NhanVien dalNV = new DAL_NhanVien();
        public (DTO_TaiKhoan? , string) Login(DTO_TaiKhoan tk)
        { 
            (DTO_TaiKhoan temp, string mess) = dalTK.CheckLogicDTO(tk);
            if (temp != null )
            {
                if (dalNV.GetByID(temp.MANV).MANV == "")
                {
                    return (null, "Bạn không có quyền truy cập vào tài khoản này");
                }    
            }
            else if (temp == null)
            {
                if (mess != "")
                    return (temp, mess);
                return (temp, "Thông tin đăng nhập không chính xác.");
            }
            return (temp, "Đăng nhập thành công");
            
        }

        public (string ,DTO_TaiKhoan?) ChangePassWord(string email, string oldPass, string newPass) 
        {
            if(IsValidEmail(email))
            {
                return ("Invalid_email", null);
            }
            else if(IsValidPassword(oldPass) || IsValidPassword(newPass)) 
            {
                return ("Invalid_password", null);
            }
            return dalTK.ChangePassword(email, oldPass, newPass);
            //Tra ve instance moi cua DTO_NhanVien
        }

        public string TaoMoiTaiKhoan(DTO_TaiKhoan tk)
        {
            if (!IsValidEmail(tk.EMAIL))
                return "Invalid_email";
            else
            {
                return dalTK.TaoMoiTaiKhoan(tk);
            }
        }

        public bool checkQH(DTO_TaiKhoan tk, string QH) {
            return dalQH.CheckPermission(tk, QH);
        }
        /*
        public string SendCode(string email)
        {
          
            //Tao code
            Random r = new Random();
            string code = "";
            for(int i = 0; i < 6; i++)
            {
                code += r.Next(0, 9).ToString();
            }

            // Thông tin tài khoản email
            string fromEmail = "your-email@gmail.com";
            string password = "your-email-password";
            string toEmail = "recipient-email@example.com";

            // Tạo đối tượng MimeMessage
            MimeMessage message = new MimeMessage();
            message.From.Add(new MailboxAddress("SE104_Group14", fromEmail));
            message.To.Add(new MailboxAddress("", toEmail));
            message.Subject = "MA XAC NHAN";
            message.Body = new TextPart("plain")
            {
                Text = "Ma xac nhan cua ban la: " + code
            };

            // Cấu hình thông tin SMTP
            using (var client = new SmtpClient())
            {
                try
                {
                    client.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
                    client.Authenticate(fromEmail, password);
                    client.Send(message);
                    client.Disconnect(true);

                    return ("Email đã được gửi thành công.");
                }
                catch (Exception ex)
                {
                    return ("Có lỗi xảy ra khi gửi email: " + ex.Message);
                }
            }
        }*/
        //Cac ham  kiem tra
        //Kiem tra ten dang nhap
        public bool IsValidEmail(string email)
        {
            if (email == null)
                return false;
            else
            {
                // Mẫu regex để kiểm tra định dạng email
                string pattern = @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$";

                // Kiểm tra xem chuỗi khớp với mẫu regex hay không
                bool isMatch = Regex.IsMatch(email, pattern);

                return isMatch;
            }
        }

        //Kiem tra mat khau
        public bool IsValidPassword (string password) 
        { 
            if(password == null)
            {
                return false;   
            }
            else
            {
                string pattern = @"^[A-Za-z0-9]{6,}$";
                return Regex.IsMatch(password, pattern);
            }
        }




    }
}
