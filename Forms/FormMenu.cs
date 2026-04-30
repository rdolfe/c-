using SecurIT_Memory.Forms;

namespace SecurIT_Memory
{
    /// <summary>
    /// Formulaire du Menu Principal — point d'entrée visuel du jeu SecurIT Memory.
    /// Propose les actions : Jouer, Options et Quitter.
    /// </summary>
    public class FormMenu : Form
    {
        // ────────────── Paramètres sauvegardés ──────────────
        private int _tailleGrille = 4;
        private string _difficulte = "Normal";

        // ────────────── Contrôles UI ──────────────
        private Button _btnJouer = null!;
        private Button _btnOptions = null!;
        private Button _btnQuitter = null!;
        private Panel _panelTitre = null!;
        private Panel _panelBoutons = null!;
        private System.Windows.Forms.Timer _timerAnime = null!;
        private float _pulsePhase = 0f;
        private Label _lblSousTitre = null!;

        // ────────────── Constructeur ──────────────
        public FormMenu()
        {
            InitialiserInterface();
            InitialiserAnimation();
        }

        // ────────────── Initialisation ──────────────

        private void InitialiserInterface()
        {
            this.Text = "SecurIT Memory — Menu Principal";
            this.Size = new Size(550, 680);
            this.MinimumSize = new Size(550, 680);
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(8, 12, 28);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.DoubleBuffered = true;

            // ── Panel Titre ──
            _panelTitre = new Panel
            {
                Dock = DockStyle.Top,
                Height = 300,
                BackColor = Color.Transparent
            };
            _panelTitre.Paint += PanelTitre_Paint;

            // Sous-titre
            _lblSousTitre = new Label
            {
                Text = "Le Défi Cybersécurité",
                Font = new Font("Segoe UI", 13f, FontStyle.Italic),
                ForeColor = Color.FromArgb(150, 0, 212, 255),
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Bottom,
                Height = 35
            };
            _panelTitre.Controls.Add(_lblSousTitre);

            // ── Panel Boutons ──
            _panelBoutons = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent,
                Padding = new Padding(80, 30, 80, 20)
            };

            _btnJouer = CreerBoutonMenu("▶  JOUER", Color.FromArgb(0, 130, 200), Color.FromArgb(0, 200, 255));
            _btnOptions = CreerBoutonMenu("⚙  OPTIONS", Color.FromArgb(60, 0, 120), Color.FromArgb(130, 50, 220));
            _btnQuitter = CreerBoutonMenu("✖  QUITTER", Color.FromArgb(100, 20, 20), Color.FromArgb(200, 40, 40));

            _btnJouer.Click += BtnJouer_Click;
            _btnOptions.Click += BtnOptions_Click;
            _btnQuitter.Click += (s, e) => Application.Exit();

            var layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 3,
                ColumnStyles = { new ColumnStyle(SizeType.Percent, 100f) },
            };
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 33.3f));
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 33.3f));
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 33.3f));

            _btnJouer.Dock = DockStyle.Fill;
            _btnOptions.Dock = DockStyle.Fill;
            _btnQuitter.Dock = DockStyle.Fill;
            _btnJouer.Margin = new Padding(0, 8, 0, 8);
            _btnOptions.Margin = new Padding(0, 8, 0, 8);
            _btnQuitter.Margin = new Padding(0, 8, 0, 8);

            layout.Controls.Add(_btnJouer, 0, 0);
            layout.Controls.Add(_btnOptions, 0, 1);
            layout.Controls.Add(_btnQuitter, 0, 2);
            _panelBoutons.Controls.Add(layout);

            // ── Footer ──
            var lblFooter = new Label
            {
                Text = "SecurIT © 2025 — Salon de l'Innovation Tech",
                ForeColor = Color.FromArgb(70, 100, 140),
                Font = new Font("Segoe UI", 8f),
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Bottom,
                Height = 25
            };

            this.Controls.Add(_panelBoutons);
            this.Controls.Add(_panelTitre);
            this.Controls.Add(lblFooter);
        }

        // ────────────── Dessin du titre ──────────────

        private void PanelTitre_Paint(object? sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            var r = _panelTitre.ClientRectangle;

            // Fond dégradé
            using var fondBrush = new System.Drawing.Drawing2D.LinearGradientBrush(
                r,
                Color.FromArgb(255, 5, 10, 25),
                Color.FromArgb(255, 15, 25, 60),
                System.Drawing.Drawing2D.LinearGradientMode.Vertical);
            g.FillRectangle(fondBrush, r);

            // Grille de circuit
            using var penCircuit = new Pen(Color.FromArgb(25, 0, 180, 255), 1f);
            for (int x = 0; x <= r.Width; x += 30) g.DrawLine(penCircuit, x, 0, x, r.Height);
            for (int y = 0; y <= r.Height; y += 30) g.DrawLine(penCircuit, 0, y, r.Width, y);

            // Halo animé derrière le logo
            float cx = r.Width / 2f, cy = r.Height / 2f - 20;
            float haloR = 90 + (float)Math.Sin(_pulsePhase) * 8;
            // PathGradientBrush pour l'effet de halo radial
            using var haloPath = new System.Drawing.Drawing2D.GraphicsPath();
            haloPath.AddEllipse(cx - haloR, cy - haloR, haloR * 2, haloR * 2);
            using var haloBrush = new System.Drawing.Drawing2D.PathGradientBrush(haloPath);
            haloBrush.CenterColor = Color.FromArgb(50, 0, 212, 255);
            haloBrush.SurroundColors = new[] { Color.FromArgb(0, 0, 0, 0) };
            g.FillEllipse(haloBrush, cx - haloR, cy - haloR, haloR * 2, haloR * 2);

            // Bouclier SecurIT
            DessinerBouclier(g, cx, cy, 75 + (float)Math.Sin(_pulsePhase) * 2);

            // Titre "SecurIT Memory"
            using var fontTitre = new Font("Segoe UI", 26f, FontStyle.Bold);
            string titre = "SecurIT Memory";
            var szTitre = g.MeasureString(titre, fontTitre);

            // Ombre portée
            using var brushOmbre = new SolidBrush(Color.FromArgb(80, 0, 0, 0));
            g.DrawString(titre, fontTitre, brushOmbre, cx - szTitre.Width / 2 + 3, r.Height - 90 + 3);

            // Texte principal avec dégradé cyan
            using var brushTitre = new System.Drawing.Drawing2D.LinearGradientBrush(
                new RectangleF(cx - szTitre.Width / 2, r.Height - 90, szTitre.Width, szTitre.Height),
                Color.FromArgb(0, 220, 255),
                Color.FromArgb(100, 180, 255),
                System.Drawing.Drawing2D.LinearGradientMode.Horizontal);
            g.DrawString(titre, fontTitre, brushTitre, cx - szTitre.Width / 2, r.Height - 90);
        }

        /// <summary>Dessine le bouclier SecurIT vectoriellement.</summary>
        private static void DessinerBouclier(Graphics g, float cx, float cy, float taille)
        {
            var pts = new PointF[]
            {
                new(cx, cy - taille),
                new(cx + taille * 0.75f, cy - taille * 0.5f),
                new(cx + taille * 0.75f, cy + taille * 0.2f),
                new(cx, cy + taille),
                new(cx - taille * 0.75f, cy + taille * 0.2f),
                new(cx - taille * 0.75f, cy - taille * 0.5f),
            };

            using var brushBouclier = new SolidBrush(Color.FromArgb(50, 0, 180, 255));
            using var penBouclier = new Pen(Color.FromArgb(160, 0, 212, 255), 3f);
            g.FillPolygon(brushBouclier, pts);
            g.DrawPolygon(penBouclier, pts);

            // "S" au centre
            using var fontS = new Font("Segoe UI", taille * 0.55f, FontStyle.Bold);
            using var brushS = new SolidBrush(Color.FromArgb(220, 0, 212, 255));
            var sz = g.MeasureString("S", fontS);
            g.DrawString("S", fontS, brushS, cx - sz.Width / 2, cy - sz.Height / 2 - taille * 0.05f);
        }

        // ────────────── Animation de pulse ──────────────

        private void InitialiserAnimation()
        {
            _timerAnime = new System.Windows.Forms.Timer { Interval = 50 };
            _timerAnime.Tick += (s, e) =>
            {
                _pulsePhase += 0.08f;
                if (_pulsePhase > Math.PI * 2) _pulsePhase = 0;
                _panelTitre.Invalidate();
            };
            _timerAnime.Start();
        }

        // ────────────── Événements des boutons ──────────────

        private void BtnJouer_Click(object? sender, EventArgs e)
        {
            _timerAnime.Stop();
            var formJeu = new FormJeu(_tailleGrille, _difficulte);
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
            var formOptions = new FormOptions(_tailleGrille, _difficulte);
            if (formOptions.ShowDialog(this) == DialogResult.OK)
            {
                _tailleGrille = formOptions.TailleGrille;
                _difficulte = formOptions.Difficulte;
            }
        }

        // ────────────── Création de boutons stylisés ──────────────

        private static Button CreerBoutonMenu(string texte, Color couleurFond, Color couleurBordure)
        {
            var btn = new Button
            {
                Text = texte,
                Font = new Font("Segoe UI", 14f, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = couleurFond,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Height = 55
            };
            btn.FlatAppearance.BorderColor = couleurBordure;
            btn.FlatAppearance.BorderSize = 2;
            btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(
                Math.Min(255, couleurFond.R + 40),
                Math.Min(255, couleurFond.G + 40),
                Math.Min(255, couleurFond.B + 40));
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
