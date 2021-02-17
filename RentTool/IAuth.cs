using System;
using System.Threading.Tasks;

namespace RentTool
{
    public interface Iauth
    {
        Task<string> LogInWithEmailAndPassword(string email, string password);
        Task<string> SignUpWithEmailAndPassword(string email, string password);
        bool SignOut();
        bool IsSignIn();
    }
}