namespace SecurIT_Memory.Forms
{
    /// <summary>
    /// Formulaire du Menu Principal — plein écran, animation sans clignotement,
    /// stocke les options (grille, difficulté, mode Hardcore).
    /// </summary>
    public class FormMenu : Form
    {
        // ────────────── Paramètres sauvegardés ──────────────
        private int _tailleGrille = 4;
        private string _difficulte = "Normal";
        private bool _modeContreLaMontre = false;

        // ────────────── Contrôles ──────────────
        private Button _btnJouer = null!;
        private Button _btnOptions = null!;
        private Button _btnQuitter = null!;
        private PanelDB _panelTitre = null!;   // PanelDB = Panel avec DoubleBuffer (anti-clignotement)
        private System.Windows.Forms.Timer _timerAnime = null!;
        private float _pulsePhase = 0f;

        // ────────────── Constructeur ──────────────
        public FormMenu()
        {
            InitialiserInterface();
            InitialiserAnimation();
        }

        // ────────────── Panel double-buffered (supprime le clignotement) ──────────────
        /// <summary>
        /// Panel personnalisé avec DoubleBuffered activé — élimine tout clignotement
        /// lors du redessinage de l'animation.
        /// </summary>
        private class PanelDB : Panel
        {
            public PanelDB() { DoubleBuffered = true; }
        }

        // ────────────── Initialisation ──────────────
        private void InitialiserInterface()
        {
            var ecran = Screen.PrimaryScreen?.WorkingArea ?? new Rectangle(0, 0, 1280, 900);
            int w = Math.Min(800, ecran.Width - 40);
            int h = Math.Min(920, ecran.Height - 40);

            this.Text = "SecurIT Memory — Menu Principal";
            this.ClientSize = new Size(w, h);
            this.MinimumSize = new Size(700, 750);
            this.MaximumSize = new Size(w, h);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(8, 12, 28);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.DoubleBuffered = true;

            // ── Panel Titre (double-buffered pour éliminer le clignotement) ──
            _panelTitre = new PanelDB
            {
                Dock = DockStyle.Top,
                Height = (int)(h * 0.42),
                BackColor = Color.FromArgb(8, 12, 28)
            };
            _panelTitre.Paint += PanelTitre_Paint;

            // ── Panel Boutons ──
            var panelBoutons = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent,
                Padding = new Padding((int)(w * 0.12), 25, (int)(w * 0.12), 20)
            };

            _btnJouer   = CreerBoutonMenu("▶   JOUER",   Color.FromArgb(0, 110, 190), Color.FromArgb(0, 200, 255));
            _btnOptions = CreerBoutonMenu("⚙   OPTIONS", Color.FromArgb(55, 0, 110),  Color.FromArgb(130, 50, 220));
            _btnQuitter = CreerBoutonMenu("✖   QUITTER", Color.FromArgb(100, 18, 18), Color.FromArgb(200, 40, 40));

            _btnJouer.Click   += BtnJouer_Click;
            _btnOptions.Click += BtnOptions_Click;
            _btnQuitter.Click += (s, e) => Application.Exit();

            var layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 3,
                ColumnStyles = { new ColumnStyle(SizeType.Percent, 100f) }
            };
            for (int i = 0; i < 3; i++)
                layout.RowStyles.Add(new RowStyle(SizeType.Percent, 33.33f));

            foreach (var btn in new[] { _btnJouer, _btnOptions, _btnQuitter })
            {
                btn.Dock = DockStyle.Fill;
                btn.Margin = new Padding(0, 10, 0, 10);
            }
            layout.Controls.Add(_btnJouer,   0, 0);
            layout.Controls.Add(_btnOptions, 0, 1);
            layout.Controls.Add(_btnQuitter, 0, 2);
            panelBoutons.Controls.Add(layout);

            // ── Footer ──
            var lblFooter = new Label
            {
                Text = "SecurIT © 2025 — Salon de l'Innovation Tech",
                ForeColor = Color.FromArgb(60, 90, 130),
                Font = new Font("Segoe UI", 9f),
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Bottom,
                Height = 28
            };

            this.Controls.Add(panelBoutons);
            this.Controls.Add(_panelTitre);
            this.Controls.Add(lblFooter);
        }

        // ────────────── Dessin du titre (tout dans Paint = pas de clignotement) ──────────────
        private void PanelTitre_Paint(object? sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
            var r = _panelTitre.ClientRectangle;

            // Fond dégradé
            using var fondBrush = new System.Drawing.Drawing2D.LinearGradientBrush(
                r,
                Color.FromArgb(255, 5, 10, 25),
                Color.FromArgb(255, 15, 25, 60),
                System.Drawing.Drawing2D.LinearGradientMode.Vertical);
            g.FillRectangle(fondBrush, r);

            // Grille de circuit
            using var penCircuit = new Pen(Color.FromArgb(20, 0, 160, 255), 1f);
            for (int x = 0; x <= r.Width;  x += 35) g.DrawLine(penCircuit, x, 0, x, r.Height);
            for (int y = 0; y <= r.Height; y += 35) g.DrawLine(penCircuit, 0, y, r.Width, y);

            float cx = r.Width / 2f;
            float cy = r.Height * 0.44f;

            // Halo (PathGradientBrush, pas de clignotement car tout est dans Paint)
            float haloR = 110 + (float)Math.Sin(_pulsePhase) * 9;
            using var haloPath = new System.Drawing.Drawing2D.GraphicsPath();
            haloPath.AddEllipse(cx - haloR, cy - haloR, haloR * 2, haloR * 2);
            using var haloBrush = new System.Drawing.Drawing2D.PathGradientBrush(haloPath);
            haloBrush.CenterColor   = Color.FromArgb(55, 0, 212, 255);
            haloBrush.SurroundColors = new[] { Color.FromArgb(0, 0, 0, 0) };
            g.FillEllipse(haloBrush, cx - haloR, cy - haloR, haloR * 2, haloR * 2);

            // Bouclier SecurIT
            float bouclierTaille = 85 + (float)Math.Sin(_pulsePhase) * 2.5f;
            DessinerBouclier(g, cx, cy, bouclierTaille);

            // ── Titre "SecurIT Memory" — dessiné ici, pas dans un Label ──
            using var fontTitre = new Font("Segoe UI", 30f, FontStyle.Bold);
            string titre = "SecurIT Memory";
            var szTitre = g.MeasureString(titre, fontTitre);
            float tyY = r.Height - 95;

            // Ombre
            using var brushOmbre = new SolidBrush(Color.FromArgb(100, 0, 0, 0));
            g.DrawString(titre, fontTitre, brushOmbre, cx - szTitre.Width / 2 + 3, tyY + 3);

            // Dégradé cyan
            using var brushTitre = new System.Drawing.Drawing2D.LinearGradientBrush(
                new RectangleF(cx - szTitre.Width / 2, tyY, szTitre.Width, szTitre.Height),
                Color.FromArgb(0, 220, 255),
                Color.FromArgb(120, 190, 255),
                System.Drawing.Drawing2D.LinearGradientMode.Horizontal);
            g.DrawString(titre, fontTitre, brushTitre, cx - szTitre.Width / 2, tyY);

            // ── Sous-titre (dessiné ici aussi pour éviter tout clignotement du Label) ──
            using var fontSub = new Font("Segoe UI", 14f, FontStyle.Italic);
            string sousTitre = "Le Défi Cybersécurité";
            var szSub = g.MeasureString(sousTitre, fontSub);
            using var brushSub = new SolidBrush(Color.FromArgb(150, 0, 212, 255));
            g.DrawString(sousTitre, fontSub, brushSub, cx - szSub.Width / 2, tyY + szTitre.Height + 4);
        }

        /// <summary>Dessine le bouclier SecurIT vectoriellement.</summary>
        private static void DessinerBouclier(Graphics g, float cx, float cy, float taille)
        {
            var pts = new PointF[]
            {
                new(cx,                    cy - taille),
                new(cx + taille * 0.75f,   cy - taille * 0.5f),
                new(cx + taille * 0.75f,   cy + taille * 0.20f),
                new(cx,                    cy + taille),
                new(cx - taille * 0.75f,   cy + taille * 0.20f),
                new(cx - taille * 0.75f,   cy - taille * 0.5f),
            };
            using var brushB = new SolidBrush(Color.FromArgb(55, 0, 180, 255));
            using var penB   = new Pen(Color.FromArgb(170, 0, 212, 255), 3.5f);
            g.FillPolygon(brushB, pts);
            g.DrawPolygon(penB, pts);

            using var fontS  = new Font("Segoe UI", taille * 0.58f, FontStyle.Bold);
            using var brushS = new SolidBrush(Color.FromArgb(230, 0, 212, 255));
            var sz = g.MeasureString("S", fontS);
            g.DrawString("S", fontS, brushS, cx - sz.Width / 2, cy - sz.Height / 2 - taille * 0.05f);
        }

        // ────────────── Animation de pulse (uniquement phase, pas de Refresh agressif) ──────────────
        private void InitialiserAnimation()
        {
            _timerAnime = new System.Windows.Forms.Timer { Interval = 40 }; // ~25fps
            _timerAnime.Tick += (s, e) =>
            {
                _pulsePhase = (_pulsePhase + 0.06f) % ((float)Math.PI * 2);
                _panelTitre.Invalidate(); // Invalidate suffit — DoubleBuffered évite le clignotement
            };
            _timerAnime.Start();
        }

        // ────────────── Événements des boutons ──────────────
        private void BtnJouer_Click(object? sender, EventArgs e)
        {
            _timerAnime.Stop();
            var formJeu = new FormJeu(_tailleGrille, _difficulte, _modeContreLaMontre);
            formJeu.FormClosed += (s, ev) =>
            {
                this.Show();
                _timerAnime.Start();
            };
            this.Hide();
            formJeu.Show();
        }

        private void BtnOptions_Click(object? sender, EventArgs e)
        {
            var formOptions = new FormOptions(_tailleGrille, _difficulte, _modeContreLaMontre);
            if (formOptions.ShowDialog(this) == DialogResult.OK)
            {
                _tailleGrille       = formOptions.TailleGrille;
                _difficulte         = formOptions.Difficulte;
                _modeContreLaMontre = formOptions.ModeContreLaMontre;
            }
        }

        // ────────────── Création de boutons stylisés ──────────────
        private static Button CreerBoutonMenu(string texte, Color couleurFond, Color couleurBordure)
        {
            var btn = new Button
            {
                Text = texte,
                Font = new Font("Segoe UI", 16f, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = couleurFond,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Height = 64
            };
            btn.FlatAppearance.BorderColor = couleurBordure;
            btn.FlatAppearance.BorderSize  = 2;
            btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(
                Math.Min(255, couleurFond.R + 45),
                Math.Min(255, couleurFond.G + 45),
                Math.Min(255, couleurFond.B + 45));
            return btn;
        }

        // ────────────── Nettoyage ──────────────
        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            _timerAnime?.Stop();
            base.OnFormClosed(e);
        }
    }
}
