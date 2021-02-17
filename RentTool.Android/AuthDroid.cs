using System;
using System.Threading.Tasks;
using Android.Media.TV;
using Xamarin.Forms;
using Firebase.Auth;
using Firebase.Components;
using RentTool.Droid;

[assembly : Dependency(typeof(AuthDroid))]

    namespace RentTool.Droid
{
    public class AuthDroid : Iauth

    {
        public async Task<string> LogInWithEmailAndPassword(string email, string password)
        {
            try
            {
                var user =
                    await Firebase.Auth.FirebaseAuth.Instance.SignInWithEmailAndPasswordAsync(email, password);
                var token = user.User.Uid;
                return token;

            }
            catch (FirebaseAuthInvalidUserException e)
            {
                e.PrintStackTrace();
                return string.Empty;
            }
            catch (FirebaseAuthInvalidCredentialsException e)
            {
                e.PrintStackTrace();
                return string.Empty;
            }
        }

        public async Task<string> SignUpWithEmailAndPassword(string email, string password)
        {
            try
            {
                var newUser =
                    await Firebase.Auth.FirebaseAuth.Instance.CreateUserWithEmailAndPasswordAsync(email, password);
                var token = newUser.User.Uid;
                return token;

            }
            catch (FirebaseAuthInvalidUserException e)
            {
                e.PrintStackTrace();
                return string.Empty;
            }
            catch (FirebaseAuthInvalidCredentialsException e)
            {
                e.PrintStackTrace();
                return string.Empty;
            }
        }

        public bool SignOut()
        {
            try
            {
                Firebase.Auth.FirebaseAuth.Instance.SignOut();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public bool IsSignIn()
        {
            var user = Firebase.Auth.FirebaseAuth.Instance.CurrentUser;
            return user != null;        }
    }
}