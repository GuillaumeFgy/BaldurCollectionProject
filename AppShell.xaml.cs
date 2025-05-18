namespace MyApp
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(MainView), typeof(MainView));
            Routing.RegisterRoute(nameof(DetailsView), typeof(DetailsView));
            Routing.RegisterRoute(nameof(GraphView), typeof(GraphView));
            Routing.RegisterRoute(nameof(UserView), typeof(UserView));
            Routing.RegisterRoute(nameof(LoginView), typeof(LoginView));
            Routing.RegisterRoute(nameof(AdminUsersView), typeof(AdminUsersView));
        }
    }
}
