using System.Drawing;
using System.Drawing.Drawing2D;

namespace PainelChamados
{
    public static class RoundedRectangle
    {
        // Método para criar um caminho gráfico de um retângulo arredondado
        private static GraphicsPath CreateRoundedRectPath(Rectangle bounds, int cornerRadius)
        {
            GraphicsPath path = new GraphicsPath();

            // Ajusta o raio do canto se for muito grande
            if (cornerRadius > bounds.Width / 2) cornerRadius = bounds.Width / 2;
            if (cornerRadius > bounds.Height / 2) cornerRadius = bounds.Height / 2;

            // Arcos dos cantos
            path.AddArc(bounds.X, bounds.Y, cornerRadius * 2, cornerRadius * 2, 180, 90);
            path.AddArc(bounds.Right - (cornerRadius * 2), bounds.Y, cornerRadius * 2, cornerRadius * 2, 270, 90);
            path.AddArc(bounds.Right - (cornerRadius * 2), bounds.Bottom - (cornerRadius * 2), cornerRadius * 2, cornerRadius * 2, 0, 90);
            path.AddArc(bounds.X, bounds.Bottom - (cornerRadius * 2), cornerRadius * 2, cornerRadius * 2, 90, 90);

            path.CloseFigure();
            return path;
        }

        // Método para preencher o retângulo arredondado
        public static void FillRoundedRectangle(Graphics g, Rectangle bounds, int cornerRadius, Brush b)
        {
            if (g == null || b == null) return;

            using (GraphicsPath path = CreateRoundedRectPath(bounds, cornerRadius))
            {
                g.FillPath(b, path);
            }
        }
    }
}