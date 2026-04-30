using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace SecurIT_Memory.Utils
{
    /// <summary>
    /// Générateur d'icônes cybersécurité dessinées en code (pas besoin de fichiers images).
    /// Utilise System.Drawing.Graphics pour créer des visuels vectoriels.
    /// </summary>
    public static class IconeGenerateur
    {
        // Palette de couleurs cybersécurité
        private static readonly Color[] Couleurs = new[]
        {
            Color.FromArgb(0xFF, 0x4D, 0x6A),   // Rouge virus
            Color.FromArgb(0x00, 0xD4, 0xFF),   // Cyan pare-feu
            Color.FromArgb(0x00, 0xFF, 0xB3),   // Vert cadenas
            Color.FromArgb(0xFF, 0xC8, 0x00),   // Jaune mot de passe
            Color.FromArgb(0x7C, 0x4D, 0xFF),   // Violet bouclier
            Color.FromArgb(0xFF, 0x6B, 0x35),   // Orange hacker
            Color.FromArgb(0x00, 0xE5, 0xFF),   // Bleu VPN
            Color.FromArgb(0xFF, 0x41, 0xB3),   // Rose chiffrement
        };

        /// <summary>
        /// Génère un Bitmap 120×120 représentant l'icône cybersécurité demandée.
        /// </summary>
        public static Bitmap GenererIcone(string nomIcone, int taille = 120)
        {
            return nomIcone switch
            {
                "Virus"            => DessinerVirus(taille),
                "Pare-feu"         => DessinerPareFeu(taille),
                "Cadenas"          => DessinerCadenas(taille),
                "Mot de passe"     => DessinerMotDePasse(taille),
                "Bouclier"         => DessinerBouclier(taille),
                "Hacker"           => DessinerHacker(taille),
                "VPN"              => DessinerVPN(taille),
                "Chiffrement"      => DessinerChiffrement(taille),
                // ── Icônes supplémentaires pour la grille 6×6 ──
                "Phishing"         => DessinerPhishing(taille),
                "Malware"          => DessinerMalware(taille),
                "DDoS"             => DessinerDDoS(taille),
                "Ransomware"       => DessinerRansomware(taille),
                "Antivirus"        => DessinerAntivirus(taille),
                "Certificat"       => DessinerCertificat(taille),
                "Reseau"           => DessinerReseau(taille),
                "Cloud"            => DessinerCloud(taille),
                "Base de donnees"  => DessinerBaseDeDonnees(taille),
                "Authentification" => DessinerAuthentification(taille),
                _                  => DessinerInconnu(taille)
            };
        }

        /// <summary>Crée un Bitmap avec fond dégradé sombre.</summary>
        private static (Bitmap bmp, Graphics g) CreerCanvas(int taille, int idIcone)
        {
            var bmp = new Bitmap(taille, taille);
            var g = Graphics.FromImage(bmp);
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;

            // Fond dégradé sombre cyberpunk
            using var fondBrush = new LinearGradientBrush(
                new Rectangle(0, 0, taille, taille),
                Color.FromArgb(255, 10, 15, 30),
                Color.FromArgb(255, 20, 30, 60),
                LinearGradientMode.ForwardDiagonal);
            g.FillRectangle(fondBrush, 0, 0, taille, taille);

            // Bordure arrondie colorée
            var couleur = Couleurs[idIcone % Couleurs.Length];
            using var penBordure = new Pen(Color.FromArgb(150, couleur), 3f);
            g.DrawRoundedRectangle(penBordure, 3, 3, taille - 7, taille - 7, 16);

            return (bmp, g);
        }

        // ── Icône Virus ──
        private static Bitmap DessinerVirus(int t)
        {
            var (bmp, g) = CreerCanvas(t, 0);
            float cx = t / 2f, cy = t / 2f, r = t * 0.22f;
            var rouge = Color.FromArgb(0xFF, 0x4D, 0x6A);

            // Corps du virus (cercle)
            using var brush = new SolidBrush(Color.FromArgb(200, rouge));
            g.FillEllipse(brush, cx - r, cy - r, r * 2, r * 2);

            // Pointes du virus
            using var penPointe = new Pen(rouge, 3f) { EndCap = LineCap.Round };
            for (int i = 0; i < 8; i++)
            {
                double angle = i * Math.PI / 4;
                float x1 = cx + (float)(r * Math.Cos(angle));
                float y1 = cy + (float)(r * Math.Sin(angle));
                float x2 = cx + (float)((r * 1.8) * Math.Cos(angle));
                float y2 = cy + (float)((r * 1.8) * Math.Sin(angle));
                g.DrawLine(penPointe, x1, y1, x2, y2);
                // Petite boule en bout
                g.FillEllipse(new SolidBrush(rouge), x2 - 5, y2 - 5, 10, 10);
            }

            // Yeux (points blancs)
            g.FillEllipse(Brushes.White, cx - r * 0.4f - 5, cy - r * 0.3f - 5, 10, 10);
            g.FillEllipse(Brushes.White, cx + r * 0.4f - 5, cy - r * 0.3f - 5, 10, 10);

            // Label
            DessinerLabel(g, "VIRUS", rouge, t);
            g.Dispose();
            return bmp;
        }

        // ── Icône Pare-feu ──
        private static Bitmap DessinerPareFeu(int t)
        {
            var (bmp, g) = CreerCanvas(t, 1);
            var cyan = Color.FromArgb(0x00, 0xD4, 0xFF);
            float m = t * 0.15f;

            // Mur de briques (pare-feu)
            using var penBrique = new Pen(Color.FromArgb(150, cyan), 2f);
            using var brushBrique = new SolidBrush(Color.FromArgb(40, cyan));

            float bW = (t - 2 * m) / 3f, bH = (t * 0.45f) / 3f;
            for (int row = 0; row < 3; row++)
            {
                float offset = (row % 2 == 0) ? 0 : bW / 2;
                for (int col = -1; col < 4; col++)
                {
                    var rect = new RectangleF(m + col * bW + offset - 2, m + row * bH * 1.3f, bW - 4, bH - 2);
                    g.FillRectangle(brushBrique, rect);
                    g.DrawRectangle(penBrique, rect.X, rect.Y, rect.Width, rect.Height);
                }
            }

            // Flammes (courbes orangées)
            using var penFlamme = new Pen(Color.FromArgb(200, 255, 120, 0), 4f) { LineJoin = LineJoin.Round };
            float fy = t * 0.55f, fh = t * 0.35f;
            DrawFlame(g, t / 2f, fy, fh, Color.FromArgb(220, 255, 80, 0));
            DrawFlame(g, t / 2f - 18, fy + 12, fh * 0.7f, Color.FromArgb(180, 255, 150, 0));
            DrawFlame(g, t / 2f + 18, fy + 12, fh * 0.7f, Color.FromArgb(180, 255, 150, 0));

            DessinerLabel(g, "FIREWALL", cyan, t);
            g.Dispose();
            return bmp;
        }

        private static void DrawFlame(Graphics g, float cx, float top, float height, Color c)
        {
            using var brush = new SolidBrush(c);
            var pts = new PointF[]
            {
                new(cx, top),
                new(cx + height * 0.3f, top + height * 0.4f),
                new(cx + height * 0.15f, top + height * 0.6f),
                new(cx + height * 0.25f, top + height),
                new(cx, top + height * 0.75f),
                new(cx - height * 0.25f, top + height),
                new(cx - height * 0.15f, top + height * 0.6f),
                new(cx - height * 0.3f, top + height * 0.4f),
            };
            g.FillPolygon(brush, pts);
        }

        // ── Icône Cadenas ──
        private static Bitmap DessinerCadenas(int t)
        {
            var (bmp, g) = CreerCanvas(t, 2);
            var vert = Color.FromArgb(0x00, 0xFF, 0xB3);
            float cx = t / 2f;

            // Corps du cadenas
            float bx = cx - t * 0.22f, by = t * 0.45f, bw = t * 0.44f, bh = t * 0.38f;
            using var brushCorps = new SolidBrush(Color.FromArgb(60, vert));
            using var penCorps = new Pen(vert, 3f);
            g.FillRoundedRectangle(brushCorps, (int)bx, (int)by, (int)bw, (int)bh, 8);
            g.DrawRoundedRectangle(penCorps, (int)bx, (int)by, (int)bw, (int)bh, 8);

            // Arc du cadenas
            using var penArc = new Pen(vert, 5f) { StartCap = LineCap.Round, EndCap = LineCap.Round };
            float arcX = cx - t * 0.15f, arcY = t * 0.18f, arcW = t * 0.30f, arcH = t * 0.35f;
            g.DrawArc(penArc, arcX, arcY, arcW, arcH, 180, 180);

            // Trou de serrure
            g.FillEllipse(new SolidBrush(vert), cx - 8, by + bh * 0.25f, 16, 16);
            g.FillRectangle(new SolidBrush(vert), cx - 4, by + bh * 0.4f, 8, bh * 0.3f);

            DessinerLabel(g, "CADENAS", vert, t);
            g.Dispose();
            return bmp;
        }

        // ── Icône Mot de passe ──
        private static Bitmap DessinerMotDePasse(int t)
        {
            var (bmp, g) = CreerCanvas(t, 3);
            var jaune = Color.FromArgb(0xFF, 0xC8, 0x00);

            // Astérisques de mot de passe
            using var font = new Font("Consolas", t * 0.18f, FontStyle.Bold);
            using var brush = new SolidBrush(jaune);
            string symboles = "* * * *";
            var size = g.MeasureString(symboles, font);
            g.DrawString(symboles, font, brush, (t - size.Width) / 2, t * 0.28f);

            // Barre de saisie stylisée
            float bx = t * 0.1f, by = t * 0.55f;
            using var penBarre = new Pen(Color.FromArgb(150, jaune), 2f);
            using var brushBarre = new SolidBrush(Color.FromArgb(30, jaune));
            g.FillRoundedRectangle(brushBarre, (int)bx, (int)by, (int)(t * 0.8f), (int)(t * 0.12f), 4);
            g.DrawRoundedRectangle(penBarre, (int)bx, (int)by, (int)(t * 0.8f), (int)(t * 0.12f), 4);

            // Curseur clignotant (simulé)
            using var penCurseur = new Pen(jaune, 2f);
            g.DrawLine(penCurseur, t * 0.15f, by + t * 0.02f, t * 0.15f, by + t * 0.10f);

            DessinerLabel(g, "PASSWORD", jaune, t);
            g.Dispose();
            return bmp;
        }

        // ── Icône Bouclier ──
        private static Bitmap DessinerBouclier(int t)
        {
            var (bmp, g) = CreerCanvas(t, 4);
            var violet = Color.FromArgb(0x7C, 0x4D, 0xFF);
            float cx = t / 2f;

            // Forme bouclier
            var pts = new PointF[]
            {
                new(cx, t * 0.15f),
                new(t * 0.82f, t * 0.28f),
                new(t * 0.82f, t * 0.55f),
                new(cx, t * 0.85f),
                new(t * 0.18f, t * 0.55f),
                new(t * 0.18f, t * 0.28f),
            };
            using var brushBouclier = new SolidBrush(Color.FromArgb(60, violet));
            using var penBouclier = new Pen(violet, 4f);
            g.FillPolygon(brushBouclier, pts);
            g.DrawPolygon(penBouclier, pts);

            // Coche "check" au centre
            using var penCheck = new Pen(Color.FromArgb(220, violet), 5f) { StartCap = LineCap.Round, EndCap = LineCap.Round, LineJoin = LineJoin.Round };
            g.DrawLines(penCheck, new PointF[]
            {
                new(cx - t * 0.15f, t * 0.50f),
                new(cx - t * 0.03f, t * 0.62f),
                new(cx + t * 0.18f, t * 0.38f),
            });

            DessinerLabel(g, "SHIELD", violet, t);
            g.Dispose();
            return bmp;
        }

        // ── Icône Hacker ──
        private static Bitmap DessinerHacker(int t)
        {
            var (bmp, g) = CreerCanvas(t, 5);
            var orange = Color.FromArgb(0xFF, 0x6B, 0x35);

            // Terminal / code
            using var fontCode = new Font("Consolas", t * 0.09f, FontStyle.Regular);
            using var brushCode = new SolidBrush(Color.FromArgb(160, orange));
            string[] lignes = { "> sys.intrude()", "> root@target:~#", "> ACCESS GRANTED", "> [■■■■■■■■] 100%" };
            for (int i = 0; i < lignes.Length; i++)
                g.DrawString(lignes[i], fontCode, brushCode, t * 0.08f, t * (0.20f + i * 0.14f));

            // Contour terminal
            using var penTerminal = new Pen(Color.FromArgb(120, orange), 2f);
            g.DrawRoundedRectangle(penTerminal, (int)(t * 0.06f), (int)(t * 0.15f), (int)(t * 0.88f), (int)(t * 0.58f), 6);

            DessinerLabel(g, "HACKER", orange, t);
            g.Dispose();
            return bmp;
        }

        // ── Icône VPN ──
        private static Bitmap DessinerVPN(int t)
        {
            var (bmp, g) = CreerCanvas(t, 6);
            var bleu = Color.FromArgb(0x00, 0xE5, 0xFF);
            float cx = t / 2f, cy = t * 0.44f;

            // Globe terrestre simplifié
            using var penGlobe = new Pen(Color.FromArgb(150, bleu), 2f);
            using var brushGlobe = new SolidBrush(Color.FromArgb(30, bleu));
            float rg = t * 0.28f;
            g.FillEllipse(brushGlobe, cx - rg, cy - rg, rg * 2, rg * 2);
            g.DrawEllipse(penGlobe, cx - rg, cy - rg, rg * 2, rg * 2);
            // Lignes de latitude / longitude
            g.DrawLine(penGlobe, cx - rg, cy, cx + rg, cy);
            g.DrawArc(penGlobe, cx - rg * 0.5f, cy - rg, rg, rg * 2, 0, 360);
            g.DrawArc(penGlobe, cx - rg, cy - rg * 0.5f, rg * 2, rg, 0, 360);

            // Tunnel VPN (flèche sécurisée)
            using var penTunnel = new Pen(bleu, 3f) { EndCap = LineCap.ArrowAnchor };
            g.DrawLine(penTunnel, cx - rg - 20, cy - 5, cx - rg + 5, cy - 5);
            g.DrawLine(penTunnel, cx + rg - 5, cy + 5, cx + rg + 20, cy + 5);

            // Label VPN en gras
            using var fontVPN = new Font("Segoe UI", t * 0.12f, FontStyle.Bold);
            using var brushVPN = new SolidBrush(bleu);
            var sz = g.MeasureString("VPN", fontVPN);
            g.DrawString("VPN", fontVPN, brushVPN, cx - sz.Width / 2, cy - sz.Height / 2 - 2);

            DessinerLabel(g, "VPN", bleu, t);
            g.Dispose();
            return bmp;
        }

        // ── Icône Chiffrement ──
        private static Bitmap DessinerChiffrement(int t)
        {
            var (bmp, g) = CreerCanvas(t, 7);
            var rose = Color.FromArgb(0xFF, 0x41, 0xB3);
            float cx = t / 2f;

            // Clé de chiffrement
            float kx = t * 0.15f, ky = t * 0.28f, kr = t * 0.13f;
            using var penCle = new Pen(rose, 4f);
            g.DrawEllipse(penCle, kx, ky, kr * 2, kr * 2);
            // Tige de la clé
            using var penTige = new Pen(rose, 4f) { EndCap = LineCap.Square };
            g.DrawLine(penTige, kx + kr * 2, ky + kr, t * 0.78f, ky + kr);
            g.DrawLine(penTige, t * 0.65f, ky + kr, t * 0.65f, ky + kr + 10);
            g.DrawLine(penTige, t * 0.78f, ky + kr, t * 0.78f, ky + kr + 14);

            // Flux de bits
            using var fontBits = new Font("Consolas", t * 0.08f, FontStyle.Regular);
            using var brushBits = new SolidBrush(Color.FromArgb(140, rose));
            string[] bits = { "01101000", "11001010", "10110011" };
            for (int i = 0; i < bits.Length; i++)
                g.DrawString(bits[i], fontBits, brushBits, t * 0.1f, t * (0.52f + i * 0.11f));

            DessinerLabel(g, "ENCRYPT", rose, t);
            g.Dispose();
            return bmp;
        }

        // ── Icône inconnue ──
        private static Bitmap DessinerInconnu(int t)
        {
            var (bmp, g) = CreerCanvas(t, 0);
            using var font = new Font("Segoe UI", t * 0.3f, FontStyle.Bold);
            using var brush = new SolidBrush(Color.Gray);
            g.DrawString("?", font, brush, t * 0.3f, t * 0.25f);
            g.Dispose();
            return bmp;
        }

        // =========================================================
        // ── ICÔNES SUPPLÉMENTAIRES — GRILLE 6×6 ──
        // =========================================================

        // ── Icône Phishing ──
        private static Bitmap DessinerPhishing(int t)
        {
            var (bmp, g) = CreerCanvas(t, 8 % Couleurs.Length);
            var c = Color.FromArgb(255, 80, 80);
            float cx = t / 2f;
            // Hameçon
            using var pen = new Pen(c, 4f) { StartCap = LineCap.Round, EndCap = LineCap.Round };
            g.DrawArc(pen, cx - t*0.18f, t*0.18f, t*0.36f, t*0.35f, 0, 270);
            g.DrawLine(pen, cx + t*0.18f, t*0.35f, cx + t*0.18f, t*0.60f);
            g.DrawArc(pen, cx + t*0.06f, t*0.52f, t*0.22f, t*0.18f, 0, -180);
            // L’appât (ver)
            using var brushVer = new SolidBrush(Color.FromArgb(200, c));
            g.FillEllipse(brushVer, cx - t*0.08f, t*0.15f, t*0.16f, t*0.10f);
            DessinerLabel(g, "PHISHING", c, t);
            g.Dispose();
            return bmp;
        }

        // ── Icône Malware ──
        private static Bitmap DessinerMalware(int t)
        {
            var (bmp, g) = CreerCanvas(t, 9 % Couleurs.Length);
            var c = Color.FromArgb(180, 50, 255);
            float cx = t/2f, cy = t*0.42f;
            // Tête de tête de mort
            using var pen = new Pen(c, 3f);
            using var brush = new SolidBrush(Color.FromArgb(60, c));
            g.FillEllipse(brush, cx-t*0.22f, cy-t*0.25f, t*0.44f, t*0.38f);
            g.DrawEllipse(pen, cx-t*0.22f, cy-t*0.25f, t*0.44f, t*0.38f);
            // Yeux creux
            g.FillEllipse(new SolidBrush(Color.FromArgb(200, 8,12,28)), cx-t*0.12f, cy-t*0.12f, t*0.10f, t*0.10f);
            g.FillEllipse(new SolidBrush(Color.FromArgb(200, 8,12,28)), cx+t*0.02f, cy-t*0.12f, t*0.10f, t*0.10f);
            // Bouche à crochets
            g.DrawLine(pen, cx-t*0.12f, cy+t*0.08f, cx+t*0.12f, cy+t*0.08f);
            g.DrawLine(pen, cx-t*0.08f, cy+t*0.08f, cx-t*0.08f, cy+t*0.16f);
            g.DrawLine(pen, cx, cy+t*0.08f, cx, cy+t*0.16f);
            g.DrawLine(pen, cx+t*0.08f, cy+t*0.08f, cx+t*0.08f, cy+t*0.16f);
            DessinerLabel(g, "MALWARE", c, t);
            g.Dispose();
            return bmp;
        }

        // ── Icône DDoS ──
        private static Bitmap DessinerDDoS(int t)
        {
            var (bmp, g) = CreerCanvas(t, 10 % Couleurs.Length);
            var c = Color.FromArgb(255, 60, 0);
            float cx = t/2f, cy = t*0.42f;
            // Serveur cible
            using var penC = new Pen(c, 3f);
            using var brushC = new SolidBrush(Color.FromArgb(50, c));
            g.FillRoundedRectangle(brushC, (int)(cx-t*0.12f), (int)(cy-t*0.10f), (int)(t*0.24f), (int)(t*0.20f), 4);
            g.DrawRoundedRectangle(penC, (int)(cx-t*0.12f), (int)(cy-t*0.10f), (int)(t*0.24f), (int)(t*0.20f), 4);
            // Flèches d'attaque entrantes
            using var penF = new Pen(Color.FromArgb(200, c), 2f) { EndCap = LineCap.ArrowAnchor };
            float[] angles = { 0, 45, 90, 135, 180, 225, 270, 315 };
            foreach (var a in angles)
            {
                double rad = a * Math.PI / 180;
                float x1 = cx + (float)(t*0.40f * Math.Cos(rad));
                float y1 = cy + (float)(t*0.40f * Math.Sin(rad));
                float x2 = cx + (float)(t*0.15f * Math.Cos(rad));
                float y2 = cy + (float)(t*0.15f * Math.Sin(rad));
                g.DrawLine(penF, x1, y1, x2, y2);
            }
            DessinerLabel(g, "DDOS", c, t);
            g.Dispose();
            return bmp;
        }

        // ── Icône Ransomware ──
        private static Bitmap DessinerRansomware(int t)
        {
            var (bmp, g) = CreerCanvas(t, 11 % Couleurs.Length);
            var c = Color.FromArgb(255, 180, 0);
            float cx = t/2f, cy = t*0.38f;
            // Dossier fermé
            using var pen = new Pen(c, 3f);
            using var brush = new SolidBrush(Color.FromArgb(40, c));
            g.FillRoundedRectangle(brush, (int)(cx-t*0.25f),(int)(cy-t*0.05f),(int)(t*0.50f),(int)(t*0.28f),4);
            g.DrawRoundedRectangle(pen, (int)(cx-t*0.25f),(int)(cy-t*0.05f),(int)(t*0.50f),(int)(t*0.28f),4);
            // Cadenas dessus
            using var penV = new Pen(c, 4f);
            g.DrawArc(penV, cx-t*0.10f, cy-t*0.18f, t*0.20f, t*0.16f, 180, 180);
            g.FillEllipse(new SolidBrush(c), cx-t*0.04f, cy+t*0.02f, t*0.08f, t*0.08f);
            // "$" du ransom
            using var fnt = new Font("Segoe UI", t*0.15f, FontStyle.Bold);
            using var br = new SolidBrush(c);
            var sz = g.MeasureString("$", fnt);
            g.DrawString("$", fnt, br, cx-sz.Width/2, cy+t*0.08f);
            DessinerLabel(g, "RANSOMWARE", c, t);
            g.Dispose();
            return bmp;
        }

        // ── Icône Antivirus ──
        private static Bitmap DessinerAntivirus(int t)
        {
            var (bmp, g) = CreerCanvas(t, 12 % Couleurs.Length);
            var c = Color.FromArgb(0, 230, 120);
            float cx = t/2f, cy = t*0.42f;
            // Bouclier vert
            var pts = new PointF[]
            {
                new(cx, cy-t*0.26f), new(cx+t*0.20f,cy-t*0.14f),
                new(cx+t*0.20f,cy+t*0.06f), new(cx,cy+t*0.24f),
                new(cx-t*0.20f,cy+t*0.06f), new(cx-t*0.20f,cy-t*0.14f)
            };
            using var brushB = new SolidBrush(Color.FromArgb(50, c));
            using var penB = new Pen(c, 3f);
            g.FillPolygon(brushB, pts); g.DrawPolygon(penB, pts);
            // Croix médicale
            using var penCroix = new Pen(c, 5f) { StartCap = LineCap.Round, EndCap = LineCap.Round };
            g.DrawLine(penCroix, cx, cy-t*0.12f, cx, cy+t*0.12f);
            g.DrawLine(penCroix, cx-t*0.10f, cy, cx+t*0.10f, cy);
            DessinerLabel(g, "ANTIVIRUS", c, t);
            g.Dispose();
            return bmp;
        }

        // ── Icône Certificat ──
        private static Bitmap DessinerCertificat(int t)
        {
            var (bmp, g) = CreerCanvas(t, 13 % Couleurs.Length);
            var c = Color.FromArgb(0, 190, 255);
            float cx = t/2f;
            // Parchemin
            using var pen = new Pen(c, 2f);
            using var brush = new SolidBrush(Color.FromArgb(30, c));
            g.FillRoundedRectangle(brush, (int)(cx-t*0.28f),(int)(t*0.15f),(int)(t*0.56f),(int)(t*0.55f),6);
            g.DrawRoundedRectangle(pen, (int)(cx-t*0.28f),(int)(t*0.15f),(int)(t*0.56f),(int)(t*0.55f),6);
            // Lignes de texte
            for(int i=0;i<3;i++)
                g.DrawLine(pen, cx-t*0.18f, t*(0.27f+i*0.10f), cx+t*0.18f, t*(0.27f+i*0.10f));
            // Médaille
            using var brushM = new SolidBrush(Color.FromArgb(80, c));
            g.FillEllipse(brushM, cx-t*0.10f, t*0.60f, t*0.20f, t*0.20f);
            g.DrawEllipse(pen, cx-t*0.10f, t*0.60f, t*0.20f, t*0.20f);
            using var fnt = new Font("Segoe UI", t*0.08f, FontStyle.Bold);
            var sz = g.MeasureString("SSL", fnt);
            g.DrawString("SSL", fnt, new SolidBrush(c), cx-sz.Width/2, t*0.645f);
            DessinerLabel(g, "CERTIFICAT", c, t);
            g.Dispose();
            return bmp;
        }

        // ── Icône Réseau ──
        private static Bitmap DessinerReseau(int t)
        {
            var (bmp, g) = CreerCanvas(t, 14 % Couleurs.Length);
            var c = Color.FromArgb(80, 200, 255);
            float cx = t/2f, cy = t*0.42f;
            // Noeuds du réseau
            PointF[] noeuds = {
                new(cx, cy-t*0.22f), new(cx+t*0.22f, cy),
                new(cx, cy+t*0.22f), new(cx-t*0.22f, cy), new(cx, cy)
            };
            using var pen = new Pen(Color.FromArgb(120, c), 2f);
            // Connexions
            foreach(var n in noeuds)
                g.DrawLine(pen, cx, cy, n.X, n.Y);
            // Cercles
            using var brushN = new SolidBrush(c);
            foreach(var n in noeuds)
                g.FillEllipse(brushN, n.X-7, n.Y-7, 14, 14);
            DessinerLabel(g, "RESEAU", c, t);
            g.Dispose();
            return bmp;
        }

        // ── Icône Cloud ──
        private static Bitmap DessinerCloud(int t)
        {
            var (bmp, g) = CreerCanvas(t, 15 % Couleurs.Length);
            var c = Color.FromArgb(120, 180, 255);
            float cx = t/2f, cy = t*0.40f;
            // Nuage
            using var brush = new SolidBrush(Color.FromArgb(80, c));
            using var pen = new Pen(c, 3f);
            g.FillEllipse(brush, cx-t*0.22f, cy-t*0.10f, t*0.24f, t*0.22f);
            g.FillEllipse(brush, cx-t*0.06f, cy-t*0.18f, t*0.28f, t*0.26f);
            g.FillEllipse(brush, cx+t*0.08f, cy-t*0.08f, t*0.18f, t*0.18f);
            g.FillRectangle(brush, cx-t*0.22f, cy+t*0.02f, t*0.44f, t*0.10f);
            g.DrawEllipse(pen, cx-t*0.22f, cy-t*0.10f, t*0.24f, t*0.22f);
            g.DrawEllipse(pen, cx-t*0.06f, cy-t*0.18f, t*0.28f, t*0.26f);
            g.DrawEllipse(pen, cx+t*0.08f, cy-t*0.08f, t*0.18f, t*0.18f);
            // Flèche de téléchargement
            using var penF = new Pen(c, 3f);
            g.DrawLine(penF, cx, cy+t*0.12f, cx, cy+t*0.30f);
            g.DrawLine(penF, cx-t*0.08f, cy+t*0.22f, cx, cy+t*0.30f);
            g.DrawLine(penF, cx+t*0.08f, cy+t*0.22f, cx, cy+t*0.30f);
            DessinerLabel(g, "CLOUD", c, t);
            g.Dispose();
            return bmp;
        }

        // ── Icône Base de Données ──
        private static Bitmap DessinerBaseDeDonnees(int t)
        {
            var (bmp, g) = CreerCanvas(t, 16 % Couleurs.Length);
            var c = Color.FromArgb(255, 160, 50);
            float cx = t/2f;
            // Cylindres (disques)
            float ew = t*0.40f, eh = t*0.10f, ex = cx-t*0.20f;
            float[] ys = { t*0.20f, t*0.34f, t*0.48f };
            using var brush = new SolidBrush(Color.FromArgb(50, c));
            using var pen = new Pen(c, 2f);
            foreach(var y in ys)
            {
                g.FillEllipse(brush, ex, y, ew, eh);
                g.DrawEllipse(pen, ex, y, ew, eh);
                g.FillRectangle(brush, ex, y+eh/2, ew, t*0.10f);
                g.DrawLine(pen, ex, y+eh/2, ex, y+eh/2+t*0.10f);
                g.DrawLine(pen, ex+ew, y+eh/2, ex+ew, y+eh/2+t*0.10f);
            }
            DessinerLabel(g, "DATABASE", c, t);
            g.Dispose();
            return bmp;
        }

        // ── Icône Authentification ──
        private static Bitmap DessinerAuthentification(int t)
        {
            var (bmp, g) = CreerCanvas(t, 17 % Couleurs.Length);
            var c = Color.FromArgb(0, 255, 200);
            float cx = t/2f, cy = t*0.35f;
            // Silhouette utilisateur
            using var brush = new SolidBrush(Color.FromArgb(60, c));
            using var pen = new Pen(c, 3f);
            g.FillEllipse(brush, cx-t*0.12f, cy-t*0.22f, t*0.24f, t*0.20f);
            g.DrawEllipse(pen, cx-t*0.12f, cy-t*0.22f, t*0.24f, t*0.20f);
            g.FillEllipse(brush, cx-t*0.20f, cy, t*0.40f, t*0.22f);
            g.DrawEllipse(pen, cx-t*0.20f, cy, t*0.40f, t*0.22f);
            // Coche verte de validation
            using var penCheck = new Pen(c, 4f){StartCap=LineCap.Round,EndCap=LineCap.Round,LineJoin=LineJoin.Round};
            g.DrawLines(penCheck, new PointF[]{
                new(cx+t*0.02f, cy+t*0.30f),
                new(cx+t*0.10f, cy+t*0.40f),
                new(cx+t*0.22f, cy+t*0.22f)
            });
            DessinerLabel(g, "AUTH", c, t);
            g.Dispose();
            return bmp;
        }

        /// <summary>Dessine le label de l'icône en bas de la carte.</summary>
        private static void DessinerLabel(Graphics g, string texte, Color couleur, int t)
        {
            using var font = new Font("Segoe UI", t * 0.085f, FontStyle.Bold);
            using var brush = new SolidBrush(Color.FromArgb(200, couleur));
            var size = g.MeasureString(texte, font);
            g.DrawString(texte, font, brush, (t - size.Width) / 2, t * 0.87f);
        }

        /// <summary>Génère l'image du dos de carte (face cachée).</summary>
        public static Bitmap GenererDos(int taille = 120)
        {
            var bmp = new Bitmap(taille, taille);
            var g = Graphics.FromImage(bmp);
            g.SmoothingMode = SmoothingMode.AntiAlias;

            // Fond dégradé bleu marine → violet foncé
            using var fondBrush = new LinearGradientBrush(
                new Rectangle(0, 0, taille, taille),
                Color.FromArgb(255, 8, 15, 50),
                Color.FromArgb(255, 25, 10, 60),
                LinearGradientMode.ForwardDiagonal);
            g.FillRectangle(fondBrush, 0, 0, taille, taille);

            // Grille de circuit imprimé (ambiance cyber)
            using var penCircuit = new Pen(Color.FromArgb(40, 0, 200, 255), 1f);
            for (int i = 0; i <= taille; i += 20)
            {
                g.DrawLine(penCircuit, i, 0, i, taille);
                g.DrawLine(penCircuit, 0, i, taille, i);
            }

            // Logo SecurIT au centre
            float cx = taille / 2f, cy = taille / 2f;
            using var penLogo = new Pen(Color.FromArgb(180, 0, 212, 255), 3f);
            using var brushLogo = new SolidBrush(Color.FromArgb(60, 0, 212, 255));

            // Bouclier simplifié
            var ptsBouclier = new PointF[]
            {
                new(cx, cy - taille * 0.28f),
                new(cx + taille * 0.22f, cy - taille * 0.15f),
                new(cx + taille * 0.22f, cy + taille * 0.08f),
                new(cx, cy + taille * 0.28f),
                new(cx - taille * 0.22f, cy + taille * 0.08f),
                new(cx - taille * 0.22f, cy - taille * 0.15f),
            };
            g.FillPolygon(brushLogo, ptsBouclier);
            g.DrawPolygon(penLogo, ptsBouclier);

            // Texte "S" dans le bouclier
            using var fontS = new Font("Segoe UI", taille * 0.2f, FontStyle.Bold);
            using var brushS = new SolidBrush(Color.FromArgb(200, 0, 212, 255));
            var sz = g.MeasureString("S", fontS);
            g.DrawString("S", fontS, brushS, cx - sz.Width / 2, cy - sz.Height / 2);

            // Bordure externe
            using var penBordure = new Pen(Color.FromArgb(100, 0, 212, 255), 2f);
            g.DrawRoundedRectangle(penBordure, 3, 3, taille - 7, taille - 7, 12);

            g.Dispose();
            return bmp;
        }
    }

    /// <summary>Extension pour dessiner des rectangles aux coins arrondis.</summary>
    public static class GraphicsExtensions
    {
        public static void DrawRoundedRectangle(this Graphics g, Pen pen, int x, int y, int width, int height, int radius)
        {
            using var path = GetRoundedPath(x, y, width, height, radius);
            g.DrawPath(pen, path);
        }

        public static void FillRoundedRectangle(this Graphics g, Brush brush, int x, int y, int width, int height, int radius)
        {
            using var path = GetRoundedPath(x, y, width, height, radius);
            g.FillPath(brush, path);
        }

        private static System.Drawing.Drawing2D.GraphicsPath GetRoundedPath(int x, int y, int w, int h, int r)
        {
            var path = new System.Drawing.Drawing2D.GraphicsPath();
            path.AddArc(x, y, r * 2, r * 2, 180, 90);
            path.AddArc(x + w - r * 2, y, r * 2, r * 2, 270, 90);
            path.AddArc(x + w - r * 2, y + h - r * 2, r * 2, r * 2, 0, 90);
            path.AddArc(x, y + h - r * 2, r * 2, r * 2, 90, 90);
            path.CloseFigure();
            return path;
        }
    }
}
