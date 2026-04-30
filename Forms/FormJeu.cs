using SecurIT_Memory.Models;
using SecurIT_Memory.Utils;

namespace SecurIT_Memory.Forms
{
    /// <summary>
    /// Formulaire principal de jeu — grille Memory plein écran, timers, Mode Contre-la-montre.
    /// La taille des cartes est calculée dynamiquement à partir de la taille de l'écran.
    /// </summary>
    public class FormJeu : Form
    {
        // ────────────── Données du jeu ──────────────
        private JeuMemory _jeu = null!;
        private int  _tailleGrille;
        private int  _delaiRetournement;
        private bool _modeContreLaMontre;

        // ────────────── Taille dynamique des cartes ──────────────
        private int _carteTaille = 120;         // Calculé dans OnShown
        private const int CARTE_MARGE = 10;

        // ────────────── Contrôles UI ──────────────
        private Panel  _panelGrille = null!;
        private Panel  _panelInfo   = null!;
        private Label  _lblEssais   = null!;
        private Label  _lblTimer    = null!;
        private Label  _lblPaires   = null!;
        private Button _btnRestart  = null!;
        private Button _btnMenu     = null!;

        // ────────────── Images ──────────────
        private Dictionary<string, Image> _images = new();
        private Image _imageDos = null!;

        // ────────────── Timers ──────────────
        private System.Windows.Forms.Timer _timerDelai  = null!;
        private System.Windows.Forms.Timer _timerChrono = null!;
        private int _secondesChrono = 0; // Mode normal (croissant) ou CLM (décroissant)

        // ────────────── Liaison CarteBase ↔ PictureBox (polymorphisme héritage) ──────────────
        private Dictionary<PictureBox, CarteBase> _picboxVersCarte = new();
        private Dictionary<CarteBase, PictureBox> _carteVersPicbox = new();

        // ────────────── Constructeur ──────────────
        public FormJeu(int tailleGrille = 4, string difficulte = "Normal", bool modeContreLaMontre = false)
        {
            _tailleGrille       = tailleGrille;
            _modeContreLaMontre = modeContreLaMontre;
            _delaiRetournement  = difficulte switch
            {
                "Facile"    => 2000,
                "Difficile" => 800,
                _           => 1500
            };

            InitialiserInterface();
            // Les images et la grille sont créées dans OnShown,
            // quand le panel a ses vraies dimensions.
        }

        // ────────────── Initialisation de l'interface ──────────────
        private void InitialiserInterface()
        {
            this.Text             = "SecurIT Memory — Le Défi Cybersécurité";
            this.BackColor        = Color.FromArgb(8, 12, 28);
            this.StartPosition    = FormStartPosition.CenterScreen;
            this.WindowState      = FormWindowState.Maximized;   // Plein écran
            this.FormBorderStyle  = FormBorderStyle.Sizable;
            this.MinimumSize      = new Size(600, 500);
            this.DoubleBuffered   = true;

            // ── Barre d'info supérieure ──
            _panelInfo = new Panel
            {
                Dock      = DockStyle.Top,
                Height    = 72,
                BackColor = Color.FromArgb(10, 18, 42),
                Padding   = new Padding(10, 8, 10, 8)
            };

            _lblEssais = CreerLabelInfo("Essais : 0");
            _lblTimer  = CreerLabelInfo("⏱  00:00");
            _lblPaires = CreerLabelInfo("Paires : 0/0");

            if (_modeContreLaMontre)
            {
                _lblTimer.ForeColor = Color.FromArgb(255, 180, 40); // Jaune-orange pour CLM
            }

            _btnRestart = CreerBoutonInfo("↺  Rejouer", Color.FromArgb(0, 90, 175));
            _btnMenu    = CreerBoutonInfo("🏠  Menu",   Color.FromArgb(55, 15, 80));
            _btnRestart.Click += (s, e) => NouvellePartie();
            _btnMenu.Click    += (s, e) => this.Close();

            var flowInfo = new FlowLayoutPanel
            {
                Dock          = DockStyle.Fill,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents  = false,
                Padding       = new Padding(8, 7, 8, 7)
            };
            flowInfo.Controls.Add(_lblEssais);
            flowInfo.Controls.Add(_lblTimer);
            flowInfo.Controls.Add(_lblPaires);
            flowInfo.Controls.Add(_btnRestart);
            flowInfo.Controls.Add(_btnMenu);
            _panelInfo.Controls.Add(flowInfo);

            // ── Panel de la grille ──
            _panelGrille = new Panel
            {
                Dock      = DockStyle.Fill,
                BackColor = Color.FromArgb(8, 12, 28)
            };

            this.Controls.Add(_panelGrille);
            this.Controls.Add(_panelInfo);

            // ── Timers ──
            _timerDelai = new System.Windows.Forms.Timer { Interval = _delaiRetournement };
            _timerDelai.Tick += TimerDelai_Tick;

            _timerChrono = new System.Windows.Forms.Timer { Interval = 1000 };
            _timerChrono.Tick += TimerChrono_Tick;
        }

        // ────────────── OnShown : calcul dynamique de la taille des cartes ──────────────
        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            // À ce stade, le formulaire est maximisé → panel a ses vraies dimensions
            _carteTaille = CalculerTailleCarte();
            InitialiserImages();
            NouvellePartie();
        }

        /// <summary>
        /// Calcule la taille optimale d'une carte en fonction de l'espace disponible.
        /// </summary>
        private int CalculerTailleCarte()
        {
            int panelW = _panelGrille.ClientSize.Width  - 20;
            int panelH = _panelGrille.ClientSize.Height - 20;
            int parLargeur = (panelW - (_tailleGrille + 1) * CARTE_MARGE) / _tailleGrille;
            int parHauteur = (panelH - (_tailleGrille + 1) * CARTE_MARGE) / _tailleGrille;
            return Math.Clamp(Math.Min(parLargeur, parHauteur), 80, 200);
        }

        /// <summary>Génère toutes les images cybersécurité à la taille calculée.</summary>
        private void InitialiserImages()
        {
            _imageDos?.Dispose();
            foreach (var img in _images.Values) img?.Dispose();
            _images.Clear();

            _imageDos = IconeGenerateur.GenererDos(_carteTaille);
            foreach (var nom in JeuMemory.IconesCybersec)
                _images[nom] = IconeGenerateur.GenererIcone(nom, _carteTaille);
        }

        // ────────────── Nouvelle partie ──────────────
        private void NouvellePartie()
        {
            _timerDelai.Stop();
            _timerChrono.Stop();
            
            // Mode normal : on part de 0. Mode CLM : on part du max (60s pour 4x4, 180s pour 6x6)
            if (_modeContreLaMontre)
            {
                _secondesChrono = _tailleGrille == 4 ? 60 : 180;
                _lblTimer.ForeColor = Color.FromArgb(255, 180, 40);
            }
            else
            {
                _secondesChrono = 0;
            }

            _timerDelai.Interval = _delaiRetournement;

            int nbPaires = (_tailleGrille * _tailleGrille) / 2;
            _jeu = new JeuMemory(nbPaires);

            ConstruireGrille();
            MettreAJourLabels();
            _lblTimer.Text = _modeContreLaMontre ? $"⏳  {_secondesChrono / 60:D2}:{_secondesChrono % 60:D2}" : $"⏱  00:00";
            
            _timerChrono.Start();
        }

        /// <summary>Génère dynamiquement les PictureBox centrées sur la grille WinForms.</summary>
        private void ConstruireGrille()
        {
            // Nettoyage
            foreach (var pb in _picboxVersCarte.Keys) { pb.Click -= PictureBox_Click; pb.Dispose(); }
            _picboxVersCarte.Clear();
            _carteVersPicbox.Clear();
            _panelGrille.Controls.Clear();

            // Centrage
            int totalW = _tailleGrille * (_carteTaille + CARTE_MARGE) + CARTE_MARGE;
            int totalH = _tailleGrille * (_carteTaille + CARTE_MARGE) + CARTE_MARGE;
            int pw     = _panelGrille.ClientSize.Width;
            int ph     = _panelGrille.ClientSize.Height;
            int offsetX = Math.Max(12, (pw - totalW) / 2);
            int offsetY = Math.Max(12, (ph - totalH) / 2);

            for (int i = 0; i < _jeu.Cartes.Count; i++)
            {
                int col   = i % _tailleGrille;
                int row   = i / _tailleGrille;
                var carte = _jeu.Cartes[i];

                var pb = new PictureBox
                {
                    Size     = new Size(_carteTaille, _carteTaille),
                    Location = new Point(
                        offsetX + CARTE_MARGE + col * (_carteTaille + CARTE_MARGE),
                        offsetY + CARTE_MARGE + row * (_carteTaille + CARTE_MARGE)),
                    SizeMode    = PictureBoxSizeMode.StretchImage,
                    Image       = _imageDos,
                    Cursor      = Cursors.Hand,
                    BorderStyle = BorderStyle.None,
                    BackColor   = Color.Transparent
                };

                pb.MouseEnter += (s, _) => { if (((PictureBox)s!).Cursor == Cursors.Hand) ((PictureBox)s).BackColor = Color.FromArgb(25, 0, 212, 255); };
                pb.MouseLeave += (s, _) => ((PictureBox)s!).BackColor = Color.Transparent;
                pb.Click += PictureBox_Click;

                _picboxVersCarte[pb]    = carte;
                _carteVersPicbox[carte] = pb;
                _panelGrille.Controls.Add(pb);
            }
        }

        // ────────────── Logique de jeu ──────────────
        private void PictureBox_Click(object? sender, EventArgs e)
        {
            if (sender is not PictureBox pb) return;
            if (!_picboxVersCarte.TryGetValue(pb, out CarteBase? carteBase)) return;
            // Polymorphisme : CarteBase → Carte (sous-classe cybersécurité)
            if (carteBase is not Carte carte) return;

            if (!_jeu.TenterRevelerCarte(carte)) return;
            AfficherFaceCarte(pb, carte);

            if (_jeu.NbCartesRetournees == 2)
            {
                MettreAJourLabels();
                bool paire = _jeu.VerifierPaire();
                if (paire)
                {
                    AnimerPaireTrouvee(carte);
                    MettreAJourLabels();
                    if (_jeu.EstTermine) { _timerChrono.Stop(); AfficherVictoire(); }
                }
                else
                {
                    _jeu.ClicsBloques = true;
                    _timerDelai.Start();
                }
            }
            else
            {
                MettreAJourLabels();
            }
        }

        private void AfficherFaceCarte(PictureBox pb, CarteBase carte)
        {
            pb.Image = carte.Etat == EtatCarte.Cachee
                ? _imageDos
                : (_images.TryGetValue(carte.NomIcone, out var img) ? img : _imageDos);
        }

        private void AnimerPaireTrouvee(CarteBase carte)
        {
            foreach (var kvp in _carteVersPicbox)
                if (kvp.Key.IdPaire == carte.IdPaire)
                    kvp.Value.BackColor = Color.FromArgb(45, 0, 255, 100);
        }

        // ────────────── Timer délai retournement ──────────────
        private void TimerDelai_Tick(object? sender, EventArgs e)
        {
            _timerDelai.Stop();
            _jeu.RetournerCartesNonPairedees();
            foreach (var kvp in _picboxVersCarte)
            {
                if (!kvp.Value.EstTrouvee())
                {
                    kvp.Key.Image    = _imageDos;
                    kvp.Key.BackColor = Color.Transparent;
                }
            }
        }

        // ────────────── Chronomètre ──────────────
        private void TimerChrono_Tick(object? sender, EventArgs e)
        {
            if (_modeContreLaMontre)
            {
                _secondesChrono--;
                _lblTimer.Text = $"⏳  {_secondesChrono / 60:D2}:{_secondesChrono % 60:D2}";

                if (_secondesChrono <= 10)
                {
                    // Alerte quand il reste 10 secondes ou moins
                    _lblTimer.ForeColor = (_secondesChrono % 2 == 0) ? Color.Red : Color.FromArgb(255, 180, 40);
                }

                if (_secondesChrono <= 0)
                {
                    _timerChrono.Stop();
                    _jeu.ClicsBloques = true; // Bloquer les clics
                    AfficherDefaite();
                }
            }
            else
            {
                _secondesChrono++;
                _lblTimer.Text = $"⏱  {_secondesChrono / 60:D2}:{_secondesChrono % 60:D2}";
            }
        }

        // ────────────── Labels et fins de jeu ──────────────
        private void MettreAJourLabels()
        {
            _lblEssais.Text = $"Essais : {_jeu.NbEssais}";
            int total = (_tailleGrille * _tailleGrille) / 2;
            _lblPaires.Text = $"Paires : {_jeu.NbPairesTrouvees}/{total}";
        }

        private void AfficherVictoire()
        {
            string mode = _modeContreLaMontre ? "  ⏳ Mode Contre-la-montre\n" : "";
            string msg =
                $"🎉  FÉLICITATIONS !\n\n" +
                $"{mode}\n" +
                $"Toutes les paires ont été trouvées !\n\n" +
                $"⏱  Temps    : {_secondesChrono / 60:D2}:{_secondesChrono % 60:D2}\n" +
                $"🔢  Essais   : {_jeu.NbEssais}\n\n" +
                $"Voulez-vous rejouer ?";

            var result = MessageBox.Show(msg, "SecurIT Memory — Victoire !",
                MessageBoxButtons.YesNo, MessageBoxIcon.Information);

            if (result == DialogResult.Yes) NouvellePartie();
            else this.Close();
        }

        private void AfficherDefaite()
        {
            string msg =
                $"💀  TEMPS ÉCOULÉ !\n\n" +
                $"Vous n'avez pas réussi à trouver toutes les paires à temps.\n" +
                $"Paires trouvées : {_jeu.NbPairesTrouvees} sur {(_tailleGrille * _tailleGrille) / 2}\n\n" +
                $"Voulez-vous réessayer ?";

            var result = MessageBox.Show(msg, "SecurIT Memory — Défaite",
                MessageBoxButtons.YesNo, MessageBoxIcon.Error);

            if (result == DialogResult.Yes) NouvellePartie();
            else this.Close();
        }

        // ────────────── Helpers ──────────────
        private static Label CreerLabelInfo(string texte) => new Label
        {
            Text      = texte,
            ForeColor = Color.FromArgb(0, 212, 255),
            Font      = new Font("Segoe UI", 12f, FontStyle.Bold),
            AutoSize  = false,
            Width     = 155,
            Height    = 44,
            TextAlign = ContentAlignment.MiddleCenter,
            Margin    = new Padding(6, 4, 6, 4)
        };

        private static Button CreerBoutonInfo(string texte, Color couleur)
        {
            var btn = new Button
            {
                Text      = texte,
                BackColor = couleur,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font      = new Font("Segoe UI", 10f, FontStyle.Bold),
                Size      = new Size(120, 44),
                Margin    = new Padding(6, 4, 6, 4),
                Cursor    = Cursors.Hand
            };
            btn.FlatAppearance.BorderColor = Color.FromArgb(couleur.R / 2, couleur.G / 2, couleur.B / 2);
            return btn;
        }

        // ────────────── Nettoyage ──────────────
        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            _timerDelai?.Stop();
            _timerChrono?.Stop();
            foreach (var img in _images.Values) img?.Dispose();
            _imageDos?.Dispose();
            base.OnFormClosed(e);
        }
    }
}
