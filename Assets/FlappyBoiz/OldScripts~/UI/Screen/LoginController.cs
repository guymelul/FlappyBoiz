using FlappyBoiz.Scripts.Managers;
using UnityEngine;

namespace FlappyBoiz.Scripts.UI.Screen
{
    [DisallowMultipleComponent]
    public class LoginController : MonoBehaviour
    {
        public void Anonymous()
        {
            AuthenticationManager.Instance.AnonymousLogin();
        }

        public void LoginWithPlayGameServices()
        {
            AuthenticationManager.Instance.GooglePlayGamesLogin();
        }
    }
}