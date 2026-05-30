// Platform-independent stubs for Windows-specific input types so the project can compile on non-Windows
// platforms. These types intentionally live in VAR.Toolbox.Code.Windows to avoid name conflicts with
// Avalonia and other libraries.

#if !WINDOWS
namespace VAR.Toolbox.Code.Windows
{
    // Minimal Keys enum used only for compilation; values are placeholders.
    public enum Keys : int
    {
        None = 0
    }

    public class KeyEventArgs : System.EventArgs
    {
        public Keys KeyCode { get; }
        public bool Handled { get; set; }
        public KeyEventArgs(Keys k) { KeyCode = k; }
    }

    public delegate void KeyEventHandler(object sender, KeyEventArgs e);
}
#endif

