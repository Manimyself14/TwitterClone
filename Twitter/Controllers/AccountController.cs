using System;
using System.Web.Mvc;
using Twitter.DAL;
using Twitter.Models;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Twitter.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account /Login 
        public ActionResult Login()
        {
            string sessionId = Session["userId"]?.ToString();
            if (!string.IsNullOrWhiteSpace(sessionId))
            {
                return RedirectToAction("Dashboard");
            }
            return View();
        }

        // GET: Account /Register 
        public ActionResult Register()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Dashboard()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(RegisterViewModel registerViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    TwitterContext context = new TwitterContext();
                    var chkUser = (from s in context.Person where s.userId == registerViewModel.Username || s.Email == registerViewModel.Email select s).FirstOrDefault();
                    if (chkUser == null)
                    {
                        var keyNew = GeneratePassword(10);
                        var password = EncodePassword(registerViewModel.Password, keyNew);
                        var person = new Person
                        {

                            userId = registerViewModel.Username,
                            Email = registerViewModel.Email,
                            FullName = registerViewModel.FullName,
                            password = password,
                            SCode = keyNew,
                            Joined = DateTime.Now,
                            Active = true
                        };

                        context.Person.Add(person);
                        context.SaveChanges();
                        Session["userId"] = person.userId;
                        return RedirectToAction("Dashboard");
                    }
                    return View(registerViewModel);
                }
                ViewBag.ErrorMessage = "User Allredy Exixts!!!!!!!!!!";
                return View();
            }
            catch (Exception e)
            {
                ViewBag.ErrorMessage = "Some exception occured" + e;
                return View();
            }
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel loginViewModel)
        {
            try
            {
                using (var context = new TwitterContext())
                {
                    var getUser = (from s in context.Person where s.userId == loginViewModel.Username select s).FirstOrDefault();
                    if (getUser != null)
                    {
                        var hashCode = getUser.SCode;
                        //Password Hasing Process Call Helper Class Method    
                        var encodingPasswordString = EncodePassword(loginViewModel.Password, hashCode);
                        //Check Login Detail User Name Or Password    
                        var query = (from s in context.Person where (s.userId == loginViewModel.Username) && s.password.Equals(encodingPasswordString) select s).FirstOrDefault();
                        if (query != null)
                        {
                            Session["userId"] = query.userId;
                            return RedirectToAction("Dashboard");
                        }
                        ViewBag.ErrorMessage = "Invallid User Name or Password";
                        return View();
                    }
                    ViewBag.ErrorMessage = "Invallid User Name or Password";
                    return View();
                }
            }
            catch (Exception e)
            {
                ViewBag.ErrorMessage = " Error!!!";
                return View();
            }
        }

        public string EncodePassword(string pass, string salt) //encrypt password    
        {
            byte[] bytes = Encoding.Unicode.GetBytes(pass);
            byte[] src = Encoding.Unicode.GetBytes(salt);
            byte[] dst = new byte[src.Length + bytes.Length];
            Buffer.BlockCopy(src, 0, dst, 0, src.Length);
            Buffer.BlockCopy(bytes, 0, dst, src.Length, bytes.Length);
            HashAlgorithm algorithm = HashAlgorithm.Create("SHA1");
            byte[] inArray = algorithm.ComputeHash(dst);
            //return Convert.ToBase64String(inArray);    
            return EncodePasswordMd5(Convert.ToBase64String(inArray));
        }

        public string EncodePasswordMd5(string pass) //Encrypt using MD5    
        {
            Byte[] originalBytes;
            Byte[] encodedBytes;
            MD5 md5;
            //Instantiate MD5CryptoServiceProvider, get bytes for original password and compute hash (encoded password)    
            md5 = new MD5CryptoServiceProvider();
            originalBytes = ASCIIEncoding.Default.GetBytes(pass);
            encodedBytes = md5.ComputeHash(originalBytes);
            //Convert encoded bytes back to a 'readable' string    
            return BitConverter.ToString(encodedBytes);
        }

        public string GeneratePassword(int length) //length of salt    
        {
            const string allowedChars = "abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ0123456789";
            var randNum = new Random();
            var chars = new char[length];
            var allowedCharCount = allowedChars.Length;
            for (var i = 0; i <= length - 1; i++)
            {
                chars[i] = allowedChars[Convert.ToInt32((allowedChars.Length) * randNum.NextDouble())];
            }
            return new string(chars);
        }

        public ActionResult Logout()
        {
            Session.Abandon();
            return RedirectToAction("Login");
        }
    }
}