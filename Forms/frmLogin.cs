using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MajesticDesktop.Services;

namespace MajesticDesktop.Forms
{
    public partial class frmLogin : Form
    {
        private sealed class IntroItem
        {
            public Control Control;
            public int Top;
            public float Delay;
        }

        [DllImport("user32.dll")]
        private static extern bool ReleaseCapture();

        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);

        [DllImport("dwmapi.dll")]
        private static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int attrValue, int attrSize);

        private const int WM_NCLBUTTONDOWN = 0xA1;
        private const int HT_CAPTION = 2;
        private const int DWMWA_WINDOW_CORNER_PREFERENCE = 33;
        private const int DWMWCP_ROUND = 2;
        private const int CS_DROPSHADOW = 0x00020000;

        private AuthService authService;

        private LoginBrandingPanel _branding;
        private LoginBufferedPanel _pnlRight;
        private LoginLabel _lblGreeting;
        private LoginLabel _lblTitle;
        private LoginLabel _lblSubtitle;
        private LoginLabel _lblCaps;
        private LoginLabel _lblError;
        private Panel _accentBar;
        private LoginInputBox _inUser;
        private LoginInputBox _inPass;
        private LoginCheck _chkRemember;
        private LoginButton _btnLogin;
        private LoginButton _btnExit;
        private LoginWindowButton _btnClose;
        private LoginWindowButton _btnMin;

        private readonly List<IntroItem> _intro = new List<IntroItem>();
        private int _barRight;
        private int _barTop;

        private Timer _introTimer;
        private Timer _shakeTimer;
        private Timer _fadeTimer;
        private readonly Stopwatch _introClock = new Stopwatch();
        private readonly Stopwatch _shakeClock = new Stopwatch();
        private readonly Stopwatch _fadeClock = new Stopwatch();
        private Point _shakeOrigin;
        private Action _fadeDone;
        private bool _busy;

        public frmLogin()
        {
            InitializeComponent();

            ClientSize = new Size(S(880), S(560));
            DoubleBuffered = true;
            Opacity = 0;

            _introTimer = new Timer(components) { Interval = 15 };
            _introTimer.Tick += IntroTick;

            _shakeTimer = new Timer(components) { Interval = 15 };
            _shakeTimer.Tick += ShakeTick;

            _fadeTimer = new Timer(components) { Interval = 15 };
            _fadeTimer.Tick += FadeTick;

            BuildUi();
            LoadRememberedUser();
        }

        private static int S(int v) => LoginDpi.S(v);
        private static float S(float v) => LoginDpi.S(v);

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ClassStyle |= CS_DROPSHADOW;
                return cp;
            }
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            try
            {
                int pref = DWMWCP_ROUND;
                DwmSetWindowAttribute(Handle, DWMWA_WINDOW_CORNER_PREFERENCE, ref pref, sizeof(int));
            }
            catch { }
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            _branding.StartAnimation();
            _introClock.Restart();
            _introTimer.Start();

            if (string.IsNullOrEmpty(_inUser.Value))
                _inUser.FocusInput();
            else
                _inPass.FocusInput();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
            {
                DoExit();
                return true;
            }
            if (keyData == Keys.Enter)
            {
                if (_btnExit != null && _btnExit.Focused)
                    DoExit();
                else
                    DoLogin();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void BuildUi()
        {
            int border = 1;
            int innerW = ClientSize.Width - 2 * border;
            int innerH = ClientSize.Height - 2 * border;
            int brandW = S(340);

            BackColor = LoginTheme.Window;

            _branding = new LoginBrandingPanel();
            _branding.SetBounds(border, border, brandW, innerH);

            _pnlRight = new LoginBufferedPanel();
            _pnlRight.BackColor = LoginTheme.PanelBg;
            _pnlRight.SetBounds(border + brandW, border, innerW - brandW, innerH);

            Controls.Add(_pnlRight);
            Controls.Add(_branding);

            _btnClose = new LoginWindowButton(LoginWindowKind.Close) { Location = new Point(S(12), S(12)) };
            _btnMin = new LoginWindowButton(LoginWindowKind.Minimize) { Location = new Point(S(12) + _btnClose.Width + S(4), S(12)) };
            _branding.Controls.Add(_btnClose);
            _branding.Controls.Add(_btnMin);

            int mx = S(64);
            int cw = _pnlRight.Width - 2 * mx;

            _lblGreeting = MakeLabel(GreetingText(), 11f, FontStyle.Bold, LoginTheme.Gold, mx, S(40), cw, S(26));
            _lblTitle = MakeLabel("تسجيل الدخول", 27f, FontStyle.Bold, LoginTheme.Text, mx, S(64), cw, S(56));

            _barTop = S(124);
            _barRight = mx + cw;
            _accentBar = new Panel { BackColor = LoginTheme.Gold };
            _accentBar.SetBounds(_barRight, _barTop, 0, S(4));

            _lblSubtitle = MakeLabel("سجّل دخولك للمتابعة إلى لوحة التحكم", 10.5f, FontStyle.Regular, LoginTheme.Muted, mx, S(136), cw, S(26));

            int y1 = S(180);
            int inputH = LoginInputBox.PreferredHeight;
            int y2 = y1 + inputH + S(2);
            int yRow = y2 + inputH + S(4);

            _inUser = new LoginInputBox("اسم المستخدم", LoginIcon.User, false);
            _inUser.SetBounds(mx, y1, cw, inputH);

            _inPass = new LoginInputBox("كلمة المرور", LoginIcon.Lock, true);
            _inPass.SetBounds(mx, y2, cw, inputH);

            _chkRemember = new LoginCheck("تذكر اسم المستخدم");
            _chkRemember.SetBounds(mx + cw - S(200), yRow, S(200), S(30));

            _lblCaps = MakeLabel("زر Caps Lock مفعّل", 9.5f, FontStyle.Bold, LoginTheme.Warn, mx, yRow, cw - S(210), S(30));
            _lblCaps.HAlign = StringAlignment.Far;
            _lblCaps.SetOpacity(0f);

            _lblError = MakeLabel(string.Empty, 10f, FontStyle.Bold, LoginTheme.Danger, mx, yRow + S(32), cw, S(26));
            _lblError.SetOpacity(0f);

            int yBtn = yRow + S(62);
            _btnLogin = new LoginButton(LoginButtonStyle.Primary, LoginTheme.Success) { Text = "تسجيل الدخول" };
            _btnLogin.SetBounds(mx - S(10), yBtn, cw + S(20), S(68));

            _btnExit = new LoginButton(LoginButtonStyle.Ghost, LoginTheme.Danger) { Text = "خروج" };
            _btnExit.SetBounds(mx - S(10), yBtn + S(70), cw + S(20), S(48));

            _pnlRight.Controls.AddRange(new Control[]
            {
                _lblGreeting, _lblTitle, _accentBar, _lblSubtitle,
                _inUser, _inPass, _chkRemember, _lblCaps, _lblError,
                _btnLogin, _btnExit
            });

            _inUser.TabIndex = 0;
            _inPass.TabIndex = 1;
            _chkRemember.TabIndex = 2;
            _btnLogin.TabIndex = 3;
            _btnExit.TabIndex = 4;

            RegisterIntro(_lblGreeting, 0.05f);
            RegisterIntro(_lblTitle, 0.12f);
            RegisterIntro(_lblSubtitle, 0.18f);
            RegisterIntro(_inUser, 0.26f);
            RegisterIntro(_inPass, 0.32f);
            RegisterIntro(_chkRemember, 0.38f);
            RegisterIntro(_btnLogin, 0.44f);
            RegisterIntro(_btnExit, 0.50f);

            _inUser.InnerTextChanged += (s, e) => ClearError();
            _inPass.InnerTextChanged += (s, e) => ClearError();
            _inPass.InnerKeyUp += (s, e) => UpdateCaps();
            _inPass.InnerGotFocus += (s, e) => UpdateCaps();
            _inPass.InnerLostFocus += (s, e) => UpdateCaps();

            _btnLogin.Click += (s, e) => DoLogin();
            _btnExit.Click += (s, e) => DoExit();
            _btnClose.Click += (s, e) => DoExit();
            _btnMin.Click += (s, e) => WindowState = FormWindowState.Minimized;

            Control[] dragSurfaces = new Control[] { _branding, _pnlRight, _lblGreeting, _lblTitle, _lblSubtitle };
            foreach (Control c in dragSurfaces)
                c.MouseDown += BeginDrag;
        }

        private LoginLabel MakeLabel(string text, float size, FontStyle style, Color color, int x, int y, int w, int h)
        {
            LoginLabel l = new LoginLabel(size, style)
            {
                Text = text,
                ForeColor = color
            };
            l.SetBounds(x, y, w, h);
            return l;
        }

        private void RegisterIntro(Control c, float delay)
        {
            IntroItem item = new IntroItem { Control = c, Top = c.Top, Delay = delay };
            c.Top = item.Top + S(26);
            _intro.Add(item);
        }

        private static string GreetingText()
        {
            int h = DateTime.Now.Hour;
            if (h >= 5 && h < 12) return "صباح الخير";
            if (h >= 12 && h < 18) return "طاب يومك";
            return "مساء الخير";
        }

        private void IntroTick(object sender, EventArgs e)
        {
            float t = (float)_introClock.Elapsed.TotalSeconds;
            Opacity = LoginFx.Clamp01(t / 0.30f);

            for (int i = 0; i < _intro.Count; i++)
            {
                IntroItem item = _intro[i];
                float p = LoginFx.EaseOutCubic(LoginFx.Seg(t, item.Delay, item.Delay + 0.6f));
                item.Control.Top = item.Top + (int)Math.Round((1f - p) * S(26));
            }

            float bp = LoginFx.EaseOutCubic(LoginFx.Seg(t, 0.35f, 0.95f));
            int barW = (int)Math.Round(S(52) * bp);
            _accentBar.SetBounds(_barRight - barW, _barTop, barW, S(4));

            if (t >= 1.3f)
            {
                _introTimer.Stop();
                Opacity = 1;
            }
        }

        private async void DoLogin()
        {
            if (_busy) return;

            string username = _inUser.Value.Trim();
            string password = _inPass.Value.Trim();

            if (username.Length == 0 || password.Length == 0)
            {
                _inUser.SetError(username.Length == 0);
                _inPass.SetError(password.Length == 0);
                ShowError("من فضلك أدخل اسم المستخدم وكلمة المرور.");

                if (username.Length == 0)
                    _inUser.FocusInput();
                else
                    _inPass.FocusInput();

                Shake();
                return;
            }

            SetBusy(true);
            bool success;

            try
            {
                if (authService == null)
                    authService = new AuthService();
                success = await authService.LoginAsync(username, password);
            }
            catch (Exception ex)
            {
                if (IsDisposed) return;
                SetBusy(false);
                MessageBox.Show(this, "حدث خطأ في الاتصال بقاعدة البيانات:\n" + ex.Message, "خطأ",
                    MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1,
                    MessageBoxOptions.RightAlign | MessageBoxOptions.RtlReading);
                return;
            }

            if (IsDisposed) return;

            if (!success)
            {
                SetBusy(false);
                _inPass.Value = string.Empty;
                _inUser.SetError(true);
                _inPass.SetError(true);
                ShowError("بيانات الدخول غير صحيحة أو لا تملك صلاحية الدخول.");
                _inPass.FocusInput();
                Shake();
                return;
            }

            SaveRememberedUser(username);
            _btnLogin.ShowSuccess();

            await Task.Delay(650);

            if (IsDisposed) return;
            StartFadeOut(() =>
            {
                DialogResult = DialogResult.OK;
                Close();
            });
        }

        private void SetBusy(bool busy)
        {
            _busy = busy;
            _btnLogin.SetBusy(busy);
            _inUser.SetLocked(busy);
            _inPass.SetLocked(busy);
        }

        private void ShowError(string message)
        {
            _lblError.Text = message;
            _lblError.SetOpacity(0f);
            _lblError.FadeTo(1f);
        }

        private void ClearError()
        {
            if (_lblError == null) return;
            _inUser.SetError(false);
            _inPass.SetError(false);

            if (_lblError.Text.Length > 0)
                _lblError.FadeTo(0f);
        }

        private void UpdateCaps()
        {
            bool on = Control.IsKeyLocked(Keys.CapsLock) && _inPass.Inner.Focused;
            _lblCaps.FadeTo(on ? 1f : 0f);
        }

        private void DoExit()
        {
            if (_busy) return;
            _busy = true;
            StartFadeOut(() => Application.Exit());
        }

        private void Shake()
        {
            if (WindowState != FormWindowState.Normal) return;

            if (_shakeTimer.Enabled)
                Location = _shakeOrigin;
            else
                _shakeOrigin = Location;

            _shakeClock.Restart();
            _shakeTimer.Start();
        }

        private void ShakeTick(object sender, EventArgs e)
        {
            float t = (float)(_shakeClock.Elapsed.TotalSeconds / 0.5);
            if (t >= 1f)
            {
                _shakeTimer.Stop();
                Location = _shakeOrigin;
                return;
            }

            int dx = (int)Math.Round(Math.Sin(t * Math.PI * 8.0) * S(11) * (1f - t));
            Location = new Point(_shakeOrigin.X + dx, _shakeOrigin.Y);
        }

        private void StartFadeOut(Action done)
        {
            _introTimer.Stop();
            _fadeDone = done;
            _fadeClock.Restart();
            _fadeTimer.Start();
        }

        private void FadeTick(object sender, EventArgs e)
        {
            float t = (float)(_fadeClock.Elapsed.TotalSeconds / 0.28);
            if (t >= 1f)
            {
                _fadeTimer.Stop();
                Opacity = 0;
                Action done = _fadeDone;
                _fadeDone = null;
                if (done != null) done();
                return;
            }
            Opacity = 1f - t;
        }

        private void BeginDrag(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;
            ReleaseCapture();
            SendMessage(Handle, WM_NCLBUTTONDOWN, (IntPtr)HT_CAPTION, IntPtr.Zero);
        }

        private static string ConfigPath => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "MajesticDesktop", "login.cfg");

        private void LoadRememberedUser()
        {
            try
            {
                string path = ConfigPath;
                if (File.Exists(path))
                {
                    string encodedText = (File.ReadAllText(path) ?? string.Empty).Trim();
                    if (encodedText.Length > 0)
                    {
                        string name = Encoding.UTF8.GetString(Convert.FromBase64String(encodedText));
                        _inUser.Value = name;
                        _chkRemember.SetCheckedImmediate(true);
                    }
                }
            }
            catch { }
        }

        private void SaveRememberedUser(string username)
        {
            try
            {
                string path = ConfigPath;
                if (_chkRemember.Checked)
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(path));
                    string encodedName = Convert.ToBase64String(Encoding.UTF8.GetBytes(username));
                    File.WriteAllText(path, encodedName);
                }
                else if (File.Exists(path))
                {
                    File.Delete(path);
                }
            }
            catch { }
        }
    }

    // =====================================================================
    //                         أدوات مساعدة للرسم
    // =====================================================================
    internal static class LoginTheme
    {
        public const string FontName = "Segoe UI";

        public static readonly Color Window = Color.FromArgb(51, 65, 85);
        public static readonly Color PanelBg = Color.FromArgb(15, 23, 42);
        public static readonly Color BrandTop = Color.FromArgb(21, 34, 78);
        public static readonly Color BrandBottom = Color.FromArgb(7, 11, 26);
        public static readonly Color Field = Color.FromArgb(22, 32, 53);
        public static readonly Color FieldFocus = Color.FromArgb(27, 40, 66);
        public static readonly Color FieldBorder = Color.FromArgb(44, 58, 82);
        public static readonly Color Accent = Color.FromArgb(59, 130, 246);
        public static readonly Color Gold = Color.FromArgb(250, 204, 21);
        public static readonly Color GoldDark = Color.FromArgb(245, 158, 11);
        public static readonly Color Success = Color.FromArgb(34, 197, 94);
        public static readonly Color Danger = Color.FromArgb(239, 68, 68);
        public static readonly Color Warn = Color.FromArgb(251, 191, 36);
        public static readonly Color Text = Color.FromArgb(241, 245, 249);
        public static readonly Color Muted = Color.FromArgb(148, 163, 184);

        // Cached Brushes/Pens to reduce GC Pressure
        public static readonly SolidBrush TextBrush = new SolidBrush(Text);
        public static readonly SolidBrush MutedBrush = new SolidBrush(Muted);
        public static readonly SolidBrush BrandBottomBrush = new SolidBrush(BrandBottom);
        public static readonly Pen WhitePen = new Pen(Color.White, 1f);
        public static readonly SolidBrush WhiteBrush = new SolidBrush(Color.White);
    }

    internal static class LoginDpi
    {
        private static float _scale;

        public static float Scale
        {
            get
            {
                if (_scale <= 0f)
                {
                    try
                    {
                        using (Graphics g = Graphics.FromHwnd(IntPtr.Zero))
                            _scale = g.DpiX / 96f;
                    }
                    catch { _scale = 1f; }
                    if (_scale <= 0f) _scale = 1f;
                }
                return _scale;
            }
        }

        public static int S(int v) => (int)Math.Round(v * Scale);
        public static float S(float v) => v * Scale;
    }

    internal static class LoginTxt
    {
        private static StringFormat Make(StringAlignment horizontal)
        {
            return new StringFormat(StringFormatFlags.DirectionRightToLeft | StringFormatFlags.NoWrap)
            {
                Alignment = horizontal,
                LineAlignment = StringAlignment.Center,
                Trimming = StringTrimming.EllipsisCharacter
            };
        }

        public static readonly StringFormat Right = Make(StringAlignment.Near);
        public static readonly StringFormat Center = Make(StringAlignment.Center);
        public static readonly StringFormat Left = Make(StringAlignment.Far);

        public static StringFormat For(StringAlignment horizontal)
        {
            if (horizontal == StringAlignment.Far) return Left;
            if (horizontal == StringAlignment.Center) return Center;
            return Right;
        }
    }

    internal static class LoginFx
    {
        public static float Clamp01(float v) => v < 0f ? 0f : (v > 1f ? 1f : v);
        public static float Seg(float t, float start, float end) => end <= start ? (t >= end ? 1f : 0f) : Clamp01((t - start) / (end - start));
        public static float Lerp(float a, float b, float t) => a + (b - a) * t;
        public static Color Lerp(Color a, Color b, float t)
        {
            t = Clamp01(t);
            return Color.FromArgb(
                (int)Math.Round(a.A + (b.A - a.A) * t),
                (int)Math.Round(a.R + (b.R - a.R) * t),
                (int)Math.Round(a.G + (b.G - a.G) * t),
                (int)Math.Round(a.B + (b.B - a.B) * t));
        }
        public static Color WithAlpha(Color c, float alpha01) => Color.FromArgb((int)Math.Round(255f * Clamp01(alpha01)), c.R, c.G, c.B);
        public static float EaseOutCubic(float t) { t = Clamp01(t); float u = 1f - t; return 1f - u * u * u; }
        public static float EaseInOutSine(float t) { t = Clamp01(t); return 0.5f - 0.5f * (float)Math.Cos(Math.PI * t); }
        public static float EaseOutBack(float t)
        {
            t = Clamp01(t);
            const float c1 = 1.70158f;
            const float c3 = c1 + 1f;
            float u = t - 1f;
            return 1f + c3 * u * u * u + c1 * u * u;
        }

        public static GraphicsPath RoundRect(RectangleF r, float radius)
        {
            float d = Math.Min(radius * 2f, Math.Min(r.Width, r.Height));
            GraphicsPath p = new GraphicsPath();
            if (d <= 0.5f) { p.AddRectangle(r); return p; }
            p.AddArc(r.X, r.Y, d, d, 180, 90);
            p.AddArc(r.Right - d, r.Y, d, d, 270, 90);
            p.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
            p.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
            p.CloseFigure();
            return p;
        }

        private static float Dist(PointF a, PointF b) { float dx = a.X - b.X, dy = a.Y - b.Y; return (float)Math.Sqrt(dx * dx + dy * dy); }
        private static PointF Mix(PointF a, PointF b, float t) => new PointF(a.X + (b.X - a.X) * t, a.Y + (b.Y - a.Y) * t);

        public static void DrawCheck(Graphics g, float cx, float cy, float progress, float k, Color color)
        {
            if (progress < 0.02f) return;
            PointF p1 = new PointF(cx - 7f * k, cy + 0.5f * k);
            PointF p2 = new PointF(cx - 2f * k, cy + 5.5f * k);
            PointF p3 = new PointF(cx + 8f * k, cy - 5f * k);
            float l1 = Dist(p1, p2), l2 = Dist(p2, p3), d = Clamp01(progress) * (l1 + l2);

            using (Pen pen = new Pen(color, 2.8f * k) { StartCap = LineCap.Round, EndCap = LineCap.Round, LineJoin = LineJoin.Round })
            {
                if (d <= l1) g.DrawLine(pen, p1, Mix(p1, p2, d / l1));
                else { g.DrawLine(pen, p1, p2); g.DrawLine(pen, p2, Mix(p2, p3, (d - l1) / l2)); }
            }
        }
    }

    internal sealed class LoginBufferedPanel : Panel
    {
        public LoginBufferedPanel()
        {
            DoubleBuffered = true;
            SetStyle(ControlStyles.ResizeRedraw, true);
        }
    }

    internal abstract class LoginAnimatedControl : Control
    {
        private readonly Timer _timer;
        private readonly Stopwatch _clock;
        private double _last;

        protected LoginAnimatedControl()
        {
            SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);
            _clock = Stopwatch.StartNew();
            _timer = new Timer { Interval = 15 };
            _timer.Tick += OnTimerTick;
        }

        protected static int S(int v) => LoginDpi.S(v);
        protected static float S(float v) => LoginDpi.S(v);

        protected static float Step(float current, float target, float dt, float speed)
        {
            float d = speed * dt;
            if (current < target) return Math.Min(current + d, target);
            return Math.Max(current - d, target);
        }

        protected int FrameInterval { set { _timer.Interval = value; } }

        protected void StartAnim()
        {
            if (!_timer.Enabled)
            {
                _last = _clock.Elapsed.TotalSeconds;
                _timer.Start();
            }
        }

        protected abstract bool OnAnimate(float dt);

        private void OnTimerTick(object sender, EventArgs e)
        {
            double now = _clock.Elapsed.TotalSeconds;
            float dt = (float)(now - _last);
            _last = now;

            if (dt > 0.1f) dt = 0.1f;
            if (dt < 0f) dt = 0f;

            bool keep = OnAnimate(dt);
            Invalidate();
            if (!keep) _timer.Stop();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) { _timer.Stop(); _timer.Dispose(); }
            base.Dispose(disposing);
        }
    }

    internal sealed class LoginBrandingPanel : LoginAnimatedControl
    {
        private sealed class Particle { public float X, Y, VX, VY, R; }

        private readonly List<Particle> _particles = new List<Particle>();
        private readonly Random _rnd = new Random();
        private readonly string[] _taglines = { "إدارة المخزون بدقة واحترافية", "متابعة الفروع والخزن لحظة بلحظة", "مبيعات وتقارير في متناول يدك" };
        private readonly string _footer;
        private readonly Font _fLogo, _fTitle, _fSub, _fTag, _fFoot;
        private float _time, _introTime;
        private bool _started;

        public LoginBrandingPanel()
        {
            SetStyle(ControlStyles.Selectable, false);
            TabStop = false;
            BackColor = LoginTheme.BrandBottom;
            FrameInterval = 20;

            string version = string.Empty;
            try { version = Application.ProductVersion; } catch { }
            _footer = "الإصدار " + version;

            _fLogo = new Font(LoginTheme.FontName, 34f, FontStyle.Bold);
            _fTitle = new Font(LoginTheme.FontName, 25f, FontStyle.Bold);
            _fSub = new Font(LoginTheme.FontName, 11f, FontStyle.Regular);
            _fTag = new Font(LoginTheme.FontName, 11.5f, FontStyle.Bold);
            _fFoot = new Font(LoginTheme.FontName, 9f, FontStyle.Regular);
        }

        public void StartAnimation() { _started = true; _introTime = 0f; StartAnim(); }
        protected override void OnSizeChanged(EventArgs e) { base.OnSizeChanged(e); EnsureParticles(); }

        private void EnsureParticles()
        {
            if (_particles.Count > 0 || Width < 50 || Height < 50) return;
            float scale = LoginDpi.Scale;
            int count = Math.Max(18, Math.Min(46, (int)((Width * Height) / (5800f * scale * scale))));
            float speed = S(16f);

            for (int i = 0; i < count; i++)
            {
                _particles.Add(new Particle
                {
                    X = (float)_rnd.NextDouble() * Width,
                    Y = (float)_rnd.NextDouble() * Height,
                    VX = ((float)_rnd.NextDouble() - 0.5f) * 2f * speed,
                    VY = ((float)_rnd.NextDouble() - 0.5f) * 2f * speed,
                    R = S(1.2f) + (float)_rnd.NextDouble() * S(1.6f)
                });
            }
        }

        protected override bool OnAnimate(float dt)
        {
            _time += dt;
            if (_started) _introTime += dt;
            float m = S(12f);
            foreach (Particle p in _particles)
            {
                p.X += p.VX * dt; p.Y += p.VY * dt;
                if (p.X < -m) p.X = Width + m; else if (p.X > Width + m) p.X = -m;
                if (p.Y < -m) p.Y = Height + m; else if (p.Y > Height + m) p.Y = -m;
            }
            return true;
        }

        private static void DrawOrb(Graphics g, float cx, float cy, float radius, Color color, float alpha)
        {
            if (alpha <= 0.003f || radius < 2f) return;
            using (GraphicsPath gp = new GraphicsPath())
            {
                gp.AddEllipse(cx - radius, cy - radius, radius * 2f, radius * 2f);
                using (PathGradientBrush pgb = new PathGradientBrush(gp))
                {
                    pgb.CenterColor = LoginFx.WithAlpha(color, alpha);
                    pgb.SurroundColors = new Color[] { LoginFx.WithAlpha(color, 0f) };
                    g.FillPath(pgb, gp);
                }
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            if (Width < 4 || Height < 4) return;

            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;

            float it = _started ? LoginFx.Clamp01(_introTime / 1.4f) : 0f;
            Rectangle rc = new Rectangle(0, 0, Width, Height);

            using (LinearGradientBrush bg = new LinearGradientBrush(rc, LoginTheme.BrandTop, LoginTheme.BrandBottom, 62f))
                g.FillRectangle(bg, rc);

            DrawOrb(g, Width * (0.25f + 0.10f * (float)Math.Sin(_time * 0.35f)), Height * (0.25f + 0.06f * (float)Math.Cos(_time * 0.28f)), Width * 0.75f, LoginTheme.Accent, 0.30f);
            DrawOrb(g, Width * (0.78f + 0.08f * (float)Math.Cos(_time * 0.31f)), Height * (0.82f + 0.06f * (float)Math.Sin(_time * 0.40f)), Width * 0.80f, LoginTheme.Gold, 0.15f);

            DrawNetwork(g, it);
            DrawBrand(g, Width, Height, it);
            DrawTaglines(g, Width, Height, it);

            float footA = LoginFx.Seg(it, 0.7f, 1f) * 0.55f;
            using (SolidBrush fb = new SolidBrush(LoginFx.WithAlpha(LoginTheme.Muted, footA)))
                g.DrawString(_footer, _fFoot, fb, new RectangleF(0, Height - S(34f), Width, S(22f)), LoginTxt.Center);
        }

        private void DrawNetwork(Graphics g, float it)
        {
            if (it <= 0f || _particles.Count == 0) return;
            float link = S(115f), link2 = link * link;
            Pen pen = LoginTheme.WhitePen;
            SolidBrush br = LoginTheme.WhiteBrush;

            for (int i = 0; i < _particles.Count; i++)
            {
                Particle a = _particles[i];
                for (int j = i + 1; j < _particles.Count; j++)
                {
                    Particle b = _particles[j];
                    float dx = a.X - b.X, dy = a.Y - b.Y, d2 = dx * dx + dy * dy;
                    if (d2 >= link2) continue;
                    float d = (float)Math.Sqrt(d2);
                    using (Pen dynPen = new Pen(LoginFx.WithAlpha(Color.White, (1f - d / link) * 0.22f * it), 1f))
                    {
                        g.DrawLine(dynPen, a.X, a.Y, b.X, b.Y);
                    }
                }
                using (SolidBrush dynBrush = new SolidBrush(LoginFx.WithAlpha(Color.White, 0.40f * it)))
                {
                    g.FillEllipse(dynBrush, a.X - a.R, a.Y - a.R, a.R * 2f, a.R * 2f);
                }
            }
        }

        private void DrawBrand(Graphics g, int w, int h, float it)
        {
            float cx = w / 2f, cy = h * 0.27f, ls = S(92f);
            float logoP = LoginFx.Seg(it, 0f, 0.45f), titleP = LoginFx.EaseOutCubic(LoginFx.Seg(it, 0.25f, 0.70f)), subP = LoginFx.EaseOutCubic(LoginFx.Seg(it, 0.40f, 0.85f));

            if (logoP > 0.02f)
            {
                float pulse = 0.5f + 0.5f * (float)Math.Sin(_time * 2.1f);
                DrawOrb(g, cx, cy, ls * 1.35f, LoginTheme.Gold, (0.16f + 0.10f * pulse) * logoP);

                for (int k = 0; k < 2; k++)
                {
                    float rt = (_time * 0.45f + k * 0.5f) % 1f, rr = ls * 0.5f + rt * ls * 0.75f, ra = (1f - rt) * 0.30f * logoP;
                    using (Pen rp = new Pen(LoginFx.WithAlpha(LoginTheme.Gold, ra), 1.6f))
                        g.DrawEllipse(rp, cx - rr, cy - rr, rr * 2f, rr * 2f);
                }

                float sc = LoginFx.EaseOutBack(logoP);
                if (sc > 0.02f)
                {
                    GraphicsState state = g.Save();
                    g.TranslateTransform(cx, cy);
                    g.ScaleTransform(sc, sc);

                    RectangleF lr = new RectangleF(-ls / 2f, -ls / 2f, ls, ls);
                    using (GraphicsPath lp = LoginFx.RoundRect(lr, ls * 0.27f))
                    {
                        using (LinearGradientBrush lb = new LinearGradientBrush(lr, LoginTheme.Gold, LoginTheme.GoldDark, 45f))
                            g.FillPath(lb, lp);
                        using (Pen hp = new Pen(LoginFx.WithAlpha(Color.White, 0.40f), 1.4f))
                            g.DrawPath(hp, lp);
                    }
                    g.DrawString("M", _fLogo, LoginTheme.BrandBottomBrush, lr, LoginTxt.Center);
                    g.Restore(state);
                }
            }

            float titleY = cy + ls / 2f + S(46f);
            using (SolidBrush tb2 = new SolidBrush(LoginFx.WithAlpha(Color.White, titleP)))
                g.DrawString("نظام ماجستيك", _fTitle, tb2, new RectangleF(0, titleY - S(26f) + (1f - titleP) * S(10f), w, S(52f)), LoginTxt.Center);

            using (SolidBrush sb = new SolidBrush(LoginFx.WithAlpha(LoginTheme.Muted, subP)))
                g.DrawString("إدارة المخازن والفروع والمبيعات", _fSub, sb, new RectangleF(0, titleY + S(22f) + (1f - subP) * S(10f), w, S(28f)), LoginTxt.Center);
        }

        private void DrawTaglines(Graphics g, int w, int h, float it)
        {
            float appear = LoginFx.Seg(it, 0.6f, 1f);
            if (appear <= 0f) return;

            const float period = 3.6f;
            int idx = ((int)(_time / period)) % _taglines.Length;
            float phase = (_time % period) / period;
            float fade = LoginFx.Clamp01(Math.Min(phase / 0.12f, (1f - phase) / 0.12f));
            float rise = LoginFx.EaseOutCubic(Math.Min(phase / 0.2f, 1f));
            float ty = h * 0.76f;

            using (SolidBrush tb = new SolidBrush(LoginFx.WithAlpha(Color.White, fade * appear)))
                g.DrawString(_taglines[idx], _fTag, tb, new RectangleF(S(16f), ty - S(16f) + (1f - rise) * S(8f), w - S(32f), S(32f)), LoginTxt.Center);

            float dotH = S(7f), gap = S(7f), total = gap * (_taglines.Length - 1);
            float[] widths = new float[_taglines.Length];

            for (int i = 0; i < _taglines.Length; i++)
            {
                float act = (i == idx) ? fade : 0f;
                widths[i] = LoginFx.Lerp(S(7f), S(24f), act);
                total += widths[i];
            }

            float x = (w - total) / 2f, dy = ty + S(26f);
            for (int i = 0; i < _taglines.Length; i++)
            {
                float act = (i == idx) ? fade : 0f;
                Color c = LoginFx.Lerp(LoginFx.WithAlpha(Color.White, 0.25f), LoginTheme.Gold, act);
                c = LoginFx.WithAlpha(c, (c.A / 255f) * appear);

                RectangleF dr = new RectangleF(x, dy, widths[i], dotH);
                using (GraphicsPath dp = LoginFx.RoundRect(dr, dotH / 2f))
                using (SolidBrush db = new SolidBrush(c))
                    g.FillPath(db, dp);
                x += widths[i] + gap;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) { _fLogo.Dispose(); _fTitle.Dispose(); _fSub.Dispose(); _fTag.Dispose(); _fFoot.Dispose(); }
            base.Dispose(disposing);
        }
    }

    internal sealed class LoginLabel : LoginAnimatedControl
    {
        private readonly Font _font;
        private float _alpha = 1f, _target = 1f;
        private StringAlignment _align = StringAlignment.Near;

        public LoginLabel(float sizePt, FontStyle style)
        {
            SetStyle(ControlStyles.Selectable, false);
            TabStop = false;
            BackColor = LoginTheme.PanelBg;
            ForeColor = LoginTheme.Text;
            _font = new Font(LoginTheme.FontName, sizePt, style);
        }

        public StringAlignment HAlign { get { return _align; } set { _align = value; Invalidate(); } }
        public void SetOpacity(float a) { _alpha = LoginFx.Clamp01(a); _target = _alpha; Invalidate(); }
        public void FadeTo(float a) { _target = LoginFx.Clamp01(a); StartAnim(); }
        protected override bool OnAnimate(float dt) { _alpha = Step(_alpha, _target, dt, 5f); return _alpha != _target; }
        protected override void OnTextChanged(EventArgs e) { base.OnTextChanged(e); Invalidate(); }
        protected override void OnForeColorChanged(EventArgs e) { base.OnForeColorChanged(e); Invalidate(); }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (_alpha <= 0.003f || string.IsNullOrEmpty(Text)) return;
            e.Graphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
            using (SolidBrush b = new SolidBrush(LoginFx.WithAlpha(ForeColor, _alpha)))
                e.Graphics.DrawString(Text, _font, b, new RectangleF(0, 0, Width, Height), LoginTxt.For(_align));
        }

        protected override void Dispose(bool disposing) { if (disposing) _font.Dispose(); base.Dispose(disposing); }
    }

    internal enum LoginIcon { User, Lock }

    internal sealed class LoginInputBox : LoginAnimatedControl
    {
        private readonly TextBox _tb;
        private readonly Font _tbFont, _labelFont;
        private readonly string _label;
        private readonly LoginIcon _icon;
        private readonly bool _password;
        private bool _focused, _error, _revealed, _eyeHover;
        private float _focusT, _errorT, _eyeT;

        public event EventHandler InnerTextChanged, InnerGotFocus, InnerLostFocus;
        public event KeyEventHandler InnerKeyDown, InnerKeyUp;

        public static int PreferredHeight => LoginDpi.S(22) + LoginDpi.S(4) + LoginDpi.S(50) + LoginDpi.S(8);

        public LoginInputBox(string label, LoginIcon icon, bool password)
        {
            _label = label; _icon = icon; _password = password;
            SetStyle(ControlStyles.Selectable, false); TabStop = false; BackColor = LoginTheme.PanelBg;

            _labelFont = new Font(LoginTheme.FontName, 10f, FontStyle.Bold);
            _tbFont = new Font(LoginTheme.FontName, 13f, FontStyle.Regular);

            _tb = new TextBox { BorderStyle = BorderStyle.None, Font = _tbFont, BackColor = LoginTheme.Field, ForeColor = LoginTheme.Text, RightToLeft = RightToLeft.Yes, UseSystemPasswordChar = password, TabStop = true };
            _tb.GotFocus += (s, e) => { _focused = true; StartAnim(); InnerGotFocus?.Invoke(this, e); };
            _tb.LostFocus += (s, e) => { _focused = false; StartAnim(); InnerLostFocus?.Invoke(this, e); };
            _tb.TextChanged += (s, e) => InnerTextChanged?.Invoke(this, e);
            _tb.KeyDown += (s, e) => InnerKeyDown?.Invoke(this, e);
            _tb.KeyUp += (s, e) => InnerKeyUp?.Invoke(this, e);

            Controls.Add(_tb);
            Height = PreferredHeight;
        }

        public TextBox Inner => _tb;
        public string Value { get => _tb.Text; set => _tb.Text = value ?? string.Empty; }
        public void FocusInput() => _tb.Focus();
        public void SetLocked(bool locked) => _tb.ReadOnly = locked;
        public void SetError(bool on) { if (_error == on) return; _error = on; StartAnim(); }

        private RectangleF FieldRect { get { float x = S(6), y = S(22) + S(4); return new RectangleF(x, y, Math.Max(10, Width - 2 * x), S(50)); } }
        private RectangleF EyeRect { get { RectangleF f = FieldRect; return new RectangleF(f.Left + S(4), f.Top, S(44), f.Height); } }

        private void LayoutInner()
        {
            if (_tb == null) return;
            RectangleF f = FieldRect;
            int leftPad = _password ? S(52) : S(18), rightPad = S(56), tbW = Math.Max(20, (int)f.Width - leftPad - rightPad), tbH = _tb.PreferredHeight;
            _tb.SetBounds((int)f.Left + leftPad, (int)Math.Round(f.Top + (f.Height - tbH) / 2f), tbW, tbH);
        }

        protected override void OnSizeChanged(EventArgs e) { base.OnSizeChanged(e); LayoutInner(); }

        protected override bool OnAnimate(float dt)
        {
            float focusTarget = _focused ? 1f : 0f, errorTarget = _error ? 1f : 0f, eyeTarget = _eyeHover ? 1f : 0f;
            _focusT = Step(_focusT, focusTarget, dt, 6.5f); _errorT = Step(_errorT, errorTarget, dt, 7f); _eyeT = Step(_eyeT, eyeTarget, dt, 9f);
            Color fill = LoginFx.Lerp(LoginTheme.Field, LoginTheme.FieldFocus, LoginFx.EaseOutCubic(_focusT));
            if (_tb.BackColor != fill) _tb.BackColor = fill;
            return _focusT != focusTarget || _errorT != errorTarget || _eyeT != eyeTarget;
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (!_password) return;
            bool over = EyeRect.Contains(e.Location);
            if (over != _eyeHover) { _eyeHover = over; Cursor = over ? Cursors.Hand : Cursors.Default; StartAnim(); }
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            if (_eyeHover) { _eyeHover = false; Cursor = Cursors.Default; StartAnim(); }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Button != MouseButtons.Left) return;
            if (_password && EyeRect.Contains(e.Location)) { _revealed = !_revealed; _tb.UseSystemPasswordChar = !_revealed; Invalidate(); }
            _tb.Focus(); _tb.SelectionStart = _tb.TextLength;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics; g.SmoothingMode = SmoothingMode.AntiAlias; g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
            float f = LoginFx.EaseOutCubic(_focusT), er = _errorT; RectangleF field = FieldRect;
            Color accentNow = LoginFx.Lerp(LoginTheme.Accent, LoginTheme.Danger, er);
            Color labelColor = LoginFx.Lerp(LoginTheme.Muted, accentNow, Math.Max(f, er));

            using (SolidBrush lb = new SolidBrush(labelColor))
                g.DrawString(_label, _labelFont, lb, new RectangleF(field.Left, 0, field.Width, S(22)), LoginTxt.Right);

            float glow = Math.Max(f, er);
            if (glow > 0.01f)
            {
                for (int i = 3; i >= 1; i--)
                {
                    using (GraphicsPath gp = LoginFx.RoundRect(RectangleF.Inflate(field, i * 1.6f, i * 1.6f), S(10) + i * 1.6f))
                    using (Pen gpn = new Pen(LoginFx.WithAlpha(accentNow, glow * (0.20f / i)), 1.6f))
                        g.DrawPath(gpn, gp);
                }
            }

            Color fill = LoginFx.Lerp(LoginTheme.Field, LoginTheme.FieldFocus, f);
            Color border = LoginFx.Lerp(LoginFx.Lerp(LoginTheme.FieldBorder, LoginTheme.Accent, f), LoginTheme.Danger, er);

            using (GraphicsPath path = LoginFx.RoundRect(field, S(10)))
            {
                using (SolidBrush fb = new SolidBrush(fill)) g.FillPath(fb, path);
                using (Pen bp = new Pen(border, 1.6f)) g.DrawPath(bp, path);
            }

            float cy = field.Top + field.Height / 2f;
            Color iconColor = LoginFx.Lerp(LoginTheme.Muted, accentNow, Math.Max(f, er));
            DrawIcon(g, field.Right - S(28), cy, iconColor);

            if (_password)
            {
                Color eyeColor = LoginFx.Lerp(LoginTheme.Muted, LoginTheme.Text, LoginFx.EaseOutCubic(_eyeT));
                DrawEye(g, field.Left + S(28), cy, eyeColor);
            }
        }

        private void DrawIcon(Graphics g, float cx, float cy, Color color)
        {
            using (Pen p = new Pen(color, 1.9f) { StartCap = LineCap.Round, EndCap = LineCap.Round })
            {
                if (_icon == LoginIcon.User)
                {
                    g.DrawEllipse(p, cx - S(5f), cy - S(10f), S(10f), S(10f));
                    g.DrawArc(p, cx - S(9f), cy + S(2f), S(18f), S(16f), 180, 180);
                }
                else
                {
                    float bw = S(16f), bh = S(11f); RectangleF body = new RectangleF(cx - bw / 2f, cy - bh / 2f + S(3f), bw, bh);
                    using (GraphicsPath bp = LoginFx.RoundRect(body, S(3f))) g.DrawPath(p, bp);
                    g.DrawArc(p, cx - S(5f), body.Top - S(7f), S(10f), S(14f), 180, 180);
                    using (SolidBrush kb = new SolidBrush(color)) g.FillEllipse(kb, cx - S(1.6f), body.Top + bh / 2f - S(1.6f), S(3.2f), S(3.2f));
                }
            }
        }

        private void DrawEye(Graphics g, float cx, float cy, Color color)
        {
            float w = S(9f), h = S(6.5f);
            using (Pen p = new Pen(color, 1.8f) { StartCap = LineCap.Round, EndCap = LineCap.Round })
            {
                using (GraphicsPath eye = new GraphicsPath())
                {
                    eye.AddBezier(cx - w, cy, cx - w * 0.5f, cy - h * 1.7f, cx + w * 0.5f, cy - h * 1.7f, cx + w, cy);
                    eye.AddBezier(cx + w, cy, cx + w * 0.5f, cy + h * 1.7f, cx - w * 0.5f, cy + h * 1.7f, cx - w, cy);
                    g.DrawPath(p, eye);
                }
                g.DrawEllipse(p, cx - S(2.6f), cy - S(2.6f), S(5.2f), S(5.2f));
                if (_revealed) g.DrawLine(p, cx - w, cy + h * 1.3f, cx + w, cy - h * 1.3f);
            }
        }

        protected override void Dispose(bool disposing) { if (disposing) { _labelFont.Dispose(); _tbFont.Dispose(); } base.Dispose(disposing); }
    }

    internal enum LoginButtonStyle { Primary, Ghost }

    internal sealed class LoginButton : LoginAnimatedControl
    {
        private readonly LoginButtonStyle _style;
        private readonly Color _accent;
        private readonly Font _font;
        private bool _hover, _down, _busy, _success;
        private float _hoverT, _downT, _busyT, _successT, _shine = 1f, _spin;

        public string BusyText { get; set; } = "جاري التحقق...";
        public string SuccessText { get; set; } = "تم الدخول بنجاح";

        public LoginButton(LoginButtonStyle style, Color accent)
        {
            _style = style; _accent = accent;
            BackColor = LoginTheme.PanelBg; Cursor = Cursors.Hand; TabStop = true; SetStyle(ControlStyles.Selectable, true);
            _font = new Font(LoginTheme.FontName, style == LoginButtonStyle.Primary ? 13.5f : 11.5f, FontStyle.Bold);
        }

        public void SetBusy(bool busy) { _busy = busy; if (!busy) _success = false; StartAnim(); }
        public void ShowSuccess() { _success = true; StartAnim(); }

        private RectangleF BtnRect
        {
            get
            {
                float bottom = _style == LoginButtonStyle.Primary ? S(14) : S(6);
                return new RectangleF(S(10), S(4), Math.Max(10, Width - S(20)), Math.Max(10, Height - S(4) - bottom));
            }
        }

        protected override bool OnAnimate(float dt)
        {
            float hoverTarget = _hover ? 1f : 0f, downTarget = _down ? 1f : 0f, busyTarget = (_busy && !_success) ? 1f : 0f, successTarget = _success ? 1f : 0f;
            _hoverT = Step(_hoverT, hoverTarget, dt, 8f); _downT = Step(_downT, downTarget, dt, 14f); _busyT = Step(_busyT, busyTarget, dt, 6f); _successT = Step(_successT, successTarget, dt, 3.2f);
            if (_shine < 1f) _shine = Math.Min(1f, _shine + dt / 0.7f);
            if (_busy && !_success) _spin += dt;
            return _hoverT != hoverTarget || _downT != downTarget || _busyT != busyTarget || _successT != successTarget || _shine < 1f || (_busy && !_success);
        }

        protected override void OnMouseEnter(EventArgs e) { base.OnMouseEnter(e); _hover = true; if (_style == LoginButtonStyle.Primary && !_busy) _shine = 0f; StartAnim(); }
        protected override void OnMouseLeave(EventArgs e) { base.OnMouseLeave(e); _hover = false; _down = false; StartAnim(); }
        protected override void OnMouseDown(MouseEventArgs e) { base.OnMouseDown(e); if (e.Button == MouseButtons.Left) { _down = true; Focus(); StartAnim(); } }
        protected override void OnMouseUp(MouseEventArgs e) { base.OnMouseUp(e); _down = false; StartAnim(); }
        protected override void OnKeyDown(KeyEventArgs e) { base.OnKeyDown(e); if (e.KeyCode == Keys.Space) { _down = true; StartAnim(); } }
        protected override void OnKeyUp(KeyEventArgs e) { base.OnKeyUp(e); if (e.KeyCode == Keys.Space) { _down = false; StartAnim(); OnClick(EventArgs.Empty); } }
        protected override void OnClick(EventArgs e) { if (!_busy) base.OnClick(e); }
        protected override void OnTextChanged(EventArgs e) { base.OnTextChanged(e); Invalidate(); }
        protected override void OnGotFocus(EventArgs e) { base.OnGotFocus(e); Invalidate(); }
        protected override void OnLostFocus(EventArgs e) { base.OnLostFocus(e); Invalidate(); }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics; g.SmoothingMode = SmoothingMode.AntiAlias; g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
            RectangleF r = BtnRect; if (r.Width < 4f || r.Height < 4f) return;
            float h = LoginFx.EaseOutCubic(_hoverT), p = _downT, s = LoginFx.EaseOutCubic(_successT), radius = S(12);
            r = new RectangleF(r.X + p * 1.2f, r.Y + p * 1.5f, r.Width - p * 2.4f, r.Height - p * 3f);
            Color textColor;

            if (_style == LoginButtonStyle.Primary)
            {
                Color baseColor = LoginFx.Lerp(_accent, LoginTheme.Success, s);
                Color top = LoginFx.Lerp(baseColor, Color.White, 0.10f + 0.10f * h), bottom = LoginFx.Lerp(baseColor, Color.Black, 0.22f - 0.08f * h);
                float gl = 0.18f + 0.22f * h;

                for (int i = 4; i >= 1; i--)
                {
                    RectangleF gr = RectangleF.Inflate(r, i * 2f, i * 1.2f); gr.Offset(0f, S(4));
                    using (GraphicsPath gp = LoginFx.RoundRect(gr, radius + i * 1.5f))
                    using (SolidBrush gb = new SolidBrush(LoginFx.WithAlpha(baseColor, gl * (0.14f / i))))
                        g.FillPath(gb, gp);
                }

                using (GraphicsPath path = LoginFx.RoundRect(r, radius))
                {
                    using (LinearGradientBrush fb = new LinearGradientBrush(r, top, bottom, 90f)) g.FillPath(fb, path);
                    if (_shine < 1f)
                    {
                        float sx = LoginFx.Lerp(r.Left - r.Width * 0.35f, r.Right + r.Width * 0.1f, LoginFx.EaseInOutSine(_shine));
                        float bandW = r.Width * 0.18f, skew = r.Height * 0.5f;
                        PointF[] band = { new PointF(sx, r.Bottom), new PointF(sx + bandW, r.Bottom), new PointF(sx + bandW + skew, r.Top), new PointF(sx + skew, r.Top) };
                        GraphicsState state = g.Save();
                        g.SetClip(path, CombineMode.Intersect);
                        using (SolidBrush sb = new SolidBrush(LoginFx.WithAlpha(Color.White, 0.20f))) g.FillPolygon(sb, band);
                        g.Restore(state);
                    }
                    using (Pen hp = new Pen(LoginFx.WithAlpha(Color.White, 0.22f), 1f)) g.DrawPath(hp, path);
                }
                textColor = Color.White;
            }
            else
            {
                using (GraphicsPath path = LoginFx.RoundRect(r, radius))
                {
                    using (SolidBrush fb = new SolidBrush(LoginFx.WithAlpha(_accent, 0.14f * h))) g.FillPath(fb, path);
                    using (Pen bp = new Pen(LoginFx.WithAlpha(_accent, 0.55f + 0.45f * h), 1.6f)) g.DrawPath(bp, path);
                }
                textColor = LoginFx.Lerp(_accent, Color.White, h);
            }

            if (Focused)
            {
                using (GraphicsPath fp = LoginFx.RoundRect(RectangleF.Inflate(r, 2.5f, 2.5f), radius + 2f))
                using (Pen fpn = new Pen(LoginFx.WithAlpha(Color.White, 0.35f), 1.2f)) g.DrawPath(fpn, fp);
            }

            string label = _success ? SuccessText : (_busy ? BusyText : Text);
            float iconA = LoginFx.EaseOutCubic(Math.Max(_busyT, s));
            SizeF ts = g.MeasureString(label, _font, 10000, LoginTxt.Center);
            float tw = ts.Width + S(6f), gap = S(10f) * iconA, iconSize = S(20f) * iconA;
            float startX = r.Left + (r.Width - (tw + gap + iconSize)) / 2f;

            using (SolidBrush tb = new SolidBrush(textColor))
                g.DrawString(label, _font, tb, new RectangleF(startX, r.Top, tw, r.Height), LoginTxt.Center);

            if (iconA > 0.02f)
            {
                float icx = startX + tw + gap + iconSize / 2f, icy = r.Top + r.Height / 2f;
                if (_success) { LoginFx.DrawCheck(g, icx, icy, s, LoginDpi.Scale, Color.White); }
                else
                {
                    float rad = S(8.5f) * iconA; RectangleF sr = new RectangleF(icx - rad, icy - rad, rad * 2f, rad * 2f);
                    using (Pen track = new Pen(LoginFx.WithAlpha(Color.White, 0.25f), S(2.4f))) g.DrawEllipse(track, sr);
                    float start = _spin * 320f, sweep = 110f + 140f * (0.5f + 0.5f * (float)Math.Sin(_spin * 4.2f));
                    using (Pen sp = new Pen(Color.White, S(2.4f)) { StartCap = LineCap.Round, EndCap = LineCap.Round }) g.DrawArc(sp, sr, start, sweep);
                }
            }
        }

        protected override void Dispose(bool disposing) { if (disposing) _font.Dispose(); base.Dispose(disposing); }
    }

    internal enum LoginWindowKind { Close, Minimize }

    internal sealed class LoginWindowButton : LoginAnimatedControl
    {
        private readonly LoginWindowKind _kind;
        private bool _hover;
        private float _hoverT;

        public LoginWindowButton(LoginWindowKind kind)
        {
            _kind = kind; SetStyle(ControlStyles.SupportsTransparentBackColor, true); SetStyle(ControlStyles.Selectable, false);
            TabStop = false; BackColor = Color.Transparent; Cursor = Cursors.Hand; Size = new Size(S(34), S(28));
        }

        protected override bool OnAnimate(float dt) { float target = _hover ? 1f : 0f; _hoverT = Step(_hoverT, target, dt, 9f); return _hoverT != target; }
        protected override void OnMouseEnter(EventArgs e) { base.OnMouseEnter(e); _hover = true; StartAnim(); }
        protected override void OnMouseLeave(EventArgs e) { base.OnMouseLeave(e); _hover = false; StartAnim(); }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics; g.SmoothingMode = SmoothingMode.AntiAlias;
            float h = LoginFx.EaseOutCubic(_hoverT); RectangleF r = new RectangleF(0.5f, 0.5f, Width - 1f, Height - 1f);
            Color hoverColor = _kind == LoginWindowKind.Close ? LoginTheme.Danger : Color.White;
            float a = _kind == LoginWindowKind.Close ? 0.95f * h : 0.14f * h;

            using (GraphicsPath path = LoginFx.RoundRect(r, S(8)))
            using (SolidBrush b = new SolidBrush(LoginFx.WithAlpha(hoverColor, a))) g.FillPath(b, path);

            Color glyph = LoginFx.Lerp(LoginFx.WithAlpha(Color.White, 0.65f), Color.White, h);
            float cx = Width / 2f, cy = Height / 2f;
            using (Pen p = new Pen(glyph, 1.6f) { StartCap = LineCap.Round, EndCap = LineCap.Round })
            {
                if (_kind == LoginWindowKind.Close) { float d = S(4.5f); g.DrawLine(p, cx - d, cy - d, cx + d, cy + d); g.DrawLine(p, cx - d, cy + d, cx + d, cy - d); }
                else g.DrawLine(p, cx - S(5f), cy + S(2f), cx + S(5f), cy + S(2f));
            }
        }
    }

    internal sealed class LoginCheck : LoginAnimatedControl
    {
        private readonly Font _font;
        private bool _checked, _hover;
        private float _checkT, _hoverT;

        public LoginCheck(string text)
        {
            Text = text; BackColor = LoginTheme.PanelBg; Cursor = Cursors.Hand; TabStop = true;
            SetStyle(ControlStyles.Selectable, true); _font = new Font(LoginTheme.FontName, 10f, FontStyle.Regular);
        }

        public bool Checked { get => _checked; set { _checked = value; StartAnim(); } }
        public void SetCheckedImmediate(bool value) { _checked = value; _checkT = value ? 1f : 0f; Invalidate(); }

        protected override bool OnAnimate(float dt)
        {
            float checkTarget = _checked ? 1f : 0f, hoverTarget = _hover ? 1f : 0f;
            _checkT = Step(_checkT, checkTarget, dt, 6f); _hoverT = Step(_hoverT, hoverTarget, dt, 8f);
            return _checkT != checkTarget || _hoverT != hoverTarget;
        }

        protected override void OnMouseEnter(EventArgs e) { base.OnMouseEnter(e); _hover = true; StartAnim(); }
        protected override void OnMouseLeave(EventArgs e) { base.OnMouseLeave(e); _hover = false; StartAnim(); }
        protected override void OnClick(EventArgs e) { _checked = !_checked; StartAnim(); base.OnClick(e); }
        protected override void OnKeyUp(KeyEventArgs e) { base.OnKeyUp(e); if (e.KeyCode == Keys.Space) OnClick(EventArgs.Empty); }
        protected override void OnGotFocus(EventArgs e) { base.OnGotFocus(e); Invalidate(); }
        protected override void OnLostFocus(EventArgs e) { base.OnLostFocus(e); Invalidate(); }
        protected override void OnTextChanged(EventArgs e) { base.OnTextChanged(e); Invalidate(); }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics; g.SmoothingMode = SmoothingMode.AntiAlias; g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
            float c = LoginFx.EaseOutCubic(_checkT), hv = LoginFx.EaseOutCubic(_hoverT), size = S(20f);
            RectangleF box = new RectangleF(Width - S(8) - size, (Height - size) / 2f, size, size);
            Color fill = LoginFx.Lerp(LoginTheme.Field, LoginTheme.Accent, c), border = LoginFx.Lerp(LoginFx.Lerp(LoginTheme.FieldBorder, LoginTheme.Muted, hv), LoginTheme.Accent, c);

            using (GraphicsPath path = LoginFx.RoundRect(box, S(6f)))
            {
                using (SolidBrush fb = new SolidBrush(fill)) g.FillPath(fb, path);
                using (Pen bp = new Pen(border, 1.6f)) g.DrawPath(bp, path);
            }

            if (Focused)
            {
                using (GraphicsPath fp = LoginFx.RoundRect(RectangleF.Inflate(box, 3f, 3f), S(8f)))
                using (Pen fpn = new Pen(LoginFx.WithAlpha(LoginTheme.Accent, 0.55f), 1.2f)) g.DrawPath(fpn, fp);
            }

            LoginFx.DrawCheck(g, box.X + box.Width / 2f, box.Y + box.Height / 2f + 0.5f, c, LoginDpi.Scale * 0.9f, Color.White);

            Color textColor = LoginFx.Lerp(LoginTheme.Muted, LoginTheme.Text, Math.Max(hv, c));
            using (SolidBrush tb = new SolidBrush(textColor))
                g.DrawString(Text, _font, tb, new RectangleF(0, 0, box.Left - S(8f), Height), LoginTxt.Right);
        }

        protected override void Dispose(bool disposing) { if (disposing) _font.Dispose(); base.Dispose(disposing); }
    }
}