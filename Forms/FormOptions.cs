using SecurIT_Memory.Models;

namespace SecurIT_Memory.Forms
{
    /// <summary>
    /// Formulaire Options — permet de choisir la taille de la grille et le niveau de difficulté.
    /// </summary>
    public class FormOptions : Form
    {
        // ────────────── Contrôles ──────────────
        private ComboBox _combGrille = null!;
        private ComboBox _combDifficulte = null!;
        private Button _btnValider = null!;
        private Button _btnAnnuler = null!;
        private Label _lblTitre = null!;
        private Label _lblGrille = null!;
        private Label _lblDiff = null!;
        private Panel _panelContenu = null!;

        // ────────────── Propriétés ──────────────
        /// <summary>Taille de la grille choisie (ex: 4 = grille 4×4).</summary>
        public int TailleGrille { get; private set; } = 4;

        /// <summary>Niveau de difficulté sélectionné.</summary>
        public string Difficulte { get; private set; } = "Normal";

        // ────────────── Constructeur ──────────────
        public FormOptions(int tailleActuelle = 4, string diffActuelle = "Normal")
        {
            TailleGrille = tailleActuelle;
            Difficulte = diffActuelle;
            InitialiserInterface();
        }

        private void InitialiserInterface()
        {
            // Fenêtre
            this.Text = "SecurIT Memory — Options";
            this.Size = new Size(420, 380);
            this.MinimumSize = new Size(420, 380);
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterParent;
            this.BackColor = Color.FromArgb(8, 12, 28);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;

            // ── Titre ──
            _lblTitre = new Label
            {
                Text = "⚙  OPTIONS DU JEU",
                Font = new Font("Segoe UI", 16f, FontStyle.Bold),
                ForeColor = Color.FromArgb(0, 212, 255),
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Top,
                Height = 60,
                Padding = new Padding(0, 15, 0, 0)
            };

            // ── Panel contenu ──
            _panelContenu = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(40, 10, 40, 10)
            };

            // ── Taille de grille ──
            _lblGrille = CreerLabel("Taille de la grille :");
            _combGrille = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Items = { "4×4 (8 paires)", "6×6 (18 paires)" },
                SelectedIndex = TailleGrille == 4 ? 0 : 1,
                BackColor = Color.FromArgb(20, 30, 60),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 11f),
                Height = 35,
                Width = 320
            };

            // ── Difficulté ──
            _lblDiff = CreerLabel("Niveau de difficulté :");
            _combDifficulte = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Items = { "Facile (délai 2s)", "Normal (délai 1.5s)", "Difficile (délai 0.8s)" },
                BackColor = Color.FromArgb(20, 30, 60),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 11f),
                Height = 35,
                Width = 320
            };
            _combDifficulte.SelectedIndex = Difficulte switch
            {
                "Facile" => 0,
                "Difficile" => 2,
                _ => 1
            };

            // ── Boutons ──
            var panelBtns = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.LeftToRight,
                Height = 55,
                Dock = DockStyle.Bottom,
                Padding = new Padding(30, 8, 30, 8),
                BackColor = Color.Transparent
            };

            _btnAnnuler = CreerBouton("Annuler", Color.FromArgb(60, 70, 90));
            _btnValider = CreerBouton("✔ Valider", Color.FromArgb(0, 170, 100));
            _btnValider.Click += (s, e) => Valider();
            _btnAnnuler.Click += (s, e) => { DialogResult = DialogResult.Cancel; Close(); };

            panelBtns.Controls.Add(_btnAnnuler);
            panelBtns.Controls.Add(_btnValider);

            // ── Layout dans panelContenu ──
            var layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 4,
                Padding = new Padding(0)
            };
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 30));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 45));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 30));
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 45));
            _lblGrille.Dock = DockStyle.Fill;
            _combGrille.Dock = DockStyle.Fill;
            _lblDiff.Dock = DockStyle.Fill;
            _combDifficulte.Dock = DockStyle.Fill;
            layout.Controls.Add(_lblGrille, 0, 0);
            layout.Controls.Add(_combGrille, 0, 1);
            layout.Controls.Add(_lblDiff, 0, 2);
            layout.Controls.Add(_combDifficulte, 0, 3);

            _panelContenu.Controls.Add(layout);

            this.Controls.Add(panelBtns);
            this.Controls.Add(_panelContenu);
            this.Controls.Add(_lblTitre);
        }

        private void Valider()
        {
            TailleGrille = _combGrille.SelectedIndex == 0 ? 4 : 6;
            Difficulte = _combDifficulte.SelectedIndex switch
            {
                0 => "Facile",
                2 => "Difficile",
                _ => "Normal"
            };
            DialogResult = DialogResult.OK;
            Close();
        }

        private static Label CreerLabel(string texte) => new Label
        {
            Text = texte,
            ForeColor = Color.FromArgb(180, 200, 220),
            Font = new Font("Segoe UI", 10f),
            TextAlign = ContentAlignment.BottomLeft
        };

        private static Button CreerBouton(string texte, Color couleur)
        {
            var btn = new Button
            {
                Text = texte,
                BackColor = couleur,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10f, FontStyle.Bold),
                Size = new Size(140, 38),
                Margin = new Padding(5, 0, 5, 0),
                Cursor = Cursors.Hand
            };
            btn.FlatAppearance.BorderColor = Color.FromArgb(couleur.R / 2, couleur.G / 2, couleur.B / 2);
            return btn;
        }
    }
}
