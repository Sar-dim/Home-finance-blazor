using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using net_core_backend.Services.Interfaces;
using System;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ASP.NET_Core_Blazor
{
    public class AuthOptions
    {
        public const string ISSUER = "MyAuthServer"; // издатель токена
        public const string AUDIENCE = "MyAuthClient"; // потребитель токена
        const string KEY = "mysupersecret_secretkey!123";   // ключ для шифрации
        public const int LIFETIME = 30; // время жизни токена - 30 минут
        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
        }
    }

	public class AuthService : AuthenticationStateProvider
	{
        private readonly IPersonService _personService;
        private ClaimsIdentity _user;
        private IHttpContextAccessor _context;

        public AuthService(IPersonService personService, IHttpContextAccessor httpContext)
		{
            this._personService = personService ?? throw new ArgumentNullException(nameof(personService));
            _context = httpContext ?? throw new ArgumentNullException(_context.GetType().Name);
		}

        public void Login(string user, string password)
		{
            _user = this._personService.GetIdentity(user, password);
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
		}

        public void Logout()
        {
            _user = new ClaimsIdentity();
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

		public override Task<AuthenticationState> GetAuthenticationStateAsync()
		{
            var identity = GetUserInfo();
			this.AuthenticationStateChanged += StateChanged;
            return Task.FromResult(new AuthenticationState(new ClaimsPrincipal(identity)));
        }

		public ClaimsIdentity GetUserInfo()
		{
			if (_user != null && _user.IsAuthenticated) return _user;

			return new ClaimsIdentity();
		}

        public void StateChanged(Task<AuthenticationState> state)
		{
            var stateResult = state.GetAwaiter().GetResult();

            _context.HttpContext.User = stateResult.User;
		}
	}
}
