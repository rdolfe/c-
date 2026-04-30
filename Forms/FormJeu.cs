using SecurIT_Memory.Models;
using SecurIT_Memory.Utils;

namespace SecurIT_Memory.Forms
{
    /// <summary>
    /// Formulaire principal de jeu — affiche la grille Memory et gère la logique de jeu.
    /// Contient le Timer de délai et le chronomètre de partie.
    /// </summary>
    public class FormJeu : Form
    {
        // ────────────── Données du jeu ──────────────
        private JeuMemory _jeu = null!;
        private int _tailleGrille;           // Ex: 4 pour une grille 4×4
        private int _delaiRetournement;      // Délai (ms) avant retournement des non-paires
        private string _difficulte;

        // ────────────── Contrôles UI ──────────────
        private Panel _panelGrille = null!;      // Contient les PictureBox des cartes
        private Panel _panelInfo = null!;        // Barre d'info en haut
        private Label _lblEssais = null!;
        private Label _lblTimer = null!;
        private Label _lblPaires = null!;
        private Button _btnRestart = null!;
        private Button _btnMenu = null!;

        // ────────────── Images ──────────────
        private Dictionary<string, Image> _images = new();  // Cache des images d'icônes
        private Image _imageDos = null!;                    // Image du dos de carte

        // ────────────── Timers ──────────────
        private System.Windows.Forms.Timer _timerDelai = null!;    // Délai avant retournement
        private System.Windows.Forms.Timer _timerChrono = null!;   // Chronomètre de partie
        private int _secondes;                                      // Secondes écoulées

        // ────────────── Liaison carte ↔ PictureBox ──────────────
        private Dictionary<PictureBox, Carte> _picboxVersCarte = new();
        private Dictionary<Carte, PictureBox> _carteVersPicbox = new();

        // ────────────── Constantes visuelles ──────────────
        private const int CARTE_TAILLE = 110;
        private const int CARTE_MARGE = 8;

        // ────────────── Constructeur ──────────────
        public FormJeu(int tailleGrille = 4, string difficulte = "Normal")
        {
            _tailleGrille = tailleGrille;
            _difficulte = difficulte;
            _delaiRetournement = difficulte switch
            {
                "Facile" => 2000,
                "Difficile" => 800,
                _ => 1500
            };

            InitialiserImages();
            InitialiserInterface();
            NouvellePartie();
        }

        // ────────────── Initialisation des images ──────────────

        /// <summary>Pré-génère toutes les images cybersécurité via IconeGenerateur.</summary>
        private void InitialiserImages()
        {
            _imageDos = IconeGenerateur.GenererDos(CARTE_TAILLE);

            foreach (var nom in JeuMemory.IconesCybersec)
                _images[nom] = IconeGenerateur.GenererIcone(nom, CARTE_TAILLE);
        }

        // ────────────── Initialisation de l'interface ──────────────

        private void InitialiserInterface()
        {
            int largeurGrille = _tailleGrille * (CARTE_TAILLE + CARTE_MARGE) + CARTE_MARGE;
            int hauteurGrille = _tailleGrille * (CARTE_TAILLE + CARTE_MARGE) + CARTE_MARGE;

            this.Text = "SecurIT Memory — Le Défi Cybersécurité";
            this.BackColor = Color.FromArgb(8, 12, 28);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimumSize = new Size(largeurGrille + 20, hauteurGrille + 130);
            this.ClientSize = new Size(largeurGrille + 20, hauteurGrille + 130);

            // ── Barre d'informations supérieure ──
            _panelInfo = new Panel
            {
                Dock = DockStyle.Top,
                Height = 70,
                BackColor = Color.FromArgb(12, 20, 45),
                Padding = new Padding(10, 8, 10, 8)
            };

            _lblEssais = CreerLabelInfo("Essais : 0");
            _lblTimer = CreerLabelInfo("⏱ 00:00");
            _lblPaires = CreerLabelInfo("Paires : 0/0");

            _btnRestart = new Button
            {
                Text = "↺ Rejouer",
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(0, 100, 180),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9f, FontStyle.Bold),
                Size = new Size(90, 36),
                Cursor = Cursors.Hand
            };
            _btnRestart.FlatAppearance.BorderColor = Color.FromArgb(0, 150, 230);
            _btnRestart.Click += (s, e) => NouvellePartie();

            _btnMenu = new Button
            {
                Text = "🏠 Menu",
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(60, 20, 80),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9f, FontStyle.Bold),
                Size = new Size(90, 36),
                Cursor = Cursors.Hand
            };
            _btnMenu.FlatAppearance.BorderColor = Color.FromArgb(130, 50, 180);
            _btnMenu.Click += (s, e) => { this.Close(); };

            var flowInfo = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                Padding = new Padding(5, 5, 5, 5)
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
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(10, 15, 35),
                AutoScroll = true
            };

            this.Controls.Add(_panelGrille);
            this.Controls.Add(_panelInfo);

            // ── Timers ──
            _timerDelai = new System.Windows.Forms.Timer { Interval = _delaiRetournement };
            _timerDelai.Tick += TimerDelai_Tick;

            _timerChrono = new System.Windows.Forms.Timer { Interval = 1000 };
            _timerChrono.Tick += TimerChrono_Tick;
        }

        // ────────────── Nouvelle partie ──────────────

        /// <summary>Démarre une nouvelle partie : réinitialise le jeu et reconstruit la grille.</summary>
        private void NouvellePartie()
        {
            // Stopper les timers en cours
            _timerDelai.Stop();
            _timerChrono.Stop();
            _secondes = 0;

            // Recalculer le délai si la difficulté n'a pas changé
            _timerDelai.Interval = _delaiRetournement;

            // Créer un nouveau jeu
            int nbPaires = (_tailleGrille * _tailleGrille) / 2;
            _jeu = new JeuMemory(nbPaires);

            // Reconstruire la grille visuelle
            ConstruireGrille();

            // Mettre à jour les labels
            MettreAJourLabels();

            // Démarrer le chronomètre
            _timerChrono.Start();
        }

        /// <summary>Génère dynamiquement les PictureBox sur la grille WinForms.</summary>
        private void ConstruireGrille()
        {
            // Nettoyage des anciens contrôles
            foreach (var pb in _picboxVersCarte.Keys)
            {
                pb.Click -= PictureBox_Click;
                pb.Dispose();
            }
            _picboxVersCarte.Clear();
            _carteVersPicbox.Clear();
            _panelGrille.Controls.Clear();

            int nbCartes = _jeu.Cartes.Count;

            // Calcul du centrage
            int totalW = _tailleGrille * (CARTE_TAILLE + CARTE_MARGE) + CARTE_MARGE;
            int totalH = _tailleGrille * (CARTE_TAILLE + CARTE_MARGE) + CARTE_MARGE;
            int offsetX = Math.Max(0, (_panelGrille.Width - totalW) / 2);
            int offsetY = Math.Max(0, (_panelGrille.Height - totalH) / 2);

            for (int i = 0; i < nbCartes; i++)
            {
                int col = i % _tailleGrille;
                int row = i / _tailleGrille;

                var carte = _jeu.Cartes[i];

                var pb = new PictureBox
                {
                    Size = new Size(CARTE_TAILLE, CARTE_TAILLE),
                    Location = new Point(
                        offsetX + CARTE_MARGE + col * (CARTE_TAILLE + CARTE_MARGE),
                        offsetY + CARTE_MARGE + row * (CARTE_TAILLE + CARTE_MARGE)),
                    SizeMode = PictureBoxSizeMode.StretchImage,
                    Image = _imageDos,
                    Cursor = Cursors.Hand,
                    BorderStyle = BorderStyle.None,
                    BackColor = Color.Transparent
                };

                // Effet de survol
                pb.MouseEnter += (s, e) => { if (((PictureBox)s!).Cursor == Cursors.Hand) pb.BackColor = Color.FromArgb(20, 0, 212, 255); };
                pb.MouseLeave += (s, e) => pb.BackColor = Color.Transparent;
                pb.Click += PictureBox_Click;

                _picboxVersCarte[pb] = carte;
                _carteVersPicbox[carte] = pb;
                _panelGrille.Controls.Add(pb);
            }
        }

        // ────────────── Logique de jeu ──────────────

        /// <summary>
        /// Gère le clic sur une PictureBox — cœur de la logique de retournement.
        /// </summary>
        private void PictureBox_Click(object? sender, EventArgs e)
        {
            if (sender is not PictureBox pb) return;
            if (!_picboxVersCarte.TryGetValue(pb, out Carte? carte)) return;

            // Tenter de révéler la carte via le gestionnaire de jeu
            if (!_jeu.TenterRevelerCarte(carte)) return;

            // Mettre à jour visuellement la carte
            AfficherFaceCarte(pb, carte);

            // Si 2 cartes sont retournées → vérifier la paire
            if (_jeu.NbCartesRetournees == 2)
            {
                MettreAJourLabels();
                bool paire = _jeu.VerifierPaire();

                if (paire)
                {
                    // Paire trouvée : animation visuelle
                    AnimerPaireTrouvee(carte);
                    MettreAJourLabels();

                    // Vérifier victoire
                    if (_jeu.EstTermine)
                    {
                        _timerChrono.Stop();
                        AfficherVictoire();
                    }
                }
                else
                {
                    // Pas une paire → bloquer les clics et lancer le timer de délai
                    _jeu.ClicsBloques = true;
                    _timerDelai.Start();
                }
            }
            else
            {
                MettreAJourLabels();
            }
        }

        /// <summary>Affiche la face avant (icône) ou le dos d'une carte.</summary>
        private void AfficherFaceCarte(PictureBox pb, Carte carte)
        {
            if (carte.Etat == EtatCarte.Cachee)
                pb.Image = _imageDos;
            else
                pb.Image = _images.TryGetValue(carte.NomIcone, out var img) ? img : _imageDos;
        }

        /// <summary>Animation légère pour une paire trouvée (bordure verte).</summary>
        private void AnimerPaireTrouvee(Carte carte)
        {
            // On cherche les deux cartes de la même paire
            foreach (var kvp in _carteVersPicbox)
            {
                if (kvp.Key.IdPaire == carte.IdPaire)
                {
                    kvp.Value.BackColor = Color.FromArgb(40, 0, 255, 100);
                }
            }
        }

        // ────────────── Timers ──────────────

        /// <summary>
        /// Déclenché après le délai — retourne les cartes non-appariées face cachée.
        /// </summary>
        private void TimerDelai_Tick(object? sender, EventArgs e)
        {
            _timerDelai.Stop();
            _jeu.RetournerCartesNonPairedees();

            // Mettre à jour les visuels de toutes les cartes non-trouvées
            foreach (var kvp in _picboxVersCarte)
            {
                var pb = kvp.Key;
                var carte = kvp.Value;
                if (!carte.EstTrouvee())
                {
                    pb.Image = _imageDos;
                    pb.BackColor = Color.Transparent;
                }
            }
        }

        /// <summary>Met à jour le chronomètre chaque seconde.</summary>
        private void TimerChrono_Tick(object? sender, EventArgs e)
        {
            _secondes++;
            int min = _secondes / 60;
            int sec = _secondes % 60;
            _lblTimer.Text = $"⏱ {min:D2}:{sec:D2}";
        }

        // ────────────── Affichage ──────────────

        private void MettreAJourLabels()
        {
            _lblEssais.Text = $"Essais : {_jeu.NbEssais}";
            int totalPaires = (_tailleGrille * _tailleGrille) / 2;
            _lblPaires.Text = $"Paires : {_jeu.NbPairesTrouvees}/{totalPaires}";
        }

        /// <summary>Affiche la fenêtre de victoire avec le temps et le score.</summary>
        private void AfficherVictoire()
        {
            int min = _secondes / 60;
            int sec = _secondes % 60;

            string msg =
                $"🎉  FÉLICITATIONS !\n\n" +
                $"Toutes les paires ont été trouvées !\n\n" +
                $"⏱  Temps : {min:D2}:{sec:D2}\n" +
                $"🔢  Essais : {_jeu.NbEssais}\n\n" +
                $"Voulez-vous rejouer ?";

            var result = MessageBox.Show(msg, "SecurIT Memory — Victoire !",
                MessageBoxButtons.YesNo, MessageBoxIcon.Information);

            if (result == DialogResult.Yes)
                NouvellePartie();
            else
                this.Close();
        }

        private static Label CreerLabelInfo(string texte) => new Label
        {
            Text = texte,
            ForeColor = Color.FromArgb(0, 212, 255),
            Font = new Font("Segoe UI", 11f, FontStyle.Bold),
            AutoSize = false,
            Width = 120,
            Height = 40,
            TextAlign = ContentAlignment.MiddleCenter,
            Margin = new Padding(5, 3, 5, 3)
        };

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
