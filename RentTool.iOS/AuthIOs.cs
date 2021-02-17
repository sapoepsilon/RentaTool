using System;
using System.Threading.Tasks;
using RentTool.iOS;
using Firebase.Auth;
using Foundation;
using Xamarin.Forms;


[assembly: Dependency(typeof(AuthIOs))]
namespace RentTool.iOS
{
    public class AuthIOs : Iauth
    {
        public async Task<string> LogInWithEmailAndPassword(string email, string password)
        {
            try
            {
                var user = await Auth.DefaultInstance.SignInWithPasswordAsync(email, password);
                return await user.User.GetIdTokenAsync();
            }
            catch (Exception e)
            {
                return String.Empty;
            }
        }

        public async Task<string> SignUpWithEmailAndPassword(string email, string password)
        {
            try
            {
                var newUser = await Auth.DefaultInstance.CreateUserAsync(email, password);
                return await newUser.User.GetIdTokenAsync();
            }
            catch (Exception e)
            {
                return String.Empty;
            }
        }

        public bool SignOut()
        {
            try
            {
                _ = Auth.DefaultInstance.SignOut(out NSError error);
                return error == null;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public bool IsSignIn()
        {
            var user = Auth.DefaultInstance.CurrentUser;
            return user != null;
        }
    }
}