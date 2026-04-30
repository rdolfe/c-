using SecurIT_Memory.Forms;


/// <summary>
/// Point d'entrée de l'application SecurIT Memory.
/// Configure le contexte d'application WinForms et lance le menu principal.
/// </summary>
internal static class Program
{
    [STAThread]
    static void Main()
    {
        // Activer les styles visuels Windows modernes
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        ApplicationConfiguration.Initialize();

        // Lancer le menu principal
        Application.Run(new FormMenu());
    }
}