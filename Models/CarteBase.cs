using System.Drawing;

namespace SecurIT_Memory.Models
{
    /// <summary>
    /// Classe abstraite de base pour toutes les cartes du jeu Memory.
    /// Définit le contrat commun : identifiant de paire, état, et comportements.
    /// Les sous-classes spécialisent le type d'icône (cybersécurité, matériel, etc.)
    /// </summary>
    /// <remarks>
    /// Notion C# : Héritage (Inheritance).
    /// <see cref="Carte"/> étend cette classe pour le thème cybersécurité.
    /// </remarks>
    public abstract class CarteBase
    {
        // ────────────── Champs protégés (accessibles aux sous-classes) ──────────────
        protected int _idPaire;
        protected Image? _image;
        protected EtatCarte _etat;

        // ────────────── Propriétés publiques abstraites / concrètes ──────────────

        /// <summary>
        /// Identifiant de paire : deux cartes avec le même IdPaire forment une paire valide.
        /// </summary>
        public int IdPaire
        {
            get => _idPaire;
            protected set => _idPaire = value;
        }

        /// <summary>
        /// Image affichée quand la carte est révélée.
        /// </summary>
        public Image? Image
        {
            get => _image;
            set => _image = value;
        }

        /// <summary>
        /// État courant de la carte (Cachee, Revelee, Trouvee).
        /// </summary>
        public EtatCarte Etat
        {
            get => _etat;
            set => _etat = value;
        }

        /// <summary>
        /// Nom ou description de l'icône associée à cette carte (implémenté par les sous-classes).
        /// </summary>
        public abstract string NomIcone { get; }

        // ────────────── Constructeur protégé ──────────────

        /// <summary>
        /// Constructeur de base : initialise l'ID de paire et l'état à Cachée.
        /// </summary>
        protected CarteBase(int idPaire)
        {
            _idPaire = idPaire;
            _etat = EtatCarte.Cachee;
            _image = null;
        }

        // ────────────── Méthodes publiques ──────────────

        /// <summary>Indique si la carte est actuellement cachée (dos visible).</summary>
        public bool EstCachee() => _etat == EtatCarte.Cachee;

        /// <summary>Indique si la paire de cette carte a été trouvée.</summary>
        public bool EstTrouvee() => _etat == EtatCarte.Trouvee;

        /// <summary>Représentation textuelle pour le débogage.</summary>
        public override string ToString() => $"[ID={_idPaire}, Icône={NomIcone}, État={_etat}]";
    }
}
