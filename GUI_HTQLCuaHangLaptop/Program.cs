namespace GUI_HTQLCuaHangLaptop
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            // Hiển thị FormDangNhap trước khi vào FormMain
            Application.Run(new FormDangNhap());
        }
    }
}