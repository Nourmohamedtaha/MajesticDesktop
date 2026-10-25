using System.Drawing;
using System.Windows.Forms;

namespace MajesticDesktop.Forms
{
    // الهدف الوحيد من الرندرر ده: خلي خلفية القائمة عند الـHover/الضغط غامقة (نفس عيلة لون القائمة)
    public sealed class MajesticMenuRenderer : ToolStripProfessionalRenderer
    {
        public MajesticMenuRenderer() : base(new MajesticMenuColorTable()) { }
    }

    public sealed class MajesticMenuColorTable : ProfessionalColorTable
    {
        private static readonly Color BarBack = Color.FromArgb(18, 45, 78);
        private static readonly Color HoverBack = Color.FromArgb(32, 78, 128);
        private static readonly Color PressedBack = Color.FromArgb(42, 96, 150);
        private static readonly Color BorderColor = Color.FromArgb(45, 90, 135);

        public override Color MenuStripGradientBegin => BarBack;
        public override Color MenuStripGradientEnd => BarBack;

        public override Color ImageMarginGradientBegin => BarBack;
        public override Color ImageMarginGradientMiddle => BarBack;
        public override Color ImageMarginGradientEnd => BarBack;

        public override Color ToolStripDropDownBackground => BarBack;
        public override Color MenuBorder => BorderColor;
        public override Color MenuItemBorder => HoverBack;

        public override Color MenuItemSelected => HoverBack;
        public override Color MenuItemSelectedGradientBegin => HoverBack;
        public override Color MenuItemSelectedGradientEnd => HoverBack;

        public override Color MenuItemPressedGradientBegin => PressedBack;
        public override Color MenuItemPressedGradientMiddle => PressedBack;
        public override Color MenuItemPressedGradientEnd => PressedBack;

        public override Color SeparatorDark => BorderColor;
        public override Color SeparatorLight => BorderColor;
    }
}