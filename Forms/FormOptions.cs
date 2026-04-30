using SecurIT_Memory.Models;

namespace SecurIT_Memory.Forms
{
    /// <summary>
    /// Formulaire Options — grille, difficulté et Mode Contre-la-montre.
    /// </summary>
    public class FormOptions : Form
    {
        // ────────────── Contrôles ──────────────
        private ComboBox _combGrille     = null!;
        private ComboBox _combDifficulte = null!;
        private CheckBox _chkContreLaMontre = null!;
        private Button   _btnValider    = null!;
        private Button   _btnAnnuler    = null!;

        // ────────────── Propriétés ──────────────
        /// <summary>Taille de la grille choisie (4 ou 6).</summary>
        public int TailleGrille { get; private set; } = 4;

        /// <summary>Niveau de difficulté sélectionné.</summary>
        public string Difficulte { get; private set; } = "Normal";

        /// <summary>
        /// Mode Contre-la-montre activé : temps limité pour gagner.
        /// </summary>
        public bool ModeContreLaMontre { get; private set; } = false;

        // ────────────── Constructeur ──────────────
        public FormOptions(int tailleActuelle = 4, string diffActuelle = "Normal", bool contreLaMontreActuel = false)
        {
            TailleGrille = tailleActuelle;
            Difficulte   = diffActuelle;
            ModeContreLaMontre = contreLaMontreActuel;
            InitialiserInterface();
        }

        private void InitialiserInterface()
        {
            this.Text = "SecurIT Memory — Options";
            this.Size = new Size(480, 450);
            this.MinimumSize = new Size(480, 450);
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterParent;
            this.BackColor = Color.FromArgb(8, 12, 28);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.DoubleBuffered = true;

            // ── Titre ──
            var lblTitre = new Label
            {
                Text = "⚙   OPTIONS DU JEU",
                Font = new Font("Segoe UI", 17f, FontStyle.Bold),
                ForeColor = Color.FromArgb(0, 212, 255),
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Top,
                Height = 65,
                Padding = new Padding(0, 15, 0, 0)
            };

            // ── Contenu ──
            var panelContenu = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(45, 12, 45, 12)
            };

            // Grille
            var lblGrille = CreerLabel("Taille de la grille :");
            _combGrille = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                BackColor = Color.FromArgb(20, 30, 60),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 12f),
                Height = 38
            };
            _combGrille.Items.AddRange(new object[] { "4×4  (8 paires — Standard)", "6×6  (18 paires — Expert)" });
            _combGrille.SelectedIndex = TailleGrille == 4 ? 0 : 1;

            // Difficulté
            var lblDiff = CreerLabel("Niveau de difficulté :");
            _combDifficulte = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                BackColor = Color.FromArgb(20, 30, 60),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 12f),
                Height = 38
            };
            _combDifficulte.Items.AddRange(new object[] { "Facile   (délai 2s)", "Normal  (délai 1.5s)", "Difficile  (délai 0.8s)" });
            _combDifficulte.SelectedIndex = Difficulte switch { "Facile" => 0, "Difficile" => 2, _ => 1 };

            // Mode Contre-la-montre
            var lblModeCLM = CreerLabel("Mode de Jeu :");
            _chkContreLaMontre = new CheckBox
            {
                Text = "⏳  Activer le Contre-la-montre",
                Checked = ModeContreLaMontre,
                ForeColor = Color.FromArgb(255, 180, 40),
                Font = new Font("Segoe UI", 12f, FontStyle.Bold),
                Height = 32,
                Cursor = Cursors.Hand,
                AutoSize = false
            };

            // Description du mode Contre-la-montre
            var lblDescCLM = new Label
            {
                Text = "Vous avez un temps limité pour trouver toutes\nles paires (4×4 : 60s | 6×6 : 180s).",
                ForeColor = Color.FromArgb(180, 140, 60),
                Font = new Font("Segoe UI", 9f, FontStyle.Italic),
                Height = 40,
                AutoSize = false
            };

            // Layout tableau
            var layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 7,
                Padding = new Padding(0)
            };
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 28));   // lblGrille
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 42));   // combGrille
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 28));   // lblDiff
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 42));   // combDiff
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 28));   // lblModeCLM
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 36));   // chkContreLaMontre
            layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 42));   // lblDescCLM

            foreach (Control c in new Control[] { lblGrille, _combGrille, lblDiff, _combDifficulte, lblModeCLM, _chkContreLaMontre, lblDescCLM })
            {
                c.Dock = DockStyle.Fill;
                layout.Controls.Add(c);
            }
            panelContenu.Controls.Add(layout);

            // ── Boutons ──
            var panelBtns = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.RightToLeft,
                Height = 60,
                Dock = DockStyle.Bottom,
                Padding = new Padding(30, 10, 30, 10),
                BackColor = Color.Transparent
            };
            _btnValider = CreerBouton("✔  Valider", Color.FromArgb(0, 140, 90));
            _btnAnnuler = CreerBouton("Annuler",    Color.FromArgb(55, 65, 85));
            _btnValider.Click += (s, e) => Valider();
            _btnAnnuler.Click += (s, e) => { DialogResult = DialogResult.Cancel; Close(); };
            panelBtns.Controls.Add(_btnValider);
            panelBtns.Controls.Add(_btnAnnuler);

            this.Controls.Add(panelBtns);
            this.Controls.Add(panelContenu);
            this.Controls.Add(lblTitre);
        }

        private void Valider()
        {
            TailleGrille = _combGrille.SelectedIndex == 0 ? 4 : 6;
            Difficulte   = _combDifficulte.SelectedIndex switch { 0 => "Facile", 2 => "Difficile", _ => "Normal" };
            ModeContreLaMontre = _chkContreLaMontre.Checked;
            DialogResult = DialogResult.OK;
            Close();
        }

        private static Label CreerLabel(string texte) => new Label
        {
            Text = texte,
            ForeColor = Color.FromArgb(180, 200, 230),
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
                Font = new Font("Segoe UI", 11f, FontStyle.Bold),
                Size = new Size(145, 40),
                Margin = new Padding(8, 0, 8, 0),
                Cursor = Cursors.Hand
            };
            btn.FlatAppearance.BorderColor = Color.FromArgb(couleur.R / 2, couleur.G / 2, couleur.B / 2);
            return btn;
        }
    }
}
